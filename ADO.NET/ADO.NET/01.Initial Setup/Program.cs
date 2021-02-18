﻿using System;
using Microsoft.Data.SqlClient;


namespace _01.Initial_Setup
{
    class Program
    {
        private const string Master_DB = "Server=.\\SQLEXPRESS;Integrated Security=true; DataBase=master";
        private const string MinionsDB = "Server=.\\SQLEXPRESS;Integrated Security=true; DataBase=MinionsDB";
        private const string Create_Tables = "CREATE DATABASE MinionsDB";


        static void Main(string[] args)
        {

            using (SqlConnection connection = new SqlConnection(Master_DB))
            {
                connection.Open();
                SqlCommand createDbMinions = new SqlCommand(Create_Tables, connection);
                createDbMinions.ExecuteNonQuery();
            }

            using (SqlConnection connectToMinionDb = new SqlConnection(MinionsDB))
            {
                connectToMinionDb.Open();

                foreach (var query in GetCreateMinionTable())
                {
                    ExecuteQuery(query, connectToMinionDb);
                }

                foreach (var query in GetCreateInfoMinions())
                {
                    ExecuteQuery(query, connectToMinionDb);
                }
            } 
        }

        private static string[] GetCreateMinionTable()
        {
            string[] insertMinion = new string[]
                {
                "CREATE TABLE Countries (Id INT PRIMARY KEY IDENTITY,Name VARCHAR(50))",

                "CREATE TABLE Towns (Id INT PRIMARY KEY IDENTITY,Name VARCHAR(50), CountryCode INT REFERENCES Countries(Id))",

                "CREATE TABLE Minions (Id INT PRIMARY KEY IDENTITY,Name VARCHAR(30), Age INT, TownId INT REFERENCES Towns(Id))",

                "CREATE TABLE EvilnessFactors (Id INT PRIMARY KEY IDENTITY, Name VARCHAR(50))",

                "CREATE TABLE Villains (Id INT PRIMARY KEY IDENTITY, Name VARCHAR(50), EvilnessFactorId INT REFERENCES EvilnessFactors(Id))",

                "CREATE TABLE MinionsVillains (MinionId INT REFERENCES Minions(Id),VillainId INT REFERENCES Villains(Id),PRIMARY KEY(MinionId, VillainId))"
          };

            return insertMinion;
        }


        private static string[] GetCreateInfoMinions()
        {
            string[] infoMinions = new string[]
            {
                "INSERT INTO Countries ([Name]) VALUES ('Bulgaria'), ('England'), ('Cyprus'), ('Germany'), ('Norway')",
                
                "INSERT INTO Towns ([Name], CountryCode) VALUES ('Plovdiv', 1), ('Varna', 1), ('Burgas', 1), ('Sofia', 1), ('London', 2), ('Southampton', 2), ('Bath', 2) ('Liverpool', 2), ('Berlin', 3), ('Frankfurt', 3), ('Oslo', 4)",
                
                "INSERT INTO Minions (Name,Age, TownId) VALUES ('Bob', 42, 3), ('Kevin', 1, 1), ('Bob ', 32, 6), ('Simon', 45, 3), ('Cathleen', 11, 2), ('Carry ', 50, 10), ('Becky', 125, 5), ('Mars', 21, 1), ('Misho', 5, 10), ('Zoe', 125, 5), ('Json', 21, 1)",

                "INSERT INTO EvilnessFactors (Name) VALUES  ('Super good'), ('Good'), ('Bad'), ('Evil'), ('Super evil')",

                "INSERT INTO Villains (Name, EvilnessFactorId) VALUES ('Gru',2), ('Victor',1), ('Jilly',3), ('Miro',4), ('Rosen',5),('Dimityr',1), ('Dobromir',2)",

                "INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (4,2),(1,1),(5,7),(3,5),(2,6),(11,5),(8,4),(9,7),(7,1),(1,3),(7,3),(5,3),(4,3),(1,2),(2,1),(2,7)"
            };

            return infoMinions;
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

