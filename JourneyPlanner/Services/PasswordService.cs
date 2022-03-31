using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace JourneyPlanner.Services
{
    // Service for encrypting and verifying passwords
    public class PasswordService : IPasswordService
    {
        private readonly Random _random;
        private readonly string _salt;
        private readonly DatabaseService _database;
        
        public PasswordService(DatabaseService databaseService)
        {
            _random = new Random();
            _salt = "randomSalt";
            _database = databaseService;
        }

        public byte[] EncryptPassword(string password)
        {
            var pepper = Convert.ToChar(_random.Next(33, 72));
            return EncryptPasswordNoPepper(password + _salt + pepper);
        }

        // Encrypts a password without adding any additional characters to the password
        private static byte[] EncryptPasswordNoPepper(string password)
        {
            MD5 hash = new MD5CryptoServiceProvider();
            hash.ComputeHash(Encoding.ASCII.GetBytes(password));
            return hash.Hash;
        }

        public bool VerifyPassword(string newPassword, byte[] hashedPassword)
        {
            if (hashedPassword != null)
            {
                var saltedPassword = newPassword + _salt;
                
                // Checks the password with all possible peppers to see if it matches the one in the database
                for (var i = 33; i < 73; i++)
                {
                    var newHash = EncryptPasswordNoPepper(saltedPassword + Convert.ToChar(i));
                    Array.Resize(ref newHash, 128);
                    if (newHash.SequenceEqual(hashedPassword))
                    {
                        return true;
                    }
                }
            }
            
            return false;
        }

        public async Task<byte[]> GetUserPassword(string userId)
        {
            var password = new byte[128];
            
            await using var conn = _database.GetConnection();
            
            // Make database request for hashed password
            conn.Open();
            var command = new MySqlCommand(
                "SELECT Password FROM users WHERE ID = @id;", conn);
            command.Parameters.AddWithValue("@id", userId);
            var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                password = (byte[])reader["Password"];
            }

            return password;
        }
    }
}
    