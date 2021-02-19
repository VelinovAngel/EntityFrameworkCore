using Microsoft.Data.SqlClient;
using System;
using System.Linq;

namespace _08.Increase_Minion_Age
{
    class StartUp
    {
        private const string ServerConnectionString = @"Server=.\SQLEXPRESS;Database=MinionsDB;Integrated Security=true;";
        static void Main(string[] args)
        {
            int[] idMinions = Console.ReadLine()
                .Split(' ',StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();
              
            using SqlConnection sqlConnection = new SqlConnection(ServerConnectionString);
            sqlConnection.Open();

            string updateQuery = @"UPDATE Minions
                                        SET Name = UPPER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), Age += 1
                                        WHERE Id = @Id";

            using SqlCommand updateMinions = new SqlCommand(updateQuery, sqlConnection);

            foreach (var id in idMinions)
            {
                updateMinions.Parameters.AddWithValue("@Id", id);
                updateMinions.ExecuteNonQuery();
            }

            string selectMinions = @"SELECT Name, Age FROM Minions";
            using SqlCommand selectQuert = new SqlCommand(selectMinions, sqlConnection);

            using SqlDataReader reader = selectQuert.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine($"{reader[0]} {reader[1]}");
            }
        }
    }
}
