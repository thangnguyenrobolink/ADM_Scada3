using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using ADM_Scada.Core.Models;
using Serilog;

namespace ADM_Scada.Core.Respo
{
    public class UserGroupRepository : RepositoryBase, IDataRepository<UserGroupModel>
    {
        public UserGroupRepository() { }

        public async Task<List<UserGroupModel>> GetAll()
        {
            try
            {
                string query = "SELECT * FROM [dbo].[user_group]";
                DataTable dataTable = await ExecuteQueryAsync(query);
                return ConvertDataTableToList(dataTable);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving all user groups.");
                throw new RepositoryException("An error occurred while retrieving all user groups. Please try again later.", ex);
            }
        }

        public async Task<UserGroupModel> GetById(int id)
        {
            try
            {
                string query = "SELECT * FROM [dbo].[user_group] WHERE id = @Id";
                Dictionary<string, object> parameters = new Dictionary<string, object> { { "@Id", id } };
                DataTable dataTable = await ExecuteQueryAsync(query, parameters);
                return ConvertDataTableToSingleObject(dataTable);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving user group by ID: {GroupId}", id);
                throw new RepositoryException($"An error occurred while retrieving user group with ID '{id}'. Please try again later.", ex);
            }
        }

        public async Task<int> Create(UserGroupModel userGroup)
        {
            try
            {
                string query = @"INSERT INTO [dbo].[user_group] (group_description, created_date, created_by, updated_date, updated_by) 
                                 VALUES (@GroupDescription, @CreatedDate, @CreatedBy, @UpdatedDate, @UpdatedBy)";

                // Set CreatedDate and UpdatedDate
                userGroup.CreatedDate = DateTime.Now;
                userGroup.UpdatedDate = DateTime.Now;

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@GroupDescription", userGroup.GroupDescription },
                    { "@CreatedDate", userGroup.CreatedDate },
                    { "@CreatedBy", userGroup.CreatedBy },
                    { "@UpdatedDate", userGroup.UpdatedDate },
                    { "@UpdatedBy", userGroup.UpdatedBy }
                };
                return await ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while creating user group: {GroupDescription}", userGroup.GroupDescription);
                throw new RepositoryException($"An error occurred while creating user group '{userGroup.GroupDescription}'. Please try again later.", ex);
            }
        }

        public async Task<bool> Update(UserGroupModel userGroup)
        {
            try
            {
                string query = @"UPDATE [dbo].[user_group] 
                                 SET group_description = @GroupDescription, created_date = @CreatedDate, created_by = @CreatedBy, 
                                     updated_date = @UpdatedDate, updated_by = @UpdatedBy 
                                 WHERE id = @Id";

                // Set UpdatedDate
                userGroup.UpdatedDate = DateTime.Now;

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@Id", userGroup.Id },
                    { "@GroupDescription", userGroup.GroupDescription },
                    { "@CreatedDate", userGroup.CreatedDate },
                    { "@CreatedBy", userGroup.CreatedBy },
                    { "@UpdatedDate", userGroup.UpdatedDate },
                    { "@UpdatedBy", userGroup.UpdatedBy }
                };
                return await ExecuteNonQueryAsync(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while updating user group with ID: {GroupId}", userGroup.Id);
                throw new RepositoryException($"An error occurred while updating user group with ID '{userGroup.Id}'. Please try again later.", ex);
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                string query = "DELETE FROM [dbo].[user_group] WHERE id = @Id";
                Dictionary<string, object> parameters = new Dictionary<string, object> { { "@Id", id } };
                return await ExecuteNonQueryAsync(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while deleting user group with ID: {GroupId}", id);
                throw new RepositoryException($"An error occurred while deleting user group with ID '{id}'. Please try again later.", ex);
            }
        }

        private List<UserGroupModel> ConvertDataTableToList(DataTable dataTable)
        {
            List<UserGroupModel> userGroupList = new List<UserGroupModel>();

            foreach (DataRow row in dataTable.Rows)
            {
                userGroupList.Add(ConvertDataRowToUserGroupModel(row));
            }

            return userGroupList;
        }

        private UserGroupModel ConvertDataRowToUserGroupModel(DataRow row)
        {
            return new UserGroupModel
            {
                Id = Convert.ToInt32(row["id"]),
                GroupDescription = Convert.ToString(row["group_description"]),
                CreatedDate = Convert.IsDBNull(row["created_date"]) ? (DateTime?)null : Convert.ToDateTime(row["created_date"]),
                CreatedBy = Convert.ToString(row["created_by"]),
                UpdatedDate = Convert.IsDBNull(row["updated_date"]) ? (DateTime?)null : Convert.ToDateTime(row["updated_date"]),
                UpdatedBy = Convert.ToString(row["updated_by"])
            };
        }

        private UserGroupModel ConvertDataTableToSingleObject(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
            {
                return null;
            }

            DataRow row = dataTable.Rows[0];
            return ConvertDataRowToUserGroupModel(row);
        }

        public Task<UserGroupModel> GetByName(string s)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserGroupModel>> GetFiltered(params Func<UserGroupModel, bool>[] filters)
        {
            throw new NotImplementedException();
        }

        public Task<int> Count()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Exists(int id)
        {
            throw new NotImplementedException();
        }
    }
}
