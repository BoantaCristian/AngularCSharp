using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FullApi.Models;

namespace FullApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnclesController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public UnclesController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/Uncles
        [HttpGet]
        [Route("GetUncles")]
        public IEnumerable<Object> GetUncles()
        {
            return _context.Uncles.Select(ta => ta.Name).Distinct();
        }

        [HttpGet]
        [Route("GetFullUncles")]
        public IEnumerable<Object> GetFullUncles()
        {
            return _context.Uncles;
        }

        // GET: api/Uncles/5
        [HttpGet("{id}")]
        public ActionResult GetUncle([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
         
            var uncle =  _context.Uncles.Where(s => s.Brother.Id == id); //unchii care sunt frati cu tata specific id-ului
                                                                         // tata1 => unchi 1...de doua ori
            if (uncle == null)
            {
                return NotFound();
            }

            return Ok(uncle);
        }

        // PUT: api/Uncles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUncle([FromRoute] int id, [FromBody] Uncle uncle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != uncle.Id)
            {
                return BadRequest();
            }

            _context.Entry(uncle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UncleExists(id))
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

        // POST: api/Uncles
        [HttpPost]
        public async Task<IActionResult> PostUncle([FromBody] Uncle uncle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Uncles.Add(uncle);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUncle", new { id = uncle.Id }, uncle);
        }

        // DELETE: api/Uncles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUncle([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var uncle = await _context.Uncles.FindAsync(id);
            if (uncle == null)
            {
                return NotFound();
            }

            _context.Uncles.Remove(uncle);
            await _context.SaveChangesAsync();

            return Ok(uncle);
        }

        private bool UncleExists(int id)
        {
            return _context.Uncles.Any(e => e.Id == id);
        }
    }
}