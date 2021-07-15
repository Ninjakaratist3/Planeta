using Core.Models;
using Core.ViewModels.Subnet;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Core.Repositories
{
    public class SubnetRepository : ISubnetRepository
    {
        private readonly string _connectionString;

        public SubnetRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Subnet Get(int userId)
        {
            string sqlGetCommand = "SELECT * FROM Subnets WHERE UserId = @userId";

            using (IDbConnection dataBaseConnection = new SqlConnection(_connectionString))
            {
                return dataBaseConnection.Query<Subnet>(sqlGetCommand, new { userId }).FirstOrDefault();
            }
        }

        public void Add(SubnetForm model)
        {
            string sqlAddCommand = @"INSERT INTO Subnets (IP, StartOfService, EndOfService, UserId)
                                    VALUES(@IP, @StartOfService, @EndOfService, @UserId)";

            using (IDbConnection dataBaseConnection = new SqlConnection(_connectionString))
            {
                dataBaseConnection.Execute(sqlAddCommand, model);
            }
        }

        public void Update(SubnetForm model)
        {
            string sqlUpdateCommand = @"UPDATE Subnets 
                                        SET IP = @IP, 
                                            StartOfService = @StartOfService, 
                                            EndOfService = @EndOfService, 
                                            UserId = @UserId
                                        WHERE Id = @Id";

            using (IDbConnection dataBaseConnection = new SqlConnection(_connectionString))
            {
                dataBaseConnection.Execute(sqlUpdateCommand, model);
            }
        }

        public void Delete(int userId)
        {
            string sqlDeleteCommand = "DELETE FROM Subnets WHERE UserId = @userId";
            string sqlDeleteSubnetFromUserCommand = @"UPDATE Users 
                                                      SET SubnetId = NULL,
                                                      WHERE Id = @userId";

            using (IDbConnection dataBaseConnection = new SqlConnection(_connectionString))
            {
                dataBaseConnection.Execute(sqlDeleteCommand, new { userId });
                dataBaseConnection.Execute(sqlDeleteSubnetFromUserCommand, new { userId });
            }
        }
    }
}
