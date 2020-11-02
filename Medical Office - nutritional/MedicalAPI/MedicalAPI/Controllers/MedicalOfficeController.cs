using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalAPI.Models;
using MedicalAPI.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalOfficeController : ControllerBase
    {
        private ApplicationContext _context;
        private readonly UserManager<User> _userManager;

        public MedicalOfficeController(ApplicationContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpGet]
        [Route("GetDoctors")]
        public async Task<IActionResult> GetDoctors()
        {
            var result = await _userManager.GetUsersInRoleAsync("Doctor");
            var doctors = result.Select(s => new { s.UserName, s.Address, s.Email, s.PhoneNumber, s.Id });
            return Ok(doctors);
        }
        [HttpGet]
        [Route("GetPatients")]
        public async Task<IActionResult> GetPatients()
        {
            var result = await _userManager.GetUsersInRoleAsync("Pacient");

            return Ok(result.Select(s => new { s.UserName, s.Address, s.Email, s.PhoneNumber, s.IdDoctor, s.Id }));
        }
        [HttpGet("{idDoctor}")]
        [Route("GetPatientIllMedicine/{idDoctor}")]
        public IActionResult GetPatientIllMedicine(string idDoctor)
        {
            var patientDetails = (from illness in _context.Illnesses
                                  join medicine in _context.Medicines on illness.Id equals medicine.Illness.Id
                                  join medicament in _context.Medicaments on medicine.Medicament.Id equals medicament.Id
                                  join patient in _userManager.Users on illness.Id equals patient.Illnesses.Id
                                  where patient.IdDoctor == idDoctor
                                  select new
                                  {
                                      patient.UserName,
                                      ill = illness.Name,
                                      illness.Risk,
                                      illness.Symptoms,
                                      medicament = medicament.Name,
                                      medicine.Duration,
                                      medicine.DailyDose,
                                      medicine.Period,
                                      patient.Cured
                                  });
            
            if (patientDetails == null)
            {
                return NotFound();
            }
            return Ok(patientDetails);
        }
        [HttpGet("{idDoctor}")]
        [Route("GetCurrentPatients/{idDoctor}")]
        public async Task<IActionResult> GetCurrentPatients(string idDoctor)
        {
            var patients = await _userManager.GetUsersInRoleAsync("Pacient");
            var result = patients.Where(w => w.IdDoctor == idDoctor);

            return Ok(result.Select(s => new { s.UserName, s.Address, s.Email, s.PhoneNumber, s.Id }));
        }
        [HttpGet]
        [Route("GetAppointments")]
        public IActionResult GetAppointments()
        {
            var result = _context.Appointments.Include(i => i.User).Select(s => new { s.Id, s.User.UserName, s.IdPacient, s.Date, s.Hour, s.Minute });
            
            return Ok(result);
        }
        [HttpGet("idPatient")]
        [Route("GetPatientAppointments/{idPatient}")]
        public IActionResult GetPatientAppointments(string idPatient)
        {
            var result = _context.Appointments.Include(i => i.User).Where(w => w.IdPacient.Id == idPatient).Select(s => new { s.Id, s.User.UserName, s.IdPacient, s.Date, s.Hour, s.Minute });
            
            return Ok(result);
        }
        [HttpGet("idDoctor")]
        [Route("GetCurrentAppointments/{idDoctor}")]
        public IActionResult GetCurrentAppointments(string idDoctor)
        {
            var result = _context.Appointments.Include(i => i.User).Where(w => w.User.Id == idDoctor).Select(s => new { s.Id, s.User.UserName, s.IdPacient, s.Date, s.Hour, s.Minute });

            return Ok(result);
        }
        [HttpGet]
        [Authorize]
        [Route("Medicaments")]
        public IEnumerable<Medicament> GetMedicaments()
        {
            return _context.Medicaments;
        }
        [HttpGet]
        [Route("Illnesses")]
        public IEnumerable<Illness> Illnesses()
        {
            return _context.Illnesses;
        }
        [HttpPost]
        [Route("AddMedicament")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddMedicament(MedicamentModel model)
        {
            var medicament = new Medicament
            {
                Name = model.Name,
                Price = model.Price
            };

            _context.Medicaments.Add(medicament);
            await _context.SaveChangesAsync();

            return Ok(medicament);
        }
        [HttpPost]
        [Route("AddIllness")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddIllness(IllnessModel model)
        {
            var illness = new Illness
            {
                Name = model.Name,
                Symptoms = model.Symptoms,
                Risk = model.Risk
            };

            _context.Illnesses.Add(illness);
            await _context.SaveChangesAsync();

            var lastIll = _context.Illnesses.Select(s => s.Id).Last();

            return Ok(lastIll);
        }
        [HttpPost]
        [Route("AddMedicine")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddMedicine(MedicineModel model)
        {
            var medicine = new Medicine
            {
                Duration = model.Duration,
                DailyDose = model.DailyDose,
                Period = model.Period
            };
            var ill = _context.Illnesses.Find(model.IllnessId);
            var medicament = _context.Medicaments.Find(model.MedicamentId);

            medicine.Illness = ill;
            medicine.Medicament = medicament;

            _context.Medicines.Add(medicine);
            await _context.SaveChangesAsync();
            //await _context.AddAsync<Patient>(patient);
            //_context.SaveChanges();

            return Ok(medicine);
        }
        [HttpPost]
        [Route("AddAppointment")]
        [Authorize(Roles = "Admin, Doctor")]
        public async Task<IActionResult> AddAppointment(AppointmentModel model)
        {
            var appointment = new Appointments
            {
                Date = model.Date,
                Hour = model.Hour,
                Minute = model.Minute
            };
            var doctor = await _userManager.FindByIdAsync(model.MedicId);
            var patient = await _userManager.FindByIdAsync(model.PatientId);

            appointment.User = doctor;
            appointment.IdPacient = patient;

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return Ok(appointment);
        }
        [HttpDelete("{idAppointment}")]
        [Route("DeleteAppointment/{idAppointment}")]
        [Authorize(Roles = "Admin, Doctor")]
        public async Task<IActionResult> DeleteAppointment(int idAppointment)
        {
            var appointment = _context.Appointments.Where(w => w.Id == idAppointment).FirstOrDefault();

            var appointmentWithUsers = _context.Appointments.Include(i => i.User).Where(w => w.Id == idAppointment).Select(s => new { s.Id, IdDoctor = s.User.Id, IdPacient = s.IdPacient.Id, s.Date, s.Hour, s.Minute }).First();

            _context.Appointments.Remove(appointment);

            var appointmentDoctor = appointmentWithUsers.IdDoctor;
            var appointmentPacient = appointmentWithUsers.IdPacient;

            var historic = new Historic
            {
                Date = appointment.Date,
                Hour = appointment.Hour,
                Minute = appointment.Minute
            };

            historic.IdPacient = appointmentPacient;
            historic.IdMedic = appointmentDoctor;

            _context.Historics.Add(historic);
            await _context.SaveChangesAsync();

            return Ok(appointmentWithUsers);
        }
        [HttpGet]
        [Route("GetHistoric")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetHistoric()
        {
            GetHistoricModel[] result = new GetHistoricModel[_context.Historics.Count()].Select(h => new GetHistoricModel()).ToArray();
            var historic = _context.Historics;

            int i = 0;
            foreach (Historic hist in historic)
            {
                var pacient = await _userManager.FindByIdAsync(hist.IdPacient);
                var doctor = await _userManager.FindByIdAsync(hist.IdMedic);

                result[i].Date = hist.Date;
                result[i].Hour = hist.Hour;
                result[i].Minute = hist.Minute;
                result[i].IdMedic = doctor.UserName;
                result[i].IdPacient = pacient.UserName;
                i++;
            }
            return Ok(result);
        }
        [HttpGet("{idIllness}/{idPatient}")]
        [Route("AddIllnessToExistingPatient/{idIllness}/{idPatient}")]
        public async Task<IActionResult> AddIllnessToExistingPatient(int idIllness, string idPatient)
        {
            var patient = await _userManager.FindByIdAsync(idPatient);
            var ill = await _context.Illnesses.FindAsync(idIllness);
            patient.Illnesses = ill;
            patient.Cured = false;

            await _userManager.UpdateAsync(patient);
            
            return Ok(new { ill.Name });
        }
        [HttpGet("{patientUserName}")]
        [Route("CurePatient/{patientUserName}")]
        public async Task<IActionResult> CurePatient(string patientUserName)
        {
            var patient = await _userManager.FindByNameAsync(patientUserName);
            patient.Illnesses = null;
            patient.Cured = true;

            _context.Entry<User>(patient).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            await _userManager.UpdateAsync(patient);

            return Ok(patient);
        }
        [HttpGet("{idPatient}")]
        [Route("GetPatientDetails/{idPatient}")]
        public IActionResult GetPatientDetails(string idPatient)
        {
            var result = _context.Details.Where(w => w.User.Id == idPatient).First();

            if (result != null)
                return Ok(result);
            else
                return BadRequest(new { message = "No details" });
        }
        [HttpGet("{idDoctor}")]
        [Route("GetPatientDoctorDetails/{idDoctor}")]
        public IActionResult GetPatientDoctorDetails(string idDoctor)
        {
            var result = _context.Details.Include(i => i.User).Where(w => w.User.IdDoctor == idDoctor);

            if (result != null)
                return Ok(result);
            else
                return BadRequest(new { message = "No details" });
        }
        [HttpPost("{idPatient}/{idDetail}")]
        [Route("AddPatientDetails/{idPatient}/{idDetail}")]
        public async Task<IActionResult> AddPatientDetails(string idPatient, int idDetail, DetailsModel model)
        {
            var alreadyExists = false;
            foreach (Details det in _context.Details.Include(i => i.User))
            {
                if (det.User.Id == idPatient)
                {
                    alreadyExists = true;
                }
            }
            //add details
            if (alreadyExists)
            {
                var detail = _context.Details.Where(w => w.Id == idDetail).First();

                detail.Height = model.Height;
                detail.Weight = model.Weight;
                detail.Age = model.Age;
                detail.BodyType = model.BodyType;
                detail.Activity = model.Activity;
                //establish body state
                double heightMetters = (double)model.Height / 100;
                detail.MassIndex = model.Weight / Math.Pow(heightMetters, 2);
                if (detail.MassIndex < 16)
                {
                    detail.BodyState = "Sever underweight";
                }
                if (detail.MassIndex >= 16 && detail.MassIndex < 17)
                {
                    detail.BodyState = "Moderate underweight";
                }
                if (detail.MassIndex >= 17 && detail.MassIndex < 18.5)
                {
                    detail.BodyState = "Underweight";
                }
                if (detail.MassIndex >= 18.5 && detail.MassIndex < 25)
                {
                    detail.BodyState = "Normal weight";
                }
                if (detail.MassIndex >= 25 && detail.MassIndex < 30)
                {
                    detail.BodyState = "Overweight";
                }
                if (detail.MassIndex >= 30 && detail.MassIndex < 35)
                {
                    detail.BodyState = "Obesity";
                }
                if (detail.MassIndex >= 35 && detail.MassIndex < 40)
                {
                    detail.BodyState = "Moderate obesity";
                }
                if (detail.MassIndex > 40)
                {
                    detail.BodyState = "Morbid obesity";
                }
                //establish ideal weight
                switch (detail.BodyType)
                {
                    case "Slim":
                        detail.IdealWeight = (model.Height - 100 + model.Age / 10) * 0.9;
                        break;
                    case "Fit":
                        detail.IdealWeight = (model.Height - 100 + model.Age / 10) * 0.9 * 0.9;
                        break;
                    case "Robust":
                        detail.IdealWeight = (model.Height - 100 + model.Age / 10) * 0.9 * 1.1;
                        break;
                    default:
                        break;
                }
                //establish basal metabolism
                detail.BasalMetabolism = detail.IdealWeight * 24;
                //establish daily caloric ratio
                switch (detail.Activity)
                {
                    case "Sedentary":
                        detail.CaloricRatio = detail.BasalMetabolism + 800;
                        break;
                    case "Light":
                        detail.CaloricRatio = detail.BasalMetabolism + 1150;
                        break;
                    case "Moderate":
                        detail.CaloricRatio = detail.BasalMetabolism + 1600;
                        break;
                    case "Heavy":
                        detail.CaloricRatio = detail.BasalMetabolism + 2000;
                        break;
                    case "Intense":
                        detail.CaloricRatio = detail.BasalMetabolism + 4000;
                        break;
                    default:
                        break;
                }
                //establish glucid lipid protein in kcal
                detail.Glucid = (double)detail.CaloricRatio * (55.0 / 100);
                detail.Lipid = (double)detail.CaloricRatio * (30.0 / 100);
                detail.Protein = (double)detail.CaloricRatio * (15.0 / 100);
                //establish glucid lipid protein in grams
                detail.GlucidGrams = detail.Glucid / 4.1;
                detail.LipidGrams = detail.Lipid / 9.3;
                detail.ProteindGrams = detail.Protein / 4.1;
                //establish meals in in cal
                detail.Breakfast = (double)detail.CaloricRatio * (20.0 / 100);
                detail.Snack1 = (double)detail.CaloricRatio * (10.0 / 100);
                detail.Launch = (double)detail.CaloricRatio * (40.0 / 100);
                detail.Snack2 = (double)detail.CaloricRatio * (10.0 / 100);
                detail.Dinner = (double)detail.CaloricRatio * (20.0 / 100);
                //establish meals in glucid lipid protein
                detail.BreakfastGlucid = (double)detail.GlucidGrams * (20.0 / 100);
                detail.Snack1Glucid = (double)detail.GlucidGrams * (10.0 / 100);
                detail.LaunchGlucid = (double)detail.GlucidGrams * (40.0 / 100);
                detail.Snack2Glucid = (double)detail.GlucidGrams * (10.0 / 100);
                detail.DinnerGlucid = (double)detail.GlucidGrams * (20.0 / 100);
                detail.BreakfastLipid = (double)detail.LipidGrams * (20.0 / 100);
                detail.Snack1Lipid = (double)detail.LipidGrams * (10.0 / 100);
                detail.LaunchLipid = (double)detail.LipidGrams * (40.0 / 100);
                detail.Snack2Lipid = (double)detail.LipidGrams * (10.0 / 100);
                detail.DinnerLipid = (double)detail.LipidGrams * (20.0 / 100);
                detail.BreakfastProtein = (double)detail.ProteindGrams * (20.0 / 100);
                detail.Snack1Protein = (double)detail.ProteindGrams * (10.0 / 100);
                detail.LaunchProtein = (double)detail.ProteindGrams * (40.0 / 100);
                detail.Snack2Protein = (double)detail.ProteindGrams * (10.0 / 100);
                detail.DinnerProtein = (double)detail.ProteindGrams * (20.0 / 100);

                _context.Entry(detail).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(detail);
            }
            //update details
            else
            {
                var patient = await _userManager.FindByIdAsync(idPatient);

                var detail = new Details
                {
                    Height = model.Height,
                    Weight = model.Weight,
                    Age = model.Age,
                    BodyType = model.BodyType,
                    Activity = model.Activity
            };
                detail.User = patient;
                //establish body state
                double heightMetters = (double)model.Height / 100;
                detail.MassIndex = model.Weight / Math.Pow(heightMetters, 2);
                if(detail.MassIndex < 16)
                {
                    detail.BodyState = "Sever underweight";
                }
                if(detail.MassIndex >= 16 && detail.MassIndex < 17)
                {
                    detail.BodyState = "Moderate underweight";
                }if(detail.MassIndex >= 17 && detail.MassIndex < 18.5)
                {
                    detail.BodyState = "Underweight";
                }if(detail.MassIndex >= 18.5 && detail.MassIndex < 25)
                {
                    detail.BodyState = "Normal weight";
                }if(detail.MassIndex >= 25 && detail.MassIndex < 30)
                {
                    detail.BodyState = "Overweight";
                }if(detail.MassIndex >= 30 && detail.MassIndex < 35)
                {
                    detail.BodyState = "Obesity";
                }if(detail.MassIndex >= 35 && detail.MassIndex < 40)
                {
                    detail.BodyState = "Moderate obesity";
                }
                if (detail.MassIndex > 40)
                {
                    detail.BodyState = "Morbid obesity";
                }
                //establish ideal weight
                switch (detail.BodyType)
                {
                    case "Slim":
                        detail.IdealWeight = (model.Height - 100 + model.Age / 10) * 0.9;
                        break;
                    case "Fit":
                        detail.IdealWeight = (model.Height - 100 + model.Age / 10) * 0.9 * 0.9;
                        break;
                    case "Robust":
                        detail.IdealWeight = (model.Height - 100 + model.Age / 10) * 0.9 * 1.1;
                        break;
                    default:
                        break;
                }
                //establish basal metabolism
                detail.BasalMetabolism = detail.IdealWeight * 24;
                //establish daily caloric ratio
                switch (detail.Activity)
                {
                    case "Sedentary":
                        detail.CaloricRatio = detail.BasalMetabolism + 800;
                        break;
                    case "Light":
                        detail.CaloricRatio = detail.BasalMetabolism + 1150;
                        break;
                    case "Moderate":
                        detail.CaloricRatio = detail.BasalMetabolism + 1600;
                        break;
                    case "Heavy":
                        detail.CaloricRatio = detail.BasalMetabolism + 2000;
                        break;
                    case "Intense":
                        detail.CaloricRatio = detail.BasalMetabolism + 4000;
                        break;
                    default:
                        break;
                }
                //establish glucid lipid protein in kcal
                detail.Glucid = (double)detail.CaloricRatio * (55.0 / 100);
                detail.Lipid = (double)detail.CaloricRatio * (30.0 / 100);
                detail.Protein = (double)detail.CaloricRatio * (15.0 / 100);
                //establish glucid lipid protein in grams
                detail.GlucidGrams = detail.Glucid * 4.1;
                detail.LipidGrams = detail.Lipid * 9.3;
                detail.ProteindGrams = detail.Protein * 4.1;
                //establish meals in in cal
                detail.Breakfast = (double)detail.CaloricRatio * (20.0 / 100);
                detail.Snack1 = (double)detail.CaloricRatio * (10.0 / 100);
                detail.Launch = (double)detail.CaloricRatio * (40.0 / 100);
                detail.Snack2 = (double)detail.CaloricRatio * (10.0 / 100);
                detail.Dinner = (double)detail.CaloricRatio * (20.0 / 100);
                //establish meals in glucid lipid protein
                detail.BreakfastGlucid = (double)detail.GlucidGrams * (20.0 / 100);
                detail.Snack1Glucid = (double)detail.GlucidGrams * (10.0 / 100);
                detail.LaunchGlucid = (double)detail.GlucidGrams * (40.0 / 100);
                detail.Snack2Glucid = (double)detail.GlucidGrams * (10.0 / 100);
                detail.DinnerGlucid = (double)detail.GlucidGrams * (20.0 / 100);
                detail.BreakfastLipid = (double)detail.LipidGrams * (20.0 / 100);
                detail.Snack1Lipid = (double)detail.LipidGrams * (10.0 / 100);
                detail.LaunchLipid = (double)detail.LipidGrams * (40.0 / 100);
                detail.Snack2Lipid = (double)detail.LipidGrams * (10.0 / 100);
                detail.DinnerLipid = (double)detail.LipidGrams * (20.0 / 100);
                detail.BreakfastProtein = (double)detail.ProteindGrams * (20.0 / 100);
                detail.Snack1Protein = (double)detail.ProteindGrams * (10.0 / 100);
                detail.LaunchProtein = (double)detail.ProteindGrams * (40.0 / 100);
                detail.Snack2Protein = (double)detail.ProteindGrams * (10.0 / 100);
                detail.DinnerProtein = (double)detail.ProteindGrams * (20.0 / 100);

                _context.Details.Add(detail);
                await _context.SaveChangesAsync();

                return Ok(detail);
            }

        }
    }
}