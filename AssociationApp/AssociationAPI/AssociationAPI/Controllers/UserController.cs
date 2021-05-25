using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AssociationAPI.Models.ApplicationContext;
using AssociationAPI.Models.ApplicationSettings;
using AssociationAPI.Models.DataTransferObjects;
using AssociationAPI.Models.DbModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AssociationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private ApplicationContext _context;
        private UserManager<User> _userManager;
        private ApplicationSettings _appSettings;

        public UserController(ApplicationContext context, UserManager<User> userManager, IOptions<ApplicationSettings> appSettings)
        {
            _userManager = userManager;
            _appSettings = appSettings.Value;
            _context = context;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<Object> Register(RegisterDTO model)
        {
            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                CNP = model.CNP,
                Address = model.Address,
                Telephone = model.Telephone
            };
            if(model.AssociationId != 0)
            {
                var association = await _context.Associations.FindAsync(model.AssociationId);
                user.Association = association;
            }
            if (model.RepresentativeId != "")
            {
                var representative = await _userManager.FindByIdAsync(model.RepresentativeId);
                user.Representative = representative;
            }
            try
            {
                var result = await _userManager.CreateAsync(user, model.Password);
                await _userManager.AddToRoleAsync(user, model.Role);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var role = await _userManager.GetRolesAsync(user);
                IdentityOptions _options = new IdentityOptions();

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserID", user.Id.ToString()),
                        new Claim(_options.ClaimsIdentity.RoleClaimType, role.FirstOrDefault())
                    }),
                    Expires = DateTime.UtcNow.AddDays(10),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWT_Secret)), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);

                return Ok(new { token });
            }
            else
            {
                return BadRequest(new { message = "Incorrect username or password!" });
            }
        }

        [HttpGet]
        [Authorize]
        [Route("GetUser")]
        public async Task<Object> GetUser()
        {
            var userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            var roleObj = await _userManager.GetRolesAsync(user);
            var role = roleObj.First();
            return new
            {
                user.UserName,
                user.Email,
                user.PhoneNumber,
                user.Id,
                role
            };
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("GetUsers")]
        public async Task<Object> GetUsers()
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            var representatives = await _userManager.GetUsersInRoleAsync("Representative");
            var clients = await _userManager.GetUsersInRoleAsync("Client");

            return new { admins, representatives, clients };
        }
    }
}