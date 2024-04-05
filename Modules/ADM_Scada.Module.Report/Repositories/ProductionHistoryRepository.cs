using ADM_Scada.Cores.Models;
using ADM_Scada.Cores.Respo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ADM_Scada.Modules.Report.Repositories
{
    public class ProductionHistoryRepository : RepositoryBase, IDataRepository<ProductionHistoryModel>
    {
        public ProductionHistoryRepository() { }

        public async Task<List<ProductionHistoryModel>> GetAll()
        {
            string query = "SELECT * FROM productionhistory";
            DataTable dataTable = await ExecuteQueryAsync(query);
            return ConvertDataTableToList(dataTable);
        }

        public async Task<ProductionHistoryModel> GetById(int productionHistoryId)
        {
            string query = "SELECT * FROM productionhistory WHERE Id = @ProductionHistoryId";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@ProductionHistoryId", productionHistoryId } };
            DataTable dataTable = await ExecuteQueryAsync(query, parameters);
            return ConvertDataTableToSingleObject(dataTable);
        }

        public async Task<int> Create(ProductionHistoryModel productionHistory)
        {
            string query = "INSERT INTO productionhistory (ProductId, Weight, CustomerId, UserId) " +
                           "VALUES (@ProductId, @Weight, @CustomerId, @UserId)";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@ProductId", productionHistory.ProductId },
                { "@Weight", productionHistory.Weight },
                { "@CustomerId", productionHistory.CustomerId },
                { "@UserId", productionHistory.UserId }
            };
            return await ExecuteNonQueryAsync(query, parameters);
        }

        public async Task<bool> Update(ProductionHistoryModel productionHistory)
        {
            string query = "UPDATE productionhistory SET ProductId = @ProductId, Weight = @Weight, " +
                           "CustomerId = @CustomerId, UserId = @UserId " + "WHERE Id = @ProductionHistoryId";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@ProductionHistoryId", productionHistory.Id },
                { "@ProductId", productionHistory.ProductId },
                { "@Weight", productionHistory.Weight },
                { "@CustomerId", productionHistory.CustomerId },
                { "@UserId", productionHistory.UserId }
            };
            return await ExecuteNonQueryAsync(query, parameters) > 0;
        }

        public async Task<bool> Delete(int productionHistoryId)
        {
            string query = "DELETE FROM productionhistory WHERE Id = @ProductionHistoryId";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@ProductionHistoryId", productionHistoryId } };
            return await ExecuteNonQueryAsync(query, parameters) > 0;
        }

        public async Task<ProductionHistoryModel> Get(int productionHistoryId)
        {
            string query = "SELECT * FROM productionhistory WHERE Id = @ProductionHistoryId";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@ProductionHistoryId", productionHistoryId } };
            DataTable dataTable = await ExecuteQueryAsync(query, parameters);
            return ConvertDataTableToSingleObject(dataTable);
        }

        // Helper methods to convert DataTable to a list of objects
        private List<ProductionHistoryModel> ConvertDataTableToList(DataTable dataTable)
        {
            List<ProductionHistoryModel> productionHistoryList = new List<ProductionHistoryModel>();

            foreach (DataRow row in dataTable.Rows)
            {
                ProductionHistoryModel productionHistory = new ProductionHistoryModel
                {
                    Id = Convert.ToInt32(row["Id"]),
                    ProductId = Convert.ToInt32(row["ProductId"]),
                    Weight = Convert.ToSingle(row["Weight"]),
                    CustomerId = row["CustomerId"] != DBNull.Value ? Convert.ToInt32(row["CustomerId"]) : (int?)null,
                    UserId = Convert.ToInt32(row["UserId"]),
                    TimeStamp = Convert.ToDateTime(row["TimeStamp"])
                };

                productionHistoryList.Add(productionHistory);
            }

            return productionHistoryList;
        }

        private ProductionHistoryModel ConvertDataTableToSingleObject(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
            {
                return null;
            }

            DataRow row = dataTable.Rows[0];

            ProductionHistoryModel productionHistory = new ProductionHistoryModel
            {
                Id = Convert.ToInt32(row["Id"]),
                ProductId = Convert.ToInt32(row["ProductId"]),
                Weight = Convert.ToSingle(row["Weight"]),
                CustomerId = row["CustomerId"] != DBNull.Value ? Convert.ToInt32(row["CustomerId"]) : (int?)null,
                UserId = Convert.ToInt32(row["UserId"]),
                TimeStamp = Convert.ToDateTime(row["TimeStamp"])
            };

            return productionHistory;
        }

        public Task<ProductionHistoryModel> GetByName(string s)
        {
            throw new NotImplementedException();
        }
    }
}
