using Luval.AuthCodeFlowMate.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Luval.AuthCodeFlowMate.Infrastructure.Interfaces
{
    public interface IAuthCodeFlowDbContext
    {
        DbSet<Integration> Integrations { get; set; }

        public DatabaseFacade Database { get; }

        public int SaveChanges();

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}