using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartNQuick.Logic.Migrations
{
    public partial class InitDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Account");

            migrationBuilder.EnsureSchema(
                name: "MusicStore");

            migrationBuilder.EnsureSchema(
                name: "Revision");

            migrationBuilder.CreateTable(
                name: "Artist",
                schema: "MusicStore",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artist", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genre",
                schema: "MusicStore",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genre", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Identity",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Guid = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    TimeOutInMinutes = table.Column<int>(type: "int", nullable: false),
                    EnableJwtAuth = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Designation = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Album",
                schema: "MusicStore",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArtistId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Album", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Album_Artist_ArtistId",
                        column: x => x.ArtistId,
                        principalSchema: "MusicStore",
                        principalTable: "Artist",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Access",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentityId = table.Column<int>(type: "int", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", maxLength: 4096, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Access", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Access_Identity_IdentityId",
                        column: x => x.IdentityId,
                        principalSchema: "Account",
                        principalTable: "Identity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActionLog",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentityId = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Info = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionLog_Identity_IdentityId",
                        column: x => x.IdentityId,
                        principalSchema: "Account",
                        principalTable: "Identity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "History",
                schema: "Revision",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActionType = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    ActionTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdentityId = table.Column<int>(type: "int", nullable: false),
                    SubjectName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    JsonData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_History", x => x.Id);
                    table.ForeignKey(
                        name: "FK_History_Identity_IdentityId",
                        column: x => x.IdentityId,
                        principalSchema: "Account",
                        principalTable: "Identity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LoginSession",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentityId = table.Column<int>(type: "int", nullable: false),
                    SessionToken = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    LoginTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastAccess = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LogoutTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OptionalInfo = table.Column<string>(type: "nvarchar(max)", maxLength: 4096, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginSession", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoginSession_Identity_IdentityId",
                        column: x => x.IdentityId,
                        principalSchema: "Account",
                        principalTable: "Identity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentityId = table.Column<int>(type: "int", nullable: false),
                    Firstname = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Lastname = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Identity_IdentityId",
                        column: x => x.IdentityId,
                        principalSchema: "Account",
                        principalTable: "Identity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityXRole",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentityId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityXRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IdentityXRole_Identity_IdentityId",
                        column: x => x.IdentityId,
                        principalSchema: "Account",
                        principalTable: "Identity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IdentityXRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Account",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Track",
                schema: "MusicStore",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AlbumId = table.Column<int>(type: "int", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Composer = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Milliseconds = table.Column<long>(type: "bigint", nullable: false),
                    Bytes = table.Column<long>(type: "bigint", nullable: false),
                    UnitPrice = table.Column<double>(type: "float", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Track", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Track_Album_AlbumId",
                        column: x => x.AlbumId,
                        principalSchema: "MusicStore",
                        principalTable: "Album",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Track_Genre_GenreId",
                        column: x => x.GenreId,
                        principalSchema: "MusicStore",
                        principalTable: "Genre",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Access_IdentityId",
                schema: "Account",
                table: "Access",
                column: "IdentityId");

            migrationBuilder.CreateIndex(
                name: "IX_Access_Key",
                schema: "Account",
                table: "Access",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActionLog_IdentityId",
                schema: "Account",
                table: "ActionLog",
                column: "IdentityId");

            migrationBuilder.CreateIndex(
                name: "IX_Album_ArtistId",
                schema: "MusicStore",
                table: "Album",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_Album_Title",
                schema: "MusicStore",
                table: "Album",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Artist_Name",
                schema: "MusicStore",
                table: "Artist",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Genre_Name",
                schema: "MusicStore",
                table: "Genre",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_History_IdentityId",
                schema: "Revision",
                table: "History",
                column: "IdentityId");

            migrationBuilder.CreateIndex(
                name: "IX_Identity_Email",
                schema: "Account",
                table: "Identity",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Identity_Name",
                schema: "Account",
                table: "Identity",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IdentityXRole_IdentityId",
                schema: "Account",
                table: "IdentityXRole",
                column: "IdentityId");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityXRole_RoleId",
                schema: "Account",
                table: "IdentityXRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_LoginSession_IdentityId",
                schema: "Account",
                table: "LoginSession",
                column: "IdentityId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_Designation",
                schema: "Account",
                table: "Role",
                column: "Designation",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Track_AlbumId",
                schema: "MusicStore",
                table: "Track",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_Track_GenreId",
                schema: "MusicStore",
                table: "Track",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_Track_Title",
                schema: "MusicStore",
                table: "Track",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_User_IdentityId",
                schema: "Account",
                table: "User",
                column: "IdentityId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Access",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "ActionLog",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "History",
                schema: "Revision");

            migrationBuilder.DropTable(
                name: "IdentityXRole",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "LoginSession",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "Track",
                schema: "MusicStore");

            migrationBuilder.DropTable(
                name: "User",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "Album",
                schema: "MusicStore");

            migrationBuilder.DropTable(
                name: "Genre",
                schema: "MusicStore");

            migrationBuilder.DropTable(
                name: "Identity",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "Artist",
                schema: "MusicStore");
        }
    }
}
