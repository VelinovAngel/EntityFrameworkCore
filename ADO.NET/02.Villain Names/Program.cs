using System;
using Microsoft.Data.SqlClient;

namespace _02.Villain_Names
{
    class StartUp
    {
        private const string ConnectionString = @"Server=.\SQLEXPRESS;Database=MinionsDB;Integrated Security=true;";

        static void Main(string[] args)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();
                string query =
                    @"SELECT v.Name, COUNT(mv.MinionId) AS MinionsCount  FROM Villains AS v JOIN MinionsVillains AS mv ON v.Id = mv.VillainId GROUP BY v.Name HAVING COUNT(mv.MinionId) > 3 ORDER BY COUNT(mv.MinionId) DESC";

                using (SqlCommand command = new SqlCommand(query, sqlConnection))
                {
                    using (SqlDataReader sqlDataReader = command.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                        {
                            Console.WriteLine($"{sqlDataReader["Name"]} - {sqlDataReader["MinionsCount"]}");
                        }
                    }
                }
            }
        }
    }
}
