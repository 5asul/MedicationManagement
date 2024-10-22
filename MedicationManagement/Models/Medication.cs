using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicationManagement.Models
{
    // Models/Medication.cs
    public class Medication
    {
        [Key]
        public int MedicationID { get; set; }
        public string Name { get; set; }
        public string Dosage { get; set; }
        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }

        [ForeignKey(nameof(Prescription.PrescriptionID))]
        public ICollection<Prescription>? Prescriptions { get; }
    }
}
