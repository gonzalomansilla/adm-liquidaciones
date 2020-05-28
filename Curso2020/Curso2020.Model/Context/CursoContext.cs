using System.Linq;
using Curso2020.Model.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;

namespace Curso.Model.Context
{
	public class CursoContext : DbContext
	{
		private readonly string _connectionString;

		public CursoContext(string connectionString)
		{
			_connectionString = connectionString;
		}

		public CursoContext(DbContextOptions<CursoContext> options) : base(options)
		{
			_connectionString = ((RelationalOptionsExtension)options.Extensions.Where(e => e is SqlServerOptionsExtension).FirstOrDefault()).ConnectionString;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlServer(_connectionString);
				base.OnConfiguring(optionsBuilder);
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<PuestoEmpresa>().HasKey(i => new { i.puestoId, i.empresaCuit });
		}

		//public DbSet<EntidadModel> nombreEntidad { get; set; }
		public DbSet<Liquidacion> Liquidaciones { get; set; }
		public DbSet<Autorizacion> Autorizaciones { get; set; }
		public DbSet<Empresa> Empresas { get; set; }
		public DbSet<PuestoEmpresa> PuestosEmpresa { get; set; }
		public DbSet<Puesto> Puestos { get; set; }
		public DbSet<Empleado> Empleados { get; set; }
		public DbSet<ArchivoEmpleados> ArchivosEmpleados { get; set; }
	}
}
