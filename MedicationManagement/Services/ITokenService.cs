namespace MedicationManagement.Services
{
    public interface ITokenService
    {
        string GenerateToken(string name, string email, string role);
    }
}