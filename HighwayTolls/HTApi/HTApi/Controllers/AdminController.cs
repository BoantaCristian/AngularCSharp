using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HTApi.Models;
using HTApi.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HTApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private UserManager<User> _userManager;
        private readonly ApplicationContext _context;
        public AdminController(ApplicationContext context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        [Route("Users")]
        [Authorize(Roles ="Admin")]
        public IEnumerable<User> GetUsers()
        {
            return _userManager.Users;
        }
        [HttpGet]
        [Route("UserRoles")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetUserRoles()
        {
            var userRoles = _context.UserRoles;
            return Ok(userRoles);
        }
        [HttpGet]
        [Route("Roles")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetRoles()
        {
            var allRoles = _context.Roles.ToList();
            return Ok(allRoles);
        }
        [HttpDelete("{userName}")]
        [Route("Delete/{userName}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            await _userManager.DeleteAsync(user);
            return Ok(new { message=  "user deleted"});
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("Register")]
        public async Task<Object> Register(UserModel model)
        {
            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email
            };
            try
            {
                var result = await _userManager.CreateAsync(user, model.Password);
                await _userManager.AddToRoleAsync(user, model.Role);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}