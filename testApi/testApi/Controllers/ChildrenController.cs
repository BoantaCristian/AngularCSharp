using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testApi.Models;
using testApi.Models.ViewModels;

namespace testApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChildrenController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ChildrenController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Children
        [HttpGet]
        public IEnumerable GetChildren()
        {
            var children = _context.Children;
            var mappedChildren = _mapper.Map<List<ChildView>>(children);
            return mappedChildren;
        }

        // GET: api/Children/5 get children for a specified parrent
        [HttpGet("{id}")]
        public ActionResult<Parent> GetChild([FromRoute] int id)
        {


            var child = _context.Parents.Where(s => s.Id == id).Include(s => s.Children).FirstOrDefault();



            return child;
        }

        

        // POST: api/Children
        [HttpPost]
        public async Task<ActionResult> PostChild([FromBody] ChildView child)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var parent = _context.Parents.SingleOrDefault(p => p.Id == child.Parent);
            var ceaild = new Child();
            ceaild.Id = child.Id;
            ceaild.Name = child.Name;
            ceaild.Parent = parent;

            //var normalChild = _mapper.Map<ChildView>(child);
            var mapperChield = _mapper.Map<Child>(child);

            _context.Children.Add(ceaild);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChild", new { id = child.Id }, ceaild);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutChild([FromRoute] int id, [FromBody] ChildView child)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != child.Id)
            {
                return BadRequest();
            }

            var mapperChield = _mapper.Map<Child>(child);
            var parent = _context.Parents.SingleOrDefault(p => p.Id == child.Parent);
            mapperChield.Parent = parent;

            _context.Entry(mapperChield).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                
            }

            return NoContent();
        }
    }
}