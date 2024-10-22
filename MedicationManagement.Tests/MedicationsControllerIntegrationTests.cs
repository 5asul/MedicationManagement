using Xunit;
using Microsoft.EntityFrameworkCore;
using MedicationManagement.Data;
using MedicationManagement.Controllers;
using MedicationManagement.Models; // Ensure this is pointing to your models
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace MedicationManagement.Tests
{
    public class MedicationsControllerIntegrationTests
    {
        private ApplicationDbContext GetInMemoryDbContext()
        {
            // Set up in-memory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;


            var context = new ApplicationDbContext(options);

            // Seed data
            context.Prescriptions.Add(new Prescription
            {
                PatientID = 1,
                MedicationID = 101,
                Dosage = "2 pills/day"
            });

            context.SaveChanges();

            return context;
        }

        [Fact]
        public async Task GetMedicationsReturnsOkResultWithMedications()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new MedicationsForPatientController(context);

            // Act
            var result = controller.GetMedications(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var medications = Assert.IsAssignableFrom<List<Prescription>>(okResult.Value);
            Assert.Single(medications);
        }

        [Fact]
        public async Task RequestRenewal_SavesToInMemoryDatabase()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new MedicationsForPatientController(context);

            // Act
            var result = controller.RequestRenewal(1, 101);

            // Assert
            var requests = await context.Requests.ToListAsync();
            Assert.Single(requests);
            Assert.Equal(1, requests[0].PatientID);
            Assert.Equal(101, requests[0].PrescriptionID);
        }
    }
}
