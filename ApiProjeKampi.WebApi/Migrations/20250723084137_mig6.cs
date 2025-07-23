using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiProjeKampi.WebApi.Migrations
{
    public partial class mig6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_YummyEvent",
                table: "YummyEvents");

            migrationBuilder.RenameTable(
                name: "YummyEvents",
                newName: "YummyEvents");

            migrationBuilder.AddPrimaryKey(
                name: "PK_YummyEvents",
                table: "YummyEvents",
                column: "YummyEventId");

            migrationBuilder.CreateTable(
                name: "Abouts",
                columns: table => new
                {
                    AboutId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoCoverImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReservationNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abouts", x => x.AboutId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Abouts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_YummyEvents",
                table: "YummyEvents");

            migrationBuilder.RenameTable(
                name: "YummyEvents",
                newName: "YummyEvents");

            migrationBuilder.AddPrimaryKey(
                name: "PK_YummyEvent",
                table: "YummyEvents",
                column: "YummyEventId");
        }
    }
}
