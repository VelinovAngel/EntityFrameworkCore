using System;
using System.Collections.Generic;

using Microsoft.Data.SqlClient;

namespace _07.Print_All_Minion_Names
{
    class StartUp
    {
        private const string ServerConnections = @"Server=.\SQLEXPRESS;Database=MinionsDB;Integrated Security=true";
        static void Main(string[] args)
        {
            string minionsQuery = @"SELECT Name FROM Minions";

            using SqlConnection sqlConnection = new SqlConnection(ServerConnections);
            sqlConnection.Open();

            using SqlCommand selectCommand = new SqlCommand(minionsQuery, sqlConnection);
            using SqlDataReader reader = selectCommand.ExecuteReader();

            List<string> minions = new List<string>();

            while (reader.Read())
            {
                minions.Add((string)reader[0]);
            }
            //first + 1, last - 1, first + 2, last - 2 … first + n, last - n. 
            int count = 1;
            for (int i = 0; i < minions.Count / 2; i++)
            {
                Console.WriteLine(minions[0 + count]);
                Console.WriteLine(minions[minions.Count - 1 - count]);
                count++;
            }

            if (minions.Count % 2 != 0)
            {
                Console.WriteLine(minions[minions.Count / 2]);
            }
        }
    }
}
