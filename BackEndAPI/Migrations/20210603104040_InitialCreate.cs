using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BackEndAPI.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssetCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    JoinedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    StaffCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    OnFirstLogin = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    AssetName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Specification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstalledDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    State = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assets_AssetCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "AssetCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaim",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaim_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaim",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaim_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProviderKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserLogins_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserTokens_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Assignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetId = table.Column<int>(type: "int", nullable: false),
                    AssignedByUserId = table.Column<int>(type: "int", nullable: false),
                    AssignedToUserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssignedToUserId = table.Column<int>(type: "int", nullable: false),
                    AssignedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateEditDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assignments_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assignments_User_AssignedByUserId",
                        column: x => x.AssignedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReturnRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssignmentId = table.Column<int>(type: "int", nullable: true),
                    AssetCodeCopy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssetNameCopy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignedDateCopy = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestedByUserId = table.Column<int>(type: "int", nullable: false),
                    AcceptedByUserId = table.Column<int>(type: "int", nullable: true),
                    ReturnedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReturnRequest_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ReturnRequest_User_AcceptedByUserId",
                        column: x => x.AcceptedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReturnRequest_User_RequestedByUserId",
                        column: x => x.RequestedByUserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AssetCategories",
                columns: new[] { "Id", "CategoryCode", "CategoryName" },
                values: new object[,]
                {
                    { 1, "LA", "Laptop" },
                    { 2, "PC", "PC" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "Gender", "JoinedDate", "LastName", "Location", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "Password", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "StaffCode", "Status", "TwoFactorEnabled", "Type", "UserName" },
                values: new object[,]
                {
                    { 1, 0, "f99d8020-dc6f-4ff6-ac58-9964a1f4005f", new DateTime(1993, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, "Binh", 0, new DateTime(2021, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nguyen Van", 0, false, null, null, null, "binhnv@20011993", null, null, false, null, "SD0001", 0, false, 0, "binhnv" },
                    { 2, 0, "37656048-00a9-4abc-af86-d2605266a4de", new DateTime(1994, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, "Binh", 1, new DateTime(2021, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nguyen Thi", 0, false, null, null, null, "binhnt@12011994", null, null, false, null, "SD0002", 0, false, 1, "binhnt" },
                    { 3, 0, "ad6557db-e475-41df-b9df-2b80fdf1c0d3", new DateTime(1997, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, "Binh", 1, new DateTime(2019, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nguyen Thi", 0, false, null, null, null, "binhnt2@12011997", null, null, false, null, "SD0003", 0, false, 1, "binhnt2" },
                    { 4, 0, "ed3f5369-116f-4745-b484-8c7d5c0bea84", new DateTime(2000, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, "Anh", 1, new DateTime(2018, 9, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nguyen Duc", 1, false, null, null, null, "anhnd@20012000", null, null, false, null, "SD0004", 0, false, 0, "anhnd" },
                    { 5, 0, "c385c7d4-402a-44aa-ae60-5df850e6ced6", new DateTime(1990, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, "Van", 1, new DateTime(2021, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nguyen Thi", 1, false, null, null, null, "binhnt@12011990", null, null, false, null, "SD0005", 0, false, 1, "binhnt" },
                    { 6, 0, "750cea8a-9a64-4610-8a69-c25407fbc495", new DateTime(1987, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, "Binh", 0, new DateTime(2019, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nguyen Thi", 1, false, null, null, null, "binhnt2@120187", null, null, false, null, "SD0006", 0, false, 1, "binhnt2" }
                });

            migrationBuilder.InsertData(
                table: "Assets",
                columns: new[] { "Id", "AssetCode", "AssetName", "CategoryId", "InstalledDate", "Location", "Specification", "State" },
                values: new object[,]
                {
                    { 1, "LA000001", "Laptop 1", 1, new DateTime(2020, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Balls-to-the-walls laptop, specced with the latest CPU and GPU", 1 },
                    { 2, "LA000002", "Laptop 2", 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "An even more balls-to-the-walls laptop, specced with even better CPU and GPU than laptop 1", 1 },
                    { 3, "PC000001", "PC 1", 2, new DateTime(2020, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Balls-to-the-walls desktop, designed for ultimate Word experience", 0 },
                    { 4, "PC000002", "PC 2", 2, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "An even more balls-to-the-walls laptop, designed for the performant Excel workflow", 1 }
                });

            migrationBuilder.InsertData(
                table: "Assignments",
                columns: new[] { "Id", "AssetId", "AssignedByUserId", "AssignedDate", "AssignedToUserId", "AssignedToUserName", "CreateEditDate", "Note", "State" },
                values: new object[,]
                {
                    { 1, 1, 1, new DateTime(2021, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "binhnt", new DateTime(2021, 6, 2, 17, 40, 39, 883, DateTimeKind.Local).AddTicks(7485), "Make sure to upgrade RAM when you have spare time. Thanks.", 1 },
                    { 2, 2, 1, new DateTime(2021, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "binhnt2", new DateTime(2021, 6, 1, 17, 40, 39, 884, DateTimeKind.Local).AddTicks(5421), "Make sure to upgrade RAM when you have spare time. Thanks.", 1 },
                    { 3, 3, 4, new DateTime(2021, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, "binhnt", new DateTime(2021, 5, 31, 17, 40, 39, 884, DateTimeKind.Local).AddTicks(5436), "Make sure to upgrade RAM when you have spare time. Thanks.", 0 },
                    { 4, 4, 4, new DateTime(2021, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, "binhnt2", new DateTime(2021, 5, 30, 17, 40, 39, 884, DateTimeKind.Local).AddTicks(5439), "Make sure to upgrade RAM when you have spare time. Thanks.", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assets_CategoryId",
                table: "Assets",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_AssetId",
                table: "Assignments",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_AssignedByUserId",
                table: "Assignments",
                column: "AssignedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnRequest_AcceptedByUserId",
                table: "ReturnRequest",
                column: "AcceptedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnRequest_AssignmentId",
                table: "ReturnRequest",
                column: "AssignmentId",
                unique: true,
                filter: "[AssignmentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnRequest_RequestedByUserId",
                table: "ReturnRequest",
                column: "RequestedByUserId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Role",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaim_RoleId",
                table: "RoleClaim",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "User",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "User",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaim_UserId",
                table: "UserClaim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReturnRequest");

            migrationBuilder.DropTable(
                name: "RoleClaim");

            migrationBuilder.DropTable(
                name: "UserClaim");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "Assignments");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Assets");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "AssetCategories");
        }
    }
}
