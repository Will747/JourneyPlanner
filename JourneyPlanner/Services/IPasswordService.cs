using System.Threading.Tasks;

namespace JourneyPlanner.Services
{
    // Interface for a password service used to encrypt and verify passwords.
    public interface IPasswordService
    {
        public byte[] EncryptPassword(string password);
        
        // Verifies the new password matches the hashed one
        public bool VerifyPassword(string newPassword, byte[] hashedPassword);
        
        // Gets the hashed password for a specific user
        public Task<byte[]> GetUserPassword(string userId);
    }
}