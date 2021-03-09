using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Dpl.B2b.Dal.Migrations
{
    public partial class ReportTemplatesUpdatedVoucherCommonForComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
         var basePath = System.IO.Path.GetDirectoryName(typeof(OlmaDbContext).Assembly.Location);

         Func<string, byte[]> getTemplate =
             (path) => System.IO.File.ReadAllBytes(System.IO.Path.Combine(basePath, path));

         migrationBuilder.UpdateData("DocumentTemplates", "Id", 1, "Data", getTemplate("Seed\\report-template-VoucherCommon.xml"));

         migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6760);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6761);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6762);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6763);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6764);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6765);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6766);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6767);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6768);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6769);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6770);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6771);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6772);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6773);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6774);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6775);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6776);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6777);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6778);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6779);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6780);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6781);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6782);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6783);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6784);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6785);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6786);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6787);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6788);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6789);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6790);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6791);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6792);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6793);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6794);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6795);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6796);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6797);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6798);

            migrationBuilder.DeleteData(
                table: "DocumentTemplateLabel",
                keyColumn: "Id",
                keyValue: 6799);

            migrationBuilder.InsertData(
                table: "DocumentTemplateLabel",
                columns: new[] { "Id", "DocumentTemplateId", "Label", "LanguageId", "Text", "Version" },
                values: new object[,]
                {
                    { 8914, 1, "label1", 1, "Datum (Zeit)", 0 },
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
                    { 8924, 1, "xrLabel23", 3, "Destinataire :", 0 },
                    { 8923, 1, "xrLabel23", 4, "Odbiorca:", 0 },
                    { 8934, 1, "xrLabel7", 1, "DPL-Pooling-Gutschrift-Nr.", 0 },
                    { 8929, 1, "xrLabel7", 2, "Palettenannahme-Beleg TODO", 0 },
                    { 8902, 1, "tableCell4", 1, "[DPLAddressAndContact]", 0 },
                    { 8901, 1, "tableCell3", 1, "Kontakt zu DPL:", 0 },
                    { 8904, 1, "tableCell28", 1, "[Comment]", 0 },
                    { 8903, 1, "tableCell27", 1, "Kommentar:", 0 },
                    { 8925, 1, "label1", 2, "DPL-Code:", 0 },
                    { 8919, 1, "label1", 3, "DPL-Code:", 0 },
                    { 8921, 1, "label1", 4, "DPL-Code:", 0 },
                    { 8912, 1, "label2", 1, "Kennzeichen", 0 },
                    { 8911, 1, "label3", 1, "Unterschrift ([TruckDriver])", 0 },
                    { 8933, 1, "label4", 1, "[Number]", 0 },
                    { 8935, 1, "tableCell1", 1, "Beleg-Typ / -Einlösung:", 0 },
                    { 8936, 1, "tableCell11", 1, "Hinweis:", 0 },
                    { 8922, 1, "xrLabel7", 3, "Palettenannahme-Quittung FR TODO", 0 },
                    { 8918, 1, "tableCell12", 1, "Dieser Beleg ist nicht an Dritte übertragbar.", 0 },
                    { 8906, 1, "tableCell15", 1, "Menge", 0 },
                    { 8907, 1, "tableCell16", 1, "Ladungsträger-Typ", 0 },
                    { 8908, 1, "tableCell18", 1, "Grund für die Belegerstellung:", 0 },
                    { 8909, 1, "tableCell19", 1, "", 0 },
                    { 8910, 1, "tableCell21", 1, "Beleg-Einlösung / Ladungsträger-Ausgabe:", 0 },
                    { 8898, 1, "tableCell23", 1, "Referenznummer (Ausst.):", 0 },
                    { 8900, 1, "tableCell24", 1, "", 0 },
                    { 8899, 1, "tableCell26", 1, "Beschaffungslogistik:", 0 },
                    { 8905, 1, "tableCell13", 1, "Nicht getauschte Ladungsträger:", 0 },
                    { 8928, 1, "xrLabel7", 4, "Palettenannahme-Quittung PL TODO", 0 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
         migrationBuilder.UpdateData("DocumentTemplates", "Id", 1, "Data", null);

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
                    { 6776, 1, "label1", 1, "Datum (Zeit)", 0 },
                    { 6768, 1, "tableCell7", 1, "Aussteller:", 0 },
                    { 6778, 1, "tableCell9", 1, "DPL-Digitalcode:", 0 },
                    { 6799, 1, "xrLabel19", 1, "Aussteller:", 0 },
                    { 6792, 1, "xrLabel19", 2, "Issuer:", 0 },
                    { 6760, 1, "xrLabel19", 3, "Emetteur : ", 0 },
                    { 6793, 1, "xrLabel19", 4, "Wystawiajacy:", 0 },
                    { 6798, 1, "xrLabel22", 1, "Unterschrift ([IssuerName])", 0 },
                    { 6761, 1, "tableCell5", 1, "Empfänger:", 0 },
                    { 6782, 1, "xrLabel22", 2, "Signature", 0 },
                    { 6788, 1, "xrLabel22", 4, "Podpis", 0 },
                    { 6775, 1, "xrLabel23", 1, "Empfänger: (Name des Fahrers)", 0 },
                    { 6794, 1, "xrLabel23", 2, "Recipient:", 0 },
                    { 6786, 1, "xrLabel23", 3, "Destinataire :", 0 },
                    { 6785, 1, "xrLabel23", 4, "Odbiorca:", 0 },
                    { 6796, 1, "xrLabel7", 1, "DPL-Pooling-Gutschrift-Nr.", 0 },
                    { 6791, 1, "xrLabel7", 2, "Palettenannahme-Beleg TODO", 0 },
                    { 6790, 1, "xrLabel22", 3, "Signature", 0 },
                    { 6766, 1, "tableCell4", 1, "[DPLAddressAndContact]", 0 },
                    { 6765, 1, "tableCell3", 1, "Kontakt zu DPL:", 0 },
                    { 6763, 1, "tableCell26", 1, "Beschaffungslogistik:", 0 },
                    { 6787, 1, "label1", 2, "DPL-Code:", 0 },
                    { 6781, 1, "label1", 3, "DPL-Code:", 0 },
                    { 6783, 1, "label1", 4, "DPL-Code:", 0 },
                    { 6774, 1, "label2", 1, "Kennzeichen", 0 },
                    { 6773, 1, "label3", 1, "Unterschrift ([TruckDriver])", 0 },
                    { 6795, 1, "label4", 1, "[Number]", 0 },
                    { 6797, 1, "tableCell1", 1, "Beleg-Typ / -Einlösung:", 0 },
                    { 6780, 1, "tableCell11", 1, "Hinweis:", 0 },
                    { 6779, 1, "tableCell12", 1, "Dieser Beleg ist nicht an Dritte übertragbar.", 0 },
                    { 6767, 1, "tableCell13", 1, "Nicht getauschte Ladungsträger:", 0 },
                    { 6769, 1, "tableCell15", 1, "Menge", 0 },
                    { 6777, 1, "tableCell16", 1, "Ladungsträger-Typ", 0 },
                    { 6770, 1, "tableCell18", 1, "Grund für die Belegerstellung:", 0 },
                    { 6771, 1, "tableCell19", 1, "", 0 },
                    { 6772, 1, "tableCell21", 1, "Beleg-Einlösung / Ladungsträger-Ausgabe:", 0 },
                    { 6762, 1, "tableCell23", 1, "Referenznummer (Ausst.):", 0 },
                    { 6764, 1, "tableCell24", 1, "", 0 },
                    { 6784, 1, "xrLabel7", 3, "Palettenannahme-Quittung FR TODO", 0 },
                    { 6789, 1, "xrLabel7", 4, "Palettenannahme-Quittung PL TODO", 0 }
                });
        }
    }
}
