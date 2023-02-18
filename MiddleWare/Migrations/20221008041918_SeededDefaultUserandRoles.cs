using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiddleWare.Migrations
{
    public partial class SeededDefaultUserandRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "f0c49c86-448e-48e1-a2f4-52290a13b172", "8726c846-4e68-4b34-b13d-d2a79c3de0e1", "User", "USER" },
                    { "f3a39e15-3bd5-466b-b406-4f11ceb983ca", "8171ab02-aaca-4084-8a66-21daa3160b92", "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "Firstname", "Lastname", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "32614026-3bb6-49f1-9420-86f185b79e6a", 0, "0e2cbc21-4d5f-40de-abc4-9b10acc9f440", "user@thesis2022.com", false, "System", "User", false, null, "USER@THESIS2022.COM", "USER@THESIS2022.COM", "AQAAAAEAACcQAAAAEJZ3Cyn6aZ5052pyFx2rBwGj/16Igaca7l3dluhuzv7gu2Kls3Y+rfs3SCCQJNM9aw==", null, false, "188d59dc-3bab-47cc-97e8-7a6d320d9f37", false, "user@thesis2022.com" },
                    { "d4a5ab24-9aae-4816-9629-5c45856f8945", 0, "511ba251-953c-46cb-8eac-137c091021d3", "admin@thesis2022.com", false, "System", "Admin", false, null, "ADMIN@THESIS2022.COM", "ADMIN@THESIS2022.COM", "AQAAAAEAACcQAAAAEAV3rWjnduLli8nrLVHx1WMN2N+qiakVDD7GVs/8d35joJl7EXXNYCvRl7VD0Zdfrg==", null, false, "a2c241cd-f365-4681-ac1d-41287d0e2ce0", false, "admin@thesis2022.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "f0c49c86-448e-48e1-a2f4-52290a13b172", "32614026-3bb6-49f1-9420-86f185b79e6a" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "f3a39e15-3bd5-466b-b406-4f11ceb983ca", "d4a5ab24-9aae-4816-9629-5c45856f8945" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "f0c49c86-448e-48e1-a2f4-52290a13b172", "32614026-3bb6-49f1-9420-86f185b79e6a" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "f3a39e15-3bd5-466b-b406-4f11ceb983ca", "d4a5ab24-9aae-4816-9629-5c45856f8945" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f0c49c86-448e-48e1-a2f4-52290a13b172");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f3a39e15-3bd5-466b-b406-4f11ceb983ca");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "32614026-3bb6-49f1-9420-86f185b79e6a");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d4a5ab24-9aae-4816-9629-5c45856f8945");
        }
    }
}
