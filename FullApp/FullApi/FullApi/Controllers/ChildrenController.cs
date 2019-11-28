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
    public class ChildrenController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public ChildrenController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/Children
        [HttpGet]
        public IEnumerable<Child> GetChildren()
        {
            return _context.Children;
        }

        // GET: api/Children/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetChild([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var child = await _context.Children.FindAsync(id);

            if (child == null)
            {
                return NotFound();
            }

            return Ok(child);
        }

        // PUT: api/Children/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChild([FromRoute] int id, [FromBody] Child child)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != child.Id)
            {
                return BadRequest();
            }

            _context.Entry(child).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChildExists(id))
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

        // POST: api/Children
        [HttpPost]
        public async Task<IActionResult> PostChild([FromBody] Child child)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Children.Add(child);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChild", new { id = child.Id }, child);
        }

        // DELETE: api/Children/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChild([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var child = await _context.Children.FindAsync(id);
            if (child == null)
            {
                return NotFound();
            }

            _context.Children.Remove(child);
            await _context.SaveChangesAsync();

            return Ok(child);
        }

        private bool ChildExists(int id)
        {
            return _context.Children.Any(e => e.Id == id);
        }
    }
}