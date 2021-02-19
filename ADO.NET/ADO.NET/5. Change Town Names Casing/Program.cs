using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace _5._Change_Town_Names_Casing
{
    class StartUp
    {
        private const string ConnectionString = @"Server=.\SQLEXPRESS;Database=MinionsDB;Integrated Security=true;";
        static void Main(string[] args)
        {
            string countryName = Console.ReadLine();

            string updateTownsNameQuery = @"UPDATE Towns
                                                    SET Name = UPPER(Name)
                                            WHERE CountryCode = (SELECT c.Id FROM Countries AS c WHERE c.Name = @countryName)";

            string selectTownsNameQuery = @" SELECT t.Name 
                                                     FROM Towns as t
                                                JOIN Countries AS c ON c.Id = t.CountryCode
                                                    WHERE c.Name = @countryName";

            using SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            sqlConnection.Open();

            using SqlCommand updateCommand = new SqlCommand(updateTownsNameQuery, sqlConnection);

            updateCommand.Parameters.AddWithValue("@countryName", countryName);

            int affectedRows = updateCommand.ExecuteNonQuery();

            if (affectedRows == 0)
            {
                Console.WriteLine("No town names were affected.");
            }
            else
            {
                Console.WriteLine($"{affectedRows} town names were affected.");

                using SqlCommand selectedCommand = new SqlCommand(selectTownsNameQuery, sqlConnection);

                selectedCommand.Parameters.AddWithValue("@countryName", countryName);

                using (var reader = selectedCommand.ExecuteReader())
                {
                    List<string> towns = new List<string>();

                    while (reader.Read())
                    {
                        towns.Add((string)reader[0]);
                    }

                    Console.WriteLine($"[{string.Join(", ",towns)}]");
                }
            }

        }
    }
}
