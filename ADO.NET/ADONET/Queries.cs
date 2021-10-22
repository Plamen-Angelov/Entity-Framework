using System;
using System.Collections.Generic;
using System.Text;

namespace ADONET
{
    public static class Queries
    {
        public const string CreateDb = "CREATE DATABASE MinionsDB";

        public const string UseMinionsDB = "USE MinionsDB";

        public const string CreateTableCountries = "CREATE TABLE Countries (Id INT PRIMARY KEY IDENTITY,Name VARCHAR(50))";

        public const string InsertIntoCountries = "INSERT INTO Countries ([Name]) VALUES ('Bulgaria'),('England')," +
            "('Cyprus'),('Germany'),('Norway')";

        public const string VillainsWithMoreThan3Minions = @"SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount  
                                                              FROM Villains AS v
                                                              JOIN MinionsVillains AS mv ON v.Id = mv.VillainId
                                                          GROUP BY v.Id, v.Name
                                                            HAVING COUNT(mv.VillainId) > 3 
                                                          ORDER BY COUNT(mv.VillainId)";


        public const string MinionsByVillainId = @"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) as RowNum,
                                                          m.Name, 
                                                          m.Age
                                                     FROM MinionsVillains AS mv
                                                     JOIN Minions As m ON mv.MinionId = m.Id
                                                    WHERE mv.VillainId = @Id
                                                 ORDER BY m.Name";


        public const string GetVillainNameWithGivenId = @"SELECT [Name] 
                                                            FROM [Villains] 
                                                           WHERE Id = @Id";


        public const string GetTownsInGivenCountry = @" SELECT t.Name 
                                                          FROM Towns as t
                                                          JOIN Countries AS c ON c.Id = t.CountryCode
                                                         WHERE c.Name = @countryName";

        public const string MakeTownsUpperCase = @"UPDATE Towns
                                                      SET Name = UPPER(Name)
                                                    WHERE CountryCode = (SELECT c.Id FROM Countries AS c WHERE c.Name = @countryName)";


        public const string MinionNames = @"SELECT Name FROM Minions";


        public const string UpdateMinionWithId = @"UPDATE Minions
                                                      SET Name = UPPER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), Age += 1
                                                    WHERE Id = @Id";


        public const string MinionsWithAge = @"SELECT Name, Age FROM Minions";

        public const string IncreaseAgeAndReturnMinionInfo = @"CREATE PROC usp_GetOlder @id INT
                                                               AS
                                                               UPDATE Minions
                                                                  SET Age += 1
                                                                WHERE Id = @id
                                                               
                                                               SELECT Name, Age FROM Minions WHERE Id = @Id";

        public const string ExecuteProcedure = @"EXEC usp_GetOlder @Id";


        public const string GetTownId = @"SELECT Id FROM Towns WHERE Name = @townName";

        public const string InsertTown = @"INSERT INTO Towns (Name) VALUES (@townName)";

        public const string GetVillainId = @"SELECT Id FROM Villains WHERE Name = @Name";

        public const string InsertVillain = @"INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, 4)";

        public const string GetMinionId = @"SELECT Id FROM Minions WHERE Name = @Name";

        public const string InsertMinion = @"INSERT INTO Minions (Name, Age, TownId) VALUES (@name, @age, @townId)";

        public const string MakeMinionServantVillain = @"INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@villainId, @minionId)";


        public const string GetVillainName = @"SELECT Name FROM Villains WHERE Id = @villainId";

        public const string CreateDeleteProc = @"CREATE PROC usp_DeleteVillain (@villainId int)
                                                    AS
                                                    BEGIN
                                                    BEGIN TRANSACTION
                                                    		If @villainId IS NULL
                                                    			ROLLBACK
                                                    
                                                    		DELETE FROM MinionsVillains 
                                                    		      WHERE VillainId = @villainId
                                                    		
                                                    		DELETE FROM Villains
                                                    		      WHERE Id = @villainId
                                                    		
                                                    COMMIT
                                                    END";

        public const string ExecuteDeleteProc = @"EXEC PROC usp_DeleteVillain @villainId";
    }
}
