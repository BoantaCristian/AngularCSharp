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
    public class ParentsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ParentsController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Parents
        [HttpGet]
        public IEnumerable GetParents()
        {
            var parents = _context.Parents;
            var mappedParents = _mapper.Map<List<ParentView>>(parents);
            return mappedParents;
        }

        // GET: api/Parents/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetParent([FromRoute] int id)
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

            return Ok(parent);
        }
    }
}