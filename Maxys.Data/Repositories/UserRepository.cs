using Dapper;
using ERP_MaxysHC.Maxys.Model;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Text;

namespace ERP_MaxysHC.Maxys.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public UserRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<IEnumerable<UsersModel>> GetAllUsers()
        {
            var db = GetConnection();
            var query = "SELECT * FROM users";
            var users = await db.QueryAsync<UsersModel>(query);
            return users;
        }

        public async Task<bool> UpdateUser(UsersModel userModel)
        {
            var db = GetConnection();
            var query = "UPDATE users SET User_name = @User_name, User_lastname = @User_lastname, Position = @Position, User = @User, Email = @Email, Password = @Password WHERE Id_user = @Id_user";
            userModel.Password = GetSHA256(userModel.Password);
            var result = await db.ExecuteAsync(query, new { userModel.Id_user, userModel.User_name, userModel.User_lastname, userModel.Position, userModel.User, userModel.Email, userModel.Password });
            return result > 0;
        }

        public static string GetSHA256(string str)
        {
            SHA256 sha256 = SHA256Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }
    }
}
