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
    public class MedicalController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        public MedicalController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpGet]
        [Route("GetPatients")]
        public IActionResult GetPatients()
        {
            var patients = _context.Patients.Include(i => i.ApplicationUser);
            return Ok(patients);
        }
        [HttpGet]
        [Route("Symptoms")]
        public IEnumerable<Symptom> GetSymptoms()
        {
            return _context.Symptoms;
        }
        [HttpGet("{idSymptom}")]
        [Route("Symptom/{idSymptom}")]
        public IActionResult GetSymptom(int idSymptom)
        {
            var symptom = _context.Symptoms.Find(idSymptom);
            return Ok(symptom);
        }
        [HttpGet]
        [Route("Medicaments")]
        public IEnumerable<Medicament> GetMedicaments()
        {
            return _context.Medicaments;
        }
        [HttpGet]
        [Route("GetHistoric")]
        public IActionResult GetHistoric()
        {
            var result = _context.Histories.Include(i => i.Patient).Include(i => i.Illness);
            return Ok(result);
        }
        [HttpGet]
        [Route("Appointments")]
        public IActionResult GetAppointments()
        {
            var appointments = _context.Appointments.Include(i => i.ApplicationUser).Include(i => i.Patient);
            var result = appointments.Select(s => new { s.Id, MedicName = s.ApplicationUser.UserName, MedicId = s.ApplicationUser.Id, PatientName = s.Patient.Name, PatientId = s.Patient.Id, Date = s.Date, Hour = s.Hour, Minute = s.Minute, Type = s.Type });
            return Ok(result);
        }
        [HttpGet("{idMedic}")]
        [Route("MedicAppointments/{idMedic}")]
        public IActionResult MedicAppointments(string idMedic)
        {
            var appointments = _context.Appointments.Include(i => i.ApplicationUser).Include(i => i.Patient).Where(w => w.ApplicationUser.Id == idMedic);
            var result = appointments.Select(s => new { s.Id, MedicName = s.ApplicationUser.UserName, MedicId = s.ApplicationUser.Id, PatientName = s.Patient.Name, PatientId = s.Patient.Id, Date = s.Date, Hour = s.Hour, Minute = s.Minute, Type = s.Type });
            return Ok(result);
        }
        [HttpGet("{id}")]
        [Route("GetPatient/{id}")]
        public IActionResult GetPatient(int id)
        {
            //var PatientHistoryIllness = from x in (_context.Histories.Include(i => i.Patient).Include(i => i.Illness).Include(i => i.Illness.Treatments).Include(i => i.Illness.IllnessSymptomes).Where(w => w.Patient.Id == id))
            //                            join illnessSymptome in _context.IllnessSymptomes on x.Illness.Id equals illnessSymptome.Illness.Id
            //                            join symptome in _context.Symptoms on illnessSymptome.Symptom.Id equals symptome.Id
            //                            select x.Illness;

            var patientDetails = (from patient in _context.Patients
                                  join history in _context.Histories on patient.Id equals history.Patient.Id
                                  join illness in _context.Illnesses on history.Illness.Id equals illness.Id
                                  join treatment in _context.Treatments on illness.Id equals treatment.Illness.Id
                                  join medicament in _context.Medicaments on treatment.Medicament.Id equals medicament.Id
                                  join illnessSymptome in _context.IllnessSymptomes on illness.Id equals illnessSymptome.Illness.Id
                                  join symptome in _context.Symptoms on illnessSymptome.Symptom.Id equals symptome.Id
                                  where patient.Id == id
                                  group new { historicId = history.Id,
                                              patient.Id, patient.Name, patient.Address, patient.Date, 
                                              history.StartDate, history.FinishDate, 
                                              treatmentDuration = treatment.Duration, treatmentFrequency = treatment.PillPerDay, treatment.DayTime, treatmentQuantity = treatment.Quantity, 
                                              medicamentName =  medicament.Name, medicamentPrice = medicament.Price, 
                                              illnessName = illness.Name, illnessId = illness.Id, illnessSeverity = illness.Severity, illnessSymptomes = illness.IllnessSymptomes.Select(s => s.Symptom.Description) } by history.Id into g
                                  select g.First());
                                 //select new { patient, history, illness,symptome,medicament,treatment });
                                 //select new
                                 //{
                                 //    Patient = patient.Name,
                                 //    Illness = illness.Name,
                                 //    IllnessSeverity = illness.Severity,
                                 //    Symptome = symptome.Description,
                                 //    Medicament = medicament.Name,
                                 //    MedicamentPrice = medicament.Price,
                                 //    TreatmentQuantity = treatment.Quantity,
                                 //    TreatmentDuration = treatment.Duration,
                                 //    TreatmentDayTime = treatment.DayTime,
                                 //    TreatmentFrequency = treatment.PillPerDay,
                                 //    HistoricStart = history.StartDate,
                                 //    HistoricFinish = history.FinishDate
                                 //});
            if (patientDetails == null)
            {
                return NotFound();
            }
            return Ok(patientDetails);
        }
        [HttpGet("{idMedic}")]
        [Route("GetCurrentPatients/{idMedic}")]
        public IActionResult GetCurrentPatients(string idMedic)
        {
            var patients = _context.Patients.Where(w => w.ApplicationUser.Id == idMedic);
            return Ok(patients);
        }
        //get patient's medic
        [HttpGet("{idPatient}")]
        [Route("GetPatientCurrentMedic/{idPatient}")]
        public IActionResult GetPatientCurrentMedic(int idPatient)
        {
            var currentPatient = _context.Patients.Include(i => i.ApplicationUser).Where(w => w.Id == idPatient).FirstOrDefault();
            var result = new
            {
                idPatient,
                name = currentPatient.Name,
                medicId = currentPatient.ApplicationUser.Id,
                medicName = currentPatient.ApplicationUser.UserName
            };
            return Ok(result);
        }
        [HttpGet]
        [Route("GetIllnesses")]
        public IEnumerable<Illness> GetIllnesses()
        {
            return _context.Illnesses;
        }
        [HttpGet]
        [Route("GetLastIllness")]
        public IActionResult GetLastIllness()
        {
            var lastIll = _context.Illnesses.Last();
            return Ok(lastIll);
        }
        [HttpGet("{idIllness}")]
        [Route("GetIllness/{idIllness}")]
        public async Task<IActionResult> GetIllness(int idIllness)
        {
            var illness = await _context.Illnesses.FindAsync(idIllness);
            if(illness == null)
            {
                return BadRequest();
            }
            return Ok(illness);
        }
        [HttpPost]
        [Route("AddPatient")]
        public async Task<IActionResult> AddPatient(PatientModel model)
        {
            var patient = new Patient
            {
                Name = model.Name,
                Address = model.Address,
                Date = model.Date
            };
            var user = await _userManager.FindByIdAsync(model.MedicId);
            patient.ApplicationUser = user;

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            //await _context.AddAsync<Patient>(patient);
            //_context.SaveChanges();

            return Ok(patient);
        }
        [HttpPost("{idIllness}/{idSymptom}")]
        [Route("AddSymptom/{idIllness}/{idSymptom}")]
        public async Task<IActionResult> AddSymptome(int idIllness, int idSymptom)
        {
            var ill = _context.Illnesses.Find(idIllness);
            var symptom = _context.Symptoms.Find(idSymptom);

            var illSymptom = new IllnessSymptome { };
            illSymptom.Illness = ill;
            illSymptom.Symptom = symptom;

            _context.IllnessSymptomes.Add(illSymptom);
            await _context.SaveChangesAsync();

            return Ok(illSymptom);
        }
        [HttpPost]
        [Route("AddTreatment")]
        public async Task<IActionResult> AddTreatment(TreatmentModel model)
        {
            var treatment = new Treatment
            {
                Duration = model.Duration,
                Quantity = model.Quantity,
                PillPerDay = model.PillPerDay,
                DayTime = model.DayTime
            };
            var ill = _context.Illnesses.Find(model.IllnessId);
            var medicament = _context.Medicaments.Find(model.MedicamentId);

            treatment.Illness = ill;
            treatment.Medicament = medicament;

            _context.Treatments.Add(treatment);
            await _context.SaveChangesAsync();
            //await _context.AddAsync<Patient>(patient);
            //_context.SaveChanges();

            return Ok(treatment);
        }
        [HttpGet("{PatientName}/{IllnessId}")]
        [Route("AddPatientToHistory/{PatientName}/{IllnessId}")]
        public async Task<IActionResult> AddPatientToHistory(string PatientName, int IllnessId)
        {
            if (IllnessId == 0)
            {
                return Ok();
            }
            var historic = new History
            {
                StartDate = DateTime.Today
            };
            var illness = _context.Illnesses.Where(w => w.Id == IllnessId).FirstOrDefault();
            var patientIll = _context.Patients.SingleOrDefault(s => s.Name == PatientName);
            historic.Illness = illness;
            historic.Patient = patientIll;

            _context.Histories.Add(historic);
            await _context.SaveChangesAsync();
            return Ok(historic);
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
        [Route("AddAppointment")]
        public async Task<IActionResult> AddAppointment(AppointmentModel model)
        {
            var appointment = new Appointment
            {
                Type = model.Type,
                Date = model.Date,
                Hour = model.Hour,
                Minute = model.Minute
            };

            var medic = await _userManager.FindByIdAsync(model.MedicId);
            var patient = _context.Patients.Find(model.PatientId);

            appointment.ApplicationUser = medic;
            appointment.Patient = patient;

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return Ok(appointment);
        }

        [HttpPost]
        [Route("AddIllness")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddIllness(IllnessModel model)
        {
            var illness = new Illness
            {
                Name = model.Name,
                Severity = model.Severity
            };

            _context.Illnesses.Add(illness);
            await _context.SaveChangesAsync();

            return Ok(illness);
        }

        [HttpGet("{idHistoric}/{idPatient}/{idIllness}")]
        [Route("HealPatient/{idHistoric}/{idPatient}/{idIllness}")]
        public async Task<IActionResult> HealPatient(int idHistoric, int idPatient, int idIllness)
        {
            var patient = await _context.Patients.FindAsync(idPatient);
            var patientHistoric = _context.Histories.Where(w => w.Id == idHistoric).FirstOrDefault();
            var illness = _context.Illnesses.Where(w => w.Id == idIllness).FirstOrDefault();

            patientHistoric.FinishDate = DateTime.Today;
            patientHistoric.Patient = patient;
            patientHistoric.Illness = illness;

            _context.Entry(patientHistoric).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(patientHistoric);
        }
        [HttpGet("{idPatient}/{idMedic}")]
        [Route("TransferPatient/{idPatient}/{idMedic}")]
        public async Task<IActionResult> TransferPatient(int idPatient, string idMedic)
        {
            var patient = await _context.Patients.FindAsync(idPatient);
            var newMedic = await _userManager.FindByIdAsync(idMedic);

            patient.ApplicationUser = newMedic;
            _context.Entry(patient).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Patient",patient.Name, message2 = "transfered to", newMedic.UserName });
        }
        [HttpDelete("{idAppointment}")]
        [Route("DeleteAppointment/{idAppointment}")]
        public async Task<IActionResult> DeleteAppointment(int idAppointment)
        {
            var appointment = _context.Appointments.Where(w => w.Id == idAppointment).FirstOrDefault();
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return Ok(appointment);
        }
    }
}