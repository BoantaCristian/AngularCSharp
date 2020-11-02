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
        private UserManager<ApplicationUser> _userManager;
        private ApplicationSettings _appSettings;
        private readonly ApplicationDbContext _context;
        public UserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IOptions<ApplicationSettings> appSettings)
        {
            _userManager = userManager;
            _appSettings = appSettings.Value;
            _context = context;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<Object> Register(UserModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Seniority = model.Seniority
            };
            try
            {
                var result = await _userManager.CreateAsync(user, model.Password);
                await _userManager.AddToRoleAsync(user, model.Role);
                return Ok(result);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if(user != null && await _userManager.CheckPasswordAsync(user, model.Password))
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
        [Route("GetUsers")]
        public IEnumerable<ApplicationUser> GetPatients()
        {
            //add inclusion with role to get role of every user
            return _userManager.Users;
        }
        [HttpGet]
        [Route("GetMedics")]
        public async Task<IEnumerable<ApplicationUser>> GetMedics()
        {
            //add inclusion with role to get role of every user
            return await _userManager.GetUsersInRoleAsync("Medic");
        }
        [HttpDelete("{userId}")]
        [Authorize(Roles = "Admin")]
        [Route("DeleteUser/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roleObj = await _userManager.GetRolesAsync(user);
            var role = roleObj.First();
            //delete patients also plus their illnesses from historic
            foreach(Patient patient in _context.Patients)
            {
                if(patient.ApplicationUser == user)
                {
                    foreach(History history in _context.Histories)
                    {
                        if(history.Patient == patient)
                        {
                            _context.Histories.Remove(history);
                            //await _context.SaveChangesAsync();
                        }
                    }
                    _context.Patients.Remove(patient);
                    //await _context.SaveChangesAsync();
                }
            }

            await _context.SaveChangesAsync();
            //!doesnt delete historic but deletes the medic and his patients
            //interface not affected
            await _userManager.DeleteAsync(user);
            await _userManager.RemoveFromRoleAsync(user, role);

            return Ok(user);
        }
        [HttpDelete("{idPatient}")]
        [Authorize(Roles = "Admin, Medic")]
        [Route("DeletePatient/{idPatient}")]
        public async Task<IActionResult> DeletePatient(int idPatient)
        {
            var patient = _context.Patients.Where(w => w.Id == idPatient).FirstOrDefault();

            foreach(History history in _context.Histories)
            {
                if(history.Patient == patient)
                {
                    _context.Histories.Remove(history);
                }
            }
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return Ok(patient);
        }
    }
}