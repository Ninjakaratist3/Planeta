using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Core.Repositories.User
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Models.User Get(int id)
        {
            string sqlGetCommand = "SELECT * FROM Users WHERE Id = @id";

            using (IDbConnection dataBaseConnection = new SqlConnection(_connectionString))
            {
                return dataBaseConnection.Query<Models.User>(sqlGetCommand, new { id }).FirstOrDefault();
            }
        }

        public List<Models.User> GetUsers()
        {
            string sqlGetAllCommand = "SELECT * FROM Users";

            using (IDbConnection dataBaseConnection = new SqlConnection(_connectionString))
            {
                return dataBaseConnection.Query<Models.User>(sqlGetAllCommand).ToList();
            }
        }

        public void Add(Models.User model)
        {
            string sqlAddCommand = @"INSERT INTO Users (FirstName, MiddleName, Surname, Age, Gender)
                                    VALUES(@FirstName, @MiddleName, @Surname, @Age, @Gender)";

            using (IDbConnection dataBaseConnection = new SqlConnection(_connectionString))
            {
                dataBaseConnection.Execute(sqlAddCommand, model);
            }
        }

        public void Update(Models.User model)
        {
            string sqlUpdateCommand = @"UPDATE Users 
                                        SET FirstName = @FirstName, 
                                            MiddleName = @MiddleName, 
                                            Surname = @Surname, 
                                            Age = @Age, 
                                            Gender = @Gender
                                        WHERE Id = @Id";

            using (IDbConnection dataBaseConnection = new SqlConnection(_connectionString))
            {
                dataBaseConnection.Execute(sqlUpdateCommand, model);
            }
        }

        public void Delete(int id)
        {
            string sqlDeleteCommand = "DELETE FROM Users WHERE Id = @id";
            string sqlSubnetDeleteCommand = "DELETE FROM Subnets WHERE UserId = @id";

            using (IDbConnection dataBaseConnection = new SqlConnection(_connectionString))
            {
                dataBaseConnection.Execute(sqlDeleteCommand, new { id });
                dataBaseConnection.Execute(sqlSubnetDeleteCommand, new { id });
            }
        }
    }
}
