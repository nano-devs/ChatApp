using AuthEndpoints;
using AuthEndpoints.Controllers;
using AuthEndpoints.Services;
using ChatApp.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace ChatApp.Api.Controllers;

public class BasicAuthController : BaseEndpointsController<int, User>
{
    public BasicAuthController(UserManager<User> userManager, IdentityErrorDescriber errorDescriber, IOptions<AuthEndpointsOptions> options, IEmailSender emailSender, IEmailFactory emailFactory) : base(userManager, errorDescriber, options, emailSender, emailFactory)
    {
    }
}
