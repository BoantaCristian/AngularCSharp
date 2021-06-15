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

        [HttpGet("{idClient}")]
        [Route("GetAssociationOfClient/{idClient}")]
        public IEnumerable<Association> GetAssociationOfClient(string idClient)
        {
            var result = _context.Users.Include(i => i.Representative).ThenInclude(ti => ti.Association).Where(w => w.Id == idClient).Select(s => s.Representative.Association);
            return result;
        }

        [HttpGet]
        [Route("GetProviders")]
        public IEnumerable<Provider> GetProviders()
        {
            return _context.Providers;
        }

        [HttpGet("{idClient}")]
        [Route("GetProvidersOfClient/{idClient}")]
        public async Task<IActionResult> GetProvidersOfClient(string idClient)
        {
            var result = await _context.ClientProviders.Include(i => i.Client).Include(i => i.ElectricityProvider).Include(i => i.GasProvider).Include(i => i.WaterProvider).Where(w => w.Client.Id == idClient).Select(s => new { s.ElectricityProvider, s.GasProvider, s.WaterProvider }).FirstAsync();
            return Ok(result);
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

        [HttpDelete("{idArchive}")]
        [Authorize(Roles = "Admin")]
        [Route("DeleteArchive/{idArchive}")]
        public async Task<IActionResult> DeleteArchive(int idArchive)
        {
            var archive = await _context.FindAsync<Archive>(idArchive);

            if (archive != null)
            {
                try
                {
                    _context.Archives.Remove(archive);
                    await _context.SaveChangesAsync();

                    return Ok(archive);
                }
                catch (Exception e)
                {

                    throw e;
                }
            }
            else
                return BadRequest(new { message = "Archive not found!" });
        }

        [HttpDelete("{idReceipt}")]
        [Authorize(Roles = "Admin")]
        [Route("DeleteReceipt/{idReceipt}")]
        public async Task<IActionResult> DeleteReceipt(int idReceipt)
        {
            var receipt = await _context.FindAsync<Receipt>(idReceipt);

            if (receipt != null)
            {
                try
                {
                    _context.Receipts.Remove(receipt);
                    await _context.SaveChangesAsync();

                    return Ok(receipt);
                }
                catch (Exception e)
                {

                    throw e;
                }
            }
            else
                return BadRequest(new { message = "Receipt not found!" });
        }

        [HttpDelete("{idPayment}")]
        [Authorize(Roles = "Admin, Representative")]
        [Route("deletePayment/{idPayment}")]
        public async Task<IActionResult> deletePayment(int idPayment)
        {
            var payment = await _context.FindAsync<Payment>(idPayment);

            if (payment != null)
            {
                var paymentHasReceipts = false;
                foreach (Receipt receipt in _context.Receipts.Include(i => i.Payment))
                {
                    if (receipt.Payment.Id == payment.Id)
                    {
                        paymentHasReceipts = true;
                        break;
                    }
                }
                if (paymentHasReceipts)
                    return BadRequest(new { message = "Payment has receipts!" });
                else
                {

                    try
                    {
                        _context.Payments.Remove(payment);
                        await _context.SaveChangesAsync();

                        return Ok(payment);
                    }
                    catch (Exception e)
                    {

                        throw e;
                    }
                }
            }
            else
                return BadRequest(new { message = "Payment not found!" });
        }

        [HttpGet]
        [Authorize]
        [Route("GetArchives")]
        public IActionResult GetArchives()
        {
            try
            {
                //With Method Without posibility To Include Payments
                var result = _context.Archives.Include(w => w.Client).ThenInclude(ti => ti.Representative).Select(s => new { s.Id, s.Client.UserName, Association = s.Client.Representative.Association.Description, s.HotWaterKitchenQuantity, s.HotWaterBathroomQuantity, s.HotWaterKitchenDue, s.HotWaterBathroomDue, s.ColdWaterBathroomDue, s.ColdWaterBathroomQuantity, s.ColdWaterKitchenDue, s.ColdWaterKitchenQuantity, s.GasQuantity, s.GasDue, s.ElectricityQuantity, s.ElectricityDue, s.Date, s.TotalPayment });
                //with method: archives with users and their representatives
                var result1 = _context.Archives.Include(w => w.Client).ThenInclude(ti => ti.Representative);
                //linq with inner join of all tables and select desired columns
                var linq = (from archive in _context.Archives
                            join user in _context.Users on archive.Client.Id equals user.Id
                            join rep in _context.Users on user.Representative.Id equals rep.Id
                            join association in _context.Associations on rep.Association.Id equals association.Id
                            join payment in _context.Payments on user.Id equals payment.Client.Id
                            select new { user.UserName,
                                payment.UtilitiesPaper,
                                Association = association.Description,
                                archive.Id,
                                archive.HotWaterBathroomQuantity,
                                archive.HotWaterKitchenQuantity,
                                archive.HotWaterBathroomDue,
                                archive.HotWaterKitchenDue,
                                archive.ColdWaterBathroomDue,
                                archive.ColdWaterBathroomQuantity,
                                archive.ColdWaterKitchenDue,
                                archive.ColdWaterKitchenQuantity,
                                archive.ElectricityDue,
                                archive.ElectricityQuantity,
                                archive.GasDue,
                                archive.GasQuantity,
                                archive.Date,
                                archive.TotalPayment }).Distinct();
                //with linq from mehtod that selects users with reps and associations
                var linq2 = (from res in result1
                             join payment in _context.Payments on res.Client.Id equals payment.Client.Id
                             select new { res.Id,
                                 res.Client.UserName,
                                 res.HotWaterBathroomQuantity,
                                 res.HotWaterKitchenQuantity,
                                 res.HotWaterBathroomDue,
                                 res.HotWaterKitchenDue,
                                 res.ColdWaterBathroomDue,
                                 res.ColdWaterBathroomQuantity,
                                 res.ColdWaterKitchenDue,
                                 res.ColdWaterKitchenQuantity,
                                 res.ElectricityDue,
                                 res.ElectricityQuantity,
                                 res.GasDue,
                                 res.GasQuantity,
                                 res.Date,
                                 res.TotalPayment,
                                 Association = res.Client.Representative.Association.Description,
                                 payment.UtilitiesPaper }).Distinct();
                return Ok(linq2);
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        [HttpGet("{idRepresentative}")]
        [Authorize(Roles =  "Representative")]
        [Route("GetArchivesOfRepresentative/{idRepresentative}")]
        public IActionResult GetArchivesOfRepresentative(string idRepresentative)
        {
            try
            {
                //with method: archives with users and their representatives
                var result1 = _context.Archives.Include(w => w.Client).ThenInclude(ti => ti.Representative).Where(w => w.Client.Representative.Id == idRepresentative);
                //with linq from mehtod that selects users with reps and associations
                var linq2 = (from res in result1
                             join payment in _context.Payments on res.Client.Id equals payment.Client.Id
                             select new
                             {
                                 res.Id,
                                 res.Client.UserName,
                                 res.HotWaterBathroomQuantity,
                                 res.HotWaterKitchenQuantity,
                                 res.HotWaterBathroomDue,
                                 res.HotWaterKitchenDue,
                                 res.ColdWaterBathroomDue,
                                 res.ColdWaterBathroomQuantity,
                                 res.ColdWaterKitchenDue,
                                 res.ColdWaterKitchenQuantity,
                                 res.ElectricityDue,
                                 res.ElectricityQuantity,
                                 res.GasDue,
                                 res.GasQuantity,
                                 res.Date,
                                 res.TotalPayment,
                                 Association = res.Client.Representative.Association.Description,
                                 payment.UtilitiesPaper
                             }).Distinct();
                return Ok(linq2);
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        [HttpGet("{idClient}")]
        [Authorize(Roles = "Client")]
        [Route("GetArchivesOfClient/{idClient}")]
        public IActionResult GetArchivesOfClient(string idClient)
        {
            try
            {
                //with method: archives with users and their representatives
                var result1 = _context.Archives.Include(w => w.Client).ThenInclude(ti => ti.Representative).Where(w => w.Client.Id == idClient);
                //with linq from mehtod that selects users with reps and associations
                var linq2 = (from res in result1
                             join payment in _context.Payments on res.Client.Id equals payment.Client.Id
                             select new
                             {
                                 res.Id,
                                 res.Client.UserName,
                                 res.HotWaterBathroomQuantity,
                                 res.HotWaterKitchenQuantity,
                                 res.HotWaterBathroomDue,
                                 res.HotWaterKitchenDue,
                                 res.ColdWaterBathroomDue,
                                 res.ColdWaterBathroomQuantity,
                                 res.ColdWaterKitchenDue,
                                 res.ColdWaterKitchenQuantity,
                                 res.ElectricityDue,
                                 res.ElectricityQuantity,
                                 res.GasDue,
                                 res.GasQuantity,
                                 res.Date,
                                 res.TotalPayment,
                                 Association = res.Client.Representative.Association.Description,
                                 payment.UtilitiesPaper
                             }).Distinct();
                return Ok(linq2);
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
                var result = _context.Receipts.Include(w => w.Client).Select(s => new { s.Id, ReceiptClient = s.Client.UserName, s.PayDate, s.AmountPayed, s.ReceiptPaper });
                return Ok(result);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet("{idRepresentative}")]
        [Authorize(Roles = "Representative")]
        [Route("GetReceiptsOfRepresentative/{idRepresentative}")]
        public IActionResult GetReceiptsOfRepresentative(string idRepresentative)
        {
            try
            {
                var result = _context.Receipts.Include(w => w.Client).ThenInclude(ti => ti.Representative).Where(w => w.Client.Representative.Id == idRepresentative).Select(s => new { s.Id, ReceiptClient = s.Client.UserName, s.PayDate, s.AmountPayed, s.ReceiptPaper });
                return Ok(result);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet("{idClient}")]
        [Authorize(Roles = "Client")]
        [Route("GetReceiptsOfClient/{idClient}")]
        public IActionResult GetReceiptsOfClient(string idClient)
        {
            try
            {
                var result = _context.Receipts.Include(w => w.Client).Where(w => w.Client.Id == idClient).Select(s => new { s.Id, ReceiptClient = s.Client.UserName, s.PayDate, s.AmountPayed, s.ReceiptPaper });
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
                var result = _context.Payments.Include(w => w.Client).Select(s => new { s.Id, s.Date, s.Client.UserName, Remaining = Math.Round(s.RemainingToPay, 2), s.DaysDelay, s.Penalties, s.TotalDueWithPenalties, s.TotalPaid, s.WorkingCapitalStatus, s.SanitationStatus, s.UtilitiesPaper, s.PaymentStatus });
                return Ok(result);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet("{idRepresentative}")]
        [Authorize(Roles ="Representative")]
        [Route("GetPaymentsOfRepresentative/{idRepresentative}")]
        public IActionResult GetPaymentsOfRepresentative(string idRepresentative)
        {
            try
            {
                var result = _context.Payments.Include(w => w.Client).ThenInclude(ti => ti.Representative).Where(w => w.Client.Representative.Id == idRepresentative).Select(s => new { s.Id, s.Date, s.Client.UserName, Remaining = Math.Round(s.RemainingToPay, 2), s.DaysDelay, s.Penalties, s.TotalDueWithPenalties, s.TotalPaid, s.WorkingCapitalStatus, s.SanitationStatus, s.UtilitiesPaper, s.PaymentStatus });
                return Ok(result);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet("{idClient}")]
        [Authorize(Roles = "Client")]
        [Route("GetPaymentsOfClient/{idClient}")]
        public IActionResult GetPaymentsOfClient(string idClient)
        {
            try
            {
                var result = _context.Payments.Include(w => w.Client).Where(w => w.Client.Id == idClient).Select(s => new { s.Id, s.Date, s.Client.UserName, Remaining = Math.Round(s.RemainingToPay, 2), s.DaysDelay, s.Penalties, s.TotalDueWithPenalties, s.TotalPaid, s.WorkingCapitalStatus, s.SanitationStatus, s.UtilitiesPaper, s.PaymentStatus });
                return Ok(result);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Representative, Client")]
        [Route("EmitPayment")]
        public async Task<IActionResult> EmitPayment(EmitPaymentDTO model)
        {
            var client = await _userManager.FindByIdAsync(model.ClientId);
            var coldWaterProvider = await _context.ClientProviders.Include(i => i.Client).Include(i => i.WaterProvider).Select(s => s.WaterProvider.ColdWaterLiterPrice).FirstOrDefaultAsync();
            var hotWaterProvider = await _context.ClientProviders.Include(i => i.Client).Include(i => i.WaterProvider).Select(s => s.WaterProvider.HotWaterLiterPrice).FirstOrDefaultAsync();
            var electricityProvider = await _context.ClientProviders.Include(i => i.Client).Include(i => i.ElectricityProvider).Select(s => s.ElectricityProvider.ElectricityPrice).FirstOrDefaultAsync();
            var gasProvider = await _context.ClientProviders.Include(i => i.Client).Include(i => i.GasProvider).Select(s => s.GasProvider.GasPrice).FirstOrDefaultAsync();

            var paymentAlreadyExists = false;
            foreach (Payment pay in _context.Payments.Include(i => i.Client))
            {
                if (pay.Date.Month == model.Date.Month && client.UserName == pay.Client.UserName)
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
                    RemainingToPay = archive.TotalPayment,
                    TotalPaid = 0,
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

        [HttpPost]
        [Authorize]
        [Route("Pay")]
        public async Task<IActionResult> Pay(PayDTO model)
        {
            var payment = await _context.Payments.FindAsync(model.PaymentId);
            var client = await _userManager.FindByNameAsync(model.ClientUserName);

            var receipt = new Receipt
            {
                Payment = payment,
                Client = client,
                AmountPayed = model.AmountPaid,
                PayDate = DateTime.UtcNow,
                ReceiptPaper = model.ReceiptPaper
            };

            try
            {
                await _context.Receipts.AddAsync(receipt);
                await _context.SaveChangesAsync();

                await UpdatePaymentAsync(model);

                return Ok();

            }
            catch (Exception e)
            {

                throw e;
            }


        }

        public async Task UpdatePaymentAsync(PayDTO model)
        {
            var payment = await _context.Payments.FindAsync(model.PaymentId);

            payment.SanitationStatus = model.Sanitation;
            payment.WorkingCapitalStatus = model.WorkingCapital;

            payment.TotalPaid += model.AmountPaid;

            if ((payment.RemainingToPay - model.AmountPaid) >= 0)
            {
                payment.RemainingToPay = Math.Round((payment.RemainingToPay - model.AmountPaid), 2);
            }
            else
            {
                var difference = payment.RemainingToPay - model.AmountPaid;
                payment.TotalDueWithPenalties = payment.TotalDueWithPenalties + difference - payment.RemainingToPay;
                payment.TotalDueWithPenalties = Math.Round(payment.TotalDueWithPenalties, 2);
                payment.Penalties += difference; 
                payment.RemainingToPay = 0;
            }

            if (payment.TotalDueWithPenalties == 0)
            {
                payment.SanitationStatus = true;
                payment.WorkingCapitalStatus = true;
            }

            if (model.Sanitation && model.WorkingCapital && payment.TotalDueWithPenalties <= 0 && payment.RemainingToPay == 0)
            {
                payment.PaymentStatus = true;
            }

            try
            {
                _context.Entry(payment).State = EntityState.Modified;
                await _context.SaveChangesAsync();

            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IActionResult> UpdatePaymentPenaltiesAsync(Payment payment)
        {
            var associationDailyPenalty = payment.Client.Representative.Association.DayPenalty;
            var currentDate = DateTime.UtcNow;
            var differenceFromEmitToPay = currentDate.DayOfYear - payment.Date.DayOfYear;                    //difference days of a year
            var differenceFromEmitToPayDifferentYear = 365 - payment.Date.DayOfYear + currentDate.DayOfYear; //difference days until end of year + days passed
            var monthsDifferenceFromEmitToPay = 0;
            var totalWithoutPenalties = await _context.Archives.Include(i => i.Client).Where(w => w.Date.Month == payment.Date.Month).Where(w => w.Client.Id == payment.Client.Id).Select(s => s.TotalPayment).FirstAsync();
            var remainingToPay = payment.RemainingToPay;
            var sanitationAndWorkingCapitalTaxes = 0;

            if (currentDate.Month > payment.Date.Month)
                monthsDifferenceFromEmitToPay = currentDate.Month - payment.Date.Month;                      //difference months of a year
            if (currentDate.Month < payment.Date.Month)
                monthsDifferenceFromEmitToPay = 12 - currentDate.Month + payment.Date.Month;    //difference months of a year

            if (!payment.SanitationStatus || !payment.WorkingCapitalStatus || (payment.SanitationStatus && payment.WorkingCapitalStatus && remainingToPay >= 0))
                for (int i = 0; i < monthsDifferenceFromEmitToPay - 1; i++)
                    sanitationAndWorkingCapitalTaxes += 15;

            if (associationDailyPenalty == null)
                return BadRequest(new { message = "Association not found" });

            if (!payment.PaymentStatus)
            {
                if (currentDate.Year == payment.Date.Year && differenceFromEmitToPay >= 30) //same year
                {
                    if (remainingToPay > 0)
                        payment.Penalties = sanitationAndWorkingCapitalTaxes + (remainingToPay * associationDailyPenalty) / 100 * (differenceFromEmitToPay - 30);

                    payment.DaysDelay = differenceFromEmitToPay - 30;
                    payment.TotalDueWithPenalties = remainingToPay + payment.Penalties;

                    _context.Entry(payment).State = EntityState.Modified;
                }
                if (currentDate.Year > payment.Date.Year && differenceFromEmitToPayDifferentYear >= 30) //year of check > year of emit
                {
                    if (remainingToPay > 0)
                        payment.Penalties = sanitationAndWorkingCapitalTaxes + (remainingToPay * associationDailyPenalty) / 100 * (differenceFromEmitToPayDifferentYear - 30);

                    payment.DaysDelay = differenceFromEmitToPayDifferentYear - 30;
                    payment.TotalDueWithPenalties = remainingToPay + payment.Penalties;

                    _context.Entry(payment).State = EntityState.Modified;
                }
            }

            return Ok();
        }

        [HttpGet]
        [Authorize]
        [Route("UpdatePenalties")]
        public async Task<IActionResult> UpdatePenalties()
        {
            try
            {
                foreach (Payment payment in _context.Payments.Include(i => i.Client).ThenInclude(i => i.Representative).ThenInclude(ti => ti.Association))
                {
                    await UpdatePaymentPenaltiesAsync(payment);
                }

                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        [HttpGet("{idRepresentative}")]
        [Authorize(Roles ="Representative")]
        [Route("UpdatePenaltiesOfRepresentative/{idRepresentative}")]
        public async Task<IActionResult> UpdatePenaltiesOfRepresentative(string idRepresentative)
        {
            try
            {
                foreach (Payment payment in _context.Payments.Include(i => i.Client).ThenInclude(i => i.Representative).ThenInclude(ti => ti.Association).Where(w => w.Client.Representative.Id == idRepresentative))
                {
                    await UpdatePaymentPenaltiesAsync(payment);
                }

                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        [HttpPost("{idReceipt}")]
        [Authorize(Roles ="Admin, Representative")]
        [Route("AddReceiptPaper/{idReceipt}")]
        public async Task<IActionResult> AddReceiptPaper(int idReceipt, AddReceiptPaper model)
        {
            var receipt = await _context.Receipts.FindAsync(idReceipt);

            try
            {
                receipt.ReceiptPaper = model.ReceiptPaper;
                _context.Entry(receipt).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}