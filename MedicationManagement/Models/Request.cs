using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicationManagement.Models
{
    public class Request
    {
        [Key]
        public int RequestID { get; set; }
        [ForeignKey(nameof(User.UserID))]
        public int PatientID { get; set; }
        [ForeignKey(nameof(Prescription.PrescriptionID))]
        public int PrescriptionID { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; } // "Pending", "Approved", "Rejected"
    }
}
