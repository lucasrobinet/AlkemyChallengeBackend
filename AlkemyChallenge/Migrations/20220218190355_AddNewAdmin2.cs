using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlkemyChallenge.Migrations
{
    public partial class AddNewAdmin2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c9707e00-eabd-420f-ad95-0d99470f37b9");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "64dfdcc9-f552-4d3b-ab31-b14a2f416f12",
                column: "ConcurrencyStamp",
                value: "c26957d6-2c97-4979-a7ba-dbcd487b764d");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "a5dfc4ce-f898-44b4-b058-47ac901dd907", 0, "06ed52af-b560-4887-a49e-7daf63d01590", "admintest@gmail.com", false, false, null, "admintest@gmail.com", "Admin", "AQAAAAEAACcQAAAAEIPN0v3egQxZeMXwGkJDaoXNdQqTen4mL6Voo8EI+jDgix409tzAIUPQ86dpg5vleA==", null, false, "ad1c26b8-670b-499c-b181-db44f850c12a", false, "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "UserId" },
                values: new object[] { 4, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Admin", "a5dfc4ce-f898-44b4-b058-47ac901dd907" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a5dfc4ce-f898-44b4-b058-47ac901dd907");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "64dfdcc9-f552-4d3b-ab31-b14a2f416f12",
                column: "ConcurrencyStamp",
                value: "88c87da5-4546-4e2c-bc21-f6911ed22bf7");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "c9707e00-eabd-420f-ad95-0d99470f37b9", 0, "6644349a-b10f-402b-9d50-5cb107dfacbf", "admintest@gmail.com", false, false, null, "admintest@gmail.com", "Admin", "AQAAAAEAACcQAAAAEFGcxOnT6wmxMLYUUOxclc+3X5pQ/k+wHIvkvFgGWkqTCECktixXWrNQ15UPhu5hFA==", null, false, "2b5d8120-4f69-44d5-8c30-70ae2db807f5", false, "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "UserId" },
                values: new object[] { 3, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Admin", "c9707e00-eabd-420f-ad95-0d99470f37b9" });
        }
    }
}
