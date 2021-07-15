using Core.Models;
using Core.Repositories.User;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
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
                                                WHERE TABLE_NAME = 'Users'))
                                                BEGIN 
                                                    RETURN 
                                                END
                                             ELSE 
                                                BEGIN
                                                    CREATE TABLE [Users] (
                                                        [Id]           INT IDENTITY (1, 1) NOT NULL,
                                                        [FirstName]    NVARCHAR(50) NOT NULL,
                                                        [MiddleName]   NVARCHAR(50) NULL,
                                                        [Surname]      NVARCHAR(50) NULL,
                                                        [Age]          INT NULL,
                                                        [Gender]       NVARCHAR(10) NULL,
                                                        [SubnetId]     INT,
                                                        PRIMARY KEY (Id)
                                                    );
                                                    INSERT INTO Users (FirstName, MiddleName, Surname, Age, Gender)
                                                    VALUES (N'Иван', N'Иванович', N'Иванов', N'19', N'Мужчина'), 
                                                           (N'Петр', N'Петрович', N'Петров', N'21', N'Мужчина'),
                                                           (N'Елена', N'Владимировна', N'Иванова', N'19', N'Женщина');
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
                                                        [IP]               NVARCHAR(32),
                                                        [Mask]             NVARCHAR(32),
                                                        [StartOfService]   DATETIME NULL,
                                                        [EndOfService]     DATETIME NULL,
                                                        [UserId]           INT,
                                                        PRIMARY KEY (Id)
                                                    );
                                                    ALTER TABLE [Users] ADD FOREIGN KEY(SubnetId) REFERENCES Subnets(Id);

                                                    INSERT INTO Users(FirstName, MiddleName, Surname, Age, Gender)
                                                    VALUES (N'255.', N'192.168.11.10', N'255.255.248.0', N'07.07.2003 12:00:00', N'07.07.2003 12:00:00', 1), 
                                                           (N'255.', N'192.168.11.10', N'255.255.248.0', N'07.07.2003 12:00:00', N'07.07.2003 12:00:00', 1)
                                                           (N'255.', N'192.168.11.10', N'255.255.248.0', N'07.07.2003 12:00:00', N'07.07.2003 12:00:00', 1);
                                                END";

            using (IDbConnection dataBaseConnection = new SqlConnection(connectionString))
            {
                dataBaseConnection.Execute(sqlCreateTablesCommand);
            }
        }
    }
}
