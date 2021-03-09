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
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using CsvHelper.TypeConversion;
using Dpl.B2b.Dal.Ltms;

namespace Dpl.B2b.Dal.LTMS
{
    /// <summary>
    /// Creates a process, transaction, and posting from a CSV file with strict notation (BookingModel)
    /// </summary>
    public class LtmsAccountFactory
    {
        #region Constructors

        public LtmsAccountFactory(OlmaDbContext dbContext)
        {
            this.dbContext = dbContext;
            Data = new DataContainer();
        }

        #endregion

        #region Fields

        private OlmaDbContext dbContext;

        #endregion

        #region Properties

        public DataContainer Data { get; }

        #endregion

        #region Public Functions

        /// <summary>
        /// Reihenfolge der Spalten(Typen) in der CSV Datei müssen übereinstimmen mit der BookingModel Klasse
        /// Für die erzeugung der CSV Datei steht die BookingSampleData.sql Datei zur Verfügung.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public LtmsAccountFactory LoadCsvFile(string path)
        {
            try
            {
                using var reader = new StreamReader(path);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                csv.Configuration.TypeConverterCache.AddConverter<string>(new NullableStringConverter());
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.Delimiter = ";";

                var records = csv.GetRecords<AccountModel>();
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

        public static LtmsAccountFactory Instance(OlmaDbContext dbContext)
        {
            return new LtmsAccountFactory(dbContext);
        }

        #endregion

        #region Private Functions

        private LtmsAccountFactory AddNewRow(AccountModel record)
        {
            //if (dbContext.Accounts.Find(record.ID) != null) //TODO: Discuss with AS: Is the DB Lookup necessary?
            //    return this;

            var process = Data.Accounts.Find(a => a.Id == record.ID);
            if (process == null)
            {
                process = new Accounts()
                {
                    Id = record.ID,
                    ParentId = record.Parent_ID,
                    RowGuid = record.RowGuid,
                    Name = record.Name,
                    FullName = record.FullName,
                    PathByName = record.PathByName,
                    PathName = record.PathName,
                    Description = record.Description,
                    AccountTypeId = record.AccountType_ID,
                    InvoiceAccountId = record.InvoiceAccount_ID,
                    AddressId = record.AddressId,
                    CustomerNumber = record.CustomerNumber,
                    Inactive = record.Inactive,
                    Locked = record.Locked,
                    //ResponsiblePersonId = record.ResponsiblePerson_ID, //Not Supported 
                    //KeyAccountManagerId = record.KeyAccountManager_ID, //Not Supported 
                    //SalespersonId = record.Salesperson_ID, //Not Supported 
                    OptimisticLockField = record.OptimisticLockField,
                    CreateUser = record.CreateUser,
                    CreateTime = record.CreateTime,
                    UpdateUser = record.UpdateUser,
                    UpdateTime = record.UpdateTime,
                    
                };
                Data.Accounts.Add(process);
            }

            return this;
        }

        #endregion

        #region Nested

        #region Nested type: AccountModel

        public class AccountModel
        {
            #region Properties

            [Name("ID")]
            public int ID { get; set; }

            [Name("RowGuid")]
            public Guid RowGuid { get; set; }

            [Name("Name")]
            public string Name { get; set; }

            [Name("Parent_ID")]
            public int? Parent_ID { get; set; }

            [Name("AccountType_ID")]
            public string AccountType_ID { get; set; }

            [Name("AccountNumber")]
            public string AccountNumber { get; set; }

            [Name("CustomerNumber")]
            public string CustomerNumber { get; set; }

            [Name("AddressId")]
            public int? AddressId { get; set; }

            [Name("Description")]
            public string Description { get; set; }

            [Name("PathByName")]
            public string PathByName { get; set; }

            [Name("OptimisticLockField")]
            public int OptimisticLockField { get; set; }

            [Name("CreateUser")]
            public string CreateUser { get; set; }

            [Name("CreateTime")]
            public DateTime CreateTime { get; set; }

            [Name("UpdateUser")]
            public string UpdateUser { get; set; }

            [Name("UpdateTime")]
            public DateTime? UpdateTime { get; set; }

            [Name("FullName")]
            public string FullName { get; set; }

            [Name("PathName")]
            public string PathName { get; set; }

            [Name("Inactive")]
            public bool Inactive { get; set; }

            [Name("Locked")]
            public bool Locked { get; set; }

            [Name("ResponsiblePerson_ID")]
            public string ResponsiblePerson_ID { get; set; }
            
            [Name("InvoiceAccount_ID")]
            public int? InvoiceAccount_ID { get; set; }
            
            [Name("KeyAccountManager_ID")]
            public string KeyAccountManager_ID { get; set; }
            
            [Name("Salesperson_ID")]
            public string Salesperson_ID { get; set; }
            
            #endregion
        }

        #endregion

        #region Nested type: DataContainer

        public class DataContainer
        {
            #region Properties

            public List<Accounts> Accounts { get; set; } = new List<Accounts>();

            #endregion
        }

        #endregion

        #endregion
    }
}


/**
 
2	31BBE88F-EA5E-11E3-87E7-180373C0EDE6	TK	1	GRP	NULL	NULL	NULL	Tauschkonten	$:TK	5	Anonym	2014-06-02 16:00:04.613	DPL-SOEST\Schulz	2017-05-19 16:44:15.007	TK	TK	0	0	NULL	NULL	NULL	NULL
4	31BBE891-EA5E-11E3-87E7-180373C0EDE6	DEP	1	GRP	NULL	NULL	NULL	Depot Konten	$:DEP	0	Anonym	2014-06-02 16:00:04.613	NULL	NULL	DEP	DEP	0	0	NULL	NULL	NULL	NULL
5	31BBE892-EA5E-11E3-87E7-180373C0EDE6	HZL	1	GRP	NULL	NULL	NULL	HZL Konten	$:HZL	0	Anonym	2014-06-02 16:00:04.613	NULL	NULL	HZL	HZL	0	0	NULL	NULL	NULL	NULL
6	31BBE893-EA5E-11E3-87E7-180373C0EDE6	PM	1	GRP	NULL	NULL	NULL	Poolingkonten	$:PM	0	Anonym	2014-06-02 16:00:04.613	NULL	NULL	PM	PM	0	0	NULL	NULL	NULL	NULL
9	31BBE896-EA5E-11E3-87E7-180373C0EDE6	LAG	1	GRP	NULL	NULL	NULL	Lager Konten	$:LAG	0	Anonym	2014-06-02 16:00:04.613	NULL	NULL	LAG	LAG	0	0	NULL	NULL	NULL	NULL
10	31BBE897-EA5E-11E3-87E7-180373C0EDE6	ERT	1	GRP	NULL	NULL	NULL	Ertrag	$:ERT	0	Anonym	2014-06-02 16:00:04.613	NULL	NULL	ERT	ERT	0	0	NULL	NULL	NULL	NULL
11	31BBE898-EA5E-11E3-87E7-180373C0EDE6	AWD	1	GRP	NULL	NULL	NULL	Aufwand	$:AWD	0	Anonym	2014-06-02 16:00:04.613	NULL	NULL	AWD	AWD	0	0	NULL	NULL	NULL	NULL
15	31BBE89C-EA5E-11E3-87E7-180373C0EDE6	QT	1	GRP	NULL	NULL	NULL	Qualitätentausch	$:QT	0	Anonym	2014-06-02 16:00:04.613	NULL	NULL	QT	QT	0	0	NULL	NULL	NULL	NULL
 */