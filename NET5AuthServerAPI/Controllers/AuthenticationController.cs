using Microsoft.AspNetCore.Mvc;
using NET5AuthServerAPI.Models;
using NET5AuthServerAPI.Models.Requests;
using NET5AuthServerAPI.Models.Responses;
using NET5AuthServerAPI.Services.PasswordHashers;
using NET5AuthServerAPI.Services.TokenGenerators;
using NET5AuthServerAPI.Services.TokenValidators;
using NET5AuthServerAPI.Services.UserRepositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NET5AuthServerAPI.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly IPasswordHasher passwordHasher;
        private readonly ITokenGenerator accessTokenGenerator;
        private readonly ITokenGenerator refreshTokenGenerator;

        // dependency inversion!
        private readonly RefreshTokenValidator refreshTokenValidator;

        public AuthenticationController(IUserRepository userRepository, 
            IPasswordHasher passwordHasher, 
            AccessTokenGenerator accessTokenGenerator, 
            RefreshTokenGenerator refreshTokenGenerator,
            RefreshTokenValidator refreshTokenValidator)
        {
            this.userRepository = userRepository;
            this.passwordHasher = passwordHasher;
            this.accessTokenGenerator = accessTokenGenerator;
            this.refreshTokenGenerator = refreshTokenGenerator;
            this.refreshTokenValidator = refreshTokenValidator;
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

            User existingUserByEmail = await userRepository.GetByEmail(registerRequest.Email);
            if (existingUserByEmail != null)
            {
                return Conflict(new ErrorResponse("Email already exists."));
            }

            User existingUserByUsername = await userRepository.GetByUserName(registerRequest.Username);
            if (existingUserByUsername != null)
            {
                return Conflict(new ErrorResponse("Username already exists."));
            }

            string passwordHash = passwordHasher.HashPassword(registerRequest.Password);
            User registrationUser = new User()
            {
                Email = registerRequest.Email,
                Username = registerRequest.Username,
                PasswordHash = passwordHash,
            };

            await userRepository.Create(registrationUser);

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            User user = await userRepository.GetByUserName(loginRequest.Username);
            if (user == null)
            {
                return Unauthorized();
            }

            bool correctPassword =  passwordHasher.VerifyPassword(loginRequest.Password, user.PasswordHash);
            if (!correctPassword)
            {
                return Unauthorized();
            }

            string accessToken = accessTokenGenerator.GenerateToken(user);
            string refreshToken = refreshTokenGenerator.GenerateToken(user);

            return Ok(new AuthenticatedUserResponse()
            { 
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
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

            // update token validator, store user id & refresh token to database.

            return Ok();
        }

        private IActionResult BadRequestModelState()
        {
            IEnumerable<string> errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
            return BadRequest(new ErrorResponse(errors));
        }
    }
}
