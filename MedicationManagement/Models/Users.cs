using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicationManagement.Models
{
    public class User
    {
        [Key]
        public int? UserID { get; set; }
        public string Role { get; set; } // Doctor or Patient
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }

        [ForeignKey(nameof(Prescription.PrescriptionID))]
        public ICollection<Prescription>? Prescriptions { get; }
    }
}
