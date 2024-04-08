using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ADM_Scada.Core.Models;
using Serilog;

namespace ADM_Scada.Core.Respo
{
    public class UserRepository : RepositoryBase, IDataRepository<UserModel>
    {
        public UserRepository() { }

        // Method to retrieve a user by ID
        public async Task<UserModel> GetById(int id)
        {
            try
            {
                string query = "SELECT * FROM [dbo].[user] WHERE id = @Id";
                Dictionary<string, object> parameters = new Dictionary<string, object> { { "@Id", id } };
                DataTable dataTable = await ExecuteQueryAsync(query, parameters);
                return ConvertDataTableToSingleObject(dataTable);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving user by ID: {UserId}", id);
                throw new RepositoryException($"An error occurred while retrieving user with ID '{id}'. Please try again later.", ex);
            }
        }
        // Method to retrieve all users
        public async Task<List<UserModel>> GetAll()
        {
            try
            {
                string query = "SELECT * FROM [dbo].[user]";
                DataTable dataTable = await ExecuteQueryAsync(query);
                return ConvertDataTableToList(dataTable);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving all users.");
                throw new RepositoryException("An error occurred while retrieving all users. Please try again later.", ex);
            }
        }

        // Method to retrieve a user by name
        public async Task<UserModel> GetByName(string userName)
        {
            try
            {
                string query = "SELECT * FROM [dbo].[user] WHERE user_name = @UserName";
                Dictionary<string, object> parameters = new Dictionary<string, object> { { "@UserName", userName } };
                DataTable dataTable = await ExecuteQueryAsync(query, parameters);
                return ConvertDataTableToSingleObject(dataTable);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving user by name: {UserName}", userName);
                throw new RepositoryException($"An error occurred while retrieving user '{userName}'. Please try again later.", ex);
            }
        }

        // Method to create a new user
        public async Task<int> Create(UserModel user)
        {
            try
            {
                string query = @"INSERT INTO [dbo].[user] (user_code, user_name, password, user_avatar, user_group, email_address, tel_no, mobile_no, created_date, created_by, updated_date, updated_by) 
                                 VALUES (@UserCode, @UserName, @Password, @UserAvatar, @UserGroup, @EmailAddress, @TelNo, @MobileNo, @CreatedDate, @CreatedBy, @UpdatedDate, @UpdatedBy)";
                // Set CreatedDate and UpdatedDate
                user.CreatedDate = DateTime.Now;
                user.UpdatedDate = DateTime.Now;

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    // Add parameters
                };
                return await ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while creating user: {UserName}", user.UserName);
                throw new RepositoryException($"An error occurred while creating user '{user.UserName}'. Please try again later.", ex);
            }
        }

        // Method to update a user
        public async Task<bool> Update(UserModel user)
        {
            try
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
                    // Add parameters
                };
                return await ExecuteNonQueryAsync(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while updating user with ID: {UserId}", user.Id);
                throw new RepositoryException($"An error occurred while updating user with ID '{user.Id}'. Please try again later.", ex);
            }
        }

        // Method to delete a user
        public async Task<bool> Delete(int id)
        {
            try
            {
                string query = "DELETE FROM [dbo].[user] WHERE id = @Id";
                Dictionary<string, object> parameters = new Dictionary<string, object> { { "@Id", id } };
                return await ExecuteNonQueryAsync(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while deleting user with ID: {UserId}", id);
                throw new RepositoryException($"An error occurred while deleting user with ID '{id}'. Please try again later.", ex);
            }
        }

        // Method to retrieve filtered users
        public async Task<List<UserModel>> GetFiltered(params Func<UserModel, bool>[] filters)
        {
            try
            {
                string whereClause = string.Join(" AND ", filters.Select(filter => $"({filter})"));
                string query = $"SELECT * FROM [dbo].[user] WHERE {whereClause}";
                DataTable dataTable = await ExecuteQueryAsync(query);
                return ConvertDataTableToList(dataTable);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving filtered users.");
                throw new RepositoryException("An error occurred while retrieving filtered users. Please try again later.", ex);
            }
        }

        // Method to count the number of users
        public async Task<int> Count()
        {
            try
            {
                string query = "SELECT COUNT(*) FROM [dbo].[user]";
                object result = await ExecuteScalarAsync(query);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while counting users.");
                throw new RepositoryException("An error occurred while counting users. Please try again later.", ex);
            }
        }

        // Method to check if a user with the given ID exists
        public async Task<bool> Exists(int id)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM [dbo].[user] WHERE id = @Id";
                Dictionary<string, object> parameters = new Dictionary<string, object> { { "@Id", id } };
                int count = Convert.ToInt32(await ExecuteScalarAsync(query, parameters));
                return count > 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while checking if user exists with ID: {UserId}", id);
                throw new RepositoryException($"An error occurred while checking if user exists with ID '{id}'. Please try again later.", ex);
            }
        }

        // Method to convert DataTable to a list of UserModel objects
        private List<UserModel> ConvertDataTableToList(DataTable dataTable)
        {
            List<UserModel> userList = new List<UserModel>();

            foreach (DataRow row in dataTable.Rows)
            {
                userList.Add(ConvertDataRowToUserModel(row));
            }

            return userList;
        }

        // Method to convert DataRow to a UserModel object
        private UserModel ConvertDataRowToUserModel(DataRow row)
        {
            return new UserModel
            {
                // Set properties from DataRow
            };
        }

        // Method to convert DataTable to a single UserModel object
        private UserModel ConvertDataTableToSingleObject(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
            {
                return null;
            }

            DataRow row = dataTable.Rows[0];
            return ConvertDataRowToUserModel(row);
        }
    }
}

