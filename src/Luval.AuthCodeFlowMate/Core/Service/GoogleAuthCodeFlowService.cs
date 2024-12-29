using Luval.AuthCodeFlowMate.Core.Entities;
using Luval.AuthCodeFlowMate.Infrastructure.Configuration;
using Luval.AuthCodeFlowMate.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Luval.AuthCodeFlowMate.Core.Service
{



    public class GoogleAuthCodeFlowService
    {

        /*
         * Scopes to request
         *  https://www.googleapis.com/auth/userinfo.profile => Access the user's profile information, including their name and profile picture.
         *  https://www.googleapis.com/auth/userinfo.email   => Access the user's email address.
         *  https://www.googleapis.com/auth/gmail.readonly   => Read the user's Gmail messages and metadata in a read-only mode.
         */

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AuthCodeFlowStorageService _flowStorageService;
        private readonly ILogger _logger;

        public AuthCodeFlowOptions Options { get; private set; }

        public GoogleAuthCodeFlowService(AuthCodeFlowOptions options, AuthCodeFlowStorageService flowStorageService, IHttpClientFactory httpClientFactory, ILogger<GoogleAuthCodeFlowService> logger)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _flowStorageService = flowStorageService ?? throw new ArgumentNullException(nameof(flowStorageService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public virtual string CreateAuthorizationConsentUrl()
        {
            var scopes = string.Join(" ", Options.Scopes);
            return string.Format(@"https://accounts.google.com/o/oauth2/v2/auth?response_type=code&client_id={0}&redirect_uri={1}&scope={2}&access_type={3}&prompt=consent",
                Options.ClientId, Options.RedirectUri, scopes, Options.AccessType);
        }

        public async Task<AuthTokenResponse> CretaeAuthorizationCodeRequestAsync(string code, string? error, CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrEmpty(error)) throw new InvalidOperationException(error);
            if (string.IsNullOrEmpty(code)) throw new ArgumentNullException(nameof(code));

            var client = _httpClientFactory.CreateClient();
            var tokenEndpoint = "https://oauth2.googleapis.com/token";
            var tokenRequestBody = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("client_id", Options.ClientId),
                new KeyValuePair<string, string>("client_secret", Options.ClientSecret),
                new KeyValuePair<string, string>("redirect_uri", Options.RedirectUri),
                new KeyValuePair<string, string>("grant_type", "authorization_code")
            });

            var tokenResponse = await client.PostAsync(tokenEndpoint, tokenRequestBody, cancellationToken);

            if (!tokenResponse.IsSuccessStatusCode)
                throw new InvalidOperationException("Failed to get token");

            var tokenResponseContent = await tokenResponse.Content.ReadAsStringAsync();
            var tokenData = JsonSerializer.Deserialize<AuthTokenResponse>(tokenResponseContent);

            if (tokenData == null)
                throw new InvalidOperationException("Failed to parse token response");

            return tokenData;
        }

        public async Task<Integration> PersistIntegrationAsync(string code, string? error, CancellationToken cancellationToken = default)
        {
            var tokenData = await CretaeAuthorizationCodeRequestAsync(code, error, cancellationToken);
            return await PersistIntegrationAsync(tokenData, cancellationToken);
        }

        public async Task<Integration> PersistIntegrationAsync(AuthTokenResponse tokenData, CancellationToken cancellationToken = default)
        {
            var integration = new Integration
            {
                ProviderName = Options.ProviderName,
                AccessToken = tokenData.AccessToken,
                RefreshToken = tokenData.RefreshToken,
                UtcExpiresOn = tokenData.IssuedUtc.AddSeconds((double)tokenData.ExpiresInSeconds),
                DurationInSeconds = tokenData.ExpiresInSeconds ?? 0,
                TokenType = tokenData.TokenType
            };
            await _flowStorageService.AddIntegration(integration, cancellationToken);
            return integration;
        }
    }
}
