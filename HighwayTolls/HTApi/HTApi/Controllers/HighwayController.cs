using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HTApi.Models;
using Microsoft.AspNetCore.Identity;
using HTApi.Models.ViewModels;

namespace HTApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HighwayController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private UserManager<User> _userManager;

        public HighwayController(ApplicationContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
                                    
        [HttpGet]
        [Route("Locations")]
        public IEnumerable<Location> GetLocations()
        {
            return _context.Locations;
        }

        [HttpGet("{id}")]
        [Route("TollBooths/{id}")]
        public IActionResult GetTollBooths(int id)
        {
            var result = _context.TollBooths.Where(q => q.Location.Id == id).Include(i => i.Location); //obj tollbooth: better
            var result1 = _context.Locations.Where(q => q.Id == id).Include(i => i.TollBooths).SingleOrDefault(); //obj loc inner join obj tollb

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("Categories")]
        public IEnumerable<Category> GetCategories()
        {
            return _context.Categories;
        }

        [HttpGet]
        [Route("History")]
        public IActionResult GetHistory()
        {
            var result = _context.Histories.Include(i => i.Location).Include(i => i.User);
            return Ok(result);
        }

        [HttpGet("{idLoc}/{idCateg}")]
        [Route("GetPrice/{idLoc}/{idCateg}")]
        public IActionResult GetPrice(int idLoc, int idCateg)
        {
            var result = _context.Prices.Where(w => w.Location.Id == idLoc).Where(w => w.Category.Id == idCateg).Include(i => i.Location).Include(i => i.Category).Select(s=>s.Amount).First();
            return Ok(result);
        }

        [HttpPost("{userName}/{idLoc}")]
        [Route("PostHistory/{userName}/{idLoc}")]
        public async Task<IActionResult> PostLocation(string userName, int idLoc, HistoryModel model)
        {
            var loc = _context.Locations.SingleOrDefault(p => p.Id == idLoc);
            var user = await _userManager.FindByNameAsync(userName);

            var history = new History
            {
                Amount = model.Amount,
                Date = DateTime.Now                             //2019-11-27 01:02:25.9131073
                //Date = DateTime.Today                         //2019-11-27 00:00:00.0000000
                //Date = DateTime.UtcNow                        //2019-11-26 23:22:43.2452964 dif fus orar
                //Date = DateTimeOffset.UtcNow.LocalDateTime    //2019-11-27 01:21:00.0942040 
                //Date = DateTimeOffset.UtcNow.Date             //2019-11-26 00:00:00.0000000 dif fus orar
                //Date = model.Date                             //2019-11-26 23:01:22.5020000 diferit de c# + fus orar diferit: nu adauga gmt +2
            };

            history.Location = loc;
            history.User = user;

            _context.Histories.Add(history);
            await _context.SaveChangesAsync();

            return Ok(history); ;
        }
        [HttpGet("{month}")]
        [Route("MonthIncome/{month}")]
        public IActionResult MonthIncome(int month)
        {
            var history = _context.Histories;

            decimal income = 0;
            foreach(History hist in history)
            {
                if(hist.Date.Month == month)
                {
                    income = income + hist.Amount;
                }
            }
            return Ok(income);
        }
        [HttpGet("{idStartLocation}/{idFinishLocation}/{idCategory}")]
        [Route("CalculateRoute/{idStartLocation}/{idFinishLocation}/{idCategory}")]
        public IActionResult CalculateRoute(int idStartLocation, int idFinishLocation, int idCategory)
        {
            //get price and add to a local variable of every location in interval start, finish(if start<finish) / finish start (if start>finish) / 0 if(st = fin)
            decimal amount = 0;
            int aux;
            if(idStartLocation >= idFinishLocation)
            {
                aux = idStartLocation;
                idStartLocation = idFinishLocation;
                idFinishLocation = aux;
            }
            for(int index = idStartLocation; index <= idFinishLocation; index++)
            {
                var result = _context.Prices.Where(w => w.Location.Id == index).Where(w => w.Category.Id == idCategory).Include(i => i.Location).Include(i => i.Category).Select(s => s.Amount).First();
                amount = amount + result;
            }
            return Ok(amount);
        }
























        // PUT: api/Highway/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocation([FromRoute] int id, [FromBody] Location location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != location.Id)
            {
                return BadRequest();
            }

            _context.Entry(location).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Highway
        [HttpPost]
        public async Task<IActionResult> PostLocation([FromBody] Location location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Locations.Add(location);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLocation", new { id = location.Id }, location);
        }

        // DELETE: api/Highway/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var location = await _context.Locations.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }

            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();

            return Ok(location);
        }

        private bool LocationExists(int id)
        {
            return _context.Locations.Any(e => e.Id == id);
        }
    }
}