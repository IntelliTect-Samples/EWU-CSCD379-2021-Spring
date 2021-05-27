using Microsoft.EntityFrameworkCore.Migrations;

namespace UserGroup.Data.Migrations
{
    public partial class AddedEventUniqueKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Speakers_FirstName_LastName",
                table: "Speakers");

            migrationBuilder.CreateIndex(
                name: "IX_Events_Date_Location",
                table: "Events",
                columns: new[] { "Date", "Location" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Events_Date_Location",
                table: "Events");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Speakers_FirstName_LastName",
                table: "Speakers",
                columns: new[] { "FirstName", "LastName" });
        }
    }
}
