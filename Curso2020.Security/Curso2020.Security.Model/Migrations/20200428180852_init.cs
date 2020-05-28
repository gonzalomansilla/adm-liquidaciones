using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Curso2020.Security.Model.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dniCuit = table.Column<string>(nullable: false),
                    rol = table.Column<string>(maxLength: 20, nullable: false),
                    nombreUsuario = table.Column<string>(maxLength: 50, nullable: false),
                    contrasenia = table.Column<string>(maxLength: 50, nullable: false),
                    fechaUltimoIngreso = table.Column<DateTime>(nullable: false),
                    guid = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
