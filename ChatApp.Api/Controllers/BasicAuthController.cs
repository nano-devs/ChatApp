using AuthEndpoints.Controllers;
using ChatApp.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace ChatApp.Api.Controllers;

public class BasicAuthController : BaseEndpointsController<int, User>
{
    public BasicAuthController(UserManager<User> userManager, IdentityErrorDescriber errorDescriber) : base(userManager, errorDescriber)
    {
    }
}
