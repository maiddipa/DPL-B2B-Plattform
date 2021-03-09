using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace Dpl.B2b.Dal.Extensions
{
    public static class FixCustomerRelationExtensions
    {
        public static OperationBuilder<SqlOperation> CreateAllUserCustomerRelations(this MigrationBuilder migrationBuilder) 
            => migrationBuilder.Sql($"EXEC ('INSERT INTO [dbo].[UserCustomerRelations] SELECT ID AS UserId, CustomerId FROM Users')");

        public static OperationBuilder<SqlOperation> SetCustomerIdForUsers(this MigrationBuilder migrationBuilder)
            => migrationBuilder.Sql($"EXEC ('UPDATE U SET CustomerId = C.CustomerId FROM Users U " +
                                    "INNER JOIN (SELECT UserId,MIN(CustomerId) AS CustomerId " +
                                    "FROM dbo.UserCustomerRelations GROUP BY UserId) C ON U.Id = C.UserId WHERE U.CustomerId IS NULL')");

        public static OperationBuilder<SqlOperation> SetOrganizationIdForUsers(this MigrationBuilder migrationBuilder)
            => migrationBuilder.Sql("EXEC ('UPDATE U SET OrganizationId = C.OrganizationId " +
                                            "FROM Users U INNER JOIN Customers C ON U.CustomerId = C.ID " +
                                            "WHERE U.OrganizationId IS NULL')");
    }
}
