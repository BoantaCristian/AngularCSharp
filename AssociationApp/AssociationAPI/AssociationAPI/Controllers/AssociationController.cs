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
using Microsoft.EntityFrameworkCore;

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
        [Route("GetAssociations")]
        public IEnumerable<Association> GetAssociations()
        {
            return _context.Associations;
        }

        [HttpGet]
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

        [HttpDelete("{idAssociation}")]
        [Authorize(Roles = "Admin")]
        [Route("DeleteAssociation/{idAssociation}")]
        public async Task<IActionResult> DeleteAssociation(int idAssociation)
        {
            var association = await _context.FindAsync<Association>(idAssociation);

            if (association != null)
            {
                var associationHasRepresentant = false;
                foreach (User user in _context.Users.Include(i => i.Association).Where(w => w.Association.Id != null))
                {
                    if (user.Association.Id == idAssociation)
                    {
                        associationHasRepresentant = true;
                        break;
                    }
                }
                if (associationHasRepresentant)
                    return BadRequest(new { message = "Association has representants!" });
                else
                {
                    try
                    {
                        _context.Associations.Remove(association);
                        await _context.SaveChangesAsync();

                        return Ok(association);
                    }
                    catch (Exception e)
                    {

                        throw e;
                    }
                }
            }
            else
                return BadRequest(new { message = "Association not found!" });
        }
        
        [HttpDelete("{idProvider}")]
        [Authorize(Roles = "Admin")]
        [Route("DeleteProvider/{idProvider}")]
        public async Task<IActionResult> DeleteProvider(int idProvider)
        {
            var provider = await _context.FindAsync<Provider>(idProvider);

            if (provider != null)
            {
                var providerHasClient = false;
                foreach (ClientProvider clientProvider in _context.ClientProviders)
                {
                    if (clientProvider.WaterFk == idProvider || clientProvider.ElectricityFk == idProvider || clientProvider.GasFk == idProvider)
                    {
                        providerHasClient = true;
                        break;
                    }
                }
                if (providerHasClient)
                    return BadRequest(new { message = "Provider supplies clients!" });
                else
                { 
                
                    try
                    {
                        _context.Providers.Remove(provider);
                        await _context.SaveChangesAsync();

                        return Ok(provider);
                    }
                    catch (Exception e)
                    {

                        throw e;
                    }
                }
            }
            else
                return BadRequest(new { message = "Provider not found!" });
        }

        [HttpGet]
        [Authorize]
        [Route("GetArchives")]
        public IActionResult GetArchives()
        {
            try
            {
                var result = _context.Archives.Include(w => w.Client).ThenInclude(ti => ti.Representative).Select(s => new { s.Client.UserName, Association = s.Client.Representative.Association.Description, s.HotWaterKitchenQuantity, s.HotWaterBathroomQuantity, s.HotWaterKitchenDue, s.HotWaterBathroomDue, s.ColdWaterBathroomDue, s.ColdWaterBathroomQuantity, s.ColdWaterKitchenDue, s.ColdWaterKitchenQuantity, s.GasQuantity, s.GasDue, s.ElectricityQuantity, s.ElectricityDue, s.Date, s.TotalPayment });
                return Ok(result);
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        [HttpGet]
        [Authorize]
        [Route("GetReceipts")]
        public IActionResult GetReceipts()
        {
            try
            {
                var result = _context.Receipts.Include(w => w.Client).Select(s => new { s.Client.UserName, s.PayDate, s.AmountPayed });
                return Ok(result);
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        [HttpGet]
        [Authorize]
        [Route("GetPayments")]
        public IActionResult GetPayments()
        {
            try
            {
                var result = _context.Payments.Include(w => w.Client).Select(s => new { s.Client.UserName, s.DaysDelay, s.Penalties, s.TotalDueWithPenalties, s.WorkingCapitalStatus, s.SanitationStatus, s.UtilitiesPaper, s.PaymentStatus });
                return Ok(result);
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        [HttpPost]
        [Authorize(Roles ="Admin, Representative, Client")]
        [Route("EmitPayment")]
        public async Task<IActionResult> EmitPayment(EmitPaymentDTO model)
        {
            var client = await _userManager.FindByIdAsync(model.ClientId);
            var coldWaterProvider = await _context.ClientProviders.Include(i => i.Client).Include(i => i.WaterProvider).Select(s => s.WaterProvider.ColdWaterLiterPrice).FirstOrDefaultAsync();
            var hotWaterProvider = await _context.ClientProviders.Include(i => i.Client).Include(i => i.WaterProvider).Select(s => s.WaterProvider.HotWaterLiterPrice).FirstOrDefaultAsync();
            var electricityProvider = await _context.ClientProviders.Include(i => i.Client).Include(i => i.ElectricityProvider).Select(s => s.ElectricityProvider.ElectricityPrice).FirstOrDefaultAsync();
            var gasProvider = await _context.ClientProviders.Include(i => i.Client).Include(i => i.GasProvider).Select(s => s.GasProvider.GasPrice).FirstOrDefaultAsync();

            var paymentAlreadyExists = false;
            foreach (Payment pay in _context.Payments) //edit ulterior in cazul in care admin face update cu entry gol
            {
                if(pay.Date.Month == model.Date.Month)
                {
                    paymentAlreadyExists = true;
                    break;
                }
            }

            if (paymentAlreadyExists)
            {
                return BadRequest(new { message = "Payment already exists for this month!" });
            }
            else
            {
                var archive = new Archive
                {
                    Client = client,
                    Date = model.Date,
                    HotWaterKitchenQuantity = model.HotWaterKitchenQuantity,
                    HotWaterBathroomQuantity = model.HotWaterBathroomQuantity,
                    ColdWaterKitchenQuantity = model.ColdWaterKitchenQuantity,
                    ColdWaterBathroomQuantity = model.ColdWaterBathroomQuantity,
                    ElectricityQuantity = model.ElectricityQuantity,
                    GasQuantity = model.GasQuantity,
                    HotWaterKitchenDue = model.HotWaterKitchenQuantity * hotWaterProvider,
                    HotWaterBathroomDue = model.HotWaterBathroomQuantity * hotWaterProvider,
                    ColdWaterKitchenDue = model.ColdWaterKitchenQuantity * coldWaterProvider,
                    ColdWaterBathroomDue = model.ColdWaterBathroomQuantity * coldWaterProvider,
                    ElectricityDue = model.ElectricityQuantity * electricityProvider,
                    GasDue = model.GasQuantity * gasProvider,
                    TotalPayment = model.HotWaterKitchenQuantity * hotWaterProvider +
                               model.HotWaterBathroomQuantity * hotWaterProvider +
                               model.ColdWaterKitchenQuantity * coldWaterProvider +
                               model.ColdWaterBathroomQuantity * coldWaterProvider +
                               model.ElectricityQuantity * electricityProvider +
                               model.GasQuantity * gasProvider
                };

                var payment = new Payment
                {
                    Client = client,
                    Date = model.Date,
                    DaysDelay = 0,
                    Penalties = 0,
                    TotalDueWithPenalties = archive.TotalPayment,
                    WorkingCapitalStatus = false,
                    SanitationStatus = false,
                    PaymentStatus = false,
                    UtilitiesPaper = model.UtilitiesPaper
                };

                try
                {
                    await _context.Archives.AddAsync(archive);
                    await _context.Payments.AddAsync(payment);

                    await _context.SaveChangesAsync();

                    return Ok();
                }
                catch (Exception e)
                {

                    throw e;
                }
            }
        }
        [HttpGet]
        [Authorize]
        [Route("UpdatePenalties")]
        public async Task<IActionResult> UpdatePenalties()
        {
            try
            {
                foreach (Payment payment in _context.Payments.Include(i => i.Client))
                {
                    var associationDailyPenalty = await _context.Users.Include(i => i.Association).Where(w => w.UserName == payment.Client.UserName).Select(s => s.Association.DayPenalty).FirstAsync();
                    var currentDate = DateTime.UtcNow;
                    var differenceFromEmitToPay = currentDate.DayOfYear - payment.Date.DayOfYear;                    //difference days of a year
                    var differenceFromEmitToPayDifferentYear = 365 - payment.Date.DayOfYear + currentDate.DayOfYear; //difference days until end of year + days passed
                    var monthsDifferenceFromEmitToPay = 0;
                    var totalWithoutPenalties = await _context.Archives.Where(w => w.Date.Month == payment.Date.Month).Select(s => s.TotalPayment).FirstAsync();

                    if(currentDate.Month > payment.Date.Month)
                    {
                        monthsDifferenceFromEmitToPay = currentDate.Month - payment.Date.Month;                      //difference months of a year
                    }
                    if (currentDate.Month < payment.Date.Month)
                    {
                        monthsDifferenceFromEmitToPay = 12 - currentDate.Month + payment.Date.Month;    //difference months of a year
                    }



                    if (!payment.SanitationStatus || !payment.WorkingCapitalStatus)
                    {
                        for (int i = 0; i < monthsDifferenceFromEmitToPay; i++)
                        {
                            totalWithoutPenalties += 15;  
                        }
                    }

                    if (associationDailyPenalty == null)
                        return BadRequest(new { message = "Association not found" });

                    if (currentDate.Year == payment.Date.Year && differenceFromEmitToPay >= 30) //same year
                    {
                        payment.Penalties = (payment.TotalDueWithPenalties * associationDailyPenalty) / 100 * (differenceFromEmitToPay - 30);
                        payment.DaysDelay = differenceFromEmitToPay - 30;
                        payment.TotalDueWithPenalties = totalWithoutPenalties + payment.Penalties;

                        _context.Entry(payment).State = EntityState.Modified;
                    }

                    if (currentDate.Year > payment.Date.Year && differenceFromEmitToPayDifferentYear >= 30) //year of check > year of emit
                    {
                        payment.Penalties = (payment.TotalDueWithPenalties * associationDailyPenalty) / 100 * (differenceFromEmitToPayDifferentYear - 30);
                        payment.DaysDelay = differenceFromEmitToPayDifferentYear - 30;
                        payment.TotalDueWithPenalties = totalWithoutPenalties + payment.Penalties;

                        _context.Entry(payment).State = EntityState.Modified;
                    }
                }

                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        [HttpGet("{idPayment}")]
        [Authorize]
        [Route("UpdatePenalty/{idPayment}")]
        public async Task<IActionResult> UpdatePenalty(int idPayment)
        {
            var payment = await _context.Payments.FindAsync(idPayment);

            try
            {
                var associationDailyPenalty = await _context.Users.Include(i => i.Association).Where(w => w.UserName == payment.Client.UserName).Select(s => s.Association.DayPenalty).FirstAsync();
                var currentDate = DateTime.UtcNow;
                var differenceFromEmitToPay = currentDate.DayOfYear - payment.Date.DayOfYear;                    //difference days of a year
                var differenceFromEmitToPayDifferentYear = 365 - payment.Date.DayOfYear + currentDate.DayOfYear; //difference days until end of year + days passed

                if (currentDate.Year == payment.Date.Year && differenceFromEmitToPay >= 30) //same year
                {
                    payment.Penalties = (payment.TotalDueWithPenalties * associationDailyPenalty) / 100 * (differenceFromEmitToPay - 30);
                    payment.DaysDelay = differenceFromEmitToPay - 30;
                    payment.TotalDueWithPenalties += payment.Penalties;

                    _context.Entry(payment).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }

                if (currentDate.Year > payment.Date.Year && differenceFromEmitToPayDifferentYear >= 30) //year of check > year of emit
                {
                    payment.Penalties = (payment.TotalDueWithPenalties * associationDailyPenalty) / 100 * (differenceFromEmitToPayDifferentYear - 30);
                    payment.DaysDelay = differenceFromEmitToPayDifferentYear - 30;
                    payment.TotalDueWithPenalties += payment.Penalties;

                    _context.Entry(payment).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }

                return Ok();
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}