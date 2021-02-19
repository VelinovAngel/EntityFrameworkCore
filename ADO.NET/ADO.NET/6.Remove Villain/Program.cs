using Microsoft.Data.SqlClient;
using System;

namespace _6.Remove_Villain
{
    class StartUp
    {
        private const string ConnectionString = @"Server=.\SQLEXPRESS;Database=MinionsDB;Integrated Security=true;";
        static void Main(string[] args)
        {
            using SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            sqlConnection.Open();

            int villainId = int.Parse(Console.ReadLine());

            string selectVillainsQuery = @"SELECT Name FROM Villains WHERE Id = @villainId";

            using SqlCommand selectVillains = new SqlCommand(selectVillainsQuery, sqlConnection);
            selectVillains.Parameters.AddWithValue("@villainId", villainId);
            string villainName = (string)selectVillains.ExecuteScalar();


            if (villainName == null)
            {
                Console.WriteLine("No such villain was found.");
            }
            else
            {
                string deleteMinionsCmd = @"DELETE FROM MinionsVillains WHERE VillainId = @villainId";
                using SqlCommand deleteMinions = new SqlCommand(deleteMinionsCmd, sqlConnection);

                deleteMinions.Parameters.AddWithValue("@villainId", villainId);
                deleteMinions.ExecuteNonQuery();

                string deleteVillainCmd = @"DELETE FROM Villains WHERE Id = @villainId";
                using SqlCommand deleteVillain = new SqlCommand(deleteVillainCmd, sqlConnection);

                deleteVillain.Parameters.AddWithValue("@villainId", villainId);

                int affectedRow = deleteVillain.ExecuteNonQuery();

                Console.WriteLine($"{villainName} was deleted.");
                Console.WriteLine($"{affectedRow} minions were released.");
            }
        }
    }
}
