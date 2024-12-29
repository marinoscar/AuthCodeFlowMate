using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.AuthCodeFlowMate.Infrastructure.Configuration
{
    public class AuthCodeFlowOptions
    {
        public string ClientId { get; set; } = default!;
        public string ClientSecret { get; set; } = default!;

        public string RedirectUri { get; set; } = default!;

        public string AccessType { get; set; } = "offline";
        public List<string> Scopes { get; set; } = 
            new List<string>() { "https://www.googleapis.com/auth/userinfo.profile", "https://www.googleapis.com/auth/userinfo.email", "https://www.googleapis.com/auth/gmail.readonly" };

        public string ProviderName { get; set; } = "Google";
    }
}
