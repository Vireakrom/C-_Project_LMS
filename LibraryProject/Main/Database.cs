
using System.Configuration;
using System.Data.SqlClient;


namespace LibraryProject.Main
{
    internal class Database
    {
        public static SqlConnection getConnection()
        {
            // "LibraryDb" should match the name in your .config file
            string connectionString = ConfigurationManager.ConnectionStrings["LibraryDB"].ConnectionString;
            return new SqlConnection(connectionString);


        }
    }

    
}
