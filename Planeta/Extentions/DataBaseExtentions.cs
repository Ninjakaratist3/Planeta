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
        public static void InitializeDataBase(this IServiceCollection services, string connectionString)
        {
            using (IDbConnection dataBaseConnection = new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=master;Trusted_Connection=True;Integrated Security=True"))
            {
                dataBaseConnection.Execute(@"If(db_id(N'Planeta') IS NULL) BEGIN CREATE DATABASE Planeta END");
            }

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
                                                        [SubnetId]     INT NULL,
                                                        PRIMARY KEY (Id)
                                                    );
                                                    INSERT INTO Users (FirstName, MiddleName, Surname, Age, Gender)
                                                    VALUES (N'Иван', N'Иванович', N'Иванов', 19, N'Мужчина'), 
                                                           (N'Петр', N'Петрович', N'Петров', 21, N'Мужчина'),
                                                           (N'Елена', N'Владимировна', N'Иванова', 20, N'Женщина');
                                                END;";
            sqlCreateTablesCommand +=      @"IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES
                                                WHERE TABLE_NAME = 'Subnets'))
                                                BEGIN 
                                                    RETURN 
                                                END
                                             ELSE 
                                                BEGIN
                                                    CREATE TABLE [Subnets] (
                                                        [Id]               INT IDENTITY (1, 1) NOT NULL,
                                                        [IP]               NVARCHAR(15),
                                                        [Mask]             NVARCHAR(15),
                                                        [StartOfService]   DATETIME NULL,
                                                        [EndOfService]     DATETIME NULL,
                                                        [UserId]           INT,
                                                        PRIMARY KEY (Id)
                                                    );
                                                    ALTER TABLE [Users] ADD FOREIGN KEY(SubnetId) REFERENCES Subnets(Id);

                                                    INSERT INTO Subnets(IP, Mask, StartOfService, EndOfService, UserId)
                                                    VALUES (N'192.168.1.0', N'255.255.255.0', N'12.12.2015 12:00:00', N'01.05.2022 12:00:00', 1), 
                                                           (N'192.168.2.0', N'255.255.255.0', N'11.07.2011 12:00:00', N'07.07.2023 12:00:00', 2),
                                                           (N'192.168.3.0', N'255.255.255.0', N'10.12.2017 12:00:00', N'05.09.2023 12:00:00', 3);
                                                END";

            using (IDbConnection dataBaseConnection = new SqlConnection(connectionString))
            {
                dataBaseConnection.Execute(sqlCreateTablesCommand);
            }
        }
    }
}
