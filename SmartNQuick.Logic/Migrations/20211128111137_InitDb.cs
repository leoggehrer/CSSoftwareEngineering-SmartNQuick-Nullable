using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartNQuick.Logic.Migrations
{
    public partial class InitDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Account");

            migrationBuilder.EnsureSchema(
                name: "Test");

            migrationBuilder.EnsureSchema(
                name: "UnitTest");

            migrationBuilder.CreateTable(
                name: "EditForm",
                schema: "Test",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TextBox = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TextArea = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TextBoxRequired = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TextAreaReadonly = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EnumState = table.Column<int>(type: "int", nullable: false),
                    CheckBox = table.Column<bool>(type: "bit", nullable: false),
                    CheckBoxNullable = table.Column<bool>(type: "bit", nullable: true),
                    ByteValue = table.Column<byte>(type: "tinyint", nullable: false),
                    ByteNullable = table.Column<byte>(type: "tinyint", nullable: true),
                    ShortValue = table.Column<short>(type: "smallint", nullable: false),
                    ShortNullable = table.Column<short>(type: "smallint", nullable: true),
                    IntegerValue = table.Column<int>(type: "int", nullable: false),
                    IntegerNullable = table.Column<int>(type: "int", nullable: true),
                    DoubleValue = table.Column<double>(type: "float", nullable: false),
                    DoubleNullable = table.Column<double>(type: "float", nullable: true),
                    TimeSpanValue = table.Column<TimeSpan>(type: "time", nullable: false),
                    TimeSpanNullable = table.Column<TimeSpan>(type: "time", nullable: true),
                    DateValue = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateNullable = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateTimeValue = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateTimeNullable = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EditForm", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genre",
                schema: "UnitTest",
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
                name: "Master",
                schema: "Test",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    State = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Master", x => x.Id);
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
                        onDelete: ReferentialAction.Restrict);
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
                        onDelete: ReferentialAction.Restrict);
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
                        onDelete: ReferentialAction.Restrict);
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
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Detail",
                schema: "Test",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MasterId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    State = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Detail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Detail_Master_MasterId",
                        column: x => x.MasterId,
                        principalSchema: "Test",
                        principalTable: "Master",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IdentityXRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Account",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "IX_Detail_MasterId",
                schema: "Test",
                table: "Detail",
                column: "MasterId");

            migrationBuilder.CreateIndex(
                name: "IX_Genre_Name",
                schema: "UnitTest",
                table: "Genre",
                column: "Name",
                unique: true);

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
                name: "Detail",
                schema: "Test");

            migrationBuilder.DropTable(
                name: "EditForm",
                schema: "Test");

            migrationBuilder.DropTable(
                name: "Genre",
                schema: "UnitTest");

            migrationBuilder.DropTable(
                name: "IdentityXRole",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "LoginSession",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "User",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "Master",
                schema: "Test");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "Identity",
                schema: "Account");
        }
    }
}
