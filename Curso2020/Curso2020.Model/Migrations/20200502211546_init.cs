using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Curso2020.Model.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArchivosEmpleados",
                columns: table => new
                {
                    nombre = table.Column<string>(nullable: false),
                    fechaDeProcesado = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArchivosEmpleados", x => x.nombre);
                });

            migrationBuilder.CreateTable(
                name: "Autorizaciones",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cuitEmpresa = table.Column<string>(maxLength: 11, nullable: false),
                    fecha = table.Column<string>(maxLength: 7, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autorizaciones", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Empresas",
                columns: table => new
                {
                    cuit = table.Column<string>(maxLength: 11, nullable: false),
                    nombre = table.Column<string>(maxLength: 25, nullable: false),
                    razonSocial = table.Column<string>(maxLength: 50, nullable: false),
                    direccion = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresas", x => x.cuit);
                });

            migrationBuilder.CreateTable(
                name: "Liquidaciones",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cuitEmpresa = table.Column<string>(maxLength: 11, nullable: false),
                    dniEmpleado = table.Column<string>(maxLength: 8, nullable: false),
                    liquidacion = table.Column<double>(nullable: false),
                    fecha = table.Column<string>(maxLength: 7, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Liquidaciones", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Puestos",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    descripcion = table.Column<string>(nullable: false),
                    salarioPorDefecto = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Puestos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Empleados",
                columns: table => new
                {
                    dni = table.Column<string>(maxLength: 8, nullable: false),
                    cuitEmpresa = table.Column<string>(nullable: false),
                    idPuesto = table.Column<int>(nullable: false),
                    archivo = table.Column<string>(nullable: true),
                    nombre = table.Column<string>(maxLength: 30, nullable: false),
                    apellido = table.Column<string>(maxLength: 30, nullable: false),
                    horasTrabajadasUltimoMes = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleados", x => x.dni);
                    table.ForeignKey(
                        name: "FK_Empleados_ArchivosEmpleados_archivo",
                        column: x => x.archivo,
                        principalTable: "ArchivosEmpleados",
                        principalColumn: "nombre",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Empleados_Empresas_cuitEmpresa",
                        column: x => x.cuitEmpresa,
                        principalTable: "Empresas",
                        principalColumn: "cuit",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Empleados_Puestos_idPuesto",
                        column: x => x.idPuesto,
                        principalTable: "Puestos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PuestosEmpresa",
                columns: table => new
                {
                    puestoId = table.Column<int>(nullable: false),
                    empresaCuit = table.Column<string>(nullable: false),
                    pagoPorHora = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PuestosEmpresa", x => new { x.puestoId, x.empresaCuit });
                    table.ForeignKey(
                        name: "FK_PuestosEmpresa_Empresas_empresaCuit",
                        column: x => x.empresaCuit,
                        principalTable: "Empresas",
                        principalColumn: "cuit",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PuestosEmpresa_Puestos_puestoId",
                        column: x => x.puestoId,
                        principalTable: "Puestos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_archivo",
                table: "Empleados",
                column: "archivo");

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_cuitEmpresa",
                table: "Empleados",
                column: "cuitEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_idPuesto",
                table: "Empleados",
                column: "idPuesto");

            migrationBuilder.CreateIndex(
                name: "IX_PuestosEmpresa_empresaCuit",
                table: "PuestosEmpresa",
                column: "empresaCuit");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Autorizaciones");

            migrationBuilder.DropTable(
                name: "Empleados");

            migrationBuilder.DropTable(
                name: "Liquidaciones");

            migrationBuilder.DropTable(
                name: "PuestosEmpresa");

            migrationBuilder.DropTable(
                name: "ArchivosEmpleados");

            migrationBuilder.DropTable(
                name: "Empresas");

            migrationBuilder.DropTable(
                name: "Puestos");
        }
    }
}
