using Microsoft.EntityFrameworkCore.Migrations;

namespace Dpl.B2b.Dal.Migrations
{
    public partial class RemoveDuplicateCustomerNumberOnCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Customers_CustomerNumber",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_CustomerNumberString",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CustomerNumberString",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CustomerNumber",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "RefLmsCustomerId",
                table: "Customers");

            migrationBuilder.AddColumn<int>(
                name: "RefErpCustomerNumber",
                table: "Customers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RefErpCustomerNumberString",
                table: "Customers",
                maxLength: 255,
                nullable: false,
                computedColumnSql: "CONVERT(nvarchar(255), [RefErpCustomerNumber])");

            migrationBuilder.InsertData(
                table: "AccountingRecordTypes",
                columns: new[] { "Id", "Description", "Name", "RefLtmBookingTypeId" },
                values: new object[,]
                {
                    { 1, "Abholung", "Abholung", "ABH" },
                    { 31, "Lieferung von Paletten im DPL-Pooling an Kunde", "Lieferung Pooling an Kunde", "LIPAK" },
                    { 32, "Manuelle Buchung", "Manuelle", "MAN" },
                    { 34, "Berechnung einer geringeren Poolinggebühr", "Preisminderung", "PM" },
                    { 35, "Abrechnung Pooling Tausch", "Abrechn. Pooling", "POOL" },
                    { 36, "Palettenqualitäten-Tausch", "Qualitätentausch", "QT" },
                    { 37, "Reinigung", "Reinigung", "REIN" },
                    { 38, "Reparatur gegen Gebühr (Aus defektem Guthaben)", "Repa. g. Gebühr", "REP" },
                    { 39, "Einreichung Reparaturauftrag für defekte Paletten vom Handel", "Reparaturauftrag", "REPAUF" },
                    { 40, "Berechnung eines Reparaturpreises vom Kunden an DPL", "Repa Kunde", "REPKND" },
                    { 41, "Reparatur-Quote", "Reparatur-Quote", "REPQU" },
                    { 42, "Retoure/Stornierung einer Versandmeldung", "Retoure", "RETOUR" },
                    { 43, "Rückgabe Palettengutschrift an Kunde", "Rückgabe PG an Kunde", "ROH" },
                    { 44, "Reparatur-Tausch (Aus defektem Guthaben)", "Reparatur-Tausch", "RPT" },
                    { 45, "Einreichung Schuldschein", "Schuldschein", "SCHULD" },
                    { 46, "Sortierung", "Sortierung", "SORT" },
                    { 47, "Stornierungsbuchung", "Stornierung", "STORNO" },
                    { 48, "System", "System", "SYS" },
                    { 49, "Transportdifferenz", "Transportdifferenz", "TADIFF" },
                    { 50, "Unbestimmt Buchung", "Unbestimmt", "U" },
                    { 51, "Verkauf von Paletten", "Verkauf", "VERK" },
                    { 52, "Abschreibung als Verlust", "Verlust", "VERLUS" },
                    { 53, "Versandmeldung", "Versandmeldung", "VERS" },
                    { 54, "Versandmeldung (ins Ausland)", "Versand (Ausland)", "VERSA" },
                    { 55, "Wandlung monetäre Vergütung in Palettengutschrift", "Wandlung monetäre Vergütung", "WMP" },
                    { 56, "Digitale Pooling Gutschrift", "Digitale Pooling Gutschrift", "DPLDPG" },
                    { 30, "Korrekturzahlung", "Korrekturzahlung", "KZA" },
                    { 29, "Kontoumbuchung (Zugang)", "Kontoumbuchung", "KUZ" },
                    { 33, "Palettenarten-Tausch", "Palettenarten-Tausch", "PAT" },
                    { 27, "Kontoumbuchung an Freistellung (Abgang)", "Freistellung", "KUFSA" },
                    { 2, "Ladungsweise Abholung", "Abholung (Ladungsweise)", "ABHL" },
                    { 3, "Ausgleich körperlich an DPL", "Ausgleich körperlich an DPL", "AKD" },
                    { 4, "Ausgleich körperlich an Kunde", "Ausgleich körperlich an Kunde", "AKK" },
                    { 5, "Anlieferung", "Anlieferung", "ANL" },
                    { 6, "Anlieferung/Abgabe am Depot", "Depotabgabe", "ANLDEP" },
                    { 7, "Anlieferung/Abgabe am Depot in Kleinmengen", "Depotabgabe Kleinmengen", "ANLDPK" },
                    { 8, "Ausgleich DPL-OPG Buchung", "Ausgleich DPL-OPG", "AUSDPG" },
                    { 9, "Ausgang OPG Buchung", "Ausgang OPG", "AUSOPG" },
                    { 28, "Kontoumbuchung an Freistellung (Zugang)", "Freistellung", "KUFSZ" },
                    { 11, "Abschreibung über Defektquoten Konto", "Defektquote", "DQ" },
                    { 12, "Eingang OPG Buchung", "Eingang OPG", "EINOPG" },
                    { 13, "Entsorgung", "Entsorgung", "ENTSOR" },
                    { 14, "Freistellung defekter Paletten an DPL", "Freistellung Defekt", "FSD" },
                    { 10, "Rückbuchung der Differenz von freigestellten/abgelehnten Paletten", "Differenzbuchung", "DIFF" },
                    { 16, "Gutschrift DPL-OPG Buchung", "Gutsch. DPL-OPG", "GUTDPG" },
                    { 26, "Kontoumbuchung an Ablehnungskonto (Zugang)", "Abholung", "KUABZ" },
                    { 15, "Freistellung körperlich an DPL", "Freistellung Intakt", "FSI" },
                    { 24, "Kontoumbuchung (Abgang)", "Kontoumbuchung", "KUA" },
                    { 23, "Abholung", "Kauf aus Konto durch Kunde", "KKK" },
                    { 22, "Abholung", "Abholung", "KKD" },
                    { 25, "Kontoumbuchung an Ablehnungskonto (Abgang)", "Ablehnung", "KUABA" },
                    { 20, "Abholung", "Palettenkorrekturbuchung", "KBU" },
                    { 19, "Kauf von Paletten", "Kauf", "KAUF" },
                    { 18, "Kontoumbuchung an Klärkonto", "Klärfall", "K" },
                    { 17, "Gutschrift OPG Buchung", "Gutschrift OPG", "GUTOPG" },
                    { 21, "Abholung", "Kauf DPL-OPG", "KDPG" }
                });

            migrationBuilder.InsertData(
                table: "BaseLoadCarrierMapping",
                columns: new[] { "LoadCarrierId", "LoadCarrierTypeId" },
                values: new object[,]
                {
                    { 402, 10 },
                    { 203, 3 },
                    { 203, 4 },
                    { 402, 4 },
                    { 203, 6 },
                    { 402, 6 },
                    { 203, 9 },
                    { 402, 9 },
                    { 203, 10 },
                    { 203, 21 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_RefErpCustomerNumber",
                table: "Customers",
                column: "RefErpCustomerNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_RefErpCustomerNumberString",
                table: "Customers",
                column: "RefErpCustomerNumberString");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Customers_RefErpCustomerNumber",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_RefErpCustomerNumberString",
                table: "Customers");

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "AccountingRecordTypes",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "BaseLoadCarrierMapping",
                keyColumns: new[] { "LoadCarrierId", "LoadCarrierTypeId" },
                keyValues: new object[] { 203, 3 });

            migrationBuilder.DeleteData(
                table: "BaseLoadCarrierMapping",
                keyColumns: new[] { "LoadCarrierId", "LoadCarrierTypeId" },
                keyValues: new object[] { 203, 4 });

            migrationBuilder.DeleteData(
                table: "BaseLoadCarrierMapping",
                keyColumns: new[] { "LoadCarrierId", "LoadCarrierTypeId" },
                keyValues: new object[] { 203, 6 });

            migrationBuilder.DeleteData(
                table: "BaseLoadCarrierMapping",
                keyColumns: new[] { "LoadCarrierId", "LoadCarrierTypeId" },
                keyValues: new object[] { 203, 9 });

            migrationBuilder.DeleteData(
                table: "BaseLoadCarrierMapping",
                keyColumns: new[] { "LoadCarrierId", "LoadCarrierTypeId" },
                keyValues: new object[] { 203, 10 });

            migrationBuilder.DeleteData(
                table: "BaseLoadCarrierMapping",
                keyColumns: new[] { "LoadCarrierId", "LoadCarrierTypeId" },
                keyValues: new object[] { 203, 21 });

            migrationBuilder.DeleteData(
                table: "BaseLoadCarrierMapping",
                keyColumns: new[] { "LoadCarrierId", "LoadCarrierTypeId" },
                keyValues: new object[] { 402, 4 });

            migrationBuilder.DeleteData(
                table: "BaseLoadCarrierMapping",
                keyColumns: new[] { "LoadCarrierId", "LoadCarrierTypeId" },
                keyValues: new object[] { 402, 6 });

            migrationBuilder.DeleteData(
                table: "BaseLoadCarrierMapping",
                keyColumns: new[] { "LoadCarrierId", "LoadCarrierTypeId" },
                keyValues: new object[] { 402, 9 });

            migrationBuilder.DeleteData(
                table: "BaseLoadCarrierMapping",
                keyColumns: new[] { "LoadCarrierId", "LoadCarrierTypeId" },
                keyValues: new object[] { 402, 10 });

            migrationBuilder.DropColumn(
                name: "RefErpCustomerNumber",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "RefErpCustomerNumberString",
                table: "Customers");

            migrationBuilder.AddColumn<int>(
                name: "CustomerNumber",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RefLmsCustomerId",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CustomerNumberString",
                table: "Customers",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                computedColumnSql: "CONVERT(nvarchar(255), [CustomerNumber])");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CustomerNumber",
                table: "Customers",
                column: "CustomerNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CustomerNumberString",
                table: "Customers",
                column: "CustomerNumberString");
        }
    }
}
