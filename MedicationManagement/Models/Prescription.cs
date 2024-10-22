using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicationManagement.Models
{
    public class Prescription
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PrescriptionID { get; set; }
        [ForeignKey(nameof(User.UserID))]
        public int DoctorID { get; set; }

        [ForeignKey(nameof(User.UserID))]
        public int PatientID { get; set; }
        [ForeignKey(nameof(Medication.MedicationID))]
        public int MedicationID { get; set; }
        public Medication Medication { get; set; } = default!;
        public string Dosage { get; set; } = default!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Frequency { get; set; } = default!;

       
    }
}
