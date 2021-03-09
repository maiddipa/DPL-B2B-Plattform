using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace Dpl.B2b.Dal.Extensions
{
    public static class AddCustomerToPostingAccountExtensions
    {
        public static OperationBuilder<SqlOperation> SetCustomerIdForPostingAccounts(this MigrationBuilder migrationBuilder)
            => migrationBuilder.Sql($"EXEC ('UPDATE A SET CustomerId = D.CustomerId " +
                                    "FROM PostingAccounts A INNER JOIN CustomerDivisions D ON A.ID = D.PostingAccountId " +
                                    "WHERE A.CustomerId IS NULL')");
    }
}
