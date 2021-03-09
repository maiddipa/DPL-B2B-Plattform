using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Dpl.B2b.Dal.Migrations
{
    public partial class ReportTemplatesUpdatedVoucherAndPickupV2 : Migration
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
                keyValue: 2169);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2170);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2171);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2172);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2173);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2174);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2175);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2176);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2177);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2178);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2179);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2180);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2181);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2182);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2183);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2184);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2185);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2186);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2187);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2188);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2189);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2190);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2191);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2192);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2193);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2194);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2195);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2196);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2197);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2198);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2199);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2200);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2201);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2202);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2203);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2204);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 2205);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3961);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3962);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3963);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3964);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3965);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3966);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3967);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3968);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3969);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3970);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3971);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3972);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3973);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3974);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3975);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3976);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3977);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3978);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3979);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3980);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3981);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3982);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3983);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3984);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3985);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3986);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3987);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3988);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3989);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3990);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3991);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3992);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3993);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3994);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3995);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3996);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3997);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3998);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3999);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4000);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4001);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4002);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4003);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4004);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4005);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4006);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4007);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4008);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4009);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4010);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4011);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4012);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4013);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4014);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4015);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4016);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4017);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4018);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4019);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4020);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4021);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4022);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4023);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4024);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4025);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4026);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4027);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4028);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4029);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4030);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4031);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4032);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4033);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4034);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4035);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4036);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4037);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4038);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4039);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4040);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4041);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4042);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4043);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4044);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4045);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4046);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 4047);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6133);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6134);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6135);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6136);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6137);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6138);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6139);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6140);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6141);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6142);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6143);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6144);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6145);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6146);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6147);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6148);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6149);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6150);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6151);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6152);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6153);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6154);

            migrationBuilder.UpdateData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3955,
                columns: new[] { "Label", "LanguageId", "Text" },
                values: new object[] { "xrLabel19", 4, "Wystawiajacy:" });

            migrationBuilder.UpdateData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3956,
                columns: new[] { "Label", "LanguageId", "Text" },
                values: new object[] { "xrLabel22", 4, "Podpis" });

            migrationBuilder.UpdateData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3957,
                columns: new[] { "Label", "LanguageId", "Text" },
                values: new object[] { "xrLabel22", 3, "Signature" });

            migrationBuilder.UpdateData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3958,
                columns: new[] { "Label", "LanguageId", "Text" },
                values: new object[] { "xrLabel23", 4, "Odbiorca:" });

            migrationBuilder.UpdateData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3959,
                columns: new[] { "Label", "Text" },
                values: new object[] { "xrLabel22", "Unterschrift ([IssuerName])" });

            migrationBuilder.UpdateData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3960,
                columns: new[] { "Label", "Text" },
                values: new object[] { "xrLabel19", "Aussteller (Ausgabe-Quittung):" });

            migrationBuilder.InsertData(
                table: "DocumentTemplateLabel",
                columns: new[] { "Id", "CustomerId", "DocumentTemplateId", "Label", "LanguageId", "Text", "Version" },
                values: new object[,]
                {
                    { 4158, null, 4, "label3", 1, "Unterschrift ([TruckDriverName])", 0 },
                    { 4159, null, 4, "label4", 1, "Kennzeichen", 0 },
                    { 4173, null, 4, "tableCell1", 1, "Hinweis:", 0 },
                    { 4161, null, 4, "tableCell12", 1, "Qualität", 0 },
                    { 4170, null, 4, "tableCell17", 1, "DPL-Digitalcode:", 0 },
                    { 4171, null, 4, "tableCell19", 1, "Beleg-Typ / -Einlösung:", 0 },
                    { 4156, null, 4, "label1", 1, "Datum (Zeit)", 0 },
                    { 4172, null, 4, "tableCell2", 1, "Achtung Annahmestelle!   Den quittierten Beleg unbedingt in Kopie aufbewahren!", 0 },
                    { 4163, null, 4, "tableCell23", 1, "Menge", 0 },
                    { 4162, null, 4, "tableCell24", 1, "Ladungsträger-Typ", 0 },
                    { 4160, null, 4, "tableCell26", 1, "Bestätigung:", 0 },
                    { 4166, null, 4, "tableCell3", 1, "Kontakt zu DPL:", 0 },
                    { 4165, null, 4, "tableCell4", 1, "[DPLAddressAndContact]", 0 },
                    { 4167, null, 4, "tableCell5", 1, "Anlieferung für Firma:", 0 },
                    { 4164, null, 4, "tableCell21", 1, "Angelieferte Ladungsträger:", 0 },
                    { 3949, null, 3, "tableCell2", 1, "Achtung Ausgabestelle!   Den quittierten Beleg unbedingt aufbewahren!", 0 },
                    { 3951, null, 3, "xrLabel23", 3, "Destinataire :", 0 },
                    { 3931, null, 3, "xrLabel23", 1, "Empfänger: (Name des Fahrers)", 0 },
                    { 3953, null, 3, "xrLabel22", 2, "Signature", 0 },
                    { 3952, null, 3, "xrLabel19", 3, "Emetteur : ", 0 },
                    { 3954, null, 3, "xrLabel19", 2, "Issuer:", 0 },
                    { 3943, null, 3, "tableCell9", 1, "Aussteller-ID:", 0 },
                    { 3942, null, 3, "tableCell7", 1, "Firma auf abholendem LKW:", 0 },
                    { 3941, null, 3, "tableCell5", 1, "Abholung für Firma:", 0 },
                    { 3939, null, 3, "tableCell4", 1, "[DPLAddressAndContact]", 0 },
                    { 3940, null, 3, "tableCell3", 1, "Kontakt zu DPL:", 0 },
                    { 3935, null, 3, "tableCell29", 1, "Qualität", 0 },
                    { 3934, null, 3, "tableCell26", 1, "Bestätigung:", 0 },
                    { 3936, null, 3, "tableCell24", 1, "Ladungsträger-Typ", 0 },
                    { 3937, null, 3, "tableCell23", 1, "Menge", 0 },
                    { 3938, null, 3, "tableCell21", 1, "Ausgegebene Ladungsträger:", 0 },
                    { 4169, null, 4, "tableCell7", 1, "Firma auf anlieferndem LKW:", 0 },
                    { 3929, null, 3, "xrLabel23", 2, "Recipient:", 0 },
                    { 4183, null, 4, "xrLabel19", 1, "Aussteller (Annahme-Quittung):", 0 },
                    { 3948, null, 3, "tableCell19", 1, "Beleg-Typ / -Einlösung:", 0 },
                    { 5149, null, 5, "tableCell20", 1, "", 0 },
                    { 5135, null, 5, "tableCell21", 1, "Zu transportierende Ladungsträger:", 0 },
                    { 5134, null, 5, "tableCell23", 1, "Menge", 0 },
                    { 5153, null, 5, "tableCell24", 1, "Ladungsträger-Typ", 0 },
                    { 5148, null, 5, "tableCell26", 1, "Jumbo LKW:", 0 },
                    { 5147, null, 5, "tableCell27", 1, "", 0 },
                    { 5150, null, 5, "tableCell19", 1, "Seitenbeladung:", 0 },
                    { 5141, null, 5, "tableCell28", 1, "Ladezeit:", 0 },
                    { 5138, null, 5, "tableCell30", 1, "Anlieferzeit:", 0 },
                    { 5146, null, 5, "tableCell32", 1, "Stapelhöhe:", 0 },
                    { 5136, null, 5, "tableCell4", 1, @"DPL Deutsche Paletten Logistik GmbH  -  Overweg 12  -  D-59494 Soest    (DPL)
                E-Mail: info@dpl-pooling.com / Tel. +49 2921 7899-0   / Fax. Tel. +49 2921 7899-178", 0 },
                    { 5140, null, 5, "tableCell5", 1, "Anlieferstelle:", 0 },
                    { 5143, null, 5, "tableCell7", 1, "Ladestelle:", 0 },
                    { 5144, null, 5, "tableCell9", 1, "Aussteller-ID:", 0 },
                    { 5137, null, 5, "tableCell3", 1, "Kontakt zu DPL:", 0 },
                    { 4182, null, 4, "tableCell9", 1, "Aussteller-ID:", 0 },
                    { 5145, null, 5, "tableCell17", 1, "DPL-Digitalcode:", 0 },
                    { 5152, null, 5, "tableCell15", 1, "Heckbeladung:", 0 },
                    { 4180, null, 4, "xrLabel19", 3, "Emetteur : ", 0 },
                    { 4177, null, 4, "xrLabel19", 4, "Wystawiajacy:", 0 },
                    { 4168, null, 4, "xrLabel22", 1, "Unterschrift ([IssuerName])", 0 },
                    { 4179, null, 4, "xrLabel22", 2, "Signature", 0 },
                    { 4175, null, 4, "xrLabel22", 3, "Signature", 0 },
                    { 4176, null, 4, "xrLabel22", 4, "Podpis", 0 },
                    { 5151, null, 5, "tableCell16", 1, "", 0 },
                    { 4157, null, 4, "xrLabel23", 1, "Empfänger (Annahme-Quittung):", 0 },
                    { 4181, null, 4, "xrLabel23", 3, "Destinataire :", 0 },
                    { 4174, null, 4, "xrLabel23", 4, "Odbiorca:", 0 },
                    { 5133, null, 5, "label5", 1, "Ladevorschriften:", 0 },
                    { 5142, null, 5, "tableCell1", 1, "", 0 },
                    { 5154, null, 5, "tableCell12", 1, "Qualität", 0 },
                    { 5139, null, 5, "tableCell13", 1, "", 0 },
                    { 4155, null, 4, "xrLabel23", 2, "Recipient:", 0 },
                    { 4178, null, 4, "xrLabel19", 2, "Issuer:", 0 },
                    { 3947, null, 3, "tableCell17", 1, "DPL-Digitalcode:", 0 },
                    { 3945, null, 3, "tableCell13", 1, "Abholschein-Nr.:", 0 },
                    { 1962, null, 1, "tableCell9", 1, "DPL-Digitalcode:", 0 },
                    { 1997, null, 1, "xrLabel19", 1, "Aussteller:", 0 },
                    { 1992, null, 1, "xrLabel19", 2, "Issuer:", 0 },
                    { 1961, null, 1, "xrLabel19", 3, "Emetteur : ", 0 },
                    { 1993, null, 1, "xrLabel19", 4, "Wystawiajacy:", 0 },
                    { 1978, null, 1, "xrLabel22", 1, "Unterschrift ([IssuerName])", 0 },
                    { 1982, null, 1, "xrLabel22", 2, "Signature", 0 },
                    { 1990, null, 1, "xrLabel22", 3, "Signature", 0 },
                    { 1988, null, 1, "xrLabel22", 4, "Podpis", 0 },
                    { 1975, null, 1, "xrLabel23", 1, "Empfänger: (Name des Fahrers)", 0 },
                    { 1994, null, 1, "xrLabel23", 2, "Recipient:", 0 },
                    { 1986, null, 1, "xrLabel23", 3, "Destinataire :", 0 },
                    { 1985, null, 1, "xrLabel23", 4, "Odbiorca:", 0 },
                    { 1980, null, 1, "xrLabel7", 1, "DPL-Pooling-Gutschrift-Nr.", 0 },
                    { 1991, null, 1, "xrLabel7", 2, "Palettenannahme-Beleg TODO", 0 },
                    { 1963, null, 1, "tableCell7", 1, "Aussteller:", 0 },
                    { 1984, null, 1, "xrLabel7", 3, "Palettenannahme-Quittung FR TODO", 0 },
                    { 1964, null, 1, "tableCell5", 1, "Empfänger:", 0 },
                    { 1965, null, 1, "tableCell3", 1, "Kontakt zu DPL:", 0 },
                    { 1987, null, 1, "label1", 2, "DPL-Code:", 0 },
                    { 1981, null, 1, "label1", 3, "DPL-Code:", 0 },
                    { 1983, null, 1, "label1", 4, "DPL-Code:", 0 },
                    { 1974, null, 1, "label2", 1, "Kennzeichen", 0 },
                    { 1973, null, 1, "label3", 1, "Unterschrift ([TruckDriver])", 0 },
                    { 1995, null, 1, "label4", 1, "[Number]", 0 },
                    { 1996, null, 1, "tableCell1", 1, "Beleg-Typ / -Einlösung:", 0 },
                    { 1979, null, 1, "tableCell11", 1, "Hinweis:", 0 },
                    { 1977, null, 1, "tableCell12", 1, "Dieser Beleg ist nicht an Dritte übertragbar.", 0 },
                    { 1967, null, 1, "tableCell13", 1, "Nicht getauschte Ladungsträger:", 0 },
                    { 1968, null, 1, "tableCell15", 1, "Menge", 0 },
                    { 1969, null, 1, "tableCell16", 1, "Ladungsträger-Typ", 0 },
                    { 1970, null, 1, "tableCell18", 1, "Grund für die Belegerstellung:", 0 },
                    { 1971, null, 1, "tableCell19", 1, "", 0 },
                    { 1972, null, 1, "tableCell21", 1, "Beleg-Einlösung / Ladungsträger-Ausgabe:", 0 },
                    { 1966, null, 1, "tableCell4", 1, "[DPLAddressAndContact]", 0 },
                    { 3946, null, 3, "tableCell15", 1, "Lieferschein-Nr.:", 0 },
                    { 1989, null, 1, "xrLabel7", 4, "Palettenannahme-Quittung PL TODO", 0 },
                    { 2019, null, 2, "label3", 1, "Unterschrift ([TruckDriverName])", 0 },
                    { 2039, null, 2, "xrLabel19", 3, "Emetteur : ", 0 },
                    { 2042, null, 2, "xrLabel19", 4, "Wystawiajacy:", 0 },
                    { 2046, null, 2, "xrLabel22", 1, "Unterschrift ([IssuerName])", 0 },
                    { 2040, null, 2, "xrLabel22", 2, "Signature", 0 },
                    { 2044, null, 2, "xrLabel22", 3, "Signature", 0 },
                    { 2043, null, 2, "xrLabel22", 4, "Podpis", 0 },
                    { 2018, null, 2, "xrLabel23", 1, "Empfänger:", 0 },
                    { 2016, null, 2, "xrLabel23", 2, "Recipient:", 0 },
                    { 2038, null, 2, "xrLabel23", 3, "Destinataire :", 0 },
                    { 2045, null, 2, "xrLabel23", 4, "Odbiorca:", 0 },
                    { 3930, null, 3, "label1", 1, "Datum (Zeit)", 0 },
                    { 3932, null, 3, "label3", 1, "Unterschrift ([TruckDriverName])", 0 },
                    { 3933, null, 3, "label4", 1, "Kennzeichen", 0 },
                    { 3950, null, 3, "tableCell1", 1, "Hinweis:", 0 },
                    { 3944, null, 3, "tableCell11", 1, "Sonstige-Nr.:", 0 },
                    { 2041, null, 2, "xrLabel19", 2, "Issuer:", 0 },
                    { 2017, null, 2, "label1", 1, "Datum", 0 },
                    { 2047, null, 2, "xrLabel19", 1, "Aussteller:", 0 },
                    { 2029, null, 2, "tableCell7", 1, "Firma auf LKW:", 0 },
                    { 2020, null, 2, "label4", 1, "Kennzeichen", 0 },
                    { 2037, null, 2, "tableCell1", 1, "Hinweis:", 0 },
                    { 2031, null, 2, "tableCell11", 1, "Sonstige-Nr.:", 0 },
                    { 2032, null, 2, "tableCell13", 1, "Abholschein-Nr.:", 0 },
                    { 2033, null, 2, "tableCell15", 1, "Lieferschein-Nr.:", 0 },
                    { 2034, null, 2, "tableCell17", 1, "DPL-Digitalcode:", 0 },
                    { 2035, null, 2, "tableCell19", 1, "Beleg-Typ / -Einlösung:", 0 },
                    { 2030, null, 2, "tableCell9", 1, "Aussteller-ID:", 0 },
                    { 2036, null, 2, "tableCell2", 1, "Achtung Annahme-/Ausgabestelle!   Den quittierten Beleg unbedingt aufbewahren!", 0 },
                    { 2024, null, 2, "tableCell23", 1, "Menge", 0 },
                    { 2023, null, 2, "tableCell24", 1, "Ladungsträger-Typ", 0 },
                    { 2021, null, 2, "tableCell26", 1, "Bestätigung:", 0 },
                    { 2022, null, 2, "tableCell29", 1, "Qualität", 0 },
                    { 2027, null, 2, "tableCell3", 1, "Kontakt zu DPL:", 0 },
                    { 2026, null, 2, "tableCell4", 1, @"DPL Deutsche Paletten Logistik GmbH  -  Overweg 12  -  D-59494 Soest    (DPL)
                E-Mail: info@dpl-pooling.com / Tel. +49 2921 7899-0   / Fax. Tel. +49 2921 7899-178", 0 },
                    { 2028, null, 2, "tableCell5", 1, "Transport für Firma:", 0 },
                    { 2025, null, 2, "tableCell21", 1, "Angenommene und ausgegebene Ladungsträger:", 0 },
                    { 1976, null, 1, "label1", 1, "Datum (Zeit)", 0 }
                });

            migrationBuilder.InsertData(
                table: "LoadCarriers",
                columns: new[] { "Id", "Name", "Order", "QualityId", "RefLmsQuality2PalletId", "RefLtmsPalletId", "TypeId" },
                values: new object[] { 306, "GB EW", 5f, 14, 6029, (short)306, 13 });
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
                keyValue: 1961);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 1962);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 1963);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 1964);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 1965);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 1966);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 1967);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 1968);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 1969);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 1970);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 1971);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 1972);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 1973);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 1974);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 1975);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 1976);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 1977);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 1978);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 1979);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 1980);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 1981);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 1982);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 1983);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 1984);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 1985);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 1986);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 1987);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 1988);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 1989);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 1990);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 1991);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 1992);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 1993);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 1994);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 1995);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 1996);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 1997);

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
                table: "LoadCarriers",
                keyColumn: "Id",
                keyValue: 306);

            migrationBuilder.UpdateData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3955,
                columns: new[] { "Label", "LanguageId", "Text" },
                values: new object[] { "xrLabel23", 2, "Recipient:" });

            migrationBuilder.UpdateData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3956,
                columns: new[] { "Label", "LanguageId", "Text" },
                values: new object[] { "label1", 1, "Datum" });

            migrationBuilder.UpdateData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3957,
                columns: new[] { "Label", "LanguageId", "Text" },
                values: new object[] { "xrLabel23", 1, "Empfänger:" });

            migrationBuilder.UpdateData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3958,
                columns: new[] { "Label", "LanguageId", "Text" },
                values: new object[] { "label3", 1, "Unterschrift ([TruckDriverName])" });

            migrationBuilder.UpdateData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3959,
                columns: new[] { "Label", "Text" },
                values: new object[] { "label4", "Kennzeichen" });

            migrationBuilder.UpdateData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 3960,
                columns: new[] { "Label", "Text" },
                values: new object[] { "tableCell26", "Bestätigung:" });

            migrationBuilder.InsertData(
                table: "DocumentTemplateLabel",
                columns: new[] { "Id", "CustomerId", "DocumentTemplateId", "Label", "LanguageId", "Text", "Version" },
                values: new object[,]
                {
                    { 3962, null, 3, "tableCell24", 1, "Ladungsträger-Typ", 0 },
                    { 3999, null, 4, "tableCell5", 1, "Anlieferung für Firma:", 0 },
                    { 3997, null, 4, "tableCell4", 1, @"DPL Deutsche Paletten Logistik GmbH  -  Overweg 12  -  D-59494 Soest    (DPL)
                E-Mail: info@dpl-pooling.com / Tel. +49 2921 7899-0   / Fax. Tel. +49 2921 7899-178", 0 },
                    { 3998, null, 4, "tableCell3", 1, "Kontakt zu DPL:", 0 },
                    { 3992, null, 4, "tableCell26", 1, "Bestätigung:", 0 },
                    { 3994, null, 4, "tableCell24", 1, "Ladungsträger-Typ", 0 },
                    { 3995, null, 4, "tableCell23", 1, "Menge", 0 },
                    { 3996, null, 4, "tableCell21", 1, "Angelieferte Ladungsträger:", 0 },
                    { 4004, null, 4, "tableCell2", 1, "Achtung Annahmestelle!   Den quittierten Beleg unbedingt in Kopie aufbewahren!", 0 },
                    { 4003, null, 4, "tableCell19", 1, "Beleg-Typ / -Einlösung:", 0 },
                    { 4002, null, 4, "tableCell17", 1, "DPL-Digitalcode:", 0 },
                    { 3993, null, 4, "tableCell12", 1, "Qualität", 0 },
                    { 4005, null, 4, "tableCell1", 1, "Hinweis:", 0 },
                    { 3991, null, 4, "label4", 1, "Kennzeichen", 0 },
                    { 3990, null, 4, "label3", 1, "Unterschrift ([TruckDriverName])", 0 },
                    { 3988, null, 4, "label1", 1, "Datum", 0 },
                    { 3984, null, 3, "xrLabel23", 4, "Odbiorca:", 0 },
                    { 4001, null, 4, "tableCell7", 1, "Firma auf anlieferndem LKW:", 0 },
                    { 3961, null, 3, "tableCell29", 1, "Qualität", 0 },
                    { 3966, null, 3, "tableCell3", 1, "Kontakt zu DPL:", 0 },
                    { 3965, null, 3, "tableCell4", 1, @"DPL Deutsche Paletten Logistik GmbH  -  Overweg 12  -  D-59494 Soest    (DPL)
                E-Mail: info@dpl-pooling.com / Tel. +49 2921 7899-0   / Fax. Tel. +49 2921 7899-178", 0 },
                    { 3967, null, 3, "tableCell5", 1, "Abholung für Firma:", 0 },
                    { 3968, null, 3, "tableCell7", 1, "Firma auf abholendem LKW:", 0 },
                    { 3969, null, 3, "tableCell9", 1, "Aussteller-ID:", 0 },
                    { 3963, null, 3, "tableCell23", 1, "Menge", 0 },
                    { 3986, null, 3, "xrLabel19", 1, "Aussteller (Ausgabe-Quittung):", 0 },
                    { 3978, null, 3, "xrLabel19", 3, "Emetteur : ", 0 },
                    { 3981, null, 3, "xrLabel19", 4, "Wystawiajacy:", 0 },
                    { 3985, null, 3, "xrLabel22", 1, "Unterschrift ([IssuerName])", 0 },
                    { 3979, null, 3, "xrLabel22", 2, "Signature", 0 },
                    { 3983, null, 3, "xrLabel22", 3, "Signature", 0 },
                    { 3982, null, 3, "xrLabel22", 4, "Podpis", 0 },
                    { 3980, null, 3, "xrLabel19", 2, "Issuer:", 0 },
                    { 3977, null, 3, "xrLabel23", 3, "Destinataire :", 0 },
                    { 4014, null, 4, "tableCell9", 1, "Aussteller-ID:", 0 },
                    { 3964, null, 3, "tableCell21", 1, "Ausgegebene Ladungsträger:", 0 },
                    { 6149, null, 5, "tableCell20", 1, "", 0 },
                    { 6135, null, 5, "tableCell21", 1, "Zu transportierende Ladungsträger:", 0 },
                    { 6134, null, 5, "tableCell23", 1, "Menge", 0 },
                    { 6153, null, 5, "tableCell24", 1, "Ladungsträger-Typ", 0 },
                    { 6148, null, 5, "tableCell26", 1, "Jumbo LKW:", 0 },
                    { 6147, null, 5, "tableCell27", 1, "", 0 },
                    { 6150, null, 5, "tableCell19", 1, "Seitenbeladung:", 0 },
                    { 6141, null, 5, "tableCell28", 1, "Ladezeit:", 0 },
                    { 6138, null, 5, "tableCell30", 1, "Anlieferzeit:", 0 },
                    { 6146, null, 5, "tableCell32", 1, "Stapelhöhe:", 0 },
                    { 6136, null, 5, "tableCell4", 1, @"DPL Deutsche Paletten Logistik GmbH  -  Overweg 12  -  D-59494 Soest    (DPL)
                E-Mail: info@dpl-pooling.com / Tel. +49 2921 7899-0   / Fax. Tel. +49 2921 7899-178", 0 },
                    { 6140, null, 5, "tableCell5", 1, "Anlieferstelle:", 0 },
                    { 6143, null, 5, "tableCell7", 1, "Ladestelle:", 0 },
                    { 6144, null, 5, "tableCell9", 1, "Aussteller-ID:", 0 },
                    { 6137, null, 5, "tableCell3", 1, "Kontakt zu DPL:", 0 },
                    { 4015, null, 4, "xrLabel19", 1, "Aussteller (Annahme-Quittung):", 0 },
                    { 6145, null, 5, "tableCell17", 1, "DPL-Digitalcode:", 0 },
                    { 6152, null, 5, "tableCell15", 1, "Heckbeladung:", 0 },
                    { 4007, null, 4, "xrLabel19", 3, "Emetteur : ", 0 },
                    { 4010, null, 4, "xrLabel19", 4, "Wystawiajacy:", 0 },
                    { 4000, null, 4, "xrLabel22", 1, "Unterschrift ([IssuerName])", 0 },
                    { 4008, null, 4, "xrLabel22", 2, "Signature", 0 },
                    { 4012, null, 4, "xrLabel22", 3, "Signature", 0 },
                    { 4011, null, 4, "xrLabel22", 4, "Podpis", 0 },
                    { 6151, null, 5, "tableCell16", 1, "", 0 },
                    { 3989, null, 4, "xrLabel23", 1, "Empfänger (Annahme-Quittung):", 0 },
                    { 4006, null, 4, "xrLabel23", 3, "Destinataire :", 0 },
                    { 4013, null, 4, "xrLabel23", 4, "Odbiorca:", 0 },
                    { 6133, null, 5, "label5", 1, "Ladevorschriften:", 0 },
                    { 6142, null, 5, "tableCell1", 1, "", 0 },
                    { 6154, null, 5, "tableCell12", 1, "Qualität", 0 },
                    { 6139, null, 5, "tableCell13", 1, "", 0 },
                    { 3987, null, 4, "xrLabel23", 2, "Recipient:", 0 },
                    { 4009, null, 4, "xrLabel19", 2, "Issuer:", 0 },
                    { 3975, null, 3, "tableCell2", 1, "Achtung Ausgabestelle!   Den quittierten Beleg unbedingt aufbewahren!", 0 },
                    { 3973, null, 3, "tableCell17", 1, "DPL-Digitalcode:", 0 },
                    { 2170, null, 1, "tableCell9", 1, "DPL-Digitalcode:", 0 },
                    { 2205, null, 1, "xrLabel19", 1, "Aussteller:", 0 },
                    { 2200, null, 1, "xrLabel19", 2, "Issuer:", 0 },
                    { 2169, null, 1, "xrLabel19", 3, "Emetteur : ", 0 },
                    { 2201, null, 1, "xrLabel19", 4, "Wystawiajacy:", 0 },
                    { 2186, null, 1, "xrLabel22", 1, "Unterschrift ([IssuerName])", 0 },
                    { 2190, null, 1, "xrLabel22", 2, "Signature", 0 },
                    { 2198, null, 1, "xrLabel22", 3, "Signature", 0 },
                    { 2196, null, 1, "xrLabel22", 4, "Podpis", 0 },
                    { 2183, null, 1, "xrLabel23", 1, "Empfänger:", 0 },
                    { 2202, null, 1, "xrLabel23", 2, "Recipient:", 0 },
                    { 2194, null, 1, "xrLabel23", 3, "Destinataire :", 0 },
                    { 2193, null, 1, "xrLabel23", 4, "Odbiorca:", 0 },
                    { 2188, null, 1, "xrLabel7", 1, "DPL-Pooling-Gutschrift-Nr.", 0 },
                    { 2199, null, 1, "xrLabel7", 2, "Palettenannahme-Beleg TODO", 0 },
                    { 2171, null, 1, "tableCell7", 1, "Aussteller:", 0 },
                    { 2192, null, 1, "xrLabel7", 3, "Palettenannahme-Quittung FR TODO", 0 },
                    { 2172, null, 1, "tableCell5", 1, "Empfänger:", 0 },
                    { 2173, null, 1, "tableCell3", 1, "Kontakt zu DPL:", 0 },
                    { 2195, null, 1, "label1", 2, "DPL-Code:", 0 },
                    { 2189, null, 1, "label1", 3, "DPL-Code:", 0 },
                    { 2191, null, 1, "label1", 4, "DPL-Code:", 0 },
                    { 2182, null, 1, "label2", 1, "Kennzeichen", 0 },
                    { 2181, null, 1, "label3", 1, "Unterschrift ([TruckDriver])", 0 },
                    { 2203, null, 1, "label4", 1, "[Number]", 0 },
                    { 2204, null, 1, "tableCell1", 1, "Beleg-Typ / -Einlösung:", 0 },
                    { 2187, null, 1, "tableCell11", 1, "Hinweis:", 0 },
                    { 2185, null, 1, "tableCell12", 1, "Dieser Beleg ist nicht an Dritte übertragbar.", 0 },
                    { 2175, null, 1, "tableCell13", 1, "Nicht getauschte Ladungsträger:", 0 },
                    { 2176, null, 1, "tableCell15", 1, "Menge", 0 },
                    { 2177, null, 1, "tableCell16", 1, "Ladungsträger-Typ", 0 },
                    { 2178, null, 1, "tableCell18", 1, "Grund für die Belegerstellung:", 0 },
                    { 2179, null, 1, "tableCell19", 1, "", 0 },
                    { 2180, null, 1, "tableCell21", 1, "Beleg-Einlösung / Ladungsträger-Ausgabe:", 0 },
                    { 2174, null, 1, "tableCell4", 1, @"DPL Deutsche Paletten Logistik GmbH  -  Overweg 12  -  D-59494 Soest    (DPL)
                E-Mail: info@dpl-pooling.com / Tel. +49 2921 7899-0   / Fax. Tel. +49 2921 7899-178", 0 },
                    { 3974, null, 3, "tableCell19", 1, "Beleg-Typ / -Einlösung:", 0 },
                    { 2197, null, 1, "xrLabel7", 4, "Palettenannahme-Quittung PL TODO", 0 },
                    { 4019, null, 2, "label3", 1, "Unterschrift ([TruckDriverName])", 0 },
                    { 4041, null, 2, "xrLabel19", 2, "Issuer:", 0 },
                    { 4039, null, 2, "xrLabel19", 3, "Emetteur : ", 0 },
                    { 4042, null, 2, "xrLabel19", 4, "Wystawiajacy:", 0 },
                    { 4046, null, 2, "xrLabel22", 1, "Unterschrift ([IssuerName])", 0 },
                    { 4040, null, 2, "xrLabel22", 2, "Signature", 0 },
                    { 4044, null, 2, "xrLabel22", 3, "Signature", 0 },
                    { 4043, null, 2, "xrLabel22", 4, "Podpis", 0 },
                    { 4018, null, 2, "xrLabel23", 1, "Empfänger:", 0 },
                    { 4016, null, 2, "xrLabel23", 2, "Recipient:", 0 },
                    { 4038, null, 2, "xrLabel23", 3, "Destinataire :", 0 },
                    { 4045, null, 2, "xrLabel23", 4, "Odbiorca:", 0 },
                    { 3976, null, 3, "tableCell1", 1, "Hinweis:", 0 },
                    { 3970, null, 3, "tableCell11", 1, "Sonstige-Nr.:", 0 },
                    { 3971, null, 3, "tableCell13", 1, "Abholschein-Nr.:", 0 },
                    { 3972, null, 3, "tableCell15", 1, "Lieferschein-Nr.:", 0 },
                    { 4047, null, 2, "xrLabel19", 1, "Aussteller:", 0 },
                    { 4017, null, 2, "label1", 1, "Datum", 0 },
                    { 4030, null, 2, "tableCell9", 1, "Aussteller-ID:", 0 },
                    { 4028, null, 2, "tableCell5", 1, "Transport für Firma:", 0 },
                    { 4020, null, 2, "label4", 1, "Kennzeichen", 0 },
                    { 4037, null, 2, "tableCell1", 1, "Hinweis:", 0 },
                    { 4031, null, 2, "tableCell11", 1, "Sonstige-Nr.:", 0 },
                    { 4032, null, 2, "tableCell13", 1, "Abholschein-Nr.:", 0 },
                    { 4033, null, 2, "tableCell15", 1, "Lieferschein-Nr.:", 0 },
                    { 4034, null, 2, "tableCell17", 1, "DPL-Digitalcode:", 0 },
                    { 4035, null, 2, "tableCell19", 1, "Beleg-Typ / -Einlösung:", 0 },
                    { 4036, null, 2, "tableCell2", 1, "Achtung Annahme-/Ausgabestelle!   Den quittierten Beleg unbedingt aufbewahren!", 0 },
                    { 4025, null, 2, "tableCell21", 1, "Angenommene und ausgegebene Ladungsträger:", 0 },
                    { 4024, null, 2, "tableCell23", 1, "Menge", 0 },
                    { 4023, null, 2, "tableCell24", 1, "Ladungsträger-Typ", 0 },
                    { 4021, null, 2, "tableCell26", 1, "Bestätigung:", 0 },
                    { 4022, null, 2, "tableCell29", 1, "Qualität", 0 },
                    { 4027, null, 2, "tableCell3", 1, "Kontakt zu DPL:", 0 },
                    { 4026, null, 2, "tableCell4", 1, @"DPL Deutsche Paletten Logistik GmbH  -  Overweg 12  -  D-59494 Soest    (DPL)
                E-Mail: info@dpl-pooling.com / Tel. +49 2921 7899-0   / Fax. Tel. +49 2921 7899-178", 0 },
                    { 4029, null, 2, "tableCell7", 1, "Firma auf LKW:", 0 },
                    { 2184, null, 1, "label1", 1, "Datum", 0 }
                });
        }
    }
}
