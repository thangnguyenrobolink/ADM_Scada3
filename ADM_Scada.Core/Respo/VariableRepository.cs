using ADM_Scada.Cores.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ADM_Scada.Cores.Respo
{
    public class VariableRepository : RepositoryBase, IDataRepository<VariableModel>
    {
        public VariableRepository() : base()
        {
        }
        public async Task<List<VariableModel>> GetAll()
        {
            string query = "SELECT * FROM variable";
            DataTable dataTable = await ExecuteQueryAsync(query);
            return ConvertDataTableToList(dataTable);
        }

        public async Task<VariableModel> GetById(int variableId)
        {
            string query = "SELECT * FROM variable WHERE Id = @VariableId";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@VariableId", variableId } };
            DataTable dataTable = await ExecuteQueryAsync(query, parameters);
            return ConvertDataTableToSingleObject(dataTable);
        }

        public async Task<int> Create(VariableModel variable)
        {
            string query = "INSERT INTO variable (DeviceId, Type, Area, Address, Name, Module, Unit, Message, Purpose) " +
                           "VALUES (@DeviceId, @Type, @Area, @Address, @Name, @Module, @Unit, @Message, @Purpose)";
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
                //{ "@Value", variable.Value },
                { "@Purpose", variable.Purpose }
            };
            return await ExecuteNonQueryAsync(query, parameters);
        }

        public async Task<bool> Update(VariableModel variable)
        {
            string query = "UPDATE variable SET DeviceId = @DeviceId, Type = @Type, Area = @Area, Address = @Address, Name = @Name, " +
                           "Module = @Module, Unit = @Unit, Message = @Message, Purpose = @Purpose " +
                           "WHERE Id = @VariableId";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@VariableId", variable.Id },
                { "@DeviceId", variable.DeviceId },
                { "@Type", variable.Type },
                { "@Area", variable.Area },
                { "@Address", variable.Address },
                { "@Name", variable.Name },
                { "@Module", variable.Module },
                { "@Unit", variable.Unit },
                { "@Message", variable.Message },
               // { "@Value", variable.Value },
                { "@Purpose", variable.Purpose }
            };
            return await ExecuteNonQueryAsync(query, parameters) > 0;
        }

        public async Task<bool> Delete(int variableId)
        {
            string query = "DELETE FROM variable WHERE Id = @VariableId";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@VariableId", variableId } };
            return await ExecuteNonQueryAsync(query, parameters) > 0;
        }

        public async Task<VariableModel> Get(int variableId)
        {
            string query = "SELECT * FROM variable WHERE Id = @VariableId";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@VariableId", variableId } };
            DataTable dataTable = await ExecuteQueryAsync(query, parameters);
            return ConvertDataTableToSingleObject(dataTable);
        }

        // Helper methods to convert DataTable to a list of objects
        private List<VariableModel> ConvertDataTableToList(DataTable dataTable)
        {
            List<VariableModel> variableList = new List<VariableModel>();

            foreach (DataRow row in dataTable.Rows)
            {
                VariableModel variable = new VariableModel
                {
                    Id = Convert.ToInt32(row["Id"]),
                    DeviceId = Convert.ToInt32(row["DeviceId"]),
                    Type = Convert.ToInt32(row["Type"]),
                    Area = Convert.ToInt32(row["Area"]),
                    Address = Convert.ToInt32(row["Address"]),
                    Name = Convert.ToString(row["Name"]),
                    Module = Convert.ToString(row["Module"]),
                    Unit = Convert.ToString(row["Unit"]),
                    Message = Convert.ToString(row["Message"]),
                    Value = 0,
                    // Value = (float)Convert.ToDouble(row["Value"]),
                    Purpose = Convert.ToString(row["Purpose"])
                };

                variableList.Add(variable);
            }

            return variableList;
        }

        private VariableModel ConvertDataTableToSingleObject(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
            {
                return null;
            }

            DataRow row = dataTable.Rows[0];

            VariableModel variable = new VariableModel
            {
                Id = Convert.ToInt32(row["Id"]),
                DeviceId = Convert.ToInt32(row["DeviceId"]),
                Type = Convert.ToInt32(row["Type"]),
                Area = Convert.ToInt32(row["Area"]),
                Address = Convert.ToInt32(row["Address"]),
                Name = Convert.ToString(row["Name"]),
                Module = Convert.ToString(row["Module"]),
                Unit = Convert.ToString(row["Unit"]),
                Message = Convert.ToString(row["Message"]),
                Value = 0,
                //Value = (float)Convert.ToDouble(row["Value"]),
                Purpose = Convert.ToString(row["Purpose"])
            };

            return variable;
        }

        public async Task<VariableModel> GetByName(string name)
        {
            try
            {
                string query = "SELECT * FROM variable WHERE Name = @Name";
                Dictionary<string, object> parameters = new Dictionary<string, object> { { "@Name", name } };
                DataTable dataTable = await ExecuteQueryAsync(query, parameters);
                return ConvertDataTableToSingleObject(dataTable);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in GetByName: {ex.Message}");
                throw; // Re-throw the exception for higher-level handling
            }
        }
    }

}
