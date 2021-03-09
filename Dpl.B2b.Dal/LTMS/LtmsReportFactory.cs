#region #region Copyright © 2019 DPL Systems, Incorporated

// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename: LtmsBookingFactory.cs
// Date:     21/11/2019
// Author:   Schlicht

#endregion

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using Dpl.B2b.Dal.Ltms;

namespace Dpl.B2b.Dal.LTMS
{
    /// <summary>
    /// Creates a report from a CSV file
    /// </summary>
    public class LtmsReportFactory
    {
        #region Constructors

        public LtmsReportFactory()
        {
            Data = new DataContainer();
        }

        #endregion

        #region Properties

        public DataContainer Data { get; }

        #endregion

        #region Public Functions

        /// <summary>
        /// Reihenfolge der Spalten(Typen) in der CSV Datei müssen übereinstimmen mit der ReportModel Klasse
        /// Für die erzeugung der CSV Datei steht die ReportSampleData.sql Datei zur Verfügung.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public LtmsReportFactory LoadCsvFile(string path)
        {
            try
            {
                using var reader = new StreamReader(path);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                csv.Configuration.TypeConverterCache.AddConverter<string>(new NullableStringConverter());
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.Delimiter = ";";

                var records = csv.GetRecords<ReportModel>();
                foreach (var record in records)
                {
                    AddNewRow(record);
                }
            }
            catch (FileNotFoundException exception)
            {
                throw new Exception("File not found exception", exception);
            }
            catch (Exception exception)
            {
                throw new Exception("Error processing File", exception);
            }

            return this;
        }

        public DataContainer Create()
        {
            return Data;
        }

        public static LtmsReportFactory Instance()
        {
            return new LtmsReportFactory();
        }

        #endregion

        #region Private Functions

        private LtmsReportFactory AddNewRow(ReportModel record)
        {
            var report = Data.Reports.Find(a => a.Id == record.ID);
            if (report != null) return this;
            report = new Reports()
            {
                Id = record.ID,
                Name = record.Name,
                ReportName = record.ReportName,
                ReportTypeId = record.ReportTypeId,
                ReportStateId = record.ReportState_ID,
                StartDate = record.StartDate,
                EndDate = record.EndDate,
                Quarter = record.Quarter,
                Month = record.Month,
                Year = record.Year,
                PrimaryAccountId = record.PrimaryAccountId,
                AddressId = record.AddressId,
                CustomerNumber = record.CustomerNumber,
                MatchedTime = record.MatchedTime,
                MatchedUser = record.MatchedUser,
                //ResponsiblePersonId = record.ResponsiblePerson_ID, //Not Supported 
                //KeyReportManagerId = record.KeyReportManager_ID, //Not Supported 
                //SalespersonId = record.Salesperson_ID, //Not Supported 
                OptimisticLockField = record.OptimisticLockField,
                CreateUser = record.CreateUser,
                CreateTime = record.CreateTime,
                UpdateUser = record.UpdateUser,
                UpdateTime = record.UpdateTime
            };
            Data.Reports.Add(report);

            return this;
        }

        #endregion

        #region Nested

        public class ReportModel
        {
            [Name("ID")] 
            public int ID { get; set; }
            [Name("Name")] 
            public string Name { get; set; }
            [Name("ReportName")] 
            public string ReportName { get; set; }
            [Name("FileName")] 
            public string FileName { get; set; }
            [Name("ReportFile")] 
            public byte[] ReportFile { get; set; }
            [Name("FilePath")] 
            public string FilePath { get; set; }
            [Name("ReportUrl")] 
            public string  ReportUrl { get; set; }
            [Name("StartDate")] 
            public DateTime? StartDate { get; set; }
            [Name("EndDate")] 
            public DateTime? EndDate { get; set; }
            [Name("Quarter")] 
            public int? Quarter { get; set; }
            [Name("Month")] 
            public int? Month { get; set; }
            [Name("AddressId")] 
            public int? AddressId { get; set; }
            [Name("CustomerNumber")] 
            public string CustomerNumber { get; set; }
            [Name("AdditionalParams")] 
            public string AdditionalParams { get; set; }
            [Name("CreateUser")] 
            public string CreateUser { get; set; }
            [Name("CreateTime")] 
            public DateTime CreateTime { get; set; }
            [Name("UpdateUser")] 
            public string UpdateUser { get; set; }
            [Name("UpdateTime")] 
            public DateTime? UpdateTime { get; set; }
            [Name("ReportState_ID")] 
            public string ReportState_ID { get; set; }
            [Name("Year")] 
            public int? Year { get; set; }
            [Name("MatchedUser")] 
            public string  MatchedUser { get; set; }
            [Name("MatchedTime")] 
            public DateTime? MatchedTime { get; set; }
            [Name("DeleteUser")] 
            public string DeleteUser { get; set; }
            [Name("DeleteTime")] 
            public DateTime? DeleteTime { get; set; }
            [Name("OptimisticLockField")] 
            public int OptimisticLockField { get; set; }
            [Name("ReportType_ID")] 
            public string ReportTypeId { get; set; }
            [Name("PrimaryAccount_ID")] 
            public int PrimaryAccountId { get; set; }
            [Name("DOC_ID")] 
            public long? DocId { get; set; }
            [Name("gdoc_id")] 
            public long? GdocId { get; set; }
        }



        public class DataContainer
        {
            #region Properties

            public List<Reports> Reports { get; set; } = new List<Reports>();

            #endregion
        }

        #endregion


    }
}

