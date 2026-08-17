using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagement.DAL.Data.Miigirations
{
    /// <inheritdoc />
    public partial class ChangeForinKeyForMemberinMemberShips : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MemberShipsId",
                table: "MemberShips");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MemberShipsId",
                table: "MemberShips",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
