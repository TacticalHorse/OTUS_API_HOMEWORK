using Microsoft.Data.Sqlite;
using WebApi.Models;

namespace WebApi
{
    public static class DBHelper
    {
        private static string Connstr = "Data Source=OtusApiDB.db";
        public static int CreateCustomer(Customer customer)
        {
            using (var connection = new SqliteConnection(Connstr))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand();
                command.Connection = connection;
                command.CommandText = $"INSERT INTO Clients (Id, Firstname, Lastname) values (@Id, @Firstname, @Lastname)";
                command.Parameters.Add(new SqliteParameter("@Id", customer.Id));
                command.Parameters.Add(new SqliteParameter("@Firstname", customer.Firstname));
                command.Parameters.Add(new SqliteParameter("@Lastname", customer.Lastname));
                return command.ExecuteNonQuery();
            }
        }

        public static Customer GetCustomer(long Id)
        {
            Customer output = null;
            using (var connection = new SqliteConnection(Connstr))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand();
                command.Connection = connection;
                command.CommandText = $"SELECT Id, Firstname, Lastname FROM Clients WHERE Id = @Id";
                command.Parameters.Add(new SqliteParameter("@Id", Id));
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows) 
                    {
                        while (reader.Read())   
                        {
                            output = new Customer
                            {
                                Id = reader.GetInt32(0),
                                Firstname = reader.GetString(1),
                                Lastname = reader.GetString(2)
                            };
                        }
                    }
                }
            }

            return output;
        }
    }
}
