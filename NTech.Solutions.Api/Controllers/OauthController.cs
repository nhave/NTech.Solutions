using Microsoft.AspNetCore.Mvc;
using NTech.Solutions.Api.HtmlBuilder;
using NTech.Solutions.Api.HtmlBuilder.Scripts;
using NTech.Solutions.Api.Services;
using NTech.Solutions.Common.Models.Database;

namespace NTech.Solutions.Api.Controllers
{
    /// <summary>
    /// Provides endpoints for initiating and handling OAuth authentication flows with external providers such as Google
    /// and GitHub.
    /// </summary>
    /// <remarks>This controller exposes endpoints to start the OAuth login process and to handle callbacks
    /// from supported external authentication providers. It is intended to be used as part of an OAuth-based
    /// authentication workflow in web applications. The controller relies on application configuration and a universal
    /// login service to manage provider-specific details and user authentication.</remarks>
    [ApiController]
    [Route("[controller]")]
    public class OauthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly UniversalLoginService _login;
        private readonly IJwtService _jwtService;

        /// <summary>
        /// Initializes a new instance of the OauthController class with the specified configuration and login service.
        /// </summary>
        /// <param name="config">The application configuration settings used to initialize the controller. Cannot be null.</param>
        /// <param name="login">The service responsible for handling universal login operations. Cannot be null.</param>
        /// <param name="jwtService">The service responsible for generating JSON Web Tokens (JWTs) for authenticated users. Cannot be null.</param>
        public OauthController(IConfiguration config, UniversalLoginService login, IJwtService jwtService)
        {
            this._config = config;
            this._login = login;
            this._jwtService = jwtService;
        }

        /// <summary>
        /// Initiates the OAuth login process by redirecting the user to the specified external authentication provider.
        /// </summary>
        /// <remarks>This endpoint is typically used to start the OAuth authentication flow with supported
        /// providers. The client is redirected to the provider's authorization page, where the user can grant access.
        /// After successful authentication, the provider will redirect the user back to the application's configured
        /// callback URL.</remarks>
        /// <param name="provider">The name of the external authentication provider to use for login. Supported values are "google" and
        /// "github". The value is case-sensitive.</param>
        /// <returns>A redirect result to the external provider's login page if the provider is supported; otherwise, a bad
        /// request result indicating an unknown provider.</returns>
        [HttpGet("Login/{provider}")]
        public IActionResult Login(string provider)
        {
            if (provider == "google")
            {
                var url =
                    "https://accounts.google.com/o/oauth2/v2/auth" +
                    "?client_id=" + _config["OAuth:Google:ClientId"] +
                    "&redirect_uri=" + _config["OAuth:Google:RedirectUri"] +
                    "&response_type=code" +
                    "&scope=openid%20email%20profile";

                return Redirect(url);
            }
            else if (provider == "github")
            {
                var url =
                    "https://github.com/login/oauth/authorize" +
                    "?client_id=" + _config["OAuth:GitHub:ClientId"] +
                    "&redirect_uri=" + _config["OAuth:GitHub:RedirectUri"] +
                    "&scope=user:email";

                return Redirect(url);
            }

            return BadRequest("Unknown provider");
        }

        /// <summary>
        /// Handles the callback from an external authentication provider after a user attempts to sign in.
        /// </summary>
        /// <remarks>This endpoint is typically invoked by the external provider as part of the OAuth or
        /// OpenID Connect authentication flow. Either <paramref name="code"/> or <paramref name="error"/> is expected
        /// to be provided in the callback query parameters.</remarks>
        /// <param name="provider">The name of the external authentication provider (for example, "Google" or "Facebook").</param>
        /// <param name="code">The authorization code returned by the provider if authentication was successful; otherwise, <see
        /// langword="null"/>.</param>
        /// <param name="error">The error message returned by the provider if authentication failed; otherwise, <see langword="null"/>.</param>
        /// <returns>An <see cref="IActionResult"/> that represents the result of the authentication callback. Returns a redirect
        /// or error response depending on the outcome.</returns>
        [HttpGet("Callback/{provider}")]
        public async Task<IActionResult> Callback(string provider, string? code, string? error)
        {
            if (error != null)
            {
                return await OnLoginFailed(provider, error);
            }
            else if (code != null)
            {
                return await OnLoginSuccess(provider, code);
            }
            else
            {
                return BadRequest("Invalid callback parameters");
            }
        }

        private async Task<IActionResult> OnLoginSuccess(string provider, string code)
        {
            var redirectUri = _config[$"OAuth:{provider}:RedirectUri"];
            if (redirectUri == null)
            {
                return BadRequest("Invalid provider");
            }

            var (tokens, oauthUser) = await _login.LoginWithCodeAsync(provider, code, redirectUri);

            var user = new User
            {
                Email = oauthUser.Email
            };
            var jwt = _jwtService.GenerateJwtToken(user);

            return Content(new HtmlBuilder.HtmlBuilder
            {
                Title = "Login Successful",
                Body = new()
                {
                    new HtmlElement("script")
                    {
                        InnerText = $"window.opener.postMessage({{type:'auth-complete',token: '{jwt}'}}, '*');"
                    },
                    new MessageScript("auth-complete", oauthUser),
                    new HtmlElement("p")
                    {
                        InnerText = "Login successful. You can close this window."
                    }
                }
            }.Render(), "text/html");
        }

        private async Task<IActionResult> OnLoginFailed(string provider, string error)
        {
            return Content(new HtmlBuilder.HtmlBuilder
            {
                Title = "Login Failed",
                Body = new()
                    {
                        new HtmlElement("p")
                        {
                            InnerText = $"Login failed. You can close this window."
                        }
                    }
            }.Render(), "text/html");
        }
    }
}
