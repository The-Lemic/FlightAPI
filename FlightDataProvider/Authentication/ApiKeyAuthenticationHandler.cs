using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace FlightDataProvider.Authentication
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        private readonly IConfiguration _configuration;

        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<ApiKeyAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            IConfiguration configuration)
            : base(options, logger, encoder)
        {
            _configuration = configuration;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Check if the header exists
            if (!Request.Headers.TryGetValue(Options.HeaderName, out var apiKeyHeaderValues))
            {
                return Task.FromResult(AuthenticateResult.Fail("API Key header not found"));
            }

            var providedApiKey = apiKeyHeaderValues.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(providedApiKey))
            {
                return Task.FromResult(AuthenticateResult.Fail("API Key is empty"));
            }

            // Get the valid API key from configuration
            var validApiKey = _configuration["ApiKey"];

            if (string.IsNullOrWhiteSpace(validApiKey))
            {
                Logger.LogError("API Key is not configured in application settings");
                return Task.FromResult(AuthenticateResult.Fail("API Key authentication is not properly configured"));
            }

            // Compare the provided key with the valid key
            if (!providedApiKey.Equals(validApiKey, StringComparison.Ordinal))
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid API Key"));
            }

            // Authentication successful - create claims principal
            var claims = new[] { new Claim(ClaimTypes.Name, "ApiKeyUser") };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
