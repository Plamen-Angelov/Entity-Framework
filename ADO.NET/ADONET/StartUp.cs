using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADONET
{
    public class StartUp
    {
        static async Task  Main(string[] args)
        {
            SqlConnection dbConnection = new SqlConnection(Configuration.ConectionString);
            await dbConnection.OpenAsync();

            //Problem 1 - Initial Setup 

            await using (dbConnection)
            {
                SqlCommand createDB = new SqlCommand(Queries.CreateDb, dbConnection);
                await createDB.ExecuteNonQueryAsync();

                SqlCommand useMinioinsDB = new SqlCommand(Queries.UseMinionsDB, dbConnection);
                await useMinioinsDB.ExecuteNonQueryAsync();

                SqlCommand createTableCountries = new SqlCommand(Queries.CreateTableCountries, dbConnection);
                await createTableCountries.ExecuteNonQueryAsync();

                SqlCommand insertIntoCountries = new SqlCommand(Queries.InsertIntoCountries, dbConnection);
                await insertIntoCountries.ExecuteNonQueryAsync();
            }


            await using (dbConnection)
            {
                //Problem 2 - Villain Names

                await PrintVillainsWithMoreThan3Minions(dbConnection);

                //Problem 3 - Minion Names

                Console.WriteLine("Villain Id:");
                int villainId = int.Parse(Console.ReadLine());

                await PrintMinionsByVillain(dbConnection, villainId);

                //Problem 4 - Add Minion

                Console.WriteLine("Mnioninfo:");
                string[] minionInfo = Console.ReadLine()
                    .Split(" ", StringSplitOptions.RemoveEmptyEntries);

                Console.WriteLine("Villaininfo:");
                string[] villainInfo = Console.ReadLine()
                    .Split(" ", StringSplitOptions.RemoveEmptyEntries);

                string minionName = minionInfo[1];
                int minionAge = int.Parse(minionInfo[2]);
                string minionTown = minionInfo[3];

                string villainName = villainInfo[1];

                await AddMinionAndVillain(dbConnection, minionName, minionAge, minionTown, villainName);


                //Problem 5 - Change Town Names Casing

                Console.WriteLine("Countryname:");
                string countryName = Console.ReadLine();

                await PrintTownsInGivenCountry(dbConnection, countryName);

               // Problem 7 - Print All Minion Names

                List<string> minionNames = new List<string>();

                await GetMinionNames(dbConnection, minionNames);

                List<string> orderedNames = new List<string>();

                for (int i = 0; i < minionNames.Count / 2; i++)
                {
                    orderedNames.Add(minionNames[i]);
                    orderedNames.Add(minionNames[minionNames.Count - 1 - i]);
                }

                if (minionNames.Count % 2 == 1)
                {
                    orderedNames.Add(minionNames[minionNames.Count / 2]);
                }

                foreach (var name in orderedNames)
                {
                    Console.WriteLine(name);
                }

                //Problem 8 - Increase Minion Age

                int[] minionIds = Console.ReadLine()
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToArray();

                foreach (var id in minionIds)
                {
                    await UpdateMInionData(dbConnection, id);
                }

                await PrintMinionsWithAge(dbConnection);

                //Problem 9 - Increase Age Stored

                Console.WriteLine("MinionId:");
                int minionId = int.Parse(Console.ReadLine());

                await PrintMinionWithCorrectedAge(dbConnection, minionId);
            }
        }

        private static async Task AddMinionAndVillain(SqlConnection dbConnection, string minionName, int minionAge, 
            string minionTown, string villainName)
        {
            SqlCommand getTownId = new SqlCommand(Queries.GetTownId, dbConnection);
            getTownId.Parameters.AddWithValue("@townName", minionTown);

            object townIdobject = await getTownId.ExecuteScalarAsync();

            if (townIdobject == null)
            {
                SqlCommand insertTown = new SqlCommand(Queries.InsertTown, dbConnection);
                insertTown.Parameters.AddWithValue("@townName", minionTown);
                int rowsAffectedT = await insertTown.ExecuteNonQueryAsync();

                if (rowsAffectedT == 0)
                {
                    Console.WriteLine("Problem ossured");
                    return;
                }
                Console.WriteLine($"Town {minionTown} was added to the database.");
            }

            int townId = (int)await getTownId.ExecuteScalarAsync();

            SqlCommand getVillainId = new SqlCommand(Queries.GetVillainId, dbConnection);
            getVillainId.Parameters.AddWithValue("@Name", villainName);
            object villainIdObject = await getVillainId.ExecuteScalarAsync();

            if (villainIdObject == null)
            {
                SqlCommand insertVillain = new SqlCommand(Queries.InsertVillain, dbConnection);
                insertVillain.Parameters.AddWithValue("@villainName", villainName);
                int rowsAffectedV = await insertVillain.ExecuteNonQueryAsync();
                if (rowsAffectedV == 0)
                {
                    Console.WriteLine("Problem occured");
                    return;
                }

                Console.WriteLine($"Villain {villainName} was added to the database.");
            }
            int villainId = (int)await getVillainId.ExecuteScalarAsync();

            SqlCommand insertMinion = new SqlCommand(Queries.InsertMinion, dbConnection);
            insertMinion.Parameters.AddWithValue("@name", minionName);
            insertMinion.Parameters.AddWithValue("@age", minionAge);
            insertMinion.Parameters.AddWithValue("@townId", townId);

            int rowsAffectedM = await insertMinion.ExecuteNonQueryAsync();
            if (rowsAffectedM == 0)
            {
                Console.WriteLine("Problem occured");
                return;
            }

            SqlCommand getMinionId = new SqlCommand(Queries.GetMinionId, dbConnection);
            getMinionId.Parameters.AddWithValue("@Name", minionName);
            int minionId = (int)await getMinionId.ExecuteScalarAsync();

            SqlCommand MakeMinionServantVillain = new SqlCommand(Queries.MakeMinionServantVillain, dbConnection);
            MakeMinionServantVillain.Parameters.AddWithValue("@villainId", villainId);
            MakeMinionServantVillain.Parameters.AddWithValue("@minionId", minionId);

            int affectedRows = await MakeMinionServantVillain.ExecuteNonQueryAsync();

            if (affectedRows == 0)
            {
                Console.WriteLine("Problem occured");
                return;
            }
            Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");
        }

        private static async Task PrintMinionWithCorrectedAge(SqlConnection dbConnection, int minionId)
        {
            SqlCommand procCommand = new SqlCommand(Queries.IncreaseAgeAndReturnMinionInfo, dbConnection);
            await procCommand.ExecuteNonQueryAsync();

            SqlCommand executeProcedure = new SqlCommand(Queries.ExecuteProcedure, dbConnection);
            executeProcedure.Parameters.AddWithValue("@Id", minionId);

            SqlDataReader minionInfoReader = await executeProcedure.ExecuteReaderAsync();

            using (minionInfoReader)
            {
                while (await minionInfoReader.ReadAsync())
                {
                    Console.WriteLine($"{minionInfoReader["Name"]} – {minionInfoReader["Age"]} years old");
                }
            }
        }

        private static async Task PrintMinionsWithAge(SqlConnection dbConnection)
        {
            SqlCommand minions = new SqlCommand(Queries.MinionsWithAge, dbConnection);
            SqlDataReader reader = await minions.ExecuteReaderAsync();

            Dictionary<string, int> minionsData = new Dictionary<string, int>();

            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    string name = (string)reader["Name"];
                    int age = (int)reader["Age"];

                    minionsData.Add(name, age);
                }
            }

            foreach (var minion in minionsData)
            {
                Console.WriteLine($"{minion.Key} {minion.Value}");
            }
        }

        private static async Task UpdateMInionData(SqlConnection dbConnection, int id)
        {
            SqlCommand updateMinionsById = new SqlCommand(Queries.UpdateMinionWithId, dbConnection);
            updateMinionsById.Parameters.AddWithValue("@Id", id);

            await updateMinionsById.ExecuteNonQueryAsync();
        }

        private static async Task GetMinionNames(SqlConnection dbConnection, List<string> minionNames)
        {
            SqlCommand getMinionNames = new SqlCommand(Queries.MinionNames, dbConnection);

            SqlDataReader namesReader = await getMinionNames.ExecuteReaderAsync();

            using (namesReader)
            {
                while (await namesReader.ReadAsync())
                {
                    minionNames.Add((string)namesReader["Name"]);
                }
            }
        }

        private static async Task PrintTownsInGivenCountry(SqlConnection dbConnection, string countryName)
        {
            SqlCommand getTowns = new SqlCommand(Queries.GetTownsInGivenCountry, dbConnection);

            getTowns.Parameters.AddWithValue("@countryName", countryName);
            SqlDataReader townReader = await getTowns.ExecuteReaderAsync();

            List<string> towns = new List<string>();

            using (townReader)
            {
                while (await townReader.ReadAsync())
                {
                    string town = (string)townReader["Name"];
                    towns.Add(town);
                }
            }

            if (towns.Count == 0)
            {
                Console.WriteLine("No town names were affected.");
                return;
            }

            SqlCommand townsToUpperCase = new SqlCommand(Queries.MakeTownsUpperCase, dbConnection);
            townsToUpperCase.Parameters.AddWithValue("@countryName", countryName);

            int townsChanged = (int)await townsToUpperCase.ExecuteNonQueryAsync();

            Console.WriteLine($"{townsChanged} town names were affected.");

            towns = towns.
                Select(t => t.ToUpper()).
                ToList();

            Console.WriteLine($"[{string.Join(", ", towns)}]");   
        }

        private static async Task PrintMinionsByVillain(SqlConnection dbConnection, int villainId)
        {
            SqlCommand getVillainName = new SqlCommand(Queries.GetVillainNameWithGivenId, dbConnection);

            getVillainName.Parameters.AddWithValue("@Id", villainId);
            string villainName = (string)await getVillainName.ExecuteScalarAsync();

            if (villainName == null)
            {
                Console.WriteLine($"No villain with ID {villainId} exists in the database.");
                return;
            }

            SqlCommand minionsByVillain = new SqlCommand(Queries.MinionsByVillainId, dbConnection);
            minionsByVillain.Parameters.AddWithValue("@Id", villainId);

            SqlDataReader reader = await minionsByVillain.ExecuteReaderAsync();
            Console.WriteLine($"Villain: {villainName}");

            await using (reader)
            {
                while (await reader.ReadAsync())
                {
                    Console.WriteLine($"{reader["RowNum"]}. {reader["Name"]} {reader["Age"]}");
                }
            }
        }

        private static async Task PrintVillainsWithMoreThan3Minions(SqlConnection dbConnection)
        {
            SqlCommand VillainsWithMoreThan3Minions = new SqlCommand(Queries.VillainsWithMoreThan3Minions, dbConnection);
            SqlDataReader reader = await VillainsWithMoreThan3Minions.ExecuteReaderAsync();
            
            await using (reader)
            {
                while (await reader.ReadAsync())
                {
                    string name = reader.GetString(0);
                    long count = reader.GetInt32(1);

                    Console.WriteLine($"{name} - {count}");
                }
            }
        }


    }
}
