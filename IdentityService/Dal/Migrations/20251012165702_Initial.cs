using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dal.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    chatId = table.Column<Guid>(type: "uuid", maxLength: 36, nullable: true),
                    resourceId = table.Column<Guid>(type: "uuid", maxLength: 36, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Resources",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    source = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "GroupDalPermissionDal",
                columns: table => new
                {
                    GroupsId = table.Column<Guid>(type: "uuid", nullable: false),
                    PermissionsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupDalPermissionDal", x => new { x.GroupsId, x.PermissionsId });
                    table.ForeignKey(
                        name: "FK_GroupDalPermissionDal_Groups_GroupsId",
                        column: x => x.GroupsId,
                        principalTable: "Groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupDalPermissionDal_Permissions_PermissionsId",
                        column: x => x.PermissionsId,
                        principalTable: "Permissions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PermissionGroups",
                columns: table => new
                {
                    PermissionId = table.Column<Guid>(type: "uuid", nullable: false),
                    GroupId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionGroups", x => new { x.GroupId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_PermissionGroups_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionGroups_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    password = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    role = table.Column<int>(type: "integer", nullable: false),
                    photoResourceId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                    table.ForeignKey(
                        name: "FK_Users_Resources_photoResourceId",
                        column: x => x.photoResourceId,
                        principalTable: "Resources",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "GroupDalUserDal",
                columns: table => new
                {
                    GroupsId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupDalUserDal", x => new { x.GroupsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_GroupDalUserDal_Groups_GroupsId",
                        column: x => x.GroupsId,
                        principalTable: "Groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupDalUserDal_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupUsers",
                columns: table => new
                {
                    GroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupUsers", x => new { x.GroupId, x.UserId });
                    table.ForeignKey(
                        name: "FK_GroupUsers_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupDalPermissionDal_PermissionsId",
                table: "GroupDalPermissionDal",
                column: "PermissionsId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupDalUserDal_UsersId",
                table: "GroupDalUserDal",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_name",
                table: "Groups",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupUsers_UserId",
                table: "GroupUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGroups_PermissionId",
                table: "PermissionGroups",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_email",
                table: "Users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_name",
                table: "Users",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_photoResourceId",
                table: "Users",
                column: "photoResourceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupDalPermissionDal");

            migrationBuilder.DropTable(
                name: "GroupDalUserDal");

            migrationBuilder.DropTable(
                name: "GroupUsers");

            migrationBuilder.DropTable(
                name: "PermissionGroups");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Resources");
        }
    }
}
