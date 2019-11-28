using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CompleteApi.Models;
using CompleteApi.Models.ViewModels;

namespace CompleteApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChildrenController : ControllerBase
    {
        private readonly DataContext _context;

        public ChildrenController(DataContext context)
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

        // POST: api/Children
        public async Task<ActionResult> PostChild([FromBody] ChildModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var parent = _context.Parents.SingleOrDefault(p => p.Id == child.Parent);
            var parent = _context.Parents.Where(s => s.Id == model.Parent).Include(s => s.Children).FirstOrDefault();

            var ceaild = new Child();
            ceaild.Name = model.Name;
            ceaild.Parent = parent;
            
            _context.Children.Add(ceaild);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChild", new { id = model.Id }, ceaild);
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