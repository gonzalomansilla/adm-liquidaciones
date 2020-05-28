using Curso2020.Security.Model.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using System.Linq;

namespace Curso2020.Security.Model.Context
{
    public class LoginContext:DbContext 
    {
        private readonly string _connectionString;

        public LoginContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        public LoginContext(DbContextOptions<LoginContext> options) : base(options)
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
        }

        public DbSet<Usuario> Usuarios { get; set; }
    }
}

