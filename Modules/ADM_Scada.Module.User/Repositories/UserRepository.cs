using ADM_Scada.Cores.Models;
using ADM_Scada.Cores.Respo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ADM_Scada.Modules.User.Repositories
{
    public class UserRepository : RepositoryBase, IDataRepository<UserModel>
    {
        public UserRepository() : base()
        {
        }

        public async Task<List<UserModel>> GetAll()
        {
            string query = "SELECT * FROM user";
            DataTable dataTable = await ExecuteQueryAsync(query);
            return ConvertDataTableToList(dataTable);
        }

        public async Task<UserModel> GetByName(string loginName)
        {
            string query = "SELECT * FROM user WHERE LoginName = @LoginName";
            Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@LoginName", loginName }
        };
            DataTable dataTable = await ExecuteQueryAsync(query, parameters);
            return ConvertDataTableToList(dataTable).FirstOrDefault();
        }

        public async Task<int> Create(UserModel user)
        {
            string query = @"INSERT INTO user (Level, FullName, Code, LoginName, Password, Avatar)
                         VALUES (@Level, @FullName, @Code, @LoginName, @Password, @Avatar);
                         SELECT SCOPE_IDENTITY()";

            Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@Level", user.Level },
            { "@FullName", user.FullName },
            { "@Code", user.Code },
            { "@LoginName", user.LoginName },
            { "@Password", user.Password },
            { "@Avatar", user.Avatar }
        };

            object result = await ExecuteScalarAsync(query, parameters);
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public async Task<bool> Update(UserModel user)
        {
            string query = @"UPDATE user
                         SET Level = @Level, FullName = @FullName, Code = @Code,
                             Password = @Password, Avatar = @Avatar
                         WHERE Id = @Id";

            Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@Level", user.Level },
            { "@FullName", user.FullName },
            { "@Code", user.Code },
            { "@Password", user.Password },
            { "@Avatar", user.Avatar },
            { "@Id", user.Id }
        };

            return await ExecuteNonQueryAsync(query, parameters) > 0;
        }

        public async Task<bool> Delete(int id)
        {
            string query = "DELETE FROM user WHERE Id = @Id";
            Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@Id", id }
        };
            return await ExecuteNonQueryAsync(query, parameters) > 0;
        }

        public async Task<UserModel> Get(int id)
        {
            string query = "SELECT * FROM user WHERE Id = @Id";
            Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@Id", id }
        };
            DataTable dataTable = await ExecuteQueryAsync(query, parameters);
            return ConvertDataTableToList(dataTable).FirstOrDefault();
        }

        private List<UserModel> ConvertDataTableToList(DataTable dataTable)
        {
            List<UserModel> userList = new List<UserModel>();

            foreach (DataRow row in dataTable.Rows)
            {
                UserModel user = new UserModel
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Level = Convert.ToInt32(row["Level"]),
                    FullName = row["FullName"].ToString(),
                    Code = row["Code"].ToString(),
                    LoginName = row["LoginName"].ToString(),
                    Password = row["Password"].ToString(),
                    Avatar = row["Avatar"].ToString(),
                    IsEnable = false
                };

                userList.Add(user);
            }

            return userList;
        }
    }
}
