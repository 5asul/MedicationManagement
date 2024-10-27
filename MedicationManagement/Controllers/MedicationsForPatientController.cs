 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicationManagement.Data;
using MedicationManagement.Models;
using Microsoft.AspNetCore.Authorization;

namespace MedicationManagement.Controllers
{
    [ApiController]
    [Route("api/patient")]
    [Authorize(Roles = "Patient")]
    public class MedicationsForPatientController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MedicationsForPatientController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Get a list of medications for the patient
        
        [HttpGet("For Patient")]
        
        public IActionResult GetMedications([FromQuery] int patientID)
        {
            var requestPrescriptionData = (from r in _context.Requests
                                           join p in _context.Prescriptions
                                           on r.PrescriptionID equals p.PrescriptionID
                                           where r.PatientID == patientID
                                           select new
                                           {
                                               r.RequestID,
                                               r.RequestDate,
                                               p.PrescriptionID,
                                               p.DoctorID,
                                               p.Medication.Name,   // Include the medication name
                                               p.Dosage,
                                               p.StartDate,
                                               p.EndDate,
                                               p.Frequency,
                                               r.Status,
                                           }).ToList();

         

            if (!requestPrescriptionData.Any())
                return NotFound("No medications found for this patient.");

            return Ok(requestPrescriptionData);
        }

        // 2. Request a renewal of a prescription
        [HttpPost("request-renewal")]
        public IActionResult RequestRenewal(int patientID, int prescriptionID)
        {
            var request = new Request
            {
                PatientID = patientID,
                PrescriptionID = prescriptionID,
                RequestDate = DateTime.UtcNow,
                Status = "Pending"
            };

            _context.Requests.Add(request);
            _context.SaveChanges();

            return Ok(new { Message = "Prescription renewal request submitted." });
        }

        // 3. Get a list of notification history for the patient
        [HttpGet("notifications")]
        public IActionResult GetNotifications(int patientID)
        {
            var notifications = _context.Notifications
                                        .Where(n => n.PatientID == patientID)
                                        .ToList();

            if (notifications == null || !notifications.Any())
                return NotFound("No notifications found.");

            return Ok(notifications);
        }
    }

}
