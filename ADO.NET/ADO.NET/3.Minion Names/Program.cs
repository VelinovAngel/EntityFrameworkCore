using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace _3.Minion_Names
{

    class StartUp
    {
        private const string MinionsDB = @"Server=.\SQLEXPRESS;Integrated Security=true; DataBase=MinionsDB";
        static void Main(string[] args)
        {
            int id = int.Parse(Console.ReadLine());

            string villianNameQuesry = @"SELECT Name FROM Villains WHERE Id = @Id";

            string minionsQuery = @"SELECT ROW_NUMBER() OVER(ORDER BY m.Name) as RowNum,
                                         m.Name, 
                                         m.Age
                                    FROM MinionsVillains AS mv
                                    JOIN Minions As m ON mv.MinionId = m.Id
                                   WHERE mv.VillainId = @Id
                                ORDER BY m.Name";


            using (SqlConnection connection = new SqlConnection(MinionsDB))
            {
                connection.Open();

                var name = GetNameById(villianNameQuesry, connection, id);

                var minions = GetMinions(minionsQuery, connection, id);

                if (name != null)
                {
                    Console.WriteLine($"Villian: {name}"); 
                    if (minions != null && minions != string.Empty )
                    {
                        Console.WriteLine(minions);
                    }
                    else
                    {
                        Console.WriteLine("(no minions)");
                    }
                }
                else
                {
                    Console.WriteLine($"No villain with ID {id} exists in the database.");
                }
            }
         }

        private static string GetMinions(string minionsQuery, SqlConnection connection, int id)
        {
            using (SqlCommand command = new SqlCommand(minionsQuery, connection))
            {
                StringBuilder sb = new StringBuilder();
                int row = 1;
                command.Parameters.AddWithValue("@Id", id);
                using (var reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        sb.AppendLine($"{row}. {reader[1]} {reader[2]}");
                        row++;  
                    }

                    return sb.ToString().TrimEnd();
                }
            }
        }
        private static object GetNameById(string villianNameQuesry, SqlConnection connection, int villainId)
        {
            using (SqlCommand command = new SqlCommand(villianNameQuesry, connection))
            {
                command.Parameters.AddWithValue("@Id", villainId);
                var result = command.ExecuteScalar()?.ToString();
                return result;
            }
        }
    }
}
