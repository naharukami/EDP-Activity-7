using MySql.Data.MySqlClient;

namespace TutorooSystem
{
    public static class DatabaseConfig
    {
        private static string connString = "server=127.0.0.1;port=3306;user=root;password=;database=act2;";

        public static MySqlConnection GetConnection()
        {
            MySqlConnection conn = new MySqlConnection(connString);
            conn.Open(); // ← this was missing; connection must be opened before use
            return conn;
        }
    }
}