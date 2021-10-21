using Microsoft.Data.SqlClient;
using System;
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

                //Problem 5 - Change Town Names Casing


            }
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
