using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace Dpl.B2b.Dal.Extensions
{
    public static class AddCustomerIdToDocumentNumberSequenceExtension
    {
        public static OperationBuilder<SqlOperation> SetCustomerIdToDocumentNumberSequence(
            this MigrationBuilder migrationBuilder)
            => migrationBuilder.Sql($"EXEC ('UPDATE SN " +
                                    "SET CustomerId = CD.CustomerId " +
                                    "FROM CustomerDivisionDocumentSettings DS " +
                                    "INNER JOIN CustomerDivisions CD on DS.DivisionId = CD.ID "+ 
                                    "INNER JOIN DocumentNumberSequences SN ON DS.DocumentNumberSequenceId = SN.ID " + 
                                    "WHERE SN.CustomerId IS NULL')");
    }
}
