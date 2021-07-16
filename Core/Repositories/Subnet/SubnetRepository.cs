using Core.ViewModels.Subnet;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net;

namespace Core.Repositories.Subnet
{
    public class SubnetRepository : ISubnetRepository
    {
        private readonly string _connectionString;

        public SubnetRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Models.Subnet Get(int userId)
        {
            string sqlGetCommand = "SELECT Id, Mask, StartOfService, EndOfService, UserId FROM Subnets WHERE UserId = @userId";
            var subnet = new Models.Subnet();
            string IP = "";

            using (IDbConnection dataBaseConnection = new SqlConnection(_connectionString))
            {
                subnet = dataBaseConnection.Query<Models.Subnet>(sqlGetCommand, new { userId }).FirstOrDefault();
                IP = dataBaseConnection.Query<string>("SELECT IP FROM Subnets WHERE UserId = @userId", new { userId }).FirstOrDefault();
            }

            if (IP != null)
            {
                subnet.IP = IPAddress.Parse(IP);
            }

            return subnet;
        }

        public void Add(Models.Subnet model)
        {
            string sqlAddCommand = @"INSERT INTO Subnets (IP, Mask, StartOfService, EndOfService, UserId)
                                    VALUES(@IP, @Mask, @StartOfService, @EndOfService, @UserId);
                                    SELECT CAST(SCOPE_IDENTITY() as int);";
            string sqlUpdateCommand = @"UPDATE Users 
                                        SET SubnetId = @SubnetId
                                        WHERE Id = @UserId";

            var subnet = ConvertSubnetToSubnetViewModel(model);

            using (IDbConnection dataBaseConnection = new SqlConnection(_connectionString))
            {
                int? subnetId = dataBaseConnection.Query<int>(sqlAddCommand, subnet).FirstOrDefault();
                dataBaseConnection.Execute(sqlUpdateCommand, new { UserId = model.UserId, SubnetId = subnetId.Value });
            }
        }

        public void Update(Models.Subnet model)
        {
            string sqlUpdateCommand = @"UPDATE Subnets 
                                        SET IP = @IP, 
                                            Mask = @Mask,
                                            StartOfService = @StartOfService, 
                                            EndOfService = @EndOfService, 
                                            UserId = @UserId
                                        WHERE UserId = @UserId";
            var subnet = ConvertSubnetToSubnetViewModel(model);

            using (IDbConnection dataBaseConnection = new SqlConnection(_connectionString))
            {
                dataBaseConnection.Execute(sqlUpdateCommand, subnet);
            }
        }

        public SubnetViewModel ConvertSubnetToSubnetViewModel(Models.Subnet model)
        {
            var subnetForm = new SubnetViewModel()
            {
                Id = model.Id,
                IP = model.IP.ToString(),
                Mask = model.Mask,
                StartOfService = model.StartOfService,
                EndOfService = model.EndOfService,
                UserId = model.UserId
            };

            return subnetForm;
        }

        public void Delete(int userId)
        {
            string sqlDeleteCommand = "DELETE FROM Subnets WHERE UserId = @userId";
            string sqlDeleteSubnetFromUserCommand = @"UPDATE Users 
                                                      SET SubnetId = NULL
                                                      WHERE Id = @userId";

            using (IDbConnection dataBaseConnection = new SqlConnection(_connectionString))
            {
                dataBaseConnection.Execute(sqlDeleteCommand, new { userId });
                dataBaseConnection.Execute(sqlDeleteSubnetFromUserCommand, new { userId });
            }
        }
    }
}
