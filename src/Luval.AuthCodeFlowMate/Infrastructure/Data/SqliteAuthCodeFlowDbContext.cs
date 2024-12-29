using Luval.AuthCodeFlowMate.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.AuthCodeFlowMate.Infrastructure.Data
{
    public class SqliteAuthCodeFlowDbContext : AuthCodeFlowDbContext, IAuthCodeFlowDbContext
    {
        private readonly string? _connectionString;

        
        public SqliteAuthCodeFlowDbContext() : this("Data Source=app.db")
        {
        }

        
        public SqliteAuthCodeFlowDbContext(DbContextOptions<AuthCodeFlowDbContext> options) : base(options)
        {
        }

        
        public SqliteAuthCodeFlowDbContext(string connectionString) : base(new DbContextOptionsBuilder<AuthCodeFlowDbContext>().UseSqlite(connectionString).Options)
        {
            _connectionString = connectionString ?? throw new ArgumentException(nameof(connectionString));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Continue with regular implementation
            base.OnConfiguring(optionsBuilder);

            // Add connection string if provided
            if (!string.IsNullOrEmpty(_connectionString))
                optionsBuilder.UseSqlite(_connectionString);
        }
    }
}
