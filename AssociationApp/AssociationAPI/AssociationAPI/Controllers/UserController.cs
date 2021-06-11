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
using Microsoft.EntityFrameworkCore;
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
            if (model.Role == "Representative")
            {
                var association = await _context.Associations.FindAsync(model.AssociationId);
                user.Association = association;
            }
            try
            {
                var result = await _userManager.CreateAsync(user, model.Password);
                await _userManager.AddToRoleAsync(user, model.Role);

                if (model.Role == "Client")
                {
                    var representative = await _userManager.FindByIdAsync(model.RepresentativeId);
                    var waterProvider = await _context.Providers.FindAsync(model.WaterProvider);
                    var gasProvider = await _context.Providers.FindAsync(model.GasProvider);
                    var electricityProvider = await _context.Providers.FindAsync(model.ElectricityProvider);

                    user.Representative = representative;

                    var createdUser = await _userManager.FindByNameAsync(model.UserName);

                    var newClientProviders = new ClientProvider
                    {
                        Client = createdUser,
                        WaterFk = waterProvider.Id,
                        GasFk = gasProvider.Id,
                        ElectricityFk = electricityProvider.Id,
                        WaterProvider = waterProvider,
                        GasProvider = gasProvider,
                        ElectricityProvider = electricityProvider
                    };

                    await _context.ClientProviders.AddAsync(newClientProviders);
                    await _context.SaveChangesAsync();
                }

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
            var association = "";
            var representative = new object();

            if (role == "Representative")
                association = await _context.Users.Include(i => i.Association).Where(w => w == user).Select(s => s.Association.Description).FirstAsync();

            if (role == "Client")
                representative = await _context.Users.Include(i => i.Representative).ThenInclude(i => i.Association).Where(w => w == user).FirstAsync();
            return new
            {
                user.UserName,
                user.Email,
                user.Telephone,
                user.Id,
                role,
                association,
                representative
            };
        }

        [HttpGet]
        [Route("GetUsers")]
        public async Task<Object> GetUsers()
        {
            var adminsDetails = await _userManager.GetUsersInRoleAsync("Admin");
            //var representativesInRoles = await _userManager.GetUsersInRoleAsync("Representative");
            //var clientsInRole = await _userManager.GetUsersInRoleAsync("Client");
            var admins = adminsDetails.Select(s => new { s.Id, s.UserName, s.Email, s.Address, s.Telephone });
            var representatives = _userManager.Users.Include(i => i.Association).Include(i => i.Representative).Where(w => w.Association != null).Select(s => new { s.Id, s.UserName, s.Email, s.Address, s.Telephone, Association = s.Association.Description, Clients = s.AssociationRepresentative });
            var clients = _userManager.Users.Include(i => i.Representative).Where(w => w.Representative != null).Select(s => new { s.Id, s.UserName, s.Email, s.Address, s.Telephone, Representative = s.Representative.UserName });

            return new { admins, representatives, clients };
        }

        [HttpGet("{idRepresentative}")]
        [Authorize(Roles ="Representative")]
        [Route("GetClientsOfRepresentative/{idRepresentative}")]
        public Object GetClientsOfRepresentative(string idRepresentative)
        {
            var clients = _context.Users.Include(i => i.Representative).Where(w => w.Representative.Id == idRepresentative);

            return clients;
        }

        [HttpDelete("{userName}")]
        [Route("DeleteUser/{userName}")]
        public async Task<IActionResult> DeleteUser(string userName)
        {
            var userToDelete = await _userManager.FindByNameAsync(userName);
            var roleObj = await _userManager.GetRolesAsync(userToDelete);
            var role = roleObj.FirstOrDefault();

            if(userToDelete != null)
            {
                switch (role)
                {
                    case "Admin":
                    {
                        var admins = await _userManager.GetUsersInRoleAsync("Admin");
                        if(admins.Count() == 1)
                            return BadRequest(new { message = "Cannot delete last admin!" });
                        else
                        {
                            try
                            {
                                await _userManager.RemoveFromRoleAsync(userToDelete, role);
                                await _userManager.DeleteAsync(userToDelete);

                                return Ok(userToDelete);
                            }
                            catch (Exception e)
                            {

                                throw e;
                            }
                        }
                    }
                    case "Representative":
                    {
                        var representativesHasClients = false;
                        foreach (User user in _context.Users.Include(i => i.AssociationRepresentative))
                        {
                            if(user.Representative.UserName == userName)
                            {
                                representativesHasClients = true;
                                break;
                            }
                            if(representativesHasClients)
                                return BadRequest(new { message = "Representative has clients!" });
                            else
                            {
                                try
                                {
                                    await _userManager.RemoveFromRoleAsync(userToDelete, role);
                                    await _userManager.DeleteAsync(userToDelete);

                                    return Ok(userToDelete);
                                }
                                catch (Exception e)
                                {

                                    throw e;
                                }
                            }

                        }
                        break;
                    }
                    case "Client":
                    {
                            var clientHasArchive = false;
                            var clientHasPayments = false;
                            var clientHasProvider = false;
                            var clientHasReceipts = false;

                            foreach (Archive archive in _context.Archives.Include(i => i.Client))
                            {
                                if(archive.Client.UserName == userName)
                                {
                                    clientHasArchive = true;
                                    break;
                                }
                            }
                            foreach (Payment payment in _context.Payments.Include(i => i.Client))
                            {
                                if(payment.Client.UserName == userName)
                                {
                                    clientHasPayments = true;
                                    break;
                                }
                            }
                            foreach (ClientProvider provider in _context.ClientProviders.Include(i => i.Client))
                            {
                                if(provider.Client.UserName == userName)
                                {
                                    clientHasProvider = true;
                                    break;
                                }
                            }
                            foreach (Receipt receipt in _context.Receipts.Include(i => i.Client))
                            {
                                if(receipt.Client.UserName == userName)
                                {
                                    clientHasReceipts = true;
                                    break;
                                }
                            }

                            if (clientHasArchive)
                            {
                                return BadRequest(new { message = "Client has archive!" });
                            }
                            if (clientHasPayments)
                            {
                                return BadRequest(new { message = "Client has payments!" });
                            }
                            if (clientHasProvider)
                            {
                                var provider = _context.ClientProviders.Include(i => i.Client).Where(w => w.Client.UserName == userName).First();

                                _context.ClientProviders.Remove(provider);
                                await _context.SaveChangesAsync();
                            }
                            if (clientHasReceipts)
                            {
                                return BadRequest(new { message = "Client has receipts!" });
                            }

                            try
                            {
                                await _userManager.RemoveFromRoleAsync(userToDelete, role);
                                await _userManager.DeleteAsync(userToDelete);

                                return Ok(userToDelete);
                            }
                            catch (Exception e)
                            {

                                throw e;
                            }   
                    }
                    default:
                        break;
                }
                return BadRequest(new { message = "Something went wrong!" });
            }
            else
            {
                return BadRequest(new { message = "User not found!" });
            }
        }
    }
}