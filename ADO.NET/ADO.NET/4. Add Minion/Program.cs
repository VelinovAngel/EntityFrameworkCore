using Microsoft.Data.SqlClient;
using System;
using System.Linq;
using System.Text;

namespace _4._Add_Minion
{
    class Program
    {
        private const string MinionsDB = @"Server=.\SQLEXPRESS;Integrated Security=true; DataBase=MinionsDB";

        static void Main(string[] args)
        {
            using (SqlConnection sqlConnection = new SqlConnection(MinionsDB))
            {
                sqlConnection.Open();

                string[] minionInput = Console.ReadLine()
                    .Split(": ", StringSplitOptions.RemoveEmptyEntries)
                    .ToArray();

                string[] minionsInfo = minionInput[1]
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .ToArray();

                string[] villainsInput = Console.ReadLine()
                    .Split(": ", StringSplitOptions.RemoveEmptyEntries)
                    .ToArray();

                string[] villainsInfo = villainsInput[1]
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries).ToArray();

                string result = AddMinionToDataBase(sqlConnection, minionsInfo, villainsInfo);

                Console.WriteLine(result);
            }
        }

        private static string AddMinionToDataBase(SqlConnection sqlConnection, string[] minionsInfo, string[] villainsInfo)
        {
            StringBuilder output = new StringBuilder();


            string minionName = minionsInfo[0];
            string minionAge = minionsInfo[1];
            string minionTown = minionsInfo[2];

            string villainName = villainsInfo[0];
            string townId = EnsureTownExists(sqlConnection, minionTown, output);

            string villainId = EnsureVillainExists(sqlConnection, villainName, output);

            string insertMinionQueryText = @"INSERT INTO Minions (Name, Age, TownId) VALUES (@nam, @age, @townId)";

            using SqlCommand insertMinion = new SqlCommand(insertMinionQueryText, sqlConnection);

            insertMinion.Parameters.AddRange(new[]
            {
                new SqlParameter("@nam", minionName),
                new SqlParameter("@age", minionAge),
                new SqlParameter("@townId", townId)
            });

            insertMinion.ExecuteNonQuery();

            string getMinionQueryId = @"SELECT Id FROM Minions WHERE Name = @Name";

            using SqlCommand getMinionId = new SqlCommand(getMinionQueryId, sqlConnection);

            getMinionId.Parameters.AddWithValue("@Name", minionName);

            string minionId = getMinionId.ExecuteScalar().ToString();

            string insertIntoMappingQueryText = @"INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@villainId, @minionId)";

            using SqlCommand insertToMapping = new SqlCommand(insertIntoMappingQueryText, sqlConnection);

            insertToMapping.Parameters.AddRange(new[]
            {
                new SqlParameter("@villainId", villainId),
                new SqlParameter("@minionId", minionId)
            });

            insertToMapping.ExecuteNonQuery();

            output.AppendLine($"Successfully added {minionName} to be minion of {villainName}.");

            return output.ToString().TrimEnd();
        }

        private static string EnsureVillainExists(SqlConnection sqlConnection, string villainName, StringBuilder output)
        {
            string getVillainIdQueryTxt = @"SELECT Id FROM Villains WHERE Name = @Name";

            using SqlCommand getVillainIdCmq = new SqlCommand(getVillainIdQueryTxt, sqlConnection);

            getVillainIdCmq.Parameters.AddWithValue(@"Name", villainName);

            string villainId = getVillainIdCmq.ExecuteScalar()?.ToString();

            if (villainId == null)
            {
                string insertVillainQueryText = @"INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, 4)";

                using SqlCommand insertVillain = new SqlCommand(insertVillainQueryText, sqlConnection);
                insertVillain.Parameters.AddWithValue(@"villainName", villainName);

                insertVillain.ExecuteNonQuery();

                villainId = getVillainIdCmq.ExecuteScalar().ToString();

                output.AppendLine($"Villain {villainName} was added to the database.");
            }

            return villainId;
        }

        private static string EnsureTownExists(SqlConnection sqlConnection, string minionTown, StringBuilder output)
        {
            string getTownIdQueryText = @"SELECT Id FROM Towns WHERE Name = @townName";

            using SqlCommand getTownId = new SqlCommand(getTownIdQueryText, sqlConnection);

            getTownId.Parameters.AddWithValue("@townName", minionTown);

            string townId = getTownId.ExecuteScalar()?.ToString();

            if (townId == null)
            {
                string insertTownQueryTxt = @"INSERT INTO Towns (Name) VALUES (@townName)";

                using SqlCommand insertTownCmd = new SqlCommand(insertTownQueryTxt, sqlConnection);

                insertTownCmd.Parameters.AddWithValue("@townName", minionTown);
                insertTownCmd.ExecuteNonQuery();

                townId = getTownId.ExecuteScalar().ToString();

                output.AppendLine($"Town {minionTown} was added to the database.");
            }

            return townId;
        }
    }
}
