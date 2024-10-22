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
    [Route("api/doctor")]
    [Authorize(Roles = "Doctor")]
    public class PrescriptionsForDoctorController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;

        public PrescriptionsForDoctorController(ApplicationDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // 4. Add or update a prescription for a patient
        //[AllowAnonymous]
        [HttpPost("prescription")]
        public IActionResult AddOrUpdatePrescription([FromBody] Prescription prescriptionDto)
        {

            var prescription = new Prescription
            {
                DoctorID = prescriptionDto.DoctorID,
                PatientID = prescriptionDto.PatientID,
                MedicationID = prescriptionDto.MedicationID, // Only set the MedicationID
                Dosage = prescriptionDto.Dosage,
                StartDate = prescriptionDto.StartDate,
                EndDate = prescriptionDto.EndDate,
                Frequency = prescriptionDto.Frequency
            };
            var existingPrescription = _context.Prescriptions
                                               .FirstOrDefault(p => p.PrescriptionID == prescription.PrescriptionID);

            if (existingPrescription != null)
            {
                // Update existing prescription
                existingPrescription.Dosage = prescription.Dosage;
                existingPrescription.StartDate = prescription.StartDate;
                existingPrescription.EndDate = prescription.EndDate;
                existingPrescription.Frequency = prescription.Frequency;
                _context.SaveChanges();
                return Ok(new { Message = "Prescription updated successfully." });
            }

            // Make sure that the patient and medication exist
            var patient = _context.Users.FirstOrDefault(u => u.UserID == prescription.PatientID && u.Role == "Patient");
            var medication = _context.Medications.FirstOrDefault(m => m.MedicationID == prescription.MedicationID);

            if (patient == null || medication == null)
            {
                return BadRequest("Invalid Patient or Medication.");
            }

            // Add new prescription
            _context.Prescriptions.Add(prescription);
            _context.SaveChanges();
            return Ok(new { Message = "Prescription added successfully." });
        }

        // 5. Get all prescriptions for a patient
        //[Authorize(Roles = "Doctor")]
        [HttpGet("patient-prescriptions")]
        public IActionResult GetPatientPrescriptions(int patientID)
        {
            var prescriptions = _context.Prescriptions
                                        .Where(p => p.PatientID == patientID)
                                        .Include(p => p.MedicationID)
                                        .ToList();

            if (!prescriptions.Any())
                return NotFound("No prescriptions found for this patient.");

            return Ok(prescriptions);
        }

        // 6. Get all renewal requests from patients
       // [Authorize(Roles = "Doctor")]
        [HttpGet("requests")]
        public IActionResult GetRenewalRequests(int doctorID)
        {
            var requests = _context.Requests
                                   .Where(r => r.Status == "Pending")
                                   .ToList();

            return Ok(requests);
        }

        // 7. Approve or reject a prescription renewal request
        //[AllowAnonymous]
        [HttpPost("request-status")]
        public IActionResult UpdateRequestStatus(int requestID, string status)
        {
            var request = _context.Requests.Find(requestID);

            if (request == null)
                return NotFound("Request not found.");

            request.Status = status;
            _context.SaveChanges();

            return Ok(new { Message = "Request status updated." });
        }

        // 8. Send notifications to patients
       // [AllowAnonymous]
        [HttpPost("send-notification")]
        public async Task<IActionResult> SendNotificationAsync(int patientID, string message, string type)
        {
            var patient = _context.Users.FirstOrDefault(p => p.UserID == patientID && p.Role == "Patient");

            if (patient == null)
            {
                return NotFound("Patient not found.");
            }

            var notification = new Notification
            {
                PatientID = patientID,
                Message = message,
                SentDate = DateTime.UtcNow,
                Type = type // "Email" or "SMS"
            };

            _context.Notifications.Add(notification);
            _context.SaveChanges();

            if (type.ToLower() == "email")
            {
                // Use the EmailService to send an email notification
                await _emailService.SendEmailAsync(patient.Email, "Medication Reminder", message);
            }

            return Ok(new { Message = "Notification sent successfully." });
        }
    }

}
