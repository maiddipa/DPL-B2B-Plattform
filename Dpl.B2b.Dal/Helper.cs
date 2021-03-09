using Dpl.B2b.Dal.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Dpl.B2b.Dal
{
    public static class Helper
    {
        public static void ExecuteSql(string connectionString, string sql)
        {
            using (var conn = new Microsoft.Data.SqlClient.SqlConnection(connectionString))
            {
                var server = new Microsoft.SqlServer.Management.Smo.Server(new Microsoft.SqlServer.Management.Common.ServerConnection(conn));
                server.ConnectionContext.ExecuteNonQuery(sql);
            }
        }

        public static string ReadTextFromFileUtf8(string relativePath, bool gzip = false, Encoding encoding = null)
        {
            // default to UTF8 encoding
            encoding = encoding ?? Encoding.UTF8;

            var basePath = System.IO.Path.GetDirectoryName(typeof(OlmaDbContext).Assembly.Location);
            var filePath = System.IO.Path.Combine(basePath, relativePath);
            if (!gzip)
            {
                return System.IO.File.ReadAllText(filePath, encoding);
            }

            using (System.IO.FileStream reader = System.IO.File.OpenRead(filePath))
            using (var zip = new System.IO.Compression.GZipStream(reader, System.IO.Compression.CompressionMode.Decompress, false))
            using (var unzip = new System.IO.StreamReader(zip, encoding))
            {
                return unzip.ReadToEnd();
            }
        }

        public static IList<TEntity> ReadCsv<TEntity, TClassMap>(string relativePath, bool gzip = true, Encoding encoding = null, string delimiter = ",", bool ignoreMissingFields = false)
            where TClassMap : ClassMap<TEntity>
        {
            var csvText = ReadTextFromFileUtf8(relativePath, gzip, encoding);

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = delimiter
            };

            if (ignoreMissingFields)
            {
                config.MissingFieldFound = null;
            }

            using (var reader = new StringReader(csvText))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.IgnoreBlankLines = true;
                csv.Configuration.IgnoreReferences = true;

                csv.Configuration.RegisterClassMap<TClassMap>();



                var list = csv.GetRecords<TEntity>().ToList();
                return list;
            }
        }

        public static IList<TEntity> ReadCsv<TEntity>(TEntity sample, string relativePath, bool gzip = true, Encoding encoding = null)
        {
            var csvText = ReadTextFromFileUtf8(relativePath, gzip, encoding);

            using (var reader = new StringReader(csvText))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.IgnoreBlankLines = true;
                csv.Configuration.IgnoreReferences = true;

                var list = csv.GetRecords(sample).ToList();
                return list;
            }
        }

        public static void BatchInsert<TEntity>(TEntity sample, OlmaDbContext context, IEnumerable<TEntity> rows, string tableName, string columns, Func<TEntity, string> getValues, bool identityInsert = true)
        {
            // 1000 is the max size for batched inserts
            double chunkSize = 1000;
            //
            var chunkedInserts = rows
                .Select((i, index) => new { row = i, index })
                .GroupBy(i => Math.Floor(i.index / chunkSize), i => i.row)
                .Select(g => {
                    var sb = new StringBuilder();
                    sb.AppendLine($"INSERT INTO {tableName} ({columns}) VALUES ");

                    foreach (var row in g)
                    {
                        sb.AppendLine($"({getValues(row)}),");
                    }
                    sb.Remove(sb.Length - 3, 3);
                    return sb.ToString();
                });

            if(identityInsert && context.Database.CurrentTransaction == null)
            {
                throw new ArgumentNullException("Transaction is required to execute batched inserts");
            }

            if (identityInsert)
            {
                context.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT {tableName} ON ");
            }

            foreach (var insert in chunkedInserts)
            {
                context.Database.ExecuteSqlRaw(insert);
            }

            if (identityInsert)
            {
                context.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT {tableName} OFF ");
            }
        }
    }
}
