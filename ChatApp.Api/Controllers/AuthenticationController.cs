namespace ChatApp.Api.Controllers;

using ChatApp.Api.Models;
using ChatApp.Api.Models.Requests;
using ChatApp.Api.Models.Responses;
using ChatApp.Api.Services.Authenticators;
using ChatApp.Api.Services.RefreshTokenRepositories;
using ChatApp.Api.Services.TokenValidators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

public class AuthenticationController : Controller
{
    private readonly IRefreshTokenRepository refreshTokenRepository;
    private readonly ITokenValidator refreshTokenValidator;

    // dependency inversion!
    private readonly UserManager<User> userRepository;
    private readonly Authenticator authenticator;
    private readonly IdentityErrorDescriber errorDescriber;

    public AuthenticationController(UserManager<User> userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        Authenticator authenticator,
        ITokenValidator refreshTokenValidator,
        IdentityErrorDescriber errorDescriber)
    {
        this.userRepository = userRepository;
        this.refreshTokenRepository = refreshTokenRepository;
        this.authenticator = authenticator;
        this.refreshTokenValidator = refreshTokenValidator;
        this.errorDescriber = errorDescriber;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequestModelState();
        }

        if (registerRequest.Password != registerRequest.ConfirmPassword)
        {
            return BadRequest(new ErrorResponse("Password not match confirm password."));
        }

        User registrationUser = new User()
        {
            Email = registerRequest.Email,
            UserName = registerRequest.Username,
            PublicName = registerRequest.Username,
        };

        IdentityResult result = await userRepository.CreateAsync(registrationUser, registerRequest.Password);

        if (!result.Succeeded)
        {
            IdentityError? primaryError = result.Errors.FirstOrDefault();

            if (primaryError!.Code == nameof(errorDescriber.DuplicateEmail))
            {
                return Conflict(new ErrorResponse("Email already exists."));
            }
            else if (primaryError?.Code == nameof(errorDescriber.DuplicateUserName))
            {
                return Conflict(new ErrorResponse("Username already exists."));
            }
        }

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequestModelState();
        }

        User user = await userRepository.FindByNameAsync(loginRequest.Username);
        if (user == null)
        {
            return Unauthorized();
        }

        bool correctPassword = await userRepository.CheckPasswordAsync(user, loginRequest.Password);
        if (!correctPassword)
        {
            return Unauthorized();
        }

        AuthenticatedUserResponse response = await authenticator.Authenticate(user);

        return Ok(response);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest refreshRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequestModelState();
        }

        bool isValidRefreshToken = refreshTokenValidator.Validate(refreshRequest.RefreshToken);
        if (!isValidRefreshToken)
        {
            // token may be expired, invalid, etc. but this good enough for now.
            return BadRequest(new ErrorResponse("Invalid refresh token."));
        }

        RefreshToken? refreshTokenDTO = await refreshTokenRepository.GetByToken(refreshRequest.RefreshToken);
        if (refreshTokenDTO == null)
        {
            return NotFound(new ErrorResponse("Invaliid refresh token."));
        }

        await refreshTokenRepository.Delete(refreshTokenDTO.Id);

        User user = await userRepository.FindByIdAsync(refreshTokenDTO.UserId.ToString());
        if (user == null)
        {
            return NotFound(new ErrorResponse("User not found."));
        }

        AuthenticatedUserResponse response = await authenticator.Authenticate(user);

        return Ok(response);
    }

    [Authorize]
    [HttpDelete("logout")]
    public async Task<IActionResult> Logout()
    {
        string rawUserId = HttpContext.User.FindFirstValue("id");

        if (!int.TryParse(rawUserId, out int userId))
        {
            return Unauthorized();
        }

        await refreshTokenRepository.DeleteAll(userId);

        return NoContent();
    }

    private IActionResult BadRequestModelState()
    {
        IEnumerable<string> errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
        return BadRequest(new ErrorResponse(errors));
    }
}
