using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using phoneShopApi.Models;
using phoneShopApi.Models.ViewModels;

namespace phoneShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhoneController : ControllerBase
    {
        public ApplicationContext _context;
        public UserManager<User> _userManager;
        public PhoneController(ApplicationContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpGet]
        [Route("GetPhones")]
        public IActionResult GetTelephones()
        {
            var result = _context.Descriptions.Include(i => i.Telephone).Where(w => w.Telephone.Id == w.Telephone.Id).Select(s => new { Name = s.Telephone.Name, s.Telephone.LaunchDate, s.Telephone.Price, s.Telephone.ImagePath, s.Dimensions, s.Weight, s.DisplayType, s.Resolution, s.OS, s.MainCamera, s.SelfieCamera, s.Battery });
            return Ok(result);
        }
        [HttpGet]
        [Route("GetPhonesWithCompanies")]
        public IActionResult GetPhonesWithCompanies()
        {
            var result = _context.Telephones.Include(i => i.Company).Include(i => i.Descriptions);
            return Ok(result);
        }
        [HttpGet("{idPhone}")]
        [Route("GetPhoneDetails/{idPhone}")]
        public IActionResult GetPhoneDetails(int idPhone)
        {
            var result = _context.Descriptions.Include(i => i.Telephone).Where(w => w.Telephone.Id == idPhone).FirstOrDefault();
            return Ok(result);
        }
        [HttpGet]
        [Route("GetAllPhonesDetails")]
        public IActionResult GetAllPhonesDetails()
        {
            var result = _context.Descriptions.Include(i => i.Telephone).Where(w => w.Telephone.Id == w.Telephone.Id).Select(s => new { s.Dimensions, s.Weight, s.DisplayType, s.Resolution, s.OS, s.MainCamera, s.SelfieCamera, s.Battery, s.Telephone.Name });
            return Ok(result);
        }
        [HttpGet]
        [Route("GetCompanies")]
        public IEnumerable<Company> GetCompanies()
        {
            return _context.Companies;
        }
        [HttpGet("{userName}")]
        [Route("GetShoppingBag/{userName}")]
        public IActionResult GetShoppingBag(string userName)
        {
            var result = _context.ShoppingBags.Include(i => i.User).Include(i => i.Telephone).Where(w => w.User.UserName == userName).Select(s => new {s.Id, s.Quantity, s.Telephone.Name, s.Telephone.Price, s.User.UserName, PhoneId = s.Telephone.Id });
            return Ok(result);
        }
        [HttpPost("{userName}/{phoneId}")]
        [Route("AddToCart/{userName}/{phoneId}")]
        public async Task<IActionResult> AddToCart(BagModel model, string userName, int phoneId)
        {
            var bag = new ShoppingBag
            {
                Quantity = model.Quantity
            };

            if(model.Quantity <= 0)
            {
                return BadRequest(new { message = "Incorrect quantity"});
            }

            var telephone = await _context.Telephones.FindAsync(phoneId);
            var user = await _userManager.FindByNameAsync(userName);

            bag.Telephone = telephone;
            bag.User = user;

            bool newPhone = true;
            foreach(ShoppingBag element in _context.ShoppingBags.Include(i => i.Telephone).Include(i => i.User))
            {
                if(element.Telephone.Id == phoneId && element.User.UserName == userName)
                {
                    newPhone = false;
                    break;
                }
            }

            if (newPhone)
            {
                if (telephone != null && user != null)
                {
                    _context.ShoppingBags.Add(bag);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return BadRequest(new { message = "Incorrect phone or user!" });
                }
            }
            else
            {
                return BadRequest(new { message = "Phone already added in shopping bag" });
            }

            return Ok(bag);
        }
        [HttpDelete("userName")]
        [Route("CheckOut/{userName}")]
        public async Task<IActionResult> CheckOut(string userName)
        {
            foreach(ShoppingBag element in _context.ShoppingBags.Include(i => i.Telephone).Include(i => i.User))
            {
                if(element.User.UserName == userName)
                {
                    _context.ShoppingBags.Remove(element);
                }
            }
            //add to historin with pending for admin si 
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpGet("{idPhone}")]
        [Route("DeletePhoneBag/{idPhone}")]
        public async Task<IActionResult> DeletePhoneBag(int idPhone)
        {
            var phone = _context.ShoppingBags.Where(w => w.Id == idPhone).FirstOrDefault();
            if(phone != null)
            {
                _context.ShoppingBags.Remove(phone);
                await _context.SaveChangesAsync();
            }
            else
            {
                return BadRequest(new { message = "Phone isn't in the shopping bag!"});
            }
            return Ok();
        }
        [HttpPut("{idPhone}/{userName}")]
        [Route("EditQuantity/{idPhone}/{userName}")]
        public async Task<IActionResult> EditQuantity(int idPhone, string userName, ShoppingBag model)
        {
            if(model.Quantity > 0)
            {
                var phone = _context.Telephones.Find(idPhone);
                var user = await _userManager.FindByNameAsync(userName);
                model.Telephone = phone;
                model.User = user;
                _context.Entry(model).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            else
            {
                return BadRequest(new { message = "Invalid quantity value"});
            }

            return Ok(model);
        }
        [HttpGet("{userName}")]
        [Route("CheckBag/{userName}")]
        public int CheckBag(string userName)
        {
           return _context.ShoppingBags.Where(w => w.User.UserName == userName).Count();
        }
        [HttpPost]
        [Route("AddToHistoric")]
        public async Task<IActionResult> AddToHistoric(HistoricModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            var historic = new Historic
            {
                Address = model.Address,
                Status = model.Status,
                DateTime = DateTime.Today,
                Order = model.Order,
                PaymentMethod = model.PaymentMethod,
                Contact = model.Contact
            };

            historic.User = user;

            _context.Historics.Add(historic);
            await _context.SaveChangesAsync();

            return Ok(historic);
        }
        [HttpPut("{idPhone}")]
        [Route("UpdatePhone/{idPhone}")]
        public async Task<IActionResult> UpdatePhone(int idPhone, UpdatePhoneModel model)
        {
            var phone = _context.Telephones.Where(w => w.Id == idPhone).FirstOrDefault();
            phone.Name = model.NewName;
            phone.LaunchDate = model.NewLaunchDate;
            phone.Price = model.NewPrice;
            var company = _context.Companies.Where(w => w.Name == model.NewCompany).FirstOrDefault();
            phone.Company = company;

            _context.Entry(phone).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(phone);
        }
        [HttpGet]
        [Route("GetHistoric")]
        public IActionResult GetHistoric()
        {
            var result = _context.Historics.Include(i => i.User).Select(s => new { s.Id, s.DateTime, s.Status, s.Address, s.Order, s.PaymentMethod, s.Contact, UserId = s.User.Id, s.User.UserName });
            return Ok(result);
        }
    }
}