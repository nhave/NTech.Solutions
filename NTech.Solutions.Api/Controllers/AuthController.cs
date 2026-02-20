using Microsoft.AspNetCore.Mvc;
using NTech.Solutions.Api.Services;
using NTech.Solutions.Common.Models.Dtos;

namespace NTech.Solutions.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        /// <summary>
        /// Registers a new user account using the specified registration details.
        /// </summary>
        /// <param name="dto">An object containing the user's registration information, such as username, password, and other required
        /// fields. Cannot be null.</param>
        /// <returns>An <see cref="ActionResult{RegisterResponseDto}"/> containing the result of the registration operation.
        /// Returns a 201 Created response with registration details if successful; otherwise, returns an appropriate
        /// error response.</returns>
        [HttpPost("Register")]
        public async Task<ActionResult<RegisterResponseDto>> Register(RegisterDto dto)
        {
            try
            {
                return await authService.RegisterAsync(dto);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Authenticates a user with the provided credentials and returns an authentication response if successful.
        /// </summary>
        /// <param name="dto">The login credentials and related information for the user attempting to authenticate. Cannot be null.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing an <see cref="AuthResponseDto"/> if authentication is
        /// successful; otherwise, a response indicating the reason for failure, such as a bad request for invalid
        /// credentials or a server error for unexpected failures.</returns>
        [HttpPost("Login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
        {
            try
            {
                return await authService.LoginAsync(dto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Generates a new authentication token pair using the provided refresh token.
        /// </summary>
        /// <param name="dto">An object containing the refresh token and related information required to obtain new authentication tokens.
        /// Cannot be null.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing an <see cref="AuthResponseDto"/> with the new authentication
        /// tokens if the refresh is successful; otherwise, a bad request or server error response.</returns>
        [HttpPost("Refresh")]
        public async Task<ActionResult<AuthResponseDto>> Refresh(RefreshTokenDto dto)
        {
            try
            {
                return await authService.RefreshAsync(dto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
