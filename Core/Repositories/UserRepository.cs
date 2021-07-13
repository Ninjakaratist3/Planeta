using Core.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Core.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public User Get(int id)
        {
            string sqlGetCommand = "SELECT * FROM Users WHERE Id = @id";

            using (IDbConnection dataBaseConnection = new SqlConnection(_connectionString))
            {
                return dataBaseConnection.Query<User>(sqlGetCommand, new { id }).FirstOrDefault();
            }
        }

        public List<User> GetUsers()
        {
            string sqlGetAllCommand = "SELECT * FROM Users";

            using (IDbConnection dataBaseConnection = new SqlConnection(_connectionString))
            {
                return dataBaseConnection.Query<User>(sqlGetAllCommand).ToList();
            }
        }

        public void Add(User user)
        {
            string sqlAddCommand = @"INSERT INTO Users (Name, MiddleName, Surname, Age, Gender)
                                    VALUES(@Name, @MiddleName, @Surname, @Age, @Gender)";

            using (IDbConnection dataBaseConnection = new SqlConnection(_connectionString))
            {
                dataBaseConnection.Execute(sqlAddCommand, user);
            }
        }

        public void Update(User user)
        {
            string sqlUpdateCommand = @"UPDATE Users 
                                        SET Name = @Name, 
                                            MiddleName = @MiddleName, 
                                            Surname = @Surname, 
                                            Age = @Age, 
                                            Gender = @Gender
                                        WHERE Id = @Id";

            using (IDbConnection dataBaseConnection = new SqlConnection(_connectionString))
            {
                dataBaseConnection.Execute(sqlUpdateCommand, user);
            }
        }

        public void Delete(int id)
        {
            string sqlDeleteCommand = "DELETE FROM Users WHERE Id = @id";

            using (IDbConnection dataBaseConnection = new SqlConnection(_connectionString))
            {
                dataBaseConnection.Execute(sqlDeleteCommand, new { id });
            }
        }
    }
}
