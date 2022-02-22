using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlkemyChallenge.Migrations
{
    public partial class AdminData2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "64dfdcc9-f552-4d3b-ab31-b14a2f416f12", "dcee1554-8983-480f-8781-98cd8604dcba", "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "c9707e00-eabd-420f-ad95-0d99470f37b9", 0, "71c0f7ef-0bf5-438a-a98d-2fcd3a4d0481", "lucasnahuelrobinet@gmail.com", false, false, null, "lucasnahuelrobinet@gmail.com", "kkyouA", "AQAAAAEAACcQAAAAEHwHSgc+3/NGmDo/5FY60luMzpdmRuSpzZYFhLQ4UFectmbquFJSpL9l2H9ryWKoCQ==", null, false, "3f5e2df6-71eb-49b9-9182-a09a911462d2", false, "kkyouA" });

            migrationBuilder.InsertData(
                table: "AspNetUserClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "UserId" },
                values: new object[] { 1, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Admin", "c9707e00-eabd-420f-ad95-0d99470f37b9" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "64dfdcc9-f552-4d3b-ab31-b14a2f416f12");

            migrationBuilder.DeleteData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c9707e00-eabd-420f-ad95-0d99470f37b9");
        }
    }
}
