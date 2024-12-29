using Luval.AuthCodeFlowMate.Core.Entities;
using Luval.AuthCodeFlowMate.Infrastructure.Interfaces;
using Luval.AuthMate.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Luval.AuthCodeFlowMate.Core.Service
{
    public class AuthCodeFlowStorageService
    {
        private readonly ILogger _logger;
        private readonly IAuthCodeFlowDbContext _context;
        private readonly IUserResolver _userResolver;
        public AuthCodeFlowStorageService(IAuthCodeFlowDbContext context, IUserResolver userResolver, ILogger<AuthCodeFlowStorageService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); 
            _userResolver = userResolver ?? throw new ArgumentNullException(nameof(userResolver));
        }

        public IQueryable<Integration> GetIntegrations(Expression<Func<Integration, bool>> filterExpression)
        {
            if(filterExpression == null) throw new ArgumentNullException(nameof(filterExpression));
            var query = _context.Integrations.Where(filterExpression);
            return query;
        }

        public async Task AddIntegration(Integration integration, CancellationToken cancellationToken = default)
        {
            if (integration == null) throw new ArgumentNullException(nameof(integration));
            
            integration.CreatedBy = _userResolver.GetUserEmail();
            integration.UtcCreatedOn = DateTime.UtcNow;
            integration.UtcUpdatedOn = DateTime.UtcNow;
            integration.UpdatedBy = _userResolver.GetUserEmail();

            await _context.Integrations.AddAsync(integration);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateIntegration(Integration integration, CancellationToken cancellationToken = default)
        {
            if (integration == null) throw new ArgumentNullException(nameof(integration));
            integration.UtcUpdatedOn = DateTime.UtcNow;
            integration.UpdatedBy = _userResolver.GetUserEmail();
            _context.Integrations.Update(integration);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteIntegration(Integration integration, CancellationToken cancellationToken = default)
        {
            if (integration == null) throw new ArgumentNullException(nameof(integration));
            _context.Integrations.Remove(integration);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
