
using System.Data.SqlClient;


namespace LibraryProject.Main
{
    internal class Database
    {
        public static SqlConnection getConnection()
        {
            string connectionString = "Server=ROMVIREAK\\LIBRARY;Database=LibraryA4;Integrated Security=True;";

           return new SqlConnection(connectionString);

            
        }
    }

    
}
