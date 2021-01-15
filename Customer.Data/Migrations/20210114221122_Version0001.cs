using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Customers.Data.Migrations
{
    public partial class Version0001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Classifications",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genders",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 20, nullable: false),
                    IsAdmin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 20, nullable: false),
                    RegionId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cities_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PasswordHash = table.Column<byte[]>(nullable: false),
                    PasswordSalt = table.Column<byte[]>(nullable: false),
                    PasswordLastUpdatedUtc = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(maxLength: 50, nullable: false),
                    UserRoleId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_UserRoles_UserRoleId",
                        column: x => x.UserRoleId,
                        principalTable: "UserRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Phone = table.Column<string>(maxLength: 50, nullable: false),
                    LastPurchase = table.Column<DateTime>(nullable: true),
                    GenderId = table.Column<long>(nullable: false),
                    CityId = table.Column<long>(nullable: false),
                    ClassificationId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Customers_Classifications_ClassificationId",
                        column: x => x.ClassificationId,
                        principalTable: "Classifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Customers_Genders_GenderId",
                        column: x => x.GenderId,
                        principalTable: "Genders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Customers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Classifications",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1L, "VIP" },
                    { 2L, "Regular" },
                    { 3L, "Sporadic" }
                });

            migrationBuilder.InsertData(
                table: "Genders",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1L, "Masculine" },
                    { 2L, "Feminine" }
                });

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1L, "Rio Grande do Sul" },
                    { 2L, "São Paulo" },
                    { 3L, "Curitiba" }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Id", "IsAdmin", "Name" },
                values: new object[,]
                {
                    { 1L, true, "Administrator" },
                    { 2L, false, "Seller" }
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Name", "RegionId" },
                values: new object[] { 1L, "Porto Alegre", 1L });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "PasswordHash", "PasswordLastUpdatedUtc", "PasswordSalt", "UserRoleId" },
                values: new object[,]
                {
                    { 1L, "admin@app.com", new byte[] { 173, 217, 14, 64, 8, 143, 123, 161, 247, 177, 169, 81, 160, 118, 53, 237, 81, 28, 141, 188, 44, 118, 233, 135, 57, 17, 125, 251, 76, 112, 32, 237, 96, 48, 108, 247, 241, 131, 35, 56, 142, 100, 95, 216, 96, 161, 18, 187, 65, 239, 116, 174, 129, 210, 212, 48, 246, 56, 250, 93, 197, 164, 60, 22 }, new DateTime(2021, 1, 14, 22, 11, 21, 908, DateTimeKind.Utc).AddTicks(7093), new byte[] { 238, 189, 42, 19, 157, 11, 184, 255, 8, 131, 84, 135, 221, 168, 254, 49, 239, 47, 2, 87, 26, 255, 72, 54, 8, 248, 248, 111, 26, 178, 121, 103, 70, 177, 16, 198, 19, 183, 200, 211, 195, 245, 28, 75, 197, 114, 236, 36, 27, 39, 146, 159, 45, 28, 239, 130, 201, 113, 115, 158, 122, 127, 184, 173, 153, 107, 230, 88, 213, 176, 199, 129, 162, 194, 66, 140, 195, 162, 182, 2, 203, 252, 101, 153, 85, 241, 46, 218, 154, 70, 119, 224, 62, 85, 224, 77, 108, 88, 153, 221, 114, 232, 205, 124, 51, 233, 251, 73, 8, 19, 15, 43, 250, 72, 171, 130, 218, 186, 196, 5, 58, 129, 248, 24, 99, 219, 109, 156 }, 1L },
                    { 2L, "seller1@app.com", new byte[] { 71, 159, 240, 167, 155, 139, 5, 10, 66, 25, 214, 129, 226, 127, 50, 101, 113, 89, 159, 93, 198, 165, 214, 199, 206, 228, 254, 129, 34, 116, 104, 93, 234, 88, 131, 91, 19, 188, 110, 172, 111, 217, 160, 148, 229, 183, 227, 100, 136, 130, 207, 14, 160, 97, 177, 66, 159, 249, 51, 134, 94, 211, 221, 116 }, new DateTime(2021, 1, 14, 22, 11, 21, 908, DateTimeKind.Utc).AddTicks(8069), new byte[] { 238, 189, 42, 19, 157, 11, 184, 255, 8, 131, 84, 135, 221, 168, 254, 49, 239, 47, 2, 87, 26, 255, 72, 54, 8, 248, 248, 111, 26, 178, 121, 103, 70, 177, 16, 198, 19, 183, 200, 211, 195, 245, 28, 75, 197, 114, 236, 36, 27, 39, 146, 159, 45, 28, 239, 130, 201, 113, 115, 158, 122, 127, 184, 173, 153, 107, 230, 88, 213, 176, 199, 129, 162, 194, 66, 140, 195, 162, 182, 2, 203, 252, 101, 153, 85, 241, 46, 218, 154, 70, 119, 224, 62, 85, 224, 77, 108, 88, 153, 221, 114, 232, 205, 124, 51, 233, 251, 73, 8, 19, 15, 43, 250, 72, 171, 130, 218, 186, 196, 5, 58, 129, 248, 24, 99, 219, 109, 156 }, 2L },
                    { 3L, "seller2@app.com", new byte[] { 115, 73, 35, 192, 103, 127, 50, 109, 143, 133, 55, 231, 174, 92, 126, 119, 73, 40, 7, 199, 223, 3, 11, 115, 159, 163, 41, 102, 181, 13, 52, 105, 47, 151, 252, 23, 242, 132, 181, 105, 237, 95, 66, 87, 46, 247, 244, 44, 244, 159, 173, 98, 213, 90, 33, 42, 35, 159, 254, 5, 187, 226, 68, 59 }, new DateTime(2021, 1, 14, 22, 11, 21, 908, DateTimeKind.Utc).AddTicks(8098), new byte[] { 238, 189, 42, 19, 157, 11, 184, 255, 8, 131, 84, 135, 221, 168, 254, 49, 239, 47, 2, 87, 26, 255, 72, 54, 8, 248, 248, 111, 26, 178, 121, 103, 70, 177, 16, 198, 19, 183, 200, 211, 195, 245, 28, 75, 197, 114, 236, 36, 27, 39, 146, 159, 45, 28, 239, 130, 201, 113, 115, 158, 122, 127, 184, 173, 153, 107, 230, 88, 213, 176, 199, 129, 162, 194, 66, 140, 195, 162, 182, 2, 203, 252, 101, 153, 85, 241, 46, 218, 154, 70, 119, 224, 62, 85, 224, 77, 108, 88, 153, 221, 114, 232, 205, 124, 51, 233, 251, 73, 8, 19, 15, 43, 250, 72, 171, 130, 218, 186, 196, 5, 58, 129, 248, 24, 99, 219, 109, 156 }, 2L }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "CityId", "ClassificationId", "GenderId", "LastPurchase", "Name", "Phone", "UserId" },
                values: new object[,]
                {
                    { 2L, 1L, 1L, 2L, new DateTime(2015, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Carla", "(53) 94569999 ", 2L },
                    { 3L, 1L, 3L, 2L, new DateTime(2013, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Maria", "(64) 94518888", 2L },
                    { 4L, 1L, 2L, 1L, new DateTime(2016, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Douglas", "(51) 12455555 ", 2L },
                    { 1L, 1L, 1L, 1L, new DateTime(2016, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Maurício", "(11) 95429999", 3L },
                    { 5L, 1L, 2L, 2L, new DateTime(2016, 8, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Marta", "(51) 45788888 ", 3L }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cities_RegionId",
                table: "Cities",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CityId",
                table: "Customers",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_ClassificationId",
                table: "Customers",
                column: "ClassificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_GenderId",
                table: "Customers",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_UserId",
                table: "Customers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserRoleId",
                table: "Users",
                column: "UserRoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Classifications");

            migrationBuilder.DropTable(
                name: "Genders");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropTable(
                name: "UserRoles");
        }
    }
}
