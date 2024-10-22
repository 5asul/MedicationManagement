using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicationManagement.Models
{
    public class Notification
    {
        [Key]
        public int NotificationID { get; set; }
        [ForeignKey(nameof(User.UserID))]
        public int PatientID { get; set; }
        public string Message { get; set; }
        public DateTime SentDate { get; set; }
        public string Type { get; set; } // "Email" or "SMS"
    }
}
