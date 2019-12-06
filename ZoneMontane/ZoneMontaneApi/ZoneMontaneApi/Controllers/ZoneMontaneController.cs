using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZoneMontaneApi.Models;
using ZoneMontaneApi.Models.ViewModels;

namespace ZoneMontaneApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZoneMontaneController : ControllerBase
    {
        private ApplicationContext _context;
        public ZoneMontaneController(ApplicationContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("GetZones")]
        public IEnumerable<Zone> GetZones()
        {
            return _context.Zones;
        }
        [HttpGet]
        [Route("GetAllAccommodation")]
        public IEnumerable<Accommodation> GetAccommodation()
        {
            return _context.Accommodations;
        }
        [HttpGet("{idZone}")]
        [Route("GetAccommodationPoint/{idZone}")]
        public IActionResult GetAccommodationPoint(int idZone)
        {
            var result = _context.Zones.Include(i => i.Accommodations).Where(w => w.Id == idZone).FirstOrDefault(); //easyer to read
            //var result1 = _context.Accommodations.Include(i => i.Zone).Where(w => w.Zone.Id == idZone);
            return Ok(result);
        }
        [HttpGet]
        [Route("GetTeams")]
        public IEnumerable<Team> GetTeams()
        {
            return _context.Teams;
        }
        [HttpGet]
        [Route("GetMembers")]
        public IEnumerable<Member> GetMembers()
        {
            return _context.Members;
        }
        [HttpGet("{idTeam}")]
        [Route("GetTeam/{idTeam}")]
        public async Task<IActionResult> GetTeam(int idTeam)
        {
            var result = await _context.Teams.FindAsync(idTeam);
            if (result != null)
                return Ok(result);
            else
                return BadRequest();
        }
        [HttpGet("{idTeam}")]
        [Route("GetTeamMembers/{idTeam}")]
        public IActionResult GetTeamMembers(int idTeam)
        {
            var result = _context.Teams.Include(i => i.Members).Where(w => w.Id == idTeam).FirstOrDefault();
            return Ok(result);
        }
        [HttpGet]
        [Route("GetRoutes")]
        public IEnumerable<Route> GetRoutes()
        {
            return _context.Routes;
        }
        [HttpGet("{idZone}")]
        [Route("GetRoute/{idZone}")]
        public IActionResult GetRoute(int idZone)
        {
            var result = _context.Zones.Include(i => i.Routes).Where(w => w.Id == idZone).FirstOrDefault();
            //var result1 = _context.Routes.Include(i => i.Zone).Where(w => w.Zone.Id == idZone);
            return Ok(result);
        }
        [HttpGet("{idRoute}")]
        [Route("GetObjectives/{idRoute}")]
        public IActionResult GetObjectives(int idRoute)
        {
            var result = _context.RouteObjectives.Include(i => i.Objective).Where(w => w.Route.Id == idRoute).Select(s => s.Objective);
            return Ok(result);
        }
        [HttpGet("{idTeam}")]
        [Route("GetTeamActivity/{idTeam}")]
        public IActionResult GetTeamActivity(int idTeam)
        {
            var result = _context.ZoneTeams.Include(i => i.Zone).Where(w => w.Team.Id == idTeam).Select(s => s.Zone);
            return Ok(result);
        }
        [HttpGet("{idTeam}")]
        [Route("CountMembers/{idTeam}")]
        public int CountMembers(int idTeam)
        {
            var result1 = _context.Members.Include(i => i.Team).Where(w => w.Team.Id == idTeam).Count(); //count members of a given team
            var result = _context.Members.Count(); //count all members
            return result1;
        }
        [HttpGet]
        [Route("GetMembersPerZone")]
        public IActionResult GetMembersPerZone()
        {
            var linq = (from p in _context.Members
                       join team in _context.Teams on p.Team.Id equals team.Id
                       join zoneTeam in _context.ZoneTeams on team.Id equals zoneTeam.Team.Id 
                       join zone in _context.Zones on zoneTeam.Zone.Id equals zone.Id
                       group p by zone.Name into g
                       select new { Zone = g.Key, MembersNumber = g.Count()}).Distinct();
            return Ok(linq);

            //var result = _context.ZoneTeams.Include(i => i.Zone).Include(i => i.Team).ThenInclude(i => i.Members).Select(s => s.Team.Members);
            //var linq1 = (from p in _context.Members
            //            join team in _context.Teams on p.Team.Id equals team.Id
            //            join zoneTeam in _context.ZoneTeams on team.Id equals zoneTeam.Team.Id
            //            join zone in _context.Zones on zoneTeam.Zone.Id equals zone.Id
            //            select new { Zone = zone.Name, MembersNumber = p.Name }).GroupBy(g =>g.Zone);

            //var linq2 = (from p in _context.Zones
            //            join zoneTeam in _context.ZoneTeams on p.Id equals zoneTeam.Zone.Id
            //            join team in _context.Teams on zoneTeam.Team.Id equals team.Id
            //            join member in _context.Members on team.Id equals member.Team.Id
            //            group p by p.Name into g
            //            select new { Zone = g.Key, Members = g }).Distinct();

            //var linq3 = (from p in _context.Members
            //            join team in _context.Teams on p.Team.Id equals team.Id
            //            join zoneTeam in _context.ZoneTeams on team.Id equals zoneTeam.Team.Id
            //            join zone in _context.Zones on zoneTeam.Zone.Id equals zone.Id
            //            //group p by zone.Name into g
            //            select p).GroupBy(g => g.Name).DistinctBy(d => d.Zone.Name);
        }
        [HttpPost]
        [Route("AddMember")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddMember(MemberModel model)
        {
            var team = await _context.Teams.FindAsync(model.TeamId);
            var member = new Member
            {
                Name = model.Name,
                Date = DateTime.Today,
                Telephone = model.Telephone,
                Experience = model.Experience,
                Team = team
            };
            _context.Members.Add(member);
            await _context.SaveChangesAsync();
            return Ok(member);
        }
        [HttpDelete("{idMember}")]
        [Route("DeleteMember/{idMember}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMember(int idMember)
        {
            var member = await _context.Members.FindAsync(idMember);
            if (member == null)
            {
                return NotFound();
            }

            _context.Members.Remove(member);
            await _context.SaveChangesAsync();

            return Ok(member);
        }
    }
}