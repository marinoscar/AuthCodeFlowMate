using Luval.AuthCodeFlowMate.Core.Entities;
using Luval.AuthCodeFlowMate.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.AuthCodeFlowMate.Infrastructure.Data
{
    public class PostgresCodeFlowDbContext : AuthCodeFlowDbContext, IAuthCodeFlowDbContext
    {
        private readonly string _connectionString;



        public PostgresCodeFlowDbContext(string connectionString): this(connectionString, new DbContextOptions<AuthCodeFlowDbContext>())
        {
            
        }
        public PostgresCodeFlowDbContext(string connectionString, DbContextOptions<AuthCodeFlowDbContext> options) : this(options)
        {
            _connectionString = connectionString;
        }

        public PostgresCodeFlowDbContext(DbContextOptions<AuthCodeFlowDbContext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //continue with regular implementation
            base.OnConfiguring(optionsBuilder);


            //add conn string if provided
            if (!string.IsNullOrEmpty(_connectionString))
                optionsBuilder.UseNpgsql(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Integration>()
               .Property(at => at.Id)
               .UseIdentityColumn()
               .HasColumnType("BIGINT");

        }
    }
}
