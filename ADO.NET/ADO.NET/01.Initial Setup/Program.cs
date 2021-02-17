using System;
using Microsoft.Data.SqlClient;


namespace _01.Initial_Setup
{
    class Program
    {
        private const string masterDb = "Server=.\\SQLEXPRESS;Integrated Security=true; DataBase=master";
        private const string minionsDb = "Server=.\\SQLEXPRESS;Integrated Security=true; DataBase=MinionsDB";
        static void Main(string[] args)
        {

            //using (SqlConnection connection = new SqlConnection(masterDb))
            //{
            //    connection.Open();
            //    SqlCommand createDbMinions = new SqlCommand("CREATE DATABASE MinionsDB", connection);
            //    createDbMinions.ExecuteNonQuery();
            //}

            using (SqlConnection connectToMinionDb = new SqlConnection(minionsDb))
            {
                connectToMinionDb.Open();

                foreach (var query in GetCreateMinionTable())
                {
                    ExecuteQuery(query, connectToMinionDb);
                }

                foreach (var query in collection)
                {

                }
            }
          
        }

        private static string[] GetCreateMinionTable()
        {
            string[] insertMinion = new string[]
                {
                "CREATE TABLE Countries " +
                "(Id INT PRIMARY KEY IDENTITY,Name VARCHAR(50))",

                "CREATE TABLE Towns" +
                "(Id INT PRIMARY KEY IDENTITY,Name VARCHAR(50), CountryCode INT REFERENCES Countries(Id))",

                "CREATE TABLE Minions" +
                "(Id INT PRIMARY KEY IDENTITY,Name VARCHAR(30), Age INT, TownId INT REFERENCES Towns(Id))",

                "CREATE TABLE EvilnessFactors" +
                "(Id INT PRIMARY KEY IDENTITY, Name VARCHAR(50))",

                "CREATE TABLE Villains " +
                "(Id INT PRIMARY KEY IDENTITY, Name VARCHAR(50), EvilnessFactorId INT REFERENCES EvilnessFactors(Id))",

                "CREATE TABLE MinionsVillains " +
                "(MinionId INT REFERENCES Minions(Id),VillainId INT REFERENCES Villains(Id),PRIMARY KEY(MinionId, VillainId))"
          };

            return insertMinion;
        }


        private static string[] GetCreateInfoMinions()
        {
            string[] infoMinions = new string[]
            {
                "INSERT INTO Countries ([Name]) VALUES" + "('Bulgaria')," + "('England')," + "('Cyprus')," + "('Germany')," + "('Norway')",
                "INSERT INTO Towns ([Name], CountryCode) VALUES" + "('Plovdiv', 1)," + "('Varna', 1)," + "('Burgas', 1)," + "('Sofia', 1)," + "('London', 2)," + "('Southampton', 2)," + "('Bath', 2)," + "('Liverpool', 2)," + "('Berlin', 3)," + "('Frankfurt', 3)," + "('Oslo', 4)",

"
            }
        }

        private static void ExecuteQuery(string query , SqlConnection connection)
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                Console.WriteLine(command.ExecuteNonQuery());
            }
        }

    }
}

