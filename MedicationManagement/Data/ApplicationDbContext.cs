using MedicationManagement.Models;
using Microsoft.EntityFrameworkCore;


namespace MedicationManagement.Data
{

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = default!;
        public DbSet<Medication> Medications { get; set; } = default!;
        public DbSet<Prescription> Prescriptions { get; set; } = default!;
        public DbSet<Request> Requests { get; set; } = default!;
        public DbSet<Notification> Notifications { get; set; } = default!;

        
    }
}
