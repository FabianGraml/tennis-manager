using Microsoft.EntityFrameworkCore.Migrations;

namespace TennisDbLib.Migrations
{
    public partial class DbCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Firstname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lastname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Week = table.Column<int>(type: "int", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    Hour = table.Column<int>(type: "int", nullable: false),
                    PersonId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Age", "Firstname", "Lastname" },
                values: new object[] { 1, 66, "Hans", "Huber" });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Age", "Firstname", "Lastname" },
                values: new object[] { 2, 55, "Kurt", "Mayr" });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Age", "Firstname", "Lastname" },
                values: new object[] { 3, 44, "Susi", "Berger" });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "DayOfWeek", "Hour", "PersonId", "Week" },
                values: new object[,]
                {
                    { 1, 1, 12, 1, 22 },
                    { 3, 2, 14, 2, 22 },
                    { 4, 2, 11, 2, 22 },
                    { 2, 4, 12, 3, 22 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_PersonId",
                table: "Bookings",
                column: "PersonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Persons");
        }
    }
}
