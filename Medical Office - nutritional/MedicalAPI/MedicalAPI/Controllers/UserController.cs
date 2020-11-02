using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MedicalAPI.Models;
using MedicalAPI.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MedicalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserManager<User> _userManager;
        private readonly ApplicationContext _context;
        public UserController(ApplicationContext context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<Object> Register(RegisterModel model)
        {
            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,
                IdDoctor = model.IdDoctor
            };
            if (model.IllnessId != 0)
            {
                var ill = await _context.Illnesses.FindAsync(model.IllnessId);
                user.Illnesses = ill;
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
        public async Task<IActionResult> Login(LoginModel model)
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
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1234567890123456")), SecurityAlgorithms.HmacSha256Signature)
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
            user.Cured = false;
            var userDoctor = await _userManager.FindByIdAsync(user.IdDoctor);
            string doctor = null;
            if(userDoctor != null)
            {
                doctor = userDoctor.UserName;
            }
            return new
            {
                user.UserName,
                user.Email,
                user.PhoneNumber,
                user.Id,
                user.IdDoctor,
                user.Address,
                doctor,
                role
            };
        }
        [HttpGet]
        [Route("GetUsers")]
        public IEnumerable<User> GetPatients()
        {
            //add inclusion with role to get role of every user
            return _userManager.Users;
        }
        [HttpGet("{idUser}")]
        [Authorize]
        [Route("VerifyUser/{idUser}")]
        public async Task<IActionResult> VerifyUser(string idUser)
        {
            var result = await _userManager.GetUsersInRoleAsync("Admin");
            foreach (User user in result)
            {
                if(idUser == user.Id)
                {
                    return Ok();
                }
            }
            return BadRequest();
        }
        [HttpDelete("{userId}")]
        [Authorize(Roles = "Admin, Doctor")]
        [Route("DeleteUser/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roleObj = await _userManager.GetRolesAsync(user);
            var role = roleObj.First();

            foreach (Details detail in _context.Details.Where(w => w.User.Id == userId))
            {
                _context.Details.Remove(detail);
            }
            foreach (Appointments appointment in _context.Appointments.Where(w => w.User.Id == userId || w.IdPacient.Id == userId))
            {
                _context.Appointments.Remove(appointment);
            }
            foreach (User patient in _userManager.Users.Where(w => w.IdDoctor == userId))
            {
                await _userManager.DeleteAsync(patient);
            }
            await _context.SaveChangesAsync();

            await _userManager.DeleteAsync(user);

            await _userManager.RemoveFromRoleAsync(user, role);

            return Ok(user);
        }

    }
}