using Microsoft.Data.SqlClient;
using System;

namespace _09.Increase_Age_Stored_Procedure
{
    class StartUp
    {
        private const string ServerConnections = @"Server=.\SQLEXPRESS;Database=MinionsDB;Integrated Security=true";
        static void Main(string[] args)
        {
            using SqlConnection sqlConnection = new SqlConnection(ServerConnections);
            sqlConnection.Open();

            int id = int.Parse(Console.ReadLine());

            string queryProc = @"EXEC usp_GetOlder @id";

            using SqlCommand sqlCommand = new SqlCommand(queryProc, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@id", id);

            sqlCommand.ExecuteNonQuery();

            string selectQuery = @"SELECT Name, Age FROM Minions WHERE Id = @Id";

            using SqlCommand readMinions = new SqlCommand(selectQuery, sqlConnection);
            readMinions.Parameters.AddWithValue("@Id", id);
            using SqlDataReader reader = readMinions.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine($"{reader[0]} - {reader[1]} years old");
            }
        }
    }
}
