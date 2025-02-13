using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OMS.Infrastructure.Persistance.EF.Migrations.OutboxDb
{
    /// <inheritdoc />
    public partial class initcounter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Published",
                table: "OutBoxes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RetryCount",
                table: "OutBoxes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Published",
                table: "OutBoxes");

            migrationBuilder.DropColumn(
                name: "RetryCount",
                table: "OutBoxes");
        }
    }
}
