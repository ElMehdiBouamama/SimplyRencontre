using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SimplyRencontre.Models;

namespace SimplyRencontre.Controllers.API
{
    // TODO: change this class to match our website requirement
    public class Credentials
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        readonly UserManager<ApplicationUser> userManager;
        readonly SignInManager<ApplicationUser> signinManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager )
        {
            this.userManager = userManager;
            this.signinManager = signinManager;
        }

        // TODO: move security credentials from register to secure files
        /// <summary>
        /// Register function for user we should move the security key to a specific file
        /// </summary>
        /// <param name="credentials">Email and structure of the user who wants to register</param>
        /// <returns>Credential Token</returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] Credentials credentials)
        {
            var user = new ApplicationUser { UserName = credentials.Email, Email = credentials.Email };
            var result = await userManager.CreateAsync(user, credentials.Password);

            if(!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            await signinManager.SignInAsync(user, isPersistent: false);

            return Ok(CreateToken(user));
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Credentials credentials)
        {
            var result = await signinManager.PasswordSignInAsync(credentials.Email, credentials.Password, false, true);
            if(!result.Succeeded)
            {
                return BadRequest();
            }

            var user = await userManager.FindByEmailAsync(credentials.Email);

            return Ok(CreateToken(user));
        }

        private string CreateToken(ApplicationUser user)
        {
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id)
            };

            // TODO: We will have 1 claim per protected data page, so we need a claim factory with the abstract factory design pattern
            var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("This is the secret password for the application"));
            var signinCredentials = new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(signingCredentials: signinCredentials, claims: claims);
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}