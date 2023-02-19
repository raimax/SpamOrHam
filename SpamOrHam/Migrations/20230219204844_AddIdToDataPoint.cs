using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpamOrHam.Migrations
{
    /// <inheritdoc />
    public partial class AddIdToDataPoint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DataPoints",
                table: "DataPoints");

            migrationBuilder.AlterColumn<string>(
                name: "Word",
                table: "DataPoints",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "DataPoints",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DataPoints",
                table: "DataPoints",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DataPoints",
                table: "DataPoints");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "DataPoints");

            migrationBuilder.AlterColumn<string>(
                name: "Word",
                table: "DataPoints",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DataPoints",
                table: "DataPoints",
                column: "Word");
        }
    }
}
