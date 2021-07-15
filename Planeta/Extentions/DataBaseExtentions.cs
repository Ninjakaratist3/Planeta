using Core.Repositories;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace Planeta.Extentions
{
    public static class DataBaseExtentions
    {
        private static IUserRepository _userRepository;

        public static void InitializeDataBase(this IServiceCollection services, string connectionString)
        {
            _userRepository = new UserRepository(connectionString);

            string sqlCreateTablesCommand = @"IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES
                                                WHERE TABLE_NAME = 'Customers'))
                                                BEGIN 
                                                    RETURN 
                                                END
                                             ELSE 
                                                BEGIN
                                                    CREATE TABLE [Customers] (
                                                    [Id]           INT IDENTITY (1, 1) NOT NULL,
                                                    [FirstName]    NVARCHAR(50) NOT NULL,
                                                    [MiddleName]   NVARCHAR(50) NULL,
                                                    [Surname]      NVARCHAR(50) NULL,
                                                    [Age]          INT NULL,
                                                    [Gender]       NVARCHAR(10) NULL,
                                                    [SubnetId]     INT,
                                                    PRIMARY KEY (Id)
                                                    );
                                                END;";
            sqlCreateTablesCommand += @"IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES
                                                WHERE TABLE_NAME = 'Subnets'))
                                                BEGIN 
                                                    RETURN 
                                                END
                                             ELSE 
                                                BEGIN
                                                    CREATE TABLE [Subnets] (
                                                        [Id]               INT IDENTITY (1, 1) NOT NULL,
                                                        [IP]               NVARCHAR(35),
                                                        [StartOfService]   DATETIME NULL,
                                                        [EndOfService]     DATETIME NULL,
                                                        [UserId]           INT,
                                                        PRIMARY KEY (Id)
                                                    );
                                                    ALTER TABLE [Customers] ADD FOREIGN KEY(SubnetId) REFERENCES Subnets(Id);
                                                    ALTER TABLE [Subnets] ADD FOREIGN KEY(UserId) REFERENCES Customers(Id);
                                                END";

            using (IDbConnection dataBaseConnection = new SqlConnection(connectionString))
            {
                dataBaseConnection.Execute(sqlCreateTablesCommand);
            }
        }
    }
}
