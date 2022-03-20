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
                name: "Test");

            migrationBuilder.EnsureSchema(
                name: "UnitTest");

            migrationBuilder.CreateTable(
                name: "Another",
                schema: "Test",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Detail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Another", x => x.Id);
                });

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
                name: "One",
                schema: "Test",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_One", x => x.Id);
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
                name: "OneXAnother",
                schema: "Test",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OneId = table.Column<int>(type: "int", nullable: false),
                    AnotherId = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OneXAnother", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OneXAnother_Another_AnotherId",
                        column: x => x.AnotherId,
                        principalSchema: "Test",
                        principalTable: "Another",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OneXAnother_One_OneId",
                        column: x => x.OneId,
                        principalSchema: "Test",
                        principalTable: "One",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                name: "IX_OneXAnother_AnotherId",
                schema: "Test",
                table: "OneXAnother",
                column: "AnotherId");

            migrationBuilder.CreateIndex(
                name: "IX_OneXAnother_OneId",
                schema: "Test",
                table: "OneXAnother",
                column: "OneId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "OneXAnother",
                schema: "Test");

            migrationBuilder.DropTable(
                name: "Master",
                schema: "Test");

            migrationBuilder.DropTable(
                name: "Another",
                schema: "Test");

            migrationBuilder.DropTable(
                name: "One",
                schema: "Test");
        }
    }
}
