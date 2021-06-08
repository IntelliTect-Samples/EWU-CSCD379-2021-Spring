using Microsoft.EntityFrameworkCore.Migrations;

namespace SecretSanta.Data.Migrations
{
    public partial class AlternateKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GiverReceiver",
                table: "Assignments",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Users_FirstName_LastName",
                table: "Users",
                columns: new[] { "FirstName", "LastName" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Groups_Name",
                table: "Groups",
                column: "Name");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Gifts_Name_UserId_Priority",
                table: "Gifts",
                columns: new[] { "Name", "UserId", "Priority" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Assignments_GiverReceiver",
                table: "Assignments",
                column: "GiverReceiver");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Users_FirstName_LastName",
                table: "Users");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Groups_Name",
                table: "Groups");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Gifts_Name_UserId_Priority",
                table: "Gifts");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Assignments_GiverReceiver",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "GiverReceiver",
                table: "Assignments");
        }
    }
}
