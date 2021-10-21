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


    }
}
