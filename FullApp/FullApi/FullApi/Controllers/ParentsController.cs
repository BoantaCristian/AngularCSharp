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
    public class ParentsController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public ParentsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/Parents
        [HttpGet]
        public IEnumerable<Parent> GetParents()
        {
            return _context.Parents;
        }

        // GET: api/Parents/5
        [HttpGet("{id}")]
        public IActionResult GetParentsChildren([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var parent = _context.Parents.Where(s => s.Id == id).Include(s => s.Children).FirstOrDefault();

            if (parent == null)
            {
                return NotFound();
            }

            return Ok(parent);
        }

        // PUT: api/Parents/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParent([FromRoute] int id, [FromBody] Parent parent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != parent.Id)
            {
                return BadRequest();
            }

            _context.Entry(parent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParentExists(id))
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

        // POST: api/Parents
        [HttpPost]
        public async Task<IActionResult> PostParent([FromBody] Parent parent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Parents.Add(parent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetParent", new { id = parent.Id }, parent);
        }

        // DELETE: api/Parents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var parent = await _context.Parents.FindAsync(id);
            if (parent == null)
            {
                return NotFound();
            }

            _context.Parents.Remove(parent);
            await _context.SaveChangesAsync();

            return Ok(parent);
        }

        private bool ParentExists(int id)
        {
            return _context.Parents.Any(e => e.Id == id);
        }
    }
}