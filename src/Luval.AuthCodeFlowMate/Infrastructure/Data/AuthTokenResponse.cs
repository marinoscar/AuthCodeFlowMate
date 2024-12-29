using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace Luval.AuthCodeFlowMate.Infrastructure.Data
{
    public class AuthTokenResponse
    {
        /// <summary>Gets or sets the access token issued by the authorization server.</summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the token type as specified in http://tools.ietf.org/html/rfc6749#section-7.1.
        /// </summary>
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        /// <summary>Gets or sets the lifetime in seconds of the access token.</summary>
        [JsonProperty("expires_in")]
        public long? ExpiresInSeconds { get; set; }

        /// <summary>
        /// Gets or sets the refresh token which can be used to obtain a new access token.
        /// For example, the value "3600" denotes that the access token will expire in one hour from the time the 
        /// response was generated.
        /// </summary>
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        /// <summary>
        /// Gets or sets the scope of the access token as specified in http://tools.ietf.org/html/rfc6749#section-3.3.
        /// </summary>
        [JsonProperty("scope")]
        public string Scope { get; set; }

        /// <summary>
        /// Gets or sets the id_token, which is a JSON Web Token (JWT) as specified in http://tools.ietf.org/html/draft-ietf-oauth-json-web-token
        /// </summary>
        [JsonProperty("id_token")]
        public string IdToken { get; set; }

        /// <summary>
        /// The date and time that this token was issued, expressed in UTC.
        /// </summary>
        /// <remarks>
        /// This should be set by the CLIENT after the token was received from the server.
        /// </remarks>
        [JsonProperty(Order = 2)]
        public DateTime IssuedUtc { get; set; }

        public override string ToString()
        {
            return System.Text.Json.JsonSerializer.Serialize(this, new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            });
        }
    }
}
