using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlkemyChallenge.Migrations
{
    public partial class Admin2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "64dfdcc9-f552-4d3b-ab31-b14a2f416f12",
                column: "ConcurrencyStamp",
                value: "afc88abf-7aa3-4ca0-adf4-d12498ee7657");

            migrationBuilder.InsertData(
                table: "AspNetUserClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "UserId" },
                values: new object[] { 2, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Admin", "c9707e00-eabd-420f-ad95-0d99470f37b9" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c9707e00-eabd-420f-ad95-0d99470f37b9",
                columns: new[] { "ConcurrencyStamp", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "d588a5d3-f8b6-4ba7-8c3e-66cf422db547", "lucasnahuelrobinet@gmail.com", "AQAAAAEAACcQAAAAEFDYRJrl/lMUVRUTsa3am5DKOTS2txBduIH8mfbu/v3Y4DUAGCRtXPVy9GJNfwgpvw==", "26810894-723f-4b9e-b6f5-4eaa9bce20d6", "lucasnahuelrobinet@gmail.com" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "64dfdcc9-f552-4d3b-ab31-b14a2f416f12",
                column: "ConcurrencyStamp",
                value: "dcee1554-8983-480f-8781-98cd8604dcba");

            migrationBuilder.InsertData(
                table: "AspNetUserClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "UserId" },
                values: new object[] { 1, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Admin", "c9707e00-eabd-420f-ad95-0d99470f37b9" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c9707e00-eabd-420f-ad95-0d99470f37b9",
                columns: new[] { "ConcurrencyStamp", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "71c0f7ef-0bf5-438a-a98d-2fcd3a4d0481", "kkyouA", "AQAAAAEAACcQAAAAEHwHSgc+3/NGmDo/5FY60luMzpdmRuSpzZYFhLQ4UFectmbquFJSpL9l2H9ryWKoCQ==", "3f5e2df6-71eb-49b9-9182-a09a911462d2", "kkyouA" });
        }
    }
}
