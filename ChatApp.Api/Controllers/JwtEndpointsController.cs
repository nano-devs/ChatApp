using AuthEndpoints;
using AuthEndpoints.Controllers;
using AuthEndpoints.Services;
using ChatApp.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace ChatApp.Api.Controllers;

public class JwtEndpointsController : JwtController<int, User>
{
    public JwtEndpointsController(UserManager<User> userManager, 
        IAuthenticator<User> authenticator, 
        IJwtValidator jwtValidator, 
        IOptions<AuthEndpointsOptions> options) : base(userManager, authenticator, jwtValidator, options)
    {
    }
}
