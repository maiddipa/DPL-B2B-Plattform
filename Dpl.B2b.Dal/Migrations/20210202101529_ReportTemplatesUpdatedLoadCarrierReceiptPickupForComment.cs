using Microsoft.EntityFrameworkCore.Migrations;

namespace Dpl.B2b.Dal.Migrations
{
    public partial class ReportTemplatesUpdatedLoadCarrierReceiptPickupForComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3929);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3930);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3931);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3932);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3933);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3934);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3935);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3936);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3937);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3938);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3939);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3940);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3941);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3942);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3943);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3944);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3945);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3946);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3947);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3948);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3949);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3950);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3951);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3952);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3953);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3954);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3955);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3956);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3957);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3958);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3959);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3960);

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 1,
                column: "Iso3Code",
                value: "DEU");

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 37,
                column: "Iso2Code",
                value: "CN");

            migrationBuilder.InsertData(
                table: "DocumentTemplateLabel",
                columns: new[] { "Id", "DocumentTemplateId", "Label", "LanguageId", "Text", "Version" },
                values: new object[,]
                {
                    { 7894, 3, "xrLabel23", 3, "Destinataire :", 0 },
                    { 7865, 3, "xrLabel23", 2, "Recipient:", 0 },
                    { 7867, 3, "xrLabel23", 1, "Empfänger: (Name des Fahrers)", 0 },
                    { 7889, 3, "xrLabel22", 4, "Podpis", 0 },
                    { 7888, 3, "xrLabel22", 3, "Signature", 0 },
                    { 7892, 3, "xrLabel22", 2, "Signature", 0 },
                    { 7877, 3, "tableCell5", 1, "Abholung für Firma:", 0 },
                    { 7890, 3, "xrLabel19", 4, "Wystawiajacy:", 0 },
                    { 7893, 3, "xrLabel19", 3, "Emetteur : ", 0 },
                    { 7891, 3, "xrLabel19", 2, "Issuer:", 0 },
                    { 7896, 3, "xrLabel19", 1, "Aussteller (Ausgabe-Quittung):", 0 },
                    { 7879, 3, "tableCell9", 1, "Aussteller-ID:", 0 },
                    { 7878, 3, "tableCell7", 1, "Firma auf abholendem LKW:", 0 },
                    { 7887, 3, "xrLabel23", 4, "Odbiorca:", 0 },
                    { 7895, 3, "xrLabel22", 1, "Unterschrift ([IssuerName])", 0 },
                    { 7875, 3, "tableCell30", 1, "Kommentar:", 0 },
                    { 7871, 3, "tableCell29", 1, "Qualität", 0 },
                    { 7870, 3, "tableCell26", 1, "Bestätigung:", 0 },
                    { 7872, 3, "tableCell24", 1, "Ladungsträger-Typ", 0 },
                    { 7873, 3, "tableCell23", 1, "Menge", 0 },
                    { 7874, 3, "tableCell21", 1, "Ausgegebene Ladungsträger:", 0 },
                    { 7885, 3, "tableCell2", 1, "Achtung Ausgabestelle!   Den quittierten Beleg unbedingt aufbewahren!", 0 },
                    { 7884, 3, "tableCell19", 1, "Beleg-Typ / -Einlösung:", 0 },
                    { 7883, 3, "tableCell17", 1, "DPL-Digitalcode:", 0 },
                    { 7882, 3, "tableCell15", 1, "Lieferschein-Nr.:", 0 },
                    { 7881, 3, "tableCell13", 1, "Abholschein-Nr.:", 0 },
                    { 7880, 3, "tableCell11", 1, "Sonstige-Nr.:", 0 },
                    { 7886, 3, "tableCell1", 1, "Hinweis:", 0 },
                    { 7869, 3, "label4", 1, "Kennzeichen", 0 },
                    { 7868, 3, "label3", 1, "Unterschrift ([TruckDriverName])", 0 },
                    { 7876, 3, "tableCell3", 1, "Kontakt zu DPL:", 0 },
                    { 7866, 3, "label1", 1, "Datum (Zeit)", 0 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 7865);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 7866);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 7867);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 7868);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 7869);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 7870);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 7871);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 7872);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 7873);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 7874);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 7875);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 7876);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 7877);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 7878);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 7879);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 7880);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 7881);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 7882);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 7883);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 7884);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 7885);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 7886);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 7887);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 7888);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 7889);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 7890);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 7891);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 7892);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 7893);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 7894);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 7895);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 7896);

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 1,
                column: "Iso3Code",
                value: "GER");

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 37,
                column: "Iso2Code",
                value: "CH");

            migrationBuilder.InsertData(
                table: "DocumentTemplateLabel",
                columns: new[] { "Id", "DocumentTemplateId", "Label", "LanguageId", "Text", "Version" },
                values: new object[,]
                {
                    { 3951, 3, "xrLabel23", 3, "Destinataire :", 0 },
                    { 3929, 3, "xrLabel23", 2, "Recipient:", 0 },
                    { 3931, 3, "xrLabel23", 1, "Empfänger: (Name des Fahrers)", 0 },
                    { 3956, 3, "xrLabel22", 4, "Podpis", 0 },
                    { 3957, 3, "xrLabel22", 3, "Signature", 0 },
                    { 3953, 3, "xrLabel22", 2, "Signature", 0 },
                    { 3941, 3, "tableCell5", 1, "Abholung für Firma:", 0 },
                    { 3955, 3, "xrLabel19", 4, "Wystawiajacy:", 0 },
                    { 3952, 3, "xrLabel19", 3, "Emetteur : ", 0 },
                    { 3954, 3, "xrLabel19", 2, "Issuer:", 0 },
                    { 3960, 3, "xrLabel19", 1, "Aussteller (Ausgabe-Quittung):", 0 },
                    { 3943, 3, "tableCell9", 1, "Aussteller-ID:", 0 },
                    { 3942, 3, "tableCell7", 1, "Firma auf abholendem LKW:", 0 },
                    { 3958, 3, "xrLabel23", 4, "Odbiorca:", 0 },
                    { 3959, 3, "xrLabel22", 1, "Unterschrift ([IssuerName])", 0 },
                    { 3939, 3, "tableCell4", 1, "[DPLAddressAndContact]", 0 },
                    { 3935, 3, "tableCell29", 1, "Qualität", 0 },
                    { 3934, 3, "tableCell26", 1, "Bestätigung:", 0 },
                    { 3936, 3, "tableCell24", 1, "Ladungsträger-Typ", 0 },
                    { 3937, 3, "tableCell23", 1, "Menge", 0 },
                    { 3938, 3, "tableCell21", 1, "Ausgegebene Ladungsträger:", 0 },
                    { 3949, 3, "tableCell2", 1, "Achtung Ausgabestelle!   Den quittierten Beleg unbedingt aufbewahren!", 0 },
                    { 3948, 3, "tableCell19", 1, "Beleg-Typ / -Einlösung:", 0 },
                    { 3947, 3, "tableCell17", 1, "DPL-Digitalcode:", 0 },
                    { 3946, 3, "tableCell15", 1, "Lieferschein-Nr.:", 0 },
                    { 3945, 3, "tableCell13", 1, "Abholschein-Nr.:", 0 },
                    { 3944, 3, "tableCell11", 1, "Sonstige-Nr.:", 0 },
                    { 3950, 3, "tableCell1", 1, "Hinweis:", 0 },
                    { 3933, 3, "label4", 1, "Kennzeichen", 0 },
                    { 3932, 3, "label3", 1, "Unterschrift ([TruckDriverName])", 0 },
                    { 3940, 3, "tableCell3", 1, "Kontakt zu DPL:", 0 },
                    { 3930, 3, "label1", 1, "Datum (Zeit)", 0 }
                });
        }
    }
}
