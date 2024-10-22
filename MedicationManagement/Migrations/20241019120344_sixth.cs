using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicationManagement.Migrations
{
    /// <inheritdoc />
    public partial class sixth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Medications_MedicationID",
                table: "Prescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Users_DoctorID",
                table: "Prescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Users_PatientID",
                table: "Prescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Users_UserID",
                table: "Prescriptions");

            migrationBuilder.DropIndex(
                name: "IX_Prescriptions_DoctorID",
                table: "Prescriptions");

            migrationBuilder.DropIndex(
                name: "IX_Prescriptions_MedicationID",
                table: "Prescriptions");

            migrationBuilder.DropIndex(
                name: "IX_Prescriptions_PatientID",
                table: "Prescriptions");

            migrationBuilder.DropIndex(
                name: "IX_Prescriptions_UserID",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Prescriptions");

            migrationBuilder.AlterColumn<int>(
                name: "PrescriptionID",
                table: "Prescriptions",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_Medications_PrescriptionID",
                table: "Prescriptions",
                column: "PrescriptionID",
                principalTable: "Medications",
                principalColumn: "MedicationID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_Users_PrescriptionID",
                table: "Prescriptions",
                column: "PrescriptionID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Medications_PrescriptionID",
                table: "Prescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Users_PrescriptionID",
                table: "Prescriptions");

            migrationBuilder.AlterColumn<int>(
                name: "PrescriptionID",
                table: "Prescriptions",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "Prescriptions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_DoctorID",
                table: "Prescriptions",
                column: "DoctorID");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_MedicationID",
                table: "Prescriptions",
                column: "MedicationID");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_PatientID",
                table: "Prescriptions",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_UserID",
                table: "Prescriptions",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_Medications_MedicationID",
                table: "Prescriptions",
                column: "MedicationID",
                principalTable: "Medications",
                principalColumn: "MedicationID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_Users_DoctorID",
                table: "Prescriptions",
                column: "DoctorID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_Users_PatientID",
                table: "Prescriptions",
                column: "PatientID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_Users_UserID",
                table: "Prescriptions",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID");
        }
    }
}
