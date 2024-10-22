using Xunit;
using Moq;
using MedicationManagement.Controllers;
using MedicationManagement.Data;
using MedicationManagement.Models; // Make sure to import your models
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MedicationManagement.Tests
{
    public class MedicationsControllerTests
    {
        [Fact]
        public void GetMedications_ReturnsOkResult_WithMedications()
        {
            // Arrange
            var mockContext = new Mock<ApplicationDbContext>();

            // Prepare fake data
            var fakePrescriptions = new List<Prescription>
            {
                new Prescription { PatientID = 1, MedicationID = 101, Dosage = "2 pills/day" }
            }.AsQueryable();

            // Mock DbSet for Prescriptions
            var mockSet = new Mock<DbSet<Prescription>>();
            mockSet.As<IQueryable<Prescription>>().Setup(m => m.Provider).Returns(fakePrescriptions.Provider);
            mockSet.As<IQueryable<Prescription>>().Setup(m => m.Expression).Returns(fakePrescriptions.Expression);
            mockSet.As<IQueryable<Prescription>>().Setup(m => m.ElementType).Returns(fakePrescriptions.ElementType);
            mockSet.As<IQueryable<Prescription>>().Setup(m => m.GetEnumerator()).Returns(fakePrescriptions.GetEnumerator());

            // Mock the DbContext and set the Prescriptions property
            mockContext.Setup(c => c.Prescriptions).Returns(mockSet.Object);

            // Create controller instance
            var controller = new MedicationsForPatientController(mockContext.Object);

            // Act
            var result = controller.GetMedications(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var medications = Assert.IsAssignableFrom<List<Prescription>>(okResult.Value);
            Assert.Single(medications);
        }

        [Fact]
        public void RequestRenewal_AddsRequestToDatabase()
        {
            // Arrange
            var mockContext = new Mock<ApplicationDbContext>();
            var mockSet = new Mock<DbSet<Request>>();

            mockContext.Setup(c => c.Requests).Returns(mockSet.Object);

            var controller = new MedicationsForPatientController(mockContext.Object);

            // Act
            var result = controller.RequestRenewal(1, 101);

            // Assert
            mockSet.Verify(m => m.Add(It.IsAny<Request>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
