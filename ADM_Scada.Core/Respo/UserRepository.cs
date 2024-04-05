using ADM_Scada.Core.Models;
using ADM_Scada.Cores.Respo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ADM_Scada.Core.Respo
{
    public class UserRepository : RepositoryBase, IDataRepository<UserModel>
    {
        public async Task<List<UserModel>> GetAll()
        {
            string query = "SELECT * FROM [dbo].[user]";
            DataTable dataTable = await ExecuteQueryAsync(query);
            return ConvertDataTableToList(dataTable);
        }

        private List<UserModel> ConvertDataTableToList(DataTable dataTable)
        {
            throw new NotImplementedException();
        }

        public async Task<UserModel> GetById(int userId)
        {
            string query = "SELECT * FROM [dbo].[user] WHERE id = @UserId";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@UserId", userId } };
            DataTable dataTable = await ExecuteQueryAsync(query, parameters);
            return ConvertDataTableToSingleObject(dataTable);
        }

        private UserModel ConvertDataTableToSingleObject(DataTable dataTable)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Create(UserModel user)
        {
            string query = @"INSERT INTO [dbo].[user] (user_code, user_name, password, user_avatar, user_group, email_address, tel_no, mobile_no, created_date, created_by, updated_date, updated_by) 
                             VALUES (@UserCode, @UserName, @Password, @UserAvatar, @UserGroup, @EmailAddress, @TelNo, @MobileNo, @CreatedDate, @CreatedBy, @UpdatedDate, @UpdatedBy)";
            // Set CreatedDate and UpdatedDate
            user.CreatedDate = DateTime.Now;
            user.UpdatedDate = DateTime.Now;

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@UserCode", user.UserCode },
                { "@UserName", user.UserName },
                { "@Password", user.Password },
                { "@UserAvatar", user.UserAvatar },
                { "@UserGroup", user.UserGroup },
                { "@EmailAddress", user.EmailAddress },
                { "@TelNo", user.TelNo },
                { "@MobileNo", user.MobileNo },
                { "@CreatedDate", user.CreatedDate },
                { "@CreatedBy", user.CreatedBy },
                { "@UpdatedDate", user.UpdatedDate },
                { "@UpdatedBy", user.UpdatedBy }
            };
            return await ExecuteNonQueryAsync(query, parameters);
        }

        public async Task<bool> Update(UserModel user)
        {
            string query = @"UPDATE [dbo].[user] 
                             SET user_code = @UserCode, user_name = @UserName, password = @Password, user_avatar = @UserAvatar, user_group = @UserGroup, 
                                 email_address = @EmailAddress, tel_no = @TelNo, mobile_no = @MobileNo, created_date = @CreatedDate, created_by = @CreatedBy, 
                                 updated_date = @UpdatedDate, updated_by = @UpdatedBy 
                             WHERE id = @Id";
            // Set UpdatedDate
            user.UpdatedDate = DateTime.Now;

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@Id", user.Id },
                { "@UserCode", user.UserCode },
                { "@UserName", user.UserName },
                { "@Password", user.Password },
                { "@UserAvatar", user.UserAvatar },
                { "@UserGroup", user.UserGroup },
                { "@EmailAddress", user.EmailAddress },
                { "@TelNo", user.TelNo },
                { "@MobileNo", user.MobileNo },
                { "@CreatedDate", user.CreatedDate },
                { "@CreatedBy", user.CreatedBy },
                { "@UpdatedDate", user.UpdatedDate },
                { "@UpdatedBy", user.UpdatedBy }
            };
            return await ExecuteNonQueryAsync(query, parameters) > 0;
        }

        public Task<UserModel> GetByName(string s)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<UserModel> Get(int id)
        {
            throw new NotImplementedException();
        }

        // Implement other CRUD methods and helper methods...
    }
}