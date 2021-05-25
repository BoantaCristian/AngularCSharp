using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssociationAPI.Models.ApplicationContext;
using AssociationAPI.Models.DataTransferObjects;
using AssociationAPI.Models.DbModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AssociationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssociationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationContext _context;

        public AssociationController(UserManager<User> userManager, ApplicationContext context)
        {
            this._userManager = userManager;
            this._context = context;
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("GetAssociations")]
        public IEnumerable<Association> GetAssociations()
        {
            return _context.Associations;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Client")]
        [Route("GetProviders")]
        public IEnumerable<Provider> GetProviders()
        {
            return _context.Providers;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("AddAssociation")]
        public async Task<IActionResult> AddAssociation(AssociationDTO model)
        {
            var newAssociation = new Association
            {
                Description = model.Description,
                Location = model.Location,
                Program = model.Program,
                WorkingCapital = model.WorkingCapital,
                Sanitation = model.Sanitation,
                DayPenalty = model.DayPenalty,
            };

            var result = await _context.Associations.AddAsync(newAssociation);
            await _context.SaveChangesAsync();

            return Ok(result);
        }
        
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("AddProvider")]
        public async Task<IActionResult> AddProvider(ProviderDTO model)
        {
            var newProvider = new Provider
            {
                Name = model.Name,
                Location = model.Location,
                Program = model.Program,
                HotWaterLiterPrice = model.HotWaterLiterPrice,
                ColdWaterLiterPrice = model.ColdWaterLiterPrice,
                GasPrice = model.GasPrice,
                ElectricityPrice = model.ElectricityPrice
            };

            var result = await _context.Providers.AddAsync(newProvider);
            await _context.SaveChangesAsync();

            return Ok(result);
        }
    }
}