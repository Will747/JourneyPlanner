using MySql.Data.MySqlClient;

namespace JourneyPlanner.Services
{
    //https://www.c-sharpcorner.com/article/how-to-connect-mysql-with-asp-net-core/
    // Used to connect and interact with the database.
    public class DatabaseService
    {
        private readonly string _connection;
        
        public DatabaseService(string connection)
        {
            _connection = connection;
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connection);
        }
    }
}