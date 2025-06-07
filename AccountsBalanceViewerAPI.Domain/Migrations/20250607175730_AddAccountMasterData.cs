using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountsBalanceViewerAPI.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountMasterData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "AccountName", "AccountCode", "Description", "CreatedDate" },
                values: new object[,]
                {
                    { Guid.NewGuid(), "R&D", "RD", "Research and development", DateTime.UtcNow },
                    { Guid.NewGuid(), "Canteen", "CT", "Food and brevarages", DateTime.UtcNow },
                    { Guid.NewGuid(), "CEO’s car expenses", "CEO_V", "Boss car expenses", DateTime.UtcNow },
                    { Guid.NewGuid(), "Marketing", "MK", "Linkin and Facebook", DateTime.UtcNow },
                    { Guid.NewGuid(), "Parking fines", "PF", "Parking fine", DateTime.UtcNow }
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "AccountName",
                keyValues: new object[]
                {
                    "R&D",
                    "Canteen",
                    "CEO’s car expenses",
                    "Marketing",
                    "Parking fines"
                }
            );
        }
    }
}
