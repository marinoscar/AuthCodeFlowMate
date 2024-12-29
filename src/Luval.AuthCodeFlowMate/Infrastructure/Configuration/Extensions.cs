using Google.Apis.Logging;
using Luval.AuthCodeFlowMate.Core.Service;
using Luval.AuthCodeFlowMate.Infrastructure.Data;
using Luval.AuthCodeFlowMate.Infrastructure.Interfaces;
using Luval.AuthMate.Core.Interfaces;
using Luval.AuthMate.Core.Resolver;
using Luval.AuthMate.Infrastructure.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.AuthCodeFlowMate.Infrastructure.Configuration
{
    public static class Extensions
    {
        public static IServiceCollection AddAuthCodeFlowMate(this IServiceCollection s, string clientId, string clientSecret, string redirectUrl)
        {
            s.AddScoped<IUserResolver, WebUserResolver>();
            s.AddScoped<AuthCodeFlowOptions>((c) => { 
                return new AuthCodeFlowOptions()
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret,
                    RedirectUri = redirectUrl
                };
            });
            s.AddScoped<IAuthCodeFlowDbContext, SqliteAuthCodeFlowDbContext>();
            s.AddScoped<AuthCodeFlowStorageService>();
            s.AddScoped<GoogleAuthCodeFlowService>();

            return s;
        }
    }
}
