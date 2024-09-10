using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ManagementSchool.Migrations
{
    /// <inheritdoc />
    public partial class AddAnnualFeesToClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AnnualFees",
                table: "Classes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "ClassId",
                keyValue: 1,
                column: "AnnualFees",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "ClassId",
                keyValue: 2,
                column: "AnnualFees",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "ClassId",
                keyValue: 3,
                column: "AnnualFees",
                value: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnnualFees",
                table: "Classes");
        }
    }
}
