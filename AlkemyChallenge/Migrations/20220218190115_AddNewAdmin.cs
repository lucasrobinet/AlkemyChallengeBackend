using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlkemyChallenge.Migrations
{
    public partial class AddNewAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                value: "88c87da5-4546-4e2c-bc21-f6911ed22bf7");

            migrationBuilder.InsertData(
                table: "AspNetUserClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "UserId" },
                values: new object[] { 3, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Admin", "c9707e00-eabd-420f-ad95-0d99470f37b9" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c9707e00-eabd-420f-ad95-0d99470f37b9",
                columns: new[] { "ConcurrencyStamp", "Email", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "6644349a-b10f-402b-9d50-5cb107dfacbf", "admintest@gmail.com", "admintest@gmail.com", "Admin", "AQAAAAEAACcQAAAAEFGcxOnT6wmxMLYUUOxclc+3X5pQ/k+wHIvkvFgGWkqTCECktixXWrNQ15UPhu5hFA==", "2b5d8120-4f69-44d5-8c30-70ae2db807f5", "Admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 3);

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
                columns: new[] { "ConcurrencyStamp", "Email", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "d588a5d3-f8b6-4ba7-8c3e-66cf422db547", "lucasnahuelrobinet@gmail.com", "lucasnahuelrobinet@gmail.com", "lucasnahuelrobinet@gmail.com", "AQAAAAEAACcQAAAAEFDYRJrl/lMUVRUTsa3am5DKOTS2txBduIH8mfbu/v3Y4DUAGCRtXPVy9GJNfwgpvw==", "26810894-723f-4b9e-b6f5-4eaa9bce20d6", "lucasnahuelrobinet@gmail.com" });
        }
    }
}
