using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Dpl.B2b.Dal.Migrations
{
    public partial class ReportTemplatesDevExpressUpdateFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
         var basePath = System.IO.Path.GetDirectoryName(typeof(OlmaDbContext).Assembly.Location);

         Func<string, byte[]> getTemplate =
             (path) => System.IO.File.ReadAllBytes(System.IO.Path.Combine(basePath, path));

         migrationBuilder.UpdateData("DocumentTemplates", "Id", 1, "Data", getTemplate("Seed\\report-template-VoucherCommon.xml"));
         migrationBuilder.UpdateData("DocumentTemplates", "Id", 2, "Data", getTemplate("Seed\\report-template-LoadCarrierReceiptExchange.xml"));
         migrationBuilder.UpdateData("DocumentTemplates", "Id", 3, "Data", getTemplate("Seed\\report-template-LoadCarrierReceiptPickup.xml"));
         migrationBuilder.UpdateData("DocumentTemplates", "Id", 4, "Data", getTemplate("Seed\\report-template-LoadCarrierReceiptDelivery.xml"));
         migrationBuilder.UpdateData("DocumentTemplates", "Id", 5, "Data", getTemplate("Seed\\report-template-TransportVoucher.xml"));

         migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2016);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2017);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2018);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2019);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2020);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2021);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2022);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2023);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2024);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2025);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2026);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2027);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2028);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2029);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2030);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2031);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2032);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2033);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2034);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2035);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2036);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2037);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2038);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2039);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2040);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2041);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2042);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2043);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2044);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2045);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2046);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2047);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4155);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4156);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4157);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4158);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4159);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4160);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4161);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4162);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4163);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4164);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4165);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4166);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4167);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4168);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4169);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4170);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4171);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4172);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4173);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4174);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4175);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4176);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4177);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4178);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4179);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4180);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4181);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4182);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4183);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 5133);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 5134);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 5135);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 5136);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 5137);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 5138);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 5139);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 5140);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 5141);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 5142);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 5143);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 5144);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 5145);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 5146);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 5147);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 5148);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 5149);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 5150);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 5151);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 5152);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 5153);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 5154);

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

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 8897);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 8898);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 8899);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 8900);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 8901);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 8902);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 8903);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 8904);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 8905);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 8906);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 8907);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 8908);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 8909);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 8910);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 8911);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 8912);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 8913);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 8914);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 8915);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 8916);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 8917);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 8918);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 8919);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 8920);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 8921);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 8922);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 8923);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 8924);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 8925);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 8926);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 8927);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 8928);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 8929);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 8930);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 8931);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 8932);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 8933);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 8934);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 8935);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 8936);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 8937);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 8938);

            migrationBuilder.InsertData(
                table: "DocumentTemplateLabel",
                columns: new[] { "Id", "DocumentTemplateId", "Label", "LanguageId", "Text", "Version" },
                values: new object[,]
                {
                    { 10040, 1, "label1", 1, "Datum (Zeit)", 0 },
                    { 12126, 3, "xrLabel22", 3, "Signature", 0 },
                    { 12125, 3, "xrLabel22", 4, "Podpis", 0 },
                    { 12100, 3, "xrLabel23", 1, "Empfänger: (Name des Fahrers)", 0 },
                    { 12098, 3, "xrLabel23", 2, "Recipient:", 0 },
                    { 12120, 3, "xrLabel23", 3, "Destinataire :", 0 },
                    { 12127, 3, "xrLabel23", 4, "Odbiorca:", 0 },
                    { 13131, 4, "label1", 1, "Datum (Zeit)", 0 },
                    { 13133, 4, "label3", 1, "Unterschrift ([TruckDriverName])", 0 },
                    { 13134, 4, "label4", 1, "Kennzeichen", 0 },
                    { 13148, 4, "tableCell1", 1, "Hinweis:", 0 },
                    { 13136, 4, "tableCell12", 1, "Qualität", 0 },
                    { 13145, 4, "tableCell17", 1, "DPL-Digitalcode:", 0 },
                    { 13146, 4, "tableCell19", 1, "Beleg-Typ / -Einlösung:", 0 },
                    { 13147, 4, "tableCell2", 1, "Achtung Annahmestelle!   Den quittierten Beleg unbedingt in Kopie aufbewahren!", 0 },
                    { 13139, 4, "tableCell21", 1, "Angelieferte Ladungsträger:", 0 },
                    { 12122, 3, "xrLabel22", 2, "Signature", 0 },
                    { 12128, 3, "xrLabel22", 1, "Unterschrift ([IssuerName])", 0 },
                    { 12124, 3, "xrLabel19", 4, "Wystawiajacy:", 0 },
                    { 12121, 3, "xrLabel19", 3, "Emetteur : ", 0 },
                    { 12115, 3, "tableCell15", 1, "Lieferschein-Nr.:", 0 },
                    { 12116, 3, "tableCell17", 1, "DPL-Digitalcode:", 0 },
                    { 12117, 3, "tableCell19", 1, "Beleg-Typ / -Einlösung:", 0 },
                    { 12118, 3, "tableCell2", 1, "Achtung Ausgabestelle!   Den quittierten Beleg unbedingt aufbewahren!", 0 },
                    { 12107, 3, "tableCell21", 1, "Ausgegebene Ladungsträger:", 0 },
                    { 12106, 3, "tableCell23", 1, "Menge", 0 },
                    { 12105, 3, "tableCell24", 1, "Ladungsträger-Typ", 0 },
                    { 13138, 4, "tableCell23", 1, "Menge", 0 },
                    { 12103, 3, "tableCell26", 1, "Bestätigung:", 0 },
                    { 12109, 3, "tableCell3", 1, "Kontakt zu DPL:", 0 },
                    { 12108, 3, "tableCell30", 1, "Kommentar:", 0 },
                    { 12110, 3, "tableCell5", 1, "Abholung für Firma:", 0 },
                    { 12111, 3, "tableCell7", 1, "Firma auf abholendem LKW:", 0 },
                    { 12112, 3, "tableCell9", 1, "Aussteller-ID:", 0 },
                    { 12129, 3, "xrLabel19", 1, "Aussteller (Ausgabe-Quittung):", 0 },
                    { 12123, 3, "xrLabel19", 2, "Issuer:", 0 },
                    { 12104, 3, "tableCell29", 1, "Qualität", 0 },
                    { 13137, 4, "tableCell24", 1, "Ladungsträger-Typ", 0 },
                    { 13135, 4, "tableCell26", 1, "Bestätigung:", 0 },
                    { 13141, 4, "tableCell3", 1, "Kontakt zu DPL:", 0 },
                    { 14178, 5, "tableCell15", 1, "Heckbeladung:", 0 },
                    { 14177, 5, "tableCell16", 1, "", 0 },
                    { 14171, 5, "tableCell17", 1, "DPL-Digitalcode:", 0 },
                    { 14176, 5, "tableCell19", 1, "Seitenbeladung:", 0 },
                    { 14175, 5, "tableCell20", 1, "", 0 },
                    { 14161, 5, "tableCell21", 1, "Zu transportierende Ladungsträger:", 0 },
                    { 14160, 5, "tableCell23", 1, "Menge", 0 },
                    { 14165, 5, "tableCell13", 1, "", 0 },
                    { 14179, 5, "tableCell24", 1, "Ladungsträger-Typ", 0 },
                    { 14173, 5, "tableCell27", 1, "", 0 },
                    { 14167, 5, "tableCell28", 1, "Ladezeit:", 0 },
                    { 14163, 5, "tableCell3", 1, "Kontakt zu DPL:", 0 },
                    { 14164, 5, "tableCell30", 1, "Anlieferzeit:", 0 },
                    { 14172, 5, "tableCell32", 1, "Stapelhöhe:", 0 },
                    { 14162, 5, "tableCell4", 1, "[DPLAddressAndContact]", 0 },
                    { 14166, 5, "tableCell5", 1, "Anlieferstelle:", 0 },
                    { 14174, 5, "tableCell26", 1, "Jumbo LKW:", 0 },
                    { 12114, 3, "tableCell13", 1, "Abholschein-Nr.:", 0 },
                    { 14180, 5, "tableCell12", 1, "Qualität", 0 },
                    { 14159, 5, "label5", 1, "Ladevorschriften:", 0 },
                    { 13140, 4, "tableCell4", 1, "[DPLAddressAndContact]", 0 },
                    { 13142, 4, "tableCell5", 1, "Anlieferung für Firma:", 0 },
                    { 13144, 4, "tableCell7", 1, "Firma auf anlieferndem LKW:", 0 },
                    { 13157, 4, "tableCell9", 1, "Aussteller-ID:", 0 },
                    { 13158, 4, "xrLabel19", 1, "Aussteller (Annahme-Quittung):", 0 },
                    { 13152, 4, "xrLabel19", 2, "Issuer:", 0 },
                    { 13150, 4, "xrLabel19", 3, "Emetteur : ", 0 },
                    { 14168, 5, "tableCell1", 1, "", 0 },
                    { 13153, 4, "xrLabel19", 4, "Wystawiajacy:", 0 },
                    { 13151, 4, "xrLabel22", 2, "Signature", 0 },
                    { 13155, 4, "xrLabel22", 3, "Signature", 0 },
                    { 13154, 4, "xrLabel22", 4, "Podpis", 0 },
                    { 13132, 4, "xrLabel23", 1, "Empfänger (Annahme-Quittung):", 0 },
                    { 13130, 4, "xrLabel23", 2, "Recipient:", 0 },
                    { 13149, 4, "xrLabel23", 3, "Destinataire :", 0 },
                    { 13156, 4, "xrLabel23", 4, "Odbiorca:", 0 },
                    { 13143, 4, "xrLabel22", 1, "Unterschrift ([IssuerName])", 0 },
                    { 14169, 5, "tableCell7", 1, "Ladestelle:", 0 },
                    { 12113, 3, "tableCell11", 1, "Sonstige-Nr.:", 0 },
                    { 12102, 3, "label4", 1, "Kennzeichen", 0 },
                    { 10027, 1, "tableCell3", 1, "Kontakt zu DPL:", 0 },
                    { 10028, 1, "tableCell4", 1, "[DPLAddressAndContact]", 0 },
                    { 10041, 1, "tableCell5", 1, "Empfänger:", 0 },
                    { 10042, 1, "tableCell7", 1, "Aussteller:", 0 },
                    { 10043, 1, "tableCell9", 1, "DPL-Digitalcode:", 0 },
                    { 10064, 1, "xrLabel19", 1, "Aussteller:", 0 },
                    { 10058, 1, "xrLabel19", 2, "Issuer:", 0 },
                    { 10023, 1, "xrLabel19", 3, "Emetteur : ", 0 },
                    { 10063, 1, "xrLabel19", 4, "Wystawiajacy:", 0 },
                    { 10053, 1, "xrLabel22", 1, "Unterschrift ([IssuerName])", 0 },
                    { 10046, 1, "xrLabel22", 2, "Signature", 0 },
                    { 10056, 1, "xrLabel22", 3, "Signature", 0 },
                    { 10052, 1, "xrLabel22", 4, "Podpis", 0 },
                    { 10039, 1, "xrLabel23", 1, "Empfänger: (Name des Fahrers)", 0 },
                    { 10055, 1, "xrLabel23", 2, "Recipient:", 0 },
                    { 10030, 1, "tableCell28", 1, "[Comment]", 0 },
                    { 10029, 1, "tableCell27", 1, "Kommentar:", 0 },
                    { 10025, 1, "tableCell26", 1, "Beschaffungslogistik:", 0 },
                    { 10026, 1, "tableCell24", 1, "", 0 },
                    { 10051, 1, "label1", 2, "DPL-Code:", 0 },
                    { 10045, 1, "label1", 3, "DPL-Code:", 0 },
                    { 10047, 1, "label1", 4, "DPL-Code:", 0 },
                    { 10038, 1, "label2", 1, "Kennzeichen", 0 },
                    { 10037, 1, "label3", 1, "Unterschrift ([TruckDriver])", 0 },
                    { 10059, 1, "label4", 1, "[Number]", 0 },
                    { 10061, 1, "tableCell1", 1, "Beleg-Typ / -Einlösung:", 0 },
                    { 10050, 1, "xrLabel23", 3, "Destinataire :", 0 },
                    { 10062, 1, "tableCell11", 1, "Hinweis:", 0 },
                    { 10031, 1, "tableCell13", 1, "Nicht getauschte Ladungsträger:", 0 },
                    { 10032, 1, "tableCell15", 1, "Menge", 0 },
                    { 10033, 1, "tableCell16", 1, "Ladungsträger-Typ", 0 },
                    { 10034, 1, "tableCell18", 1, "Grund für die Belegerstellung:", 0 },
                    { 10035, 1, "tableCell19", 1, "", 0 },
                    { 10036, 1, "tableCell21", 1, "Beleg-Einlösung / Ladungsträger-Ausgabe:", 0 },
                    { 10024, 1, "tableCell23", 1, "Referenznummer (Ausst.):", 0 },
                    { 10044, 1, "tableCell12", 1, "Dieser Beleg ist nicht an Dritte übertragbar.", 0 },
                    { 10049, 1, "xrLabel23", 4, "Odbiorca:", 0 },
                    { 10060, 1, "xrLabel7", 1, "DPL-Pooling-Gutschrift-Nr.", 0 },
                    { 10057, 1, "xrLabel7", 2, "Palettenannahme-Beleg TODO", 0 },
                    { 11079, 2, "tableCell7", 1, "Firma auf LKW:", 0 },
                    { 11080, 2, "tableCell9", 1, "Aussteller-ID:", 0 },
                    { 11097, 2, "xrLabel19", 1, "Aussteller:", 0 },
                    { 11092, 2, "xrLabel19", 2, "Issuer:", 0 },
                    { 11094, 2, "xrLabel19", 3, "Emetteur : ", 0 },
                    { 11091, 2, "xrLabel19", 4, "Wystawiajacy:", 0 },
                    { 11096, 2, "xrLabel22", 1, "Unterschrift ([IssuerName])", 0 },
                    { 11078, 2, "tableCell5", 1, "Transport für Firma:", 0 },
                    { 11093, 2, "xrLabel22", 2, "Signature", 0 },
                    { 11090, 2, "xrLabel22", 4, "Podpis", 0 },
                    { 11068, 2, "xrLabel23", 1, "Empfänger:", 0 },
                    { 11066, 2, "xrLabel23", 2, "Recipient:", 0 },
                    { 11095, 2, "xrLabel23", 3, "Destinataire :", 0 },
                    { 11088, 2, "xrLabel23", 4, "Odbiorca:", 0 },
                    { 12099, 3, "label1", 1, "Datum (Zeit)", 0 },
                    { 12101, 3, "label3", 1, "Unterschrift ([TruckDriverName])", 0 },
                    { 11089, 2, "xrLabel22", 3, "Signature", 0 },
                    { 12119, 3, "tableCell1", 1, "Hinweis:", 0 },
                    { 11076, 2, "tableCell4", 1, "[DPLAddressAndContact]", 0 },
                    { 11072, 2, "tableCell29", 1, "Qualität", 0 },
                    { 10048, 1, "xrLabel7", 3, "Palettenannahme-Quittung FR TODO", 0 },
                    { 10054, 1, "xrLabel7", 4, "Palettenannahme-Quittung PL TODO", 0 },
                    { 11067, 2, "label1", 1, "Datum", 0 },
                    { 11069, 2, "label3", 1, "Unterschrift ([TruckDriverName])", 0 },
                    { 11070, 2, "label4", 1, "Kennzeichen", 0 },
                    { 11087, 2, "tableCell1", 1, "Hinweis:", 0 },
                    { 11081, 2, "tableCell11", 1, "Sonstige-Nr.:", 0 },
                    { 11077, 2, "tableCell3", 1, "Kontakt zu DPL:", 0 },
                    { 11082, 2, "tableCell13", 1, "Abholschein-Nr.:", 0 },
                    { 11084, 2, "tableCell17", 1, "DPL-Digitalcode:", 0 },
                    { 11085, 2, "tableCell19", 1, "Beleg-Typ / -Einlösung:", 0 },
                    { 11086, 2, "tableCell2", 1, "Achtung Annahme-/Ausgabestelle!   Den quittierten Beleg unbedingt aufbewahren!", 0 },
                    { 11075, 2, "tableCell21", 1, "Angenommene und ausgegebene Ladungsträger:", 0 },
                    { 11074, 2, "tableCell23", 1, "Menge", 0 },
                    { 11073, 2, "tableCell24", 1, "Ladungsträger-Typ", 0 },
                    { 11071, 2, "tableCell26", 1, "Bestätigung:", 0 },
                    { 11083, 2, "tableCell15", 1, "Lieferschein-Nr.:", 0 },
                    { 14170, 5, "tableCell9", 1, "Aussteller-ID:", 0 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
         migrationBuilder.UpdateData("DocumentTemplates", "Id", 1, "Data", null);
         migrationBuilder.UpdateData("DocumentTemplates", "Id", 2, "Data", null);
         migrationBuilder.UpdateData("DocumentTemplates", "Id", 3, "Data", null);
         migrationBuilder.UpdateData("DocumentTemplates", "Id", 4, "Data", null);
         migrationBuilder.UpdateData("DocumentTemplates", "Id", 5, "Data", null);

         migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 10023);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 10024);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 10025);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 10026);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 10027);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 10028);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 10029);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 10030);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 10031);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 10032);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 10033);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 10034);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 10035);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 10036);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 10037);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 10038);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 10039);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 10040);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 10041);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 10042);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 10043);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 10044);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 10045);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 10046);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 10047);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 10048);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 10049);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 10050);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 10051);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 10052);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 10053);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 10054);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 10055);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 10056);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 10057);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 10058);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 10059);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 10060);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 10061);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 10062);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 10063);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 10064);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 11066);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 11067);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 11068);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 11069);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 11070);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 11071);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 11072);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 11073);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 11074);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 11075);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 11076);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 11077);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 11078);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 11079);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 11080);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 11081);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 11082);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 11083);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 11084);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 11085);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 11086);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 11087);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 11088);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 11089);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 11090);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 11091);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 11092);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 11093);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 11094);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 11095);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 11096);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 11097);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 12098);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 12099);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 12100);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 12101);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 12102);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 12103);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 12104);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 12105);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 12106);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 12107);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 12108);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 12109);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 12110);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 12111);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 12112);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 12113);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 12114);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 12115);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 12116);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 12117);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 12118);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 12119);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 12120);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 12121);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 12122);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 12123);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 12124);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 12125);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 12126);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 12127);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 12128);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 12129);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 13130);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 13131);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 13132);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 13133);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 13134);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 13135);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 13136);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 13137);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 13138);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 13139);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 13140);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 13141);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 13142);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 13143);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 13144);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 13145);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 13146);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 13147);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 13148);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 13149);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 13150);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 13151);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 13152);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 13153);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 13154);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 13155);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 13156);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 13157);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 13158);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 14159);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 14160);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 14161);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 14162);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 14163);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 14164);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 14165);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 14166);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 14167);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 14168);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 14169);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 14170);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 14171);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 14172);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 14173);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 14174);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 14175);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 14176);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 14177);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 14178);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 14179);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 14180);

            migrationBuilder.InsertData(
                table: "DocumentTemplateLabel",
                columns: new[] { "Id", "DocumentTemplateId", "Label", "LanguageId", "Text", "Version" },
                values: new object[,]
                {
                    { 8914, 1, "label1", 1, "Datum (Zeit)", 0 },
                    { 7888, 3, "xrLabel22", 3, "Signature", 0 },
                    { 7889, 3, "xrLabel22", 4, "Podpis", 0 },
                    { 7867, 3, "xrLabel23", 1, "Empfänger: (Name des Fahrers)", 0 },
                    { 7865, 3, "xrLabel23", 2, "Recipient:", 0 },
                    { 7894, 3, "xrLabel23", 3, "Destinataire :", 0 },
                    { 7887, 3, "xrLabel23", 4, "Odbiorca:", 0 },
                    { 4156, 4, "label1", 1, "Datum (Zeit)", 0 },
                    { 4158, 4, "label3", 1, "Unterschrift ([TruckDriverName])", 0 },
                    { 4159, 4, "label4", 1, "Kennzeichen", 0 },
                    { 4173, 4, "tableCell1", 1, "Hinweis:", 0 },
                    { 4161, 4, "tableCell12", 1, "Qualität", 0 },
                    { 4170, 4, "tableCell17", 1, "DPL-Digitalcode:", 0 },
                    { 4171, 4, "tableCell19", 1, "Beleg-Typ / -Einlösung:", 0 },
                    { 4172, 4, "tableCell2", 1, "Achtung Annahmestelle!   Den quittierten Beleg unbedingt in Kopie aufbewahren!", 0 },
                    { 4164, 4, "tableCell21", 1, "Angelieferte Ladungsträger:", 0 },
                    { 7892, 3, "xrLabel22", 2, "Signature", 0 },
                    { 7895, 3, "xrLabel22", 1, "Unterschrift ([IssuerName])", 0 },
                    { 7890, 3, "xrLabel19", 4, "Wystawiajacy:", 0 },
                    { 7893, 3, "xrLabel19", 3, "Emetteur : ", 0 },
                    { 7882, 3, "tableCell15", 1, "Lieferschein-Nr.:", 0 },
                    { 7883, 3, "tableCell17", 1, "DPL-Digitalcode:", 0 },
                    { 7884, 3, "tableCell19", 1, "Beleg-Typ / -Einlösung:", 0 },
                    { 7885, 3, "tableCell2", 1, "Achtung Ausgabestelle!   Den quittierten Beleg unbedingt aufbewahren!", 0 },
                    { 7874, 3, "tableCell21", 1, "Ausgegebene Ladungsträger:", 0 },
                    { 7873, 3, "tableCell23", 1, "Menge", 0 },
                    { 7872, 3, "tableCell24", 1, "Ladungsträger-Typ", 0 },
                    { 4163, 4, "tableCell23", 1, "Menge", 0 },
                    { 7870, 3, "tableCell26", 1, "Bestätigung:", 0 },
                    { 7876, 3, "tableCell3", 1, "Kontakt zu DPL:", 0 },
                    { 7875, 3, "tableCell30", 1, "Kommentar:", 0 },
                    { 7877, 3, "tableCell5", 1, "Abholung für Firma:", 0 },
                    { 7878, 3, "tableCell7", 1, "Firma auf abholendem LKW:", 0 },
                    { 7879, 3, "tableCell9", 1, "Aussteller-ID:", 0 },
                    { 7896, 3, "xrLabel19", 1, "Aussteller (Ausgabe-Quittung):", 0 },
                    { 7891, 3, "xrLabel19", 2, "Issuer:", 0 },
                    { 7871, 3, "tableCell29", 1, "Qualität", 0 },
                    { 4162, 4, "tableCell24", 1, "Ladungsträger-Typ", 0 },
                    { 4160, 4, "tableCell26", 1, "Bestätigung:", 0 },
                    { 4166, 4, "tableCell3", 1, "Kontakt zu DPL:", 0 },
                    { 5152, 5, "tableCell15", 1, "Heckbeladung:", 0 },
                    { 5151, 5, "tableCell16", 1, "", 0 },
                    { 5145, 5, "tableCell17", 1, "DPL-Digitalcode:", 0 },
                    { 5150, 5, "tableCell19", 1, "Seitenbeladung:", 0 },
                    { 5149, 5, "tableCell20", 1, "", 0 },
                    { 5135, 5, "tableCell21", 1, "Zu transportierende Ladungsträger:", 0 },
                    { 5134, 5, "tableCell23", 1, "Menge", 0 },
                    { 5139, 5, "tableCell13", 1, "", 0 },
                    { 5153, 5, "tableCell24", 1, "Ladungsträger-Typ", 0 },
                    { 5147, 5, "tableCell27", 1, "", 0 },
                    { 5141, 5, "tableCell28", 1, "Ladezeit:", 0 },
                    { 5137, 5, "tableCell3", 1, "Kontakt zu DPL:", 0 },
                    { 5138, 5, "tableCell30", 1, "Anlieferzeit:", 0 },
                    { 5146, 5, "tableCell32", 1, "Stapelhöhe:", 0 },
                    { 5136, 5, "tableCell4", 1, @"DPL Deutsche Paletten Logistik GmbH  -  Overweg 12  -  D-59494 Soest    (DPL)
                E-Mail: info@dpl-pooling.com / Tel. +49 2921 7899-0   / Fax. Tel. +49 2921 7899-178", 0 },
                    { 5140, 5, "tableCell5", 1, "Anlieferstelle:", 0 },
                    { 5148, 5, "tableCell26", 1, "Jumbo LKW:", 0 },
                    { 7881, 3, "tableCell13", 1, "Abholschein-Nr.:", 0 },
                    { 5154, 5, "tableCell12", 1, "Qualität", 0 },
                    { 5133, 5, "label5", 1, "Ladevorschriften:", 0 },
                    { 4165, 4, "tableCell4", 1, "[DPLAddressAndContact]", 0 },
                    { 4167, 4, "tableCell5", 1, "Anlieferung für Firma:", 0 },
                    { 4169, 4, "tableCell7", 1, "Firma auf anlieferndem LKW:", 0 },
                    { 4182, 4, "tableCell9", 1, "Aussteller-ID:", 0 },
                    { 4183, 4, "xrLabel19", 1, "Aussteller (Annahme-Quittung):", 0 },
                    { 4178, 4, "xrLabel19", 2, "Issuer:", 0 },
                    { 4180, 4, "xrLabel19", 3, "Emetteur : ", 0 },
                    { 5142, 5, "tableCell1", 1, "", 0 },
                    { 4177, 4, "xrLabel19", 4, "Wystawiajacy:", 0 },
                    { 4179, 4, "xrLabel22", 2, "Signature", 0 },
                    { 4175, 4, "xrLabel22", 3, "Signature", 0 },
                    { 4176, 4, "xrLabel22", 4, "Podpis", 0 },
                    { 4157, 4, "xrLabel23", 1, "Empfänger (Annahme-Quittung):", 0 },
                    { 4155, 4, "xrLabel23", 2, "Recipient:", 0 },
                    { 4181, 4, "xrLabel23", 3, "Destinataire :", 0 },
                    { 4174, 4, "xrLabel23", 4, "Odbiorca:", 0 },
                    { 4168, 4, "xrLabel22", 1, "Unterschrift ([IssuerName])", 0 },
                    { 5143, 5, "tableCell7", 1, "Ladestelle:", 0 },
                    { 7880, 3, "tableCell11", 1, "Sonstige-Nr.:", 0 },
                    { 7869, 3, "label4", 1, "Kennzeichen", 0 },
                    { 8901, 1, "tableCell3", 1, "Kontakt zu DPL:", 0 },
                    { 8902, 1, "tableCell4", 1, "[DPLAddressAndContact]", 0 },
                    { 8915, 1, "tableCell5", 1, "Empfänger:", 0 },
                    { 8916, 1, "tableCell7", 1, "Aussteller:", 0 },
                    { 8917, 1, "tableCell9", 1, "DPL-Digitalcode:", 0 },
                    { 8938, 1, "xrLabel19", 1, "Aussteller:", 0 },
                    { 8930, 1, "xrLabel19", 2, "Issuer:", 0 },
                    { 8897, 1, "xrLabel19", 3, "Emetteur : ", 0 },
                    { 8931, 1, "xrLabel19", 4, "Wystawiajacy:", 0 },
                    { 8927, 1, "xrLabel22", 1, "Unterschrift ([IssuerName])", 0 },
                    { 8920, 1, "xrLabel22", 2, "Signature", 0 },
                    { 8937, 1, "xrLabel22", 3, "Signature", 0 },
                    { 8926, 1, "xrLabel22", 4, "Podpis", 0 },
                    { 8913, 1, "xrLabel23", 1, "Empfänger: (Name des Fahrers)", 0 },
                    { 8932, 1, "xrLabel23", 2, "Recipient:", 0 },
                    { 8904, 1, "tableCell28", 1, "[Comment]", 0 },
                    { 8903, 1, "tableCell27", 1, "Kommentar:", 0 },
                    { 8899, 1, "tableCell26", 1, "Beschaffungslogistik:", 0 },
                    { 8900, 1, "tableCell24", 1, "", 0 },
                    { 8925, 1, "label1", 2, "DPL-Code:", 0 },
                    { 8919, 1, "label1", 3, "DPL-Code:", 0 },
                    { 8921, 1, "label1", 4, "DPL-Code:", 0 },
                    { 8912, 1, "label2", 1, "Kennzeichen", 0 },
                    { 8911, 1, "label3", 1, "Unterschrift ([TruckDriver])", 0 },
                    { 8933, 1, "label4", 1, "[Number]", 0 },
                    { 8935, 1, "tableCell1", 1, "Beleg-Typ / -Einlösung:", 0 },
                    { 8924, 1, "xrLabel23", 3, "Destinataire :", 0 },
                    { 8936, 1, "tableCell11", 1, "Hinweis:", 0 },
                    { 8905, 1, "tableCell13", 1, "Nicht getauschte Ladungsträger:", 0 },
                    { 8906, 1, "tableCell15", 1, "Menge", 0 },
                    { 8907, 1, "tableCell16", 1, "Ladungsträger-Typ", 0 },
                    { 8908, 1, "tableCell18", 1, "Grund für die Belegerstellung:", 0 },
                    { 8909, 1, "tableCell19", 1, "", 0 },
                    { 8910, 1, "tableCell21", 1, "Beleg-Einlösung / Ladungsträger-Ausgabe:", 0 },
                    { 8898, 1, "tableCell23", 1, "Referenznummer (Ausst.):", 0 },
                    { 8918, 1, "tableCell12", 1, "Dieser Beleg ist nicht an Dritte übertragbar.", 0 },
                    { 8923, 1, "xrLabel23", 4, "Odbiorca:", 0 },
                    { 8934, 1, "xrLabel7", 1, "DPL-Pooling-Gutschrift-Nr.", 0 },
                    { 8929, 1, "xrLabel7", 2, "Palettenannahme-Beleg TODO", 0 },
                    { 2029, 2, "tableCell7", 1, "Firma auf LKW:", 0 },
                    { 2030, 2, "tableCell9", 1, "Aussteller-ID:", 0 },
                    { 2047, 2, "xrLabel19", 1, "Aussteller:", 0 },
                    { 2041, 2, "xrLabel19", 2, "Issuer:", 0 },
                    { 2039, 2, "xrLabel19", 3, "Emetteur : ", 0 },
                    { 2042, 2, "xrLabel19", 4, "Wystawiajacy:", 0 },
                    { 2046, 2, "xrLabel22", 1, "Unterschrift ([IssuerName])", 0 },
                    { 2028, 2, "tableCell5", 1, "Transport für Firma:", 0 },
                    { 2040, 2, "xrLabel22", 2, "Signature", 0 },
                    { 2043, 2, "xrLabel22", 4, "Podpis", 0 },
                    { 2018, 2, "xrLabel23", 1, "Empfänger:", 0 },
                    { 2016, 2, "xrLabel23", 2, "Recipient:", 0 },
                    { 2038, 2, "xrLabel23", 3, "Destinataire :", 0 },
                    { 2045, 2, "xrLabel23", 4, "Odbiorca:", 0 },
                    { 7866, 3, "label1", 1, "Datum (Zeit)", 0 },
                    { 7868, 3, "label3", 1, "Unterschrift ([TruckDriverName])", 0 },
                    { 2044, 2, "xrLabel22", 3, "Signature", 0 },
                    { 7886, 3, "tableCell1", 1, "Hinweis:", 0 },
                    { 2026, 2, "tableCell4", 1, @"DPL Deutsche Paletten Logistik GmbH  -  Overweg 12  -  D-59494 Soest    (DPL)
                E-Mail: info@dpl-pooling.com / Tel. +49 2921 7899-0   / Fax. Tel. +49 2921 7899-178", 0 },
                    { 2022, 2, "tableCell29", 1, "Qualität", 0 },
                    { 8922, 1, "xrLabel7", 3, "Palettenannahme-Quittung FR TODO", 0 },
                    { 8928, 1, "xrLabel7", 4, "Palettenannahme-Quittung PL TODO", 0 },
                    { 2017, 2, "label1", 1, "Datum", 0 },
                    { 2019, 2, "label3", 1, "Unterschrift ([TruckDriverName])", 0 },
                    { 2020, 2, "label4", 1, "Kennzeichen", 0 },
                    { 2037, 2, "tableCell1", 1, "Hinweis:", 0 },
                    { 2031, 2, "tableCell11", 1, "Sonstige-Nr.:", 0 },
                    { 2027, 2, "tableCell3", 1, "Kontakt zu DPL:", 0 },
                    { 2032, 2, "tableCell13", 1, "Abholschein-Nr.:", 0 },
                    { 2034, 2, "tableCell17", 1, "DPL-Digitalcode:", 0 },
                    { 2035, 2, "tableCell19", 1, "Beleg-Typ / -Einlösung:", 0 },
                    { 2036, 2, "tableCell2", 1, "Achtung Annahme-/Ausgabestelle!   Den quittierten Beleg unbedingt aufbewahren!", 0 },
                    { 2025, 2, "tableCell21", 1, "Angenommene und ausgegebene Ladungsträger:", 0 },
                    { 2024, 2, "tableCell23", 1, "Menge", 0 },
                    { 2023, 2, "tableCell24", 1, "Ladungsträger-Typ", 0 },
                    { 2021, 2, "tableCell26", 1, "Bestätigung:", 0 },
                    { 2033, 2, "tableCell15", 1, "Lieferschein-Nr.:", 0 },
                    { 5144, 5, "tableCell9", 1, "Aussteller-ID:", 0 }
                });
        }
    }
}
