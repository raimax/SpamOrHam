using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpamOrHam.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Datasets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HamCount = table.Column<double>(type: "float", nullable: false, defaultValue: 0.0),
                    SpamCount = table.Column<double>(type: "float", nullable: false, defaultValue: 0.0),
                    PriorHamProbability = table.Column<double>(type: "float", nullable: false),
                    PriorSpamProbability = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Datasets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataPoints",
                columns: table => new
                {
                    Word = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TimesOccurredInHam = table.Column<double>(type: "float", nullable: false, defaultValue: 0.0),
                    TimesOccurredInSpam = table.Column<double>(type: "float", nullable: false, defaultValue: 0.0),
                    HamProbability = table.Column<double>(type: "float", nullable: false),
                    SpamProbability = table.Column<double>(type: "float", nullable: false),
                    LastRecordContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatasetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataPoints", x => x.Word);
                    table.ForeignKey(
                        name: "FK_DataPoints_Datasets_DatasetId",
                        column: x => x.DatasetId,
                        principalTable: "Datasets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DataPoints_DatasetId",
                table: "DataPoints",
                column: "DatasetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataPoints");

            migrationBuilder.DropTable(
                name: "Datasets");
        }
    }
}
