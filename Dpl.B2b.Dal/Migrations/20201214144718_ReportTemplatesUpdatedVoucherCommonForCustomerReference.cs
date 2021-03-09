using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Dpl.B2b.Dal.Migrations
{
    public partial class ReportTemplatesUpdatedVoucherCommonForCustomerReference : Migration
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

            migrationBuilder.InsertData(
                table: "DocumentTemplateLabel",
                columns: new[] { "Id", "CustomerId", "DocumentTemplateId", "Label", "LanguageId", "Text", "Version" },
                values: new object[,]
                {
                    { 6776, null, 1, "label1", 1, "Datum (Zeit)", 0 },
                    { 6768, null, 1, "tableCell7", 1, "Aussteller:", 0 },
                    { 6778, null, 1, "tableCell9", 1, "DPL-Digitalcode:", 0 },
                    { 6799, null, 1, "xrLabel19", 1, "Aussteller:", 0 },
                    { 6792, null, 1, "xrLabel19", 2, "Issuer:", 0 },
                    { 6760, null, 1, "xrLabel19", 3, "Emetteur : ", 0 },
                    { 6793, null, 1, "xrLabel19", 4, "Wystawiajacy:", 0 },
                    { 6798, null, 1, "xrLabel22", 1, "Unterschrift ([IssuerName])", 0 },
                    { 6761, null, 1, "tableCell5", 1, "Empfänger:", 0 },
                    { 6782, null, 1, "xrLabel22", 2, "Signature", 0 },
                    { 6788, null, 1, "xrLabel22", 4, "Podpis", 0 },
                    { 6775, null, 1, "xrLabel23", 1, "Empfänger: (Name des Fahrers)", 0 },
                    { 6794, null, 1, "xrLabel23", 2, "Recipient:", 0 },
                    { 6786, null, 1, "xrLabel23", 3, "Destinataire :", 0 },
                    { 6785, null, 1, "xrLabel23", 4, "Odbiorca:", 0 },
                    { 6796, null, 1, "xrLabel7", 1, "DPL-Pooling-Gutschrift-Nr.", 0 },
                    { 6791, null, 1, "xrLabel7", 2, "Palettenannahme-Beleg TODO", 0 },
                    { 6790, null, 1, "xrLabel22", 3, "Signature", 0 },
                    { 6766, null, 1, "tableCell4", 1, "[DPLAddressAndContact]", 0 },
                    { 6765, null, 1, "tableCell3", 1, "Kontakt zu DPL:", 0 },
                    { 6763, null, 1, "tableCell26", 1, "Beschaffungslogistik:", 0 },
                    { 6787, null, 1, "label1", 2, "DPL-Code:", 0 },
                    { 6781, null, 1, "label1", 3, "DPL-Code:", 0 },
                    { 6783, null, 1, "label1", 4, "DPL-Code:", 0 },
                    { 6774, null, 1, "label2", 1, "Kennzeichen", 0 },
                    { 6773, null, 1, "label3", 1, "Unterschrift ([TruckDriver])", 0 },
                    { 6795, null, 1, "label4", 1, "[Number]", 0 },
                    { 6797, null, 1, "tableCell1", 1, "Beleg-Typ / -Einlösung:", 0 },
                    { 6780, null, 1, "tableCell11", 1, "Hinweis:", 0 },
                    { 6779, null, 1, "tableCell12", 1, "Dieser Beleg ist nicht an Dritte übertragbar.", 0 },
                    { 6767, null, 1, "tableCell13", 1, "Nicht getauschte Ladungsträger:", 0 },
                    { 6769, null, 1, "tableCell15", 1, "Menge", 0 },
                    { 6777, null, 1, "tableCell16", 1, "Ladungsträger-Typ", 0 },
                    { 6770, null, 1, "tableCell18", 1, "Grund für die Belegerstellung:", 0 },
                    { 6771, null, 1, "tableCell19", 1, "", 0 },
                    { 6772, null, 1, "tableCell21", 1, "Beleg-Einlösung / Ladungsträger-Ausgabe:", 0 },
                    { 6762, null, 1, "tableCell23", 1, "Referenznummer (Ausst.):", 0 },
                    { 6764, null, 1, "tableCell24", 1, "", 0 },
                    { 6784, null, 1, "xrLabel7", 3, "Palettenannahme-Quittung FR TODO", 0 },
                    { 6789, null, 1, "xrLabel7", 4, "Palettenannahme-Quittung PL TODO", 0 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData("DocumentTemplates", "Id", 1, "Data", null);

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
                columns: new[] { "Id", "CustomerId", "DocumentTemplateId", "Label", "LanguageId", "Text", "Version" },
                values: new object[,]
                {
                    { 1976, null, 1, "label1", 1, "Datum (Zeit)", 0 },
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
                    { 1989, null, 1, "xrLabel7", 4, "Palettenannahme-Quittung PL TODO", 0 }
                });
        }
    }
}
