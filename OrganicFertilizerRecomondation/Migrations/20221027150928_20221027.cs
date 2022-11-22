using Microsoft.EntityFrameworkCore.Migrations;

namespace OrganicFertilizerRecomondation.Migrations
{
    public partial class _20221027 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CropAges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CropTypeId = table.Column<int>(type: "int", nullable: false),
                    Age = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nitrigion = table.Column<double>(type: "float", nullable: false),
                    Phosphurus = table.Column<double>(type: "float", nullable: false),
                    Pottasium = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CropAges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CropAges_CropTypes_CropTypeId",
                        column: x => x.CropTypeId,
                        principalTable: "CropTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NaturalSources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nitrigion = table.Column<double>(type: "float", nullable: false),
                    Phosphurus = table.Column<double>(type: "float", nullable: false),
                    Pottasium = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NaturalSources", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CropAges_CropTypeId",
                table: "CropAges",
                column: "CropTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CropAges");

            migrationBuilder.DropTable(
                name: "NaturalSources");
        }
    }
}
