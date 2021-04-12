using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class Edit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "15595191-57c0-4763-aa7b-1567e31c60e4");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "47ac1720-ca40-4bee-b111-a0e90ec7e5b0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "15595191-57c0-4763-aa7b-1567e31c60e4", "472b930f-90b6-4e8b-81fa-986ef4c51915", "admin", null });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "IsOnline", "LockoutEnabled", "LockoutEnd", "NickName", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "47ac1720-ca40-4bee-b111-a0e90ec7e5b0", 0, "e25024f6-9a17-4ddf-aa41-89669675e1a6", "admin@forum.forum", false, false, false, null, "admin", null, null, null, null, false, "76d2cb0a-7392-4816-8673-a2f182e43fa9", false, null });
        }
    }
}
