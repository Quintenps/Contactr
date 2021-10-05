using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Contactr.Persistence.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Resource",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Provider = table.Column<int>(type: "int", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ETag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResourceName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resource", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Postalcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthenticationProviders",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthenticationProviders", x => x.Key);
                    table.ForeignKey(
                        name: "FK_AuthenticationProviders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Connections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SenderUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceiverUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SynchronizedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Connections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Connections_Resource_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "Resource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Connections_Users_ReceiverUserId",
                        column: x => x.ReceiverUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Connections_Users_SenderUserId",
                        column: x => x.SenderUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PersonalCards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Firstname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lastname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: true),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Postalcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalCards_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BusinessCards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessCards_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BusinessCards_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BusinessCards_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CompanyId",
                table: "Addresses",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthenticationProviders_UserId",
                table: "AuthenticationProviders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessCards_AddressId",
                table: "BusinessCards",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessCards_CompanyId",
                table: "BusinessCards",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessCards_UserId",
                table: "BusinessCards",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_Name",
                table: "Companies",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Connections_ReceiverUserId",
                table: "Connections",
                column: "ReceiverUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Connections_ResourceId",
                table: "Connections",
                column: "ResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Connections_SenderUserId",
                table: "Connections",
                column: "SenderUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalCards_UserId",
                table: "PersonalCards",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthenticationProviders");

            migrationBuilder.DropTable(
                name: "BusinessCards");

            migrationBuilder.DropTable(
                name: "Connections");

            migrationBuilder.DropTable(
                name: "PersonalCards");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Resource");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
