using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GymManagement.DAL.Data.Miigirations
{
    /// <inheritdoc />
    public partial class Seedingdatainplanentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Plans",
                columns: new[] { "Id", "Description", "DurationDays", "IsActive", "Name", "Price", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Access to gym equpment during staffed hours", 30, true, "Basic Plan", 300m, null },
                    { 2, "includes gym equpment and 2 group classes per week", 60, false, "Standard Plan", 500m, null },
                    { 3, "unlimited access equpment,classes and sauna", 90, false, "premium Plan", 900m, null },
                    { 4, "full year access with personal trainer sessions", 365, true, "Annual Plan", 3000m, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
