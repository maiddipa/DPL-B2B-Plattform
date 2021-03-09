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
    public class LtmsBookingFactory
    {
        #region Constructors

        public LtmsBookingFactory()
        {
            Data = new DataContainer();
        }

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
        public LtmsBookingFactory LoadCsvFile(string path)
        {
            try
            {
                using var reader = new StreamReader(path);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                csv.Configuration.TypeConverterCache.AddConverter<string>(new NullableStringConverter());
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.Delimiter = ";";

                var records = csv.GetRecords<BookingModel>();
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

        public static LtmsBookingFactory Instance(OlmaDbContext dbContext)
        {
            return new LtmsBookingFactory();
        }

        public static int? ToNullableInt(string s)
        {
            int i;

            if (int.TryParse(s, out i)) return i;

            return null;
        }

        public static DateTime? ToNullableDateTime(string s)
        {
            DateTime i;

            if (DateTime.TryParse(s, out i)) return i;

            return null;
        }

        public static bool ToBoolean(string s)
        {
            if (s == "0") return false;
            if (s == "1") return true;

            return bool.Parse(s);
        }

        #endregion

        #region Private Functions

        private LtmsBookingFactory AddNewRow(BookingModel record)
        {
            var process = Data.Processes.Find(p => p.Id == record.ProcessId);
            if (process == null)
            {
                process = new Processes()
                {
                    Id = record.ProcessId,
                    Name = record.ProcessName,
                    IntDescription = record.ProcessIntDescription,
                    ExtDescription = record.ProcessExtDescription,
                    ProcessStateId = record.ProcessStateId,
                    ProcessTypeId = record.ProcessTypeId,
                    ReferenceNumber = record.ProcessReferenceNumber,
                    OptimisticLockField = record.ProcessOptimisticLockField,
                    CreateUser = record.ProcessCreateUser,
                    CreateTime = record.ProcessCreateTime,
                    UpdateUser = record.ProcessUpdateUser,
                    UpdateTime = record.ProcessUpdateTime,
                    DeleteUser = record.ProcessDeleteUser,
                    DeleteTime = record.ProcessDeleteTime,
                    ChangedTime = record.ProcessChangedTime,
                };
                Data.Processes.Add(process);
            }

            var transaction = Data.Transactions.Find(t => t.Id == record.TransactionId);
            if (transaction == null)
            {
                transaction = new Transactions()
                {
                    Id = record.TransactionId,
                    ProcessId = record.TransactionProcessId,
                    RowGuid = record.TransactionRow,
                    IntDescription = record.TransactionIntDescription,
                    ExtDescription = record.TransactionExtDescription,
                    TransactionStateId = record.TransactionStateId,
                    TransactionTypeId = record.TransactionTypeId,
                    CancellationId = record.CancellationId,
                    Valuta = record.Valuta,
                    ReferenceNumber = record.TransactionReferenceNumber,
                    OptimisticLockField = record.TransactionOptimisticLockField,
                    CreateUser = record.TransactionCreateUser,
                    CreateTime = record.TransactionCreateTime,
                    UpdateUser = record.TransactionUpdateUser,
                    UpdateTime = record.TransactionUpdateTime,
                    DeleteUser = record.TransactionDeleteUser,
                    DeleteTime = record.TransactionDeleteTime,
                    ChangedTime = record.TransactionChangedTime,
                };
                Data.Transactions.Add(transaction);
            }

            var booking = Data.Bookings.Find(t => t.Id == record.BookingId);
            if (booking == null)
            {
                booking = new Bookings()
                {
                    Id = record.BookingId,
                    TransactionId = record.BookingTransactionId,
                    RowGuid = record.BookingRowGuid,
                    AccountId = record.AccountId,
                    BookingTypeId = record.BookingTypeId,
                    ArticleId = record.ArticleId,
                    QualityId = record.QualityId,
                    Quantity = record.Quantity,
                    ExtDescription = record.BookingExtDescription,
                    IncludeInBalance = record.IncludeInBalance,
                    Matched = record.Matched,
                    MatchedUser = record.MatchedUser,
                    MatchedTime = record.MatchedTime,
                    BookingDate = record.BookingDate,
                    AccountDirection = record.AccountDirection,
                    ReferenceNumber = record.BookingReferenceNumber,
                    OptimisticLockField = record.BookingOptimisticLockField,
                    CreateUser = record.BookingCreateUser,
                    CreateTime = record.BookingCreateTime,
                    UpdateUser = record.BookingUpdateUser,
                    UpdateTime = record.BookingUpdateTime,
                    DeleteUser = record.BookingDeleteUser,
                    DeleteTime = record.BookingDeleteTime,
                    ChangedTime = record.BookingChangedTime,
                    RowModified = record.BookingRowModified,
                    Computed = record.Computed,
                };
                Data.Bookings.Add(booking);
            }

            return this;
        }

        #endregion

        #region Nested

        #region Nested type: BookingModel

        public class BookingModel
        {
            #region Properties

            [Index(0)]
            public int ProcessId { get; set; }

            [Index(1)]
            [TypeConverter(typeof(NullableStringConverter))]
            public string ProcessReferenceNumber { get; set; }

            [Index(2)]
            public string ProcessName { get; set; }

            [Index(3)]
            public short ProcessStateId { get; set; }

            [Index(4)]
            public short ProcessTypeId { get; set; }

            [Index(5)]
            public string ProcessCreateUser { get; set; }

            [Index(6)]
            public DateTime ProcessCreateTime { get; set; }

            [Index(7)]
            public string ProcessUpdateUser { get; set; }

            [Index(8)]
            public DateTime? ProcessUpdateTime { get; set; }

            [Index(9)]
            public string ProcessDeleteUser { get; set; }

            [Index(10)]
            public DateTime? ProcessDeleteTime { get; set; }

            [Index(11)]
            public int ProcessOptimisticLockField { get; set; }

            [Index(12)]
            public string ProcessIntDescription { get; set; }

            [Index(13)]
            public string ProcessExtDescription { get; set; }

            [Index(14)]
            public DateTime? ProcessChangedTime { get; set; }

            [Index(15)]
            public int TransactionId { get; set; }

            [Index(16)]
            public Guid TransactionRow { get; set; }

            [Index(17)]
            public string TransactionTypeId { get; set; }

            [Index(18)]
            public string TransactionReferenceNumber { get; set; }

            [Index(19)]
            public DateTime Valuta { get; set; }

            [Index(20)]
            public string TransactionIntDescription { get; set; }

            [Index(21)]
            public string TransactionExtDescription { get; set; }

            [Index(22)]
            public int TransactionProcessId { get; set; }

            [Index(23)]
            public string TransactionCreateUser { get; set; }

            [Index(24)]
            public DateTime TransactionCreateTime { get; set; }

            [Index(25)]
            public string TransactionUpdateUser { get; set; }

            [Index(26)]
            public DateTime? TransactionUpdateTime { get; set; }

            [Index(27)]
            public short TransactionStateId { get; set; }

            [Index(28)]
            public int? CancellationId { get; set; }

            [Index(29)]
            public string TransactionDeleteUser { get; set; }

            [Index(30)]
            public DateTime? TransactionDeleteTime { get; set; }

            [Index(31)]
            public int TransactionOptimisticLockField { get; set; }

            [Index(32)]
            public DateTime? TransactionChangedTime { get; set; }

            [Index(33)]
            public int BookingId { get; set; }

            [Index(34)]
            public string BookingTypeId { get; set; }

            [Index(35)]
            public string BookingReferenceNumber { get; set; }

            [Index(36)]
            public DateTime BookingDate { get; set; }

            [Index(37)]
            public string BookingExtDescription { get; set; }

            [Index(38)]
            public int? Quantity { get; set; }

            [Index(39)]
            public short ArticleId { get; set; }

            [Index(40)]
            public short QualityId { get; set; }

            [Index(41)]
            public bool IncludeInBalance { get; set; }

            [Index(42)]
            public bool Matched { get; set; }

            [Index(43)]
            public string BookingCreateUser { get; set; }

            [Index(44)]
            public DateTime BookingCreateTime { get; set; }

            [Index(45)]
            public string BookingUpdateUser { get; set; }

            [Index(46)]
            public DateTime? BookingUpdateTime { get; set; }

            [Index(47)]
            public int BookingTransactionId { get; set; }

            [Index(48)]
            public int AccountId { get; set; }

            [Index(49)]
            public string MatchedUser { get; set; }

            [Index(50)]
            public DateTime? MatchedTime { get; set; }

            [Index(51)]
            public string AccountDirection { get; set; }

            [Index(52)]
            public string BookingDeleteUser { get; set; }

            [Index(53)]
            public DateTime? BookingDeleteTime { get; set; }

            [Index(54)]
            public int BookingOptimisticLockField { get; set; }

            [Index(55)]
            public DateTime? BookingChangedTime { get; set; }

            [Index(56)]
            public DateTime BookingRowModified { get; set; }

            [Index(57)]
            public bool Computed { get; set; }

            [Index(58)]
            public Guid BookingRowGuid { get; set; }

            #endregion
        }

        #endregion

        #region Nested type: DataContainer

        public class DataContainer
        {
            #region Properties

            public List<Processes> Processes { get; set; } = new List<Processes>();

            public List<Transactions> Transactions { get; set; } = new List<Transactions>();

            public List<Bookings> Bookings { get; set; } = new List<Bookings>();

            #endregion
        }

        #endregion

        #endregion
    }

    internal class NullableStringConverter : StringConverter
    {
        #region Public Functions

        #region Overrides of StringConverter

        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrEmpty(text))
                return null;
            else
                return base.ConvertFromString(text, row, memberMapData);
        }

        #endregion

        #endregion
    }
}