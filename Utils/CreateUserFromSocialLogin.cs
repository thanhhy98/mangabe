using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using WebApplication3.Models;
using WebApplication3.DTOs;
using WebApplication3.Enums;

namespace WebApplication3.Utils;

public static class CreateUserFromSocialLoginExtension
{
    /// <summary>
    /// Creates user from social login
    /// </summary>
    /// <param name="userManager">the usermanager</param>
    /// <param name="context">the context</param>
    /// <param name="model">the model</param>
    /// <param name="loginProvider">the login provider</param>
    /// <returns>System.Threading.Tasks.Task&lt;User&gt;</returns>
        
    public static Task<User> CreateUserFromSocialLogin(this UserManager<User> userManager, TodoContext context, User model, LoginProvider loginProvider)
    {
            return null;
    }
}