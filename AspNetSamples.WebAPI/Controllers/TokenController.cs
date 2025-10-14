using AspNetSamples.Core.Dto;
using AspNetSamples.Models;
using AspNetSamples.Services.Abstractions;
using AspNetSamples.WebAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetSamples.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ILogger<TokenController> _logger;
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public TokenController(ILogger<TokenController> logger, IUserService userService, ITokenService tokenService)
        {
            _logger = logger;
            _userService = userService;
            _tokenService = tokenService;
        }


        [HttpPost]
        [Route("login")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(TokenModel))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> Login([FromBody]LoginModel model)
        {
            try
            {
                var user = await _userService.TryToLoginUserAsync(model.Email, model.Password);
                if (user == null)
                {
                    return Unauthorized();
                }

                var clientIp = HttpContext.Connection.RemoteIpAddress?.MapToIPv6();
                var jwtToken = _tokenService.GenerateJwtToken(user);
                var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user.Id);
                var token = new TokenModel
                {
                    AccessToken = jwtToken,
                    RefreshToken = refreshToken
                };
                return Ok(token);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error during login for user {Email}", model.Email);
                return StatusCode(500, new ErrorModel()
                {
                    Message = e.Message
                });
            }
            
        }

        [HttpPost]
        [Route("refresh")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(TokenModel))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenModel model)
        {
            try
            {
                var user = await _userService.TryToLoginUserByRefreshTokenAsync(model.RefreshToken);
                if (user == null)
                {
                    return Unauthorized();
                }

                await _tokenService.RemoveTokenAsync(model.RefreshToken);
                var jwtToken = _tokenService.GenerateJwtToken(user);
                var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user.Id);
                var token = new TokenModel
                {
                    AccessToken = jwtToken,
                    RefreshToken = refreshToken
                };
                return Ok(token);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error during login for user by refresh token");
                return StatusCode(500, new ErrorModel()
                {
                    Message = e.Message
                });
            }

        }


        [HttpPost]
        [Route("revoke")]
        [Authorize]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> Revoke([FromBody] RefreshTokenModel model)
        {
            try
            {
                var rtUser = await _userService.TryToLoginUserByRefreshTokenAsync(model.RefreshToken);
                var userIdClaim = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value);
                if (rtUser == null || rtUser.Id != userIdClaim)
                {
                    return Unauthorized();
                }

                await _tokenService.RevokeAsync(model.RefreshToken);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error during login for user by refresh token");
                return StatusCode(500, new ErrorModel()
                {
                    Message = e.Message
                });
            }

        }
    }
}
