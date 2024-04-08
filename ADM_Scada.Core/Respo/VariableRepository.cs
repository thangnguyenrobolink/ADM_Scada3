using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ADM_Scada.Core.Models;
using Serilog;

namespace ADM_Scada.Core.Respo
{
    public class VariableRepository : RepositoryBase, IDataRepository<VariableModel>
    {
        public VariableRepository() { }

        // Method to retrieve all variables
        public async Task<List<VariableModel>> GetAll()
        {
            try
            {
                string query = "SELECT * FROM [dbo].[Variable]";
                DataTable dataTable = await ExecuteQueryAsync(query);
                return ConvertDataTableToList(dataTable);
            }
            catch (Exception ex)
            {
               
                Log.Error(ex, "Error occurred while retrieving all variables.");
                throw new RepositoryException("An error occurred while retrieving all variables. Please try again later.", ex);

            }
        }

        // Method to retrieve a variable by name
        public async Task<VariableModel> GetByName(string variableName)
        {
            try
            {
                string query = "SELECT * FROM [dbo].[Variable] WHERE Name = @VariableName";
                Dictionary<string, object> parameters = new Dictionary<string, object> { { "@VariableName", variableName } };
                DataTable dataTable = await ExecuteQueryAsync(query, parameters);
                return ConvertDataTableToSingleObject(dataTable);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving variable by name: {VariableName}", variableName);
                throw new RepositoryException($"An error occurred while retrieving variable '{variableName}'. Please try again later.", ex);
            }
        }

        // Method to retrieve a variable by ID
        public async Task<VariableModel> GetById(int id)
        {
            try
            {
                string query = "SELECT * FROM [dbo].[Variable] WHERE Id = @Id";
                Dictionary<string, object> parameters = new Dictionary<string, object> { { "@Id", id } };
                DataTable dataTable = await ExecuteQueryAsync(query, parameters);
                return ConvertDataTableToSingleObject(dataTable);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving variable by ID: {VariableId}", id);
                throw new RepositoryException($"An error occurred while retrieving variable with ID '{id}'. Please try again later.", ex);
            }
        }

        // Method to create a new variable
        public async Task<int> Create(VariableModel variable)
        {
            try
            {
                string query = @"INSERT INTO [dbo].[Variable] (DeviceId, Type, Area, Address, Name, Module, Unit, Message, Value, Purpose, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy) 
                                 VALUES (@DeviceId, @Type, @Area, @Address, @Name, @Module, @Unit, @Message, @Value, @Purpose, @CreatedDate, @CreatedBy, @UpdatedDate, @UpdatedBy)";

                // Set CreatedDate and UpdatedDate
                variable.CreatedDate = DateTime.Now;
                variable.UpdatedDate = DateTime.Now;

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@DeviceId", variable.DeviceId },
                    { "@Type", variable.Type },
                    { "@Area", variable.Area },
                    { "@Address", variable.Address },
                    { "@Name", variable.Name },
                    { "@Module", variable.Module },
                    { "@Unit", variable.Unit },
                    { "@Message", variable.Message },
                    { "@Value", variable.Value },
                    { "@Purpose", variable.Purpose },
                    { "@CreatedDate", variable.CreatedDate },
                    { "@CreatedBy", variable.CreatedBy },
                    { "@UpdatedDate", variable.UpdatedDate },
                    { "@UpdatedBy", variable.UpdatedBy }
                };
                return await ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while creating variable: {VariableName}", variable.Name);
                throw new RepositoryException($"An error occurred while creating variable '{variable.Name}'. Please try again later.", ex);
            }
        }

        // Method to update a variable
        public async Task<bool> Update(VariableModel variable)
        {
            try
            {
                string query = @"UPDATE [dbo].[Variable] 
                         SET DeviceId = @DeviceId, Type = @Type, Area = @Area, Address = @Address, 
                             Name = @Name, Module = @Module, Unit = @Unit, Message = @Message, 
                             Value = @Value, Purpose = @Purpose, 
                             CreatedDate = @CreatedDate, CreatedBy = @CreatedBy, 
                             UpdatedDate = @UpdatedDate, UpdatedBy = @UpdatedBy 
                         WHERE Id = @Id";

                // Set UpdatedDate
                variable.UpdatedDate = DateTime.Now;

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@Id", variable.Id },
                    { "@DeviceId", variable.DeviceId },
                    { "@Type", variable.Type },
                    { "@Area", variable.Area },
                    { "@Address", variable.Address },
                    { "@Name", variable.Name },
                    { "@Module", variable.Module },
                    { "@Unit", variable.Unit },
                    { "@Message", variable.Message },
                    { "@Value", variable.Value },
                    { "@Purpose", variable.Purpose },
                    { "@CreatedDate", variable.CreatedDate },
                    { "@CreatedBy", variable.CreatedBy },
                    { "@UpdatedDate", variable.UpdatedDate },
                    { "@UpdatedBy", variable.UpdatedBy }
                };
                return await ExecuteNonQueryAsync(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while updating variable with ID: {VariableId}", variable.Id);
                throw new RepositoryException($"An error occurred while updating variable with ID '{variable.Id}'. Please try again later.", ex);
            }
        }

        // Method to delete a variable
        public async Task<bool> Delete(int id)
        {
            try
            {
                string query = "DELETE FROM [dbo].[Variable] WHERE Id = @Id";
                Dictionary<string, object> parameters = new Dictionary<string, object> { { "@Id", id } };
                return await ExecuteNonQueryAsync(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while deleting variable with ID: {VariableId}", id);
                throw new RepositoryException($"An error occurred while deleting variable with ID '{id}'. Please try again later.", ex);
            }
        }

        // Method to retrieve filtered variables
        public async Task<List<VariableModel>> GetFiltered(params Func<VariableModel, bool>[] filters)
        {
            try
            {
                string whereClause = string.Join(" AND ", filters.Select(filter => $"({filter})"));
                string query = $"SELECT * FROM [dbo].[Variable] WHERE {whereClause}";
                DataTable dataTable = await ExecuteQueryAsync(query);
                return ConvertDataTableToList(dataTable);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving filtered variables.");
                throw new RepositoryException("An error occurred while retrieving filtered variables. Please try again later.", ex);
            }
        }

        // Method to count the number of variables
        public async Task<int> Count()
        {
            try
            {
                string query = "SELECT COUNT(*) FROM [dbo].[Variable]";
                object result = await ExecuteScalarAsync(query);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while counting variables.");
                throw new RepositoryException("An error occurred while counting variables. Please try again later.", ex);
            }
        }

        // Method to check if a variable with the given ID exists
        public async Task<bool> Exists(int id)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM [dbo].[Variable] WHERE Id = @Id";
                Dictionary<string, object> parameters = new Dictionary<string, object> { { "@Id", id } };
                int count = Convert.ToInt32(await ExecuteScalarAsync(query, parameters));
                return count > 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while checking if variable exists with ID: {VariableId}", id);
                throw new RepositoryException($"An error occurred while checking if variable exists with ID '{id}'. Please try again later.", ex);
            }
        }

        // Method to convert DataTable to a list of VariableModel objects
        private List<VariableModel> ConvertDataTableToList(DataTable dataTable)
        {
            List<VariableModel> variableList = new List<VariableModel>();

            foreach (DataRow row in dataTable.Rows)
            {
                variableList.Add(ConvertDataRowToVariableModel(row));
            }

            return variableList;
        }

        // Method to convert DataRow to a VariableModel object
        private VariableModel ConvertDataRowToVariableModel(DataRow row)
        {
            return new VariableModel
            {
                Id = Convert.ToInt32(row["Id"]),
                DeviceId = Convert.IsDBNull(row["DeviceId"]) ? (int?)null : Convert.ToInt32(row["DeviceId"]),
                Type = Convert.IsDBNull(row["Type"]) ? (int?)null : Convert.ToInt32(row["Type"]),
                Area = Convert.IsDBNull(row["Area"]) ? (int?)null : Convert.ToInt32(row["Area"]),
                Address = Convert.IsDBNull(row["Address"]) ? (int?)null : Convert.ToInt32(row["Address"]),
                Name = Convert.ToString(row["Name"]),
                Module = Convert.ToString(row["Module"]),
                Unit = Convert.ToString(row["Unit"]),
                Message = Convert.ToString(row["Message"]),
                Value = Convert.IsDBNull(row["Value"]) ? (float?)null : (float)Convert.ToDecimal(row["Value"]),
                Purpose = Convert.ToString(row["Purpose"]),
                CreatedDate = Convert.IsDBNull(row["CreatedDate"]) ? (DateTime?)null : Convert.ToDateTime(row["CreatedDate"]),
                CreatedBy = Convert.ToString(row["CreatedBy"]),
                UpdatedDate = Convert.IsDBNull(row["UpdatedDate"]) ? (DateTime?)null : Convert.ToDateTime(row["UpdatedDate"]),
                UpdatedBy = Convert.ToString(row["UpdatedBy"])
            };
        }

        // Method to convert DataTable to a single VariableModel object
        private VariableModel ConvertDataTableToSingleObject(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
            {
                return null;
            }

            DataRow row = dataTable.Rows[0];
            return ConvertDataRowToVariableModel(row);
        }
    }
}
