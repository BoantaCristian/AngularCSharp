using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using phoneShopApi.Models;
using phoneShopApi.Models.ViewModels;

namespace phoneShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserManager<User> _userManager;
        private ApplicationContext _context;
        private readonly ApplicationSettings _appSettings;
        public UserController(UserManager<User> userManager, IOptions<ApplicationSettings> appSettings, ApplicationContext context)
        {
            _userManager = userManager;
            _context = context;
            _appSettings = appSettings.Value;
        }
        [HttpPost]
        [Route("Register")]
        public async Task<Object> Register(UserModel model)
        {
            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email
            };
            try
            {
                var result = await _userManager.CreateAsync(user, model.Password);
                await _userManager.AddToRoleAsync(user, model.Role);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw (ex);
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
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWT_Secret)), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                return Ok(new { token });
            }
            else
            {
                return BadRequest(new { message = "Incorrect Username or Password!" });
            }
        }
        [HttpGet]
        [Authorize]
        [Route("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            var userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            var roleObj = await _userManager.GetRolesAsync(user);
            var role = roleObj.FirstOrDefault();
            return Ok(new
            {
                user.UserName,
                user.Email,
                role
            });
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("GetUsers")]
        public IEnumerable<User> GetUsers()
        {
            return _userManager.Users;
        }
        [HttpDelete("{userName}")]
        [Authorize(Roles = "Admin")]
        [Route("DeleteUser/{userName}")]
        public async Task<IActionResult> DeleteUser(string userName)
        {
            //check number of admins
            var adminNumber = 0;
            foreach(User usr in _userManager.Users)
            {
                var rol = await _userManager.GetRolesAsync(usr);
                if(rol.First() == "Admin")
                {
                    adminNumber++;
                }
            }

            var user = await _userManager.FindByNameAsync(userName);
            var roleObj = await _userManager.GetRolesAsync(user);
            var role = roleObj.First();

            if (user != null && role == "Admin" && adminNumber <= 1)
            {
                return BadRequest(new { message = "Cannot delete last admin!" });
            }

            if (user != null)
            {
                roleObj = await _userManager.GetRolesAsync(user);
                role = roleObj.First();
                await _userManager.DeleteAsync(user);
                await _userManager.RemoveFromRoleAsync(user, role);

                return Ok(user);
            }
            else
                return BadRequest(new { message = "user not found" });
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("AddPhone")]
        public async Task<IActionResult> AddPhone(PhoneModel model)
        {
            var phone = new Telephone
            {
                Name = model.Name,
                LaunchDate = model.LaunchDate,
                Price = model.Price,
                ImagePath = model.ImagePath
            };
            var company = _context.Companies.Where(w => w.Id == model.CompanyId).FirstOrDefault();
            phone.Company = company;

            _context.Telephones.Add(phone);
            await _context.SaveChangesAsync();

            return Ok(phone);
        }
        [HttpDelete("{phoneId}")]
        [Authorize(Roles = "Admin")]
        [Route("DeletePhone/{phoneId}")]
        public async Task<IActionResult> DeletePhone(int phoneId)
        {
            var phone = _context.Telephones.Find(phoneId);

            if (phone != null)
            {
                try
                {
                    foreach(Description desc in _context.Descriptions)
                    {
                        if(desc.Telephone == phone)
                        {
                            _context.Descriptions.Remove(desc);
                        }
                    }
                    _context.Telephones.Remove(phone);
                    await _context.SaveChangesAsync();

                    return Ok(phone);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
                return BadRequest(new { message = "phone not found" });
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("AddPhoneDetails")]
        public async Task<IActionResult> AddPhoneDetails(DescriptionModel model)
        {
            var phone = _context.Telephones.Last();
            var details = new Description
            {
                Dimensions = model.Dimensions,
                Weight = model.Weight,
                DisplayType = model.DisplayType,
                Resolution = model.Resolution,
                OS = model.OS,
                MainCamera = model.MainCamera,
                SelfieCamera = model.SelfieCamera,
                Battery = model.Battery
            };
            details.Telephone = phone;

            _context.Descriptions.Add(details);
            await _context.SaveChangesAsync();

            return Ok(details);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("AddCompany")]
        public async Task<IActionResult> AddCompany(CompanyModel model)
        {
            var company = new Company
            {
                Name = model.Name,
                Description = model.Description,
                LaunchDate = model.LaunchDate
            };

            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            return Ok(company);
        }
        [HttpGet("{idHistoric}")]
        [Authorize(Roles = "Admin")]
        [Route("Deliver/{idHistoric}")]
        public async Task<IActionResult> Deliver(int idHistoric)
        {
            var historic = _context.Historics.Where(w => w.Id == idHistoric).FirstOrDefault();

            historic.Status = "Delivered";

            _context.Entry(historic).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(historic);
        }
    }
}