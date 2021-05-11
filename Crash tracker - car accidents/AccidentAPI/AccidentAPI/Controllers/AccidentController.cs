using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AccidentAPI.Models;
using AccidentAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AccidentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccidentController : ControllerBase
    {
        private ApplicationContext _context;
        private UserManager<User> _userManager;
        public AccidentController(ApplicationContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email
            };
            if (model.Supervizor != null)
            {
                var supervisor = await _userManager.FindByNameAsync(model.Supervizor);
                user.UserSupervisor = supervisor;
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
        public async Task<IActionResult> Login(RegisterDTO model)
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
                        new Claim("UserId", user.Id.ToString()),
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
                return BadRequest(new { message = "Incorrect username or password" });
            }
        }

        [HttpGet]
        [Authorize]
        [Route("GetUser")]
        public async Task<Object> GetUser()
        {
            var userId = User.Claims.First(c => c.Type == "UserId").Value;
            //var user = await _userManager.FindByIdAsync(userId);
            var user = _context.Users.Include(i => i.UserSupervisor).Where(w => w.Id == userId).First();
            var roleObj = await _userManager.GetRolesAsync(user);
            var role = roleObj.First();
            var supervisor = user.UserSupervisor;

            if (supervisor != null)
            {
                return new
                {
                    user.UserName,
                    user.Email,
                    role,
                    Supervisor = supervisor.UserName
                };
            }
            else
            {
                return new
                {
                    user.UserName,
                    user.Email,
                    role,
                };
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("GetUsers")]
        public async Task<Object> GetUsers()
        {
            //var admin = await _userManager.GetUsersInRoleAsync("Admin");
            //var supervizor = await _userManager.GetUsersInRoleAsync("Supervizor");
            //var agent = await _userManager.GetUsersInRoleAsync("Agent");

            //List<object> users = new List<object>();
            //users.Add(admin);
            //users.Add(supervizor);
            //users.Add(agent);

            //return Ok(users);
            var result = await _context.Users.ToListAsync();
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("GetAdmins")]
        public async Task<Object> GetAdmins()
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");

            return Ok(admins);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("GetSupervisors")]
        public async Task<Object> GetSupervisors()
        {
            var supervisors = await _userManager.GetUsersInRoleAsync("Supervizor");

            return Ok(supervisors);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Supervizor")]
        [Route("GetAgents")]
        public IActionResult GetAgents()
        {
            var agents = _context.Users.Where(w => w.UserSupervisor != null).Include(i => i.UserSupervisor);
            return Ok(agents);
        }

        [HttpGet("{supervisorName}")]
        [Authorize(Roles = "Supervizor")]
        [Route("GetAgentsOfSupervisor/{supervisorName}")]
        public IActionResult GetAgentsOfSupervisor(string supervisorName)
        {
            var agents = _context.Users.Where(w => w.UserSupervisor != null).Include(i => i.UserSupervisor).Where(w => w.UserSupervisor.UserName == supervisorName);
            return Ok(agents);
        }

        [HttpGet]
        [Route("GetPeople")]
        [Authorize]
        public IEnumerable<People> GetPeople()
        {
            return _context.People;
        }

        [HttpDelete("{UserName}")]
        [Route("DeleteUser/{UserName}")]
        [Authorize(Roles = "Admin, Supervizor")]
        public async Task<IActionResult> DeleteUser(string UserName)
        {
            var user = await _userManager.FindByNameAsync(UserName);
            var roleObj = await _userManager.GetRolesAsync(user);
            var role = roleObj.First();

            bool supervisorHasAgents = false;
            bool agentHasAccidents = false;

            if (role == "Admin")
            {
                var admins = await _userManager.GetUsersInRoleAsync("Admin");
                if (admins.Count() <= 1)
                {
                    return BadRequest(new { message = "Cannot delete last admin!" });
                }
            }

            if (role == "Agent")
            {
                foreach (Accident accident in _context.Accidents)
                {
                    if (accident.Agent == user)
                    {
                        agentHasAccidents = true;
                        break;
                    }
                }
                if (agentHasAccidents)
                {
                    return BadRequest(new { message = "Agent has accidents in suborder!" });
                }
            }

            if (role == "Supervizor")
            {
                foreach (User agent in await _userManager.GetUsersInRoleAsync("Agent"))
                {
                    if (agent.UserSupervisor == user)
                    {
                        supervisorHasAgents = true;
                        break;
                    }
                }
                if (supervisorHasAgents)
                {
                    return BadRequest(new { message = "Supervisor has agents!" });
                }
            }

            if (user != null && !supervisorHasAgents)
            {


                await _userManager.RemoveFromRoleAsync(user, role);
                await _userManager.DeleteAsync(user);

                await _context.SaveChangesAsync(); //idk if it's needed (_context != _userManager)

                return Ok();
            }
            else
                return BadRequest(new { message = "User not found" });
        }

        [HttpPost]
        [Route("AddPeople")]
        [Authorize(Roles = "Admin, Supervizor, Agent")]
        public async Task<IActionResult> AddPeople(PeopleDTO model)
        {
            var person = new People
            {
                Name = model.Name,
                Age = model.Age,
                Sex = model.Sex,
                BirthDate = model.BirthDate,
                Address = model.Address,
                AccidentsCommitted = 0,
                AccidentsInvolved = 0,
                PhoneNumber = model.PhoneNumber
            };

            _context.People.Add(person);
            await _context.SaveChangesAsync();

            return Ok(person);
        }

        [HttpDelete("{idPerson}")]
        [Route("DeletePeople/{idPerson}")]
        [Authorize(Roles = "Admin, Supervizor, Agent")]
        public async Task<IActionResult> DeletePeople(int idPerson)
        {
            var person = await _context.People.FindAsync(idPerson);
            if (person != null)
            {
                _context.People.Remove(person);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return BadRequest(new { message = "Person not found!" });
        }

        [HttpPost]
        [Route("AddAccident")]
        [Authorize(Roles = "Admin, Agent, Supervizor")]
        public async Task<IActionResult> AddAccident(AccidentDTO model)
        {
            var accident = new Accident
            {
                Date = model.Date,
                Hour = model.Hour,
                Minute = model.Minute,
                Location = model.Location,
                Photo = model.Photo,
                Guilty = model.Guilty,
                Innocent = model.Innocent,
            };

            var guiltyPerson = await _context.People.FindAsync(model.Guilty);
            var innocentPerson = await _context.People.FindAsync(model.Innocent);
            //accident.GuiltyPeople = guiltyPerson;
            //accident.InnocentPeople = innocentPerson; 

            //add accident commited si involved la people
            guiltyPerson.AccidentsCommitted++;
            innocentPerson.AccidentsInvolved++;

            _context.Entry(guiltyPerson).State = EntityState.Modified;
            _context.Entry(innocentPerson).State = EntityState.Modified;
            await _context.SaveChangesAsync();


            var severity = await _context.Severity.FindAsync(model.Severity);
            //var severity1 = _context.Severity.Where(w => w.Id == model.Severity).First();

            var agent = await _userManager.FindByNameAsync(model.AgentName);

            accident.Severity = severity;
            accident.Agent = agent;

            _context.Accidents.Add(accident);
            await _context.SaveChangesAsync();

            return Ok(accident);
        }
        [HttpPost("{accidentId}")]
        [Route("UpdateAccident/{accidentId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAccident(int accidentId, AccidentDTO model)
        {
            try
            {
                var currentAccident = await _context.Accidents.FindAsync(accidentId);

                if (currentAccident != null)
                {
                    var guilty = await _context.People.FindAsync(model.Guilty);
                    var innocent = await _context.People.FindAsync(model.Innocent);
                    var agent = await _userManager.FindByNameAsync(model.AgentName);
                    var severity = await _context.Severity.FindAsync(model.Severity);

                    currentAccident.Id = accidentId;
                    currentAccident.Date = model.Date;
                    currentAccident.Hour = model.Hour;
                    currentAccident.Minute = model.Minute;
                    currentAccident.Location = model.Location;
                    currentAccident.Photo = model.Photo;
                    currentAccident.Settled = currentAccident.Settled;
                    currentAccident.SettledDate = currentAccident.SettledDate;
                    currentAccident.GuiltyPeople = guilty;
                    currentAccident.InnocentPeople = innocent;
                    currentAccident.Guilty = model.Guilty;
                    currentAccident.Innocent = model.Innocent;
                    currentAccident.Agent = agent;
                    currentAccident.Severity = severity;

                    _context.Entry(currentAccident).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    return Ok(currentAccident);
                }
                else
                {
                    return BadRequest(new { message = "Accident not found!" });
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
        [HttpGet]
        [Route("GetAccidents")]
        public IActionResult GetAccidents()
        {
            return Ok(_context.Accidents.Include(i => i.GuiltyPeople).Include(i => i.InnocentPeople).Include(i => i.Agent).Include(i => i.Severity).Select(s => new { s.Id, s.Date, s.Hour, s.Minute, s.Location, s.Photo, s.Settled, GuiltyPeopleName = s.GuiltyPeople.Name, GuiltyPeopleSex = s.GuiltyPeople.Sex, GuiltyPeopleAge = s.GuiltyPeople.Age, GuiltyPeopleAccidentsInvolved = s.GuiltyPeople.AccidentsInvolved, GuiltyPeopleAccidentsCommitted = s.GuiltyPeople.AccidentsCommitted, GuiltyPeopleContact = s.GuiltyPeople.PhoneNumber, InnocentPeopleName = s.InnocentPeople.Name, InnocentPeopleSex = s.InnocentPeople.Sex, InnocentPeopleAge = s.GuiltyPeople.Age, InnocentPeopleActidentsInvolved = s.InnocentPeople.AccidentsInvolved, InnocentPeopleAccidentsCommitted = s.InnocentPeople.AccidentsCommitted, InnocentPeopleContact = s.InnocentPeople.PhoneNumber, s.Agent.UserName, s.Severity.Type, s.Severity.Vehicle }));
        }
        [HttpGet("{supervisorName}")]
        [Route("GetAccidentsOfAgentsOfSupervisor/{supervisorName}")]
        [Authorize(Roles = "Supervizor")]
        public IActionResult GetAccidentsOfAgentsOfSupervisor(string supervisorName)
        {
            return Ok(_context.Accidents.Include(i => i.GuiltyPeople).Include(i => i.InnocentPeople).Include(i => i.Agent).Include(i => i.Severity).Where(w => w.Agent.UserSupervisor.UserName == supervisorName).Select(s => new { s.Id, s.Date, s.Hour, s.Minute, s.Location, s.Photo, s.Settled, GuiltyPeopleName = s.GuiltyPeople.Name, GuiltyPeopleSex = s.GuiltyPeople.Sex, GuiltyPeopleAge = s.GuiltyPeople.Age, GuiltyPeopleAccidentsInvolved = s.GuiltyPeople.AccidentsInvolved, GuiltyPeopleAccidentsCommitted = s.GuiltyPeople.AccidentsCommitted, GuiltyPeopleContact = s.GuiltyPeople.PhoneNumber, InnocentPeopleName = s.InnocentPeople.Name, InnocentPeopleSex = s.InnocentPeople.Sex, InnocentPeopleAge = s.GuiltyPeople.Age, InnocentPeopleActidentsInvolved = s.InnocentPeople.AccidentsInvolved, InnocentPeopleAccidentsCommitted = s.InnocentPeople.AccidentsCommitted, InnocentPeopleContact = s.InnocentPeople.PhoneNumber, s.Agent.UserName, s.Severity.Type, s.Severity.Vehicle }));
        }
        [HttpGet("{agentName}")]
        [Route("GetAccidentsOfAgentsOfAgent/{agentName}")]
        [Authorize(Roles = "Agent")]
        public IActionResult GetAccidentsOfAgentsOfAgent(string agentName)
        {
            return Ok(_context.Accidents.Include(i => i.GuiltyPeople).Include(i => i.InnocentPeople).Include(i => i.Agent).Include(i => i.Severity).Where(w => w.Agent.UserName == agentName).Select(s => new { s.Id, s.Date, s.Hour, s.Minute, s.Location, s.Photo, s.Settled, GuiltyPeopleName = s.GuiltyPeople.Name, GuiltyPeopleSex = s.GuiltyPeople.Sex, GuiltyPeopleAge = s.GuiltyPeople.Age, GuiltyPeopleAccidentsInvolved = s.GuiltyPeople.AccidentsInvolved, GuiltyPeopleAccidentsCommitted = s.GuiltyPeople.AccidentsCommitted, GuiltyPeopleContact = s.GuiltyPeople.PhoneNumber, InnocentPeopleName = s.InnocentPeople.Name, InnocentPeopleSex = s.InnocentPeople.Sex, InnocentPeopleAge = s.GuiltyPeople.Age, InnocentPeopleActidentsInvolved = s.InnocentPeople.AccidentsInvolved, InnocentPeopleAccidentsCommitted = s.InnocentPeople.AccidentsCommitted, InnocentPeopleContact = s.InnocentPeople.PhoneNumber, s.Agent.UserName, s.Severity.Type, s.Severity.Vehicle }));
        }
        [HttpGet("{idAccident}")]
        [Route("GetAccident/{idAccident}")]
        public IActionResult GetAccident(int idAccident)
        {
            return Ok(_context.Accidents.Include(i => i.GuiltyPeople).Include(i => i.InnocentPeople).Include(i => i.Agent).Include(i => i.Severity).Where(w => w.Id == idAccident).Select(s => new { s.Id, s.Date, s.Hour, s.Minute, s.Location, s.Photo, s.Settled, s.SettledDate, GuiltyPeopleId = s.Guilty, GuiltyPeopleName = s.GuiltyPeople.Name, GuiltyPeopleSex = s.GuiltyPeople.Sex, GuiltyPeopleAge = s.GuiltyPeople.Age, GuiltyPeopleAccidentsInvolved = s.GuiltyPeople.AccidentsInvolved, GuiltyPeopleAccidentsCommitted = s.GuiltyPeople.AccidentsCommitted, GuiltyPeopleContact = s.GuiltyPeople.PhoneNumber, InnocentPeopleId = s.Innocent, InnocentPeopleName = s.InnocentPeople.Name, InnocentPeopleSex = s.InnocentPeople.Sex, InnocentPeopleAge = s.GuiltyPeople.Age, InnocentPeopleActidentsInvolved = s.InnocentPeople.AccidentsInvolved, InnocentPeopleAccidentsCommitted = s.InnocentPeople.AccidentsCommitted, InnocentPeopleContact = s.InnocentPeople.PhoneNumber, Agent = s.Agent.UserName, SeverityId = s.Severity.Id, s.Severity.Type, s.Severity.Description, s.Severity.Vehicle }).First());
        }
        [HttpDelete("{idAccident}")]
        [Route("DeleteAccident/{idAccident}")]
        [Authorize(Roles = "Admin, Agent, Supervizor")]
        public async Task<IActionResult> DeleteAccident(int idAccident)
        {
            var accident = await _context.Accidents.FindAsync(idAccident);

            _context.Accidents.Remove(accident);
            await _context.SaveChangesAsync();

            return Ok(accident);
        }

        [HttpGet("{idAccident}")]
        [Route("Settled/{idAccident}")]
        [Authorize(Roles = "Agent, Admin")]
        public async Task<IActionResult> Settled(int idAccident)
        {
            var accident = _context.Accidents.Find(idAccident);
            accident.Settled = true;
            accident.SettledDate = DateTime.Today;

            _context.Entry(accident).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(accident.Settled);
        }
        [HttpGet]
        [Route("GetSeverities")]
        [Authorize(Roles = "Agent, Admin, Supervizor")]
        public IEnumerable<Severity> GetSeverities()
        {
            return _context.Severity;
        }
        [HttpGet]
        [Route("Statistics")]
        [Authorize(Roles = "Admin")]
        public IActionResult Statistics()
        {
            decimal womenProbability = calculateAccidentChance("Feminin");
            decimal menProbability = calculateAccidentChance("Masculin");
            var locationsRiskList = ZonesRiskChance();
            var timeRiskList = TimeRiskChance();
            //hours with high risk of accidents: when happened the most accidents •	Hours or interval of hours when is most likely to happen an accident
            int totalAccidentsNumber = TotalAccidents();
            int totalPeople = _context.People.Count();
            int totalWomen = TotalGender("Feminin");
            int totalMen = TotalGender("Masculin");
            return Ok( new { totalAccidentsNumber, totalPeople, totalWomen, totalMen, womenProbability, menProbability, locationsRiskList, timeRiskList });
        }
        public int TotalAccidents()
        {
            return _context.Accidents.Count();
        }
        public int TotalGender(string gender)
        {
            return _context.People.Where(w => w.Sex == gender).Count();
        }
        public decimal calculateAccidentChance(string gender)
        {
            decimal totalAccidents = 0;
            decimal guiltyAccidents = 0;
            foreach (Accident accident in _context.Accidents.Include(i => i.GuiltyPeople).Include(i => i.InnocentPeople)) //better with accidents table than people because people can coincide on more accidents...to eliminate duplicates
            {
                if (accident.InnocentPeople.Sex == gender || accident.GuiltyPeople.Sex == gender)
                {
                    totalAccidents++;
                }
            }
            foreach (Accident accident in _context.Accidents.Include(i => i.GuiltyPeople))
            {
                if (accident.GuiltyPeople.Sex == gender)
                {
                    guiltyAccidents++;
                }
            }
            decimal result = guiltyAccidents / totalAccidents * 100;
            
            return decimal.Round(result, 2);
        }
        public class LocationRisk
        {
            public string Location { get; set; }
            public decimal Accidents { get; set; }
            public decimal Percentage { get; set; }
        }
        public List<LocationRisk> ZonesRiskChance()
        {
            List<LocationRisk> Zones = new List<LocationRisk>();

            var totalAccidents = _context.Accidents.Count();

            foreach (Accident accident in _context.Accidents)
            {
                bool newLocation = true;
                foreach (var item in Zones)
                {
                    if(item.Location == accident.Location)
                    {
                        newLocation = false;
                    }
                }
                
                if (newLocation)
                {
                    LocationRisk newLocationAccident = new LocationRisk
                    {
                        Location = accident.Location,
                        Accidents = 1,
                        Percentage = decimal.Round(1m / totalAccidents * 100, 2) // 1m means 1 as a decimal
                    };
                    Zones.Add(newLocationAccident);
                }
                else
                {
                    var currentAccident = Zones.FirstOrDefault(x => x.Location == accident.Location);
                    if(currentAccident != null)
                    {
                        currentAccident.Accidents++;
                        currentAccident.Percentage = decimal.Round(currentAccident.Accidents / totalAccidents * 100, 2);
                    }
                }
            }

            return Zones;
        }
        public class TimeRisk
        {
            public int Hour { get; set; }
            public decimal Accidents { get; set; }
            public decimal Percentage { get; set; }
        }
        public List<TimeRisk> TimeRiskChance()
        {
            List<TimeRisk> timeRisks = new List<TimeRisk>();

            var totalAccidents = TotalAccidents();

            foreach (Accident accident in _context.Accidents)
            {
                bool newHour = true;
                foreach (var item in timeRisks)
                {
                    if (accident.Hour == item.Hour)
                    {
                        newHour = false;
                        break;
                    }
                }

                if (newHour)
                {
                    TimeRisk newTimeRisk = new TimeRisk
                    {
                        Hour = accident.Hour,
                        Accidents = 1,
                        Percentage = decimal.Round(1m / totalAccidents * 100, 2)
                    };

                    timeRisks.Add(newTimeRisk);
                }
                else
                {
                    var currentAccident = timeRisks.FirstOrDefault(x => x.Hour == accident.Hour);
                    
                    if(currentAccident != null)
                    {
                        currentAccident.Accidents++;
                        currentAccident.Percentage = decimal.Round(currentAccident.Accidents / totalAccidents * 100, 2);
                    }
                }
            }

            return timeRisks;
        }
    }
}