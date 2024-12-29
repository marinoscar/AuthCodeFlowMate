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
    public class AuthCodeFlowDbContext : DbContext, IAuthCodeFlowDbContext
    {

        /// <summary>
        /// DbSet for the Integration entity.
        /// </summary>
        public DbSet<Integration> Integrations { get; set; }

        /// <summary>
        /// Configures the database context options.
        /// </summary>
        /// <param name="options">Options for configuring the DbContext.</param>
        public AuthCodeFlowDbContext(DbContextOptions<AuthCodeFlowDbContext> options) : base(options) { }

        /// <summary>
        /// Configures the entity mappings and relationships.
        /// </summary>
        /// <param name="modelBuilder">Model builder for configuring entity relationships.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure ChatMessage relationship with Chatbot
            modelBuilder.Entity<Integration>()
               .HasKey(x => x.Id);

        }
    }
}
