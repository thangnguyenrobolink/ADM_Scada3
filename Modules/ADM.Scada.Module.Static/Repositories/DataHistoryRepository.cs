using ADM.Scada.Modules.Static.Models;
using ADM_Scada.Cores.Respo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ADM.Scada.Modules.Static.Repositories
{
    public class DataHistoryRepository : RepositoryBase, IDataRepository<DataHistoryModel>
    {
        public DataHistoryRepository() { }

        public async Task<List<DataHistoryModel>> GetAll()
        {
            string query = "SELECT * FROM datahistory";
            DataTable dataTable = await ExecuteQueryAsync(query);
            return ConvertDataTableToList(dataTable);
        }

        public async Task<DataHistoryModel> GetById(int dataHistoryId)
        {
            string query = "SELECT * FROM datahistory WHERE Id = @DataHistoryId";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@DataHistoryId", dataHistoryId } };
            DataTable dataTable = await ExecuteQueryAsync(query, parameters);
            return ConvertDataTableToSingleObject(dataTable);
        }

        public async Task<int> Create(DataHistoryModel dataHistory)
        {
            string query = "INSERT INTO datahistory (DataName, Value) " +
                           "VALUES (@DataName, @Value)";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@DataName", dataHistory.DataName },
                { "@Value", dataHistory.Value }
            };
            return await ExecuteNonQueryAsync(query, parameters);
        }

        public async Task<bool> Update(DataHistoryModel dataHistory)
        {
            string query = "UPDATE datahistory SET DataName = @DataName, Value = @Value " +
                           "WHERE Id = @DataHistoryId";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@DataHistoryId", dataHistory.Id },
                { "@DataName", dataHistory.DataName },
                { "@Value", dataHistory.Value }
            };
            return await ExecuteNonQueryAsync(query, parameters) > 0;
        }

        public async Task<bool> Delete(int dataHistoryId)
        {
            string query = "DELETE FROM datahistory WHERE Id = @DataHistoryId";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@DataHistoryId", dataHistoryId } };
            return await ExecuteNonQueryAsync(query, parameters) > 0;
        }

        public async Task<DataHistoryModel> Get(int dataHistoryId)
        {
            string query = "SELECT * FROM datahistory WHERE Id = @DataHistoryId";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@DataHistoryId", dataHistoryId } };
            DataTable dataTable = await ExecuteQueryAsync(query, parameters);
            return ConvertDataTableToSingleObject(dataTable);
        }

        // Helper methods to convert DataTable to a list of objects
        private List<DataHistoryModel> ConvertDataTableToList(DataTable dataTable)
        {
            List<DataHistoryModel> dataHistoryList = new List<DataHistoryModel>();

            foreach (DataRow row in dataTable.Rows)
            {
                DataHistoryModel dataHistory = new DataHistoryModel
                {
                    Id = Convert.ToInt32(row["Id"]),
                    DataName = Convert.ToString(row["DataName"]),
                    Value = Convert.ToString(row["Value"]),
                    TimeStamp = Convert.ToDateTime(row["TimeStamp"])
                };

                dataHistoryList.Add(dataHistory);
            }

            return dataHistoryList;
        }

        private DataHistoryModel ConvertDataTableToSingleObject(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
            {
                return null;
            }

            DataRow row = dataTable.Rows[0];

            DataHistoryModel dataHistory = new DataHistoryModel
            {
                Id = Convert.ToInt32(row["Id"]),
                DataName = Convert.ToString(row["DataName"]),
                Value = Convert.ToString(row["Value"]),
                TimeStamp = Convert.ToDateTime(row["TimeStamp"])
            };

            return dataHistory;
        }

        public Task<DataHistoryModel> GetByName(string s)
        {
            throw new NotImplementedException();
        }
    }
}
