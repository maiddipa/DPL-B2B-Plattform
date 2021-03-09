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
    /// Creates a process, transaction, and posting from a CSV file with strict notation (BookingModel)
    /// </summary>
    public class LtmsReportBookingFactory
    {
        #region Constructors

        public LtmsReportBookingFactory()
        {
            Data = new DataContainer();
        }

        #endregion

        #region Properties

        public DataContainer Data { get; }

        #endregion

        #region Public Functions

        /// <summary>
        /// Reihenfolge der Spalten(Typen) in der CSV Datei müssen übereinstimmen mit der ReportBookingModel Klasse
        /// Für die erzeugung der CSV Datei steht die ReportBookingSampleData.sql Datei zur Verfügung.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public LtmsReportBookingFactory LoadCsvFile(string path)
        {
            try
            {
                using var reader = new StreamReader(path);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                csv.Configuration.TypeConverterCache.AddConverter<string>(new NullableStringConverter());
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.Delimiter = ";";

                var records = csv.GetRecords<ReportBookingModel>();
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

        public static LtmsReportBookingFactory Instance()
        {
            return new LtmsReportBookingFactory();
        }

        #endregion

        #region Private Functions

        private LtmsReportBookingFactory AddNewRow(ReportBookingModel record)
        {
            var report = Data.ReportBookings.Find(a => a.ReportId == record.Report_ID && a.BookingId == record.Booking_ID);
            if (report != null) return this;
            report = new ReportBookings()
            {
                ReportId = record.Report_ID,
                BookingId = record.Booking_ID,
            };
            Data.ReportBookings.Add(report);

            return this;
        }

        #endregion

        public class ReportBookingModel
        {
            [Name("Report_ID")] 
            public int Report_ID { get; set; }
            [Name("Booking_ID")] 
            public int Booking_ID { get; set; }
        }

        #region Nested type: DataContainer

        public class DataContainer
        {
            public List<ReportBookings> ReportBookings { get; set; } = new List<ReportBookings>();
        }

        #endregion
    }
}

