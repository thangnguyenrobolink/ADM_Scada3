using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ADM_Scada.Core.Models;
using Serilog;

namespace ADM_Scada.Core.Respo
{
    public class ProdShiftDataRepository : RepositoryBase, IDataRepository<ProdShiftDataModel>
    {
        public ProdShiftDataRepository() { }

        public async Task<List<ProdShiftDataModel>> GetAll()
        {
            try
            {
                string query = "SELECT * FROM [dbo].[prod_shift_data]";
                DataTable dataTable = await ExecuteQueryAsync(query);
                return ConvertDataTableToList(dataTable);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving all production shift data.");
                throw new RepositoryException("An error occurred while retrieving all production shift data. Please try again later.", ex);
            }
        }

        public async Task<ProdShiftDataModel> GetByName(string workOrderNo)
        {
            try
            {
                string query = "SELECT * FROM [dbo].[prod_shift_data] WHERE work_order_no = @WorkOrderNo";
                Dictionary<string, object> parameters = new Dictionary<string, object> { { "@WorkOrderNo", workOrderNo } };
                DataTable dataTable = await ExecuteQueryAsync(query, parameters);
                return ConvertDataTableToSingleObject(dataTable);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving production shift data by work order no: {WorkOrderNo}", workOrderNo);
                throw new RepositoryException($"An error occurred while retrieving production shift data by work order no '{workOrderNo}'. Please try again later.", ex);
            }
        }

        public async Task<int> Create(ProdShiftDataModel prodShiftData)
        {
            try
            {
                string query = @"INSERT INTO [dbo].[prod_shift_data] (work_order_no, prod_code, lot_no, production_date, 
                                expiry_date, user_name, shift_no, cust_code, devide_code, qty_to_pack, whole_uom, 
                                created_by, created_date, updated_by, updated_date) 
                                 VALUES (@WorkOrderNo, @ProdCode, @LotNo, @ProductionDate, @ExpiryDate, 
                                @UserName, @ShiftNo, @CustCode, @DevideCode, @QtyToPack, @WholeUom, 
                                @CreatedBy, @CreatedDate, @UpdatedBy, @UpdatedDate)";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@WorkOrderNo", prodShiftData.WorkOrderNo },
                    { "@ProdCode", prodShiftData.ProdCode },
                    { "@LotNo", prodShiftData.LotNo },
                    { "@ProductionDate", prodShiftData.ProductionDate },
                    { "@ExpiryDate", prodShiftData.ExpiryDate },
                    { "@UserName", prodShiftData.UserName },
                    { "@ShiftNo", prodShiftData.ShiftNo },
                    { "@CustCode", prodShiftData.CustCode },
                    { "@DevideCode", prodShiftData.DevideCode },
                    { "@QtyToPack", prodShiftData.QtyToPack },
                    { "@WholeUom", prodShiftData.WholeUom },
                    { "@CreatedBy", prodShiftData.CreatedBy },
                    { "@CreatedDate", prodShiftData.CreatedDate },
                    { "@UpdatedBy", prodShiftData.UpdatedBy },
                    { "@UpdatedDate", prodShiftData.UpdatedDate }
                };
                return await ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while creating production shift data with work order no: {WorkOrderNo}", prodShiftData.WorkOrderNo);
                throw new RepositoryException($"An error occurred while creating production shift data with work order no '{prodShiftData.WorkOrderNo}'. Please try again later.", ex);
            }
        }

        public async Task<bool> Update(ProdShiftDataModel prodShiftData)
        {
            try
            {
                string query = @"UPDATE [dbo].[prod_shift_data] 
                         SET prod_code = @ProdCode, lot_no = @LotNo, production_date = @ProductionDate, 
                             expiry_date = @ExpiryDate, user_name = @UserName, shift_no = @ShiftNo, 
                             cust_code = @CustCode, devide_code = @DevideCode, qty_to_pack = @QtyToPack, 
                             whole_uom = @WholeUom, created_by = @CreatedBy, created_date = @CreatedDate, 
                             updated_by = @UpdatedBy, updated_date = @UpdatedDate 
                         WHERE id = @Id";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@Id", prodShiftData.Id },
                    { "@ProdCode", prodShiftData.ProdCode },
                    { "@LotNo", prodShiftData.LotNo },
                    { "@ProductionDate", prodShiftData.ProductionDate },
                    { "@ExpiryDate", prodShiftData.ExpiryDate },
                    { "@UserName", prodShiftData.UserName },
                    { "@ShiftNo", prodShiftData.ShiftNo },
                    { "@CustCode", prodShiftData.CustCode },
                    { "@DevideCode", prodShiftData.DevideCode },
                    { "@QtyToPack", prodShiftData.QtyToPack },
                    { "@WholeUom", prodShiftData.WholeUom },
                    { "@CreatedBy", prodShiftData.CreatedBy },
                    { "@CreatedDate", prodShiftData.CreatedDate },
                    { "@UpdatedBy", prodShiftData.UpdatedBy },
                    { "@UpdatedDate", prodShiftData.UpdatedDate }
                };
                return await ExecuteNonQueryAsync(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while updating production shift data with ID: {ProdShiftDataId}", prodShiftData.Id);
                throw new RepositoryException($"An error occurred while updating production shift data with ID '{prodShiftData.Id}'. Please try again later.", ex);
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                string query = "DELETE FROM [dbo].[prod_shift_data] WHERE id = @Id";
                Dictionary<string, object> parameters = new Dictionary<string, object> { { "@Id", id } };
                return await ExecuteNonQueryAsync(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while deleting production shift data with ID: {ProdShiftDataId}", id);
                throw new RepositoryException($"An error occurred while deleting production shift data with ID '{id}'. Please try again later.", ex);
            }
        }

        public async Task<ProdShiftDataModel> GetById(int id)
        {
            try
            {
                string query = "SELECT * FROM [dbo].[prod_shift_data] WHERE id = @Id";
                Dictionary<string, object> parameters = new Dictionary<string, object> { { "@Id", id } };
                DataTable dataTable = await ExecuteQueryAsync(query, parameters);
                return ConvertDataTableToSingleObject(dataTable);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving production shift data by ID: {ProdShiftDataId}", id);
                throw new RepositoryException($"An error occurred while retrieving production shift data by ID '{id}'. Please try again later.", ex);
            }
        }

        public async Task<List<ProdShiftDataModel>> GetFiltered(params Func<ProdShiftDataModel, bool>[] filters)
        {
            try
            {
                string query = "SELECT * FROM [dbo].[prod_shift_data]";
                DataTable dataTable = await ExecuteQueryAsync(query);
                List<ProdShiftDataModel> prodShiftDataList = ConvertDataTableToList(dataTable);

                foreach (var filter in filters)
                {
                    prodShiftDataList = prodShiftDataList.Where(filter).ToList();
                }

                return prodShiftDataList;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving filtered production shift data.");
                throw new RepositoryException("An error occurred while retrieving filtered production shift data. Please try again later.", ex);
            }
        }

        public async Task<int> Count()
        {
            try
            {
                string query = "SELECT COUNT(*) FROM [dbo].[prod_shift_data]";
                return Convert.ToInt32(await ExecuteScalarAsync(query));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while counting production shift data.");
                throw new RepositoryException("An error occurred while counting production shift data. Please try again later.", ex);
            }
        }

        public async Task<bool> Exists(int id)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM [dbo].[prod_shift_data] WHERE id = @Id";
                Dictionary<string, object> parameters = new Dictionary<string, object> { { "@Id", id } };
                int count = Convert.ToInt32(await ExecuteScalarAsync(query, parameters));
                return count > 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while checking if production shift data exists with ID: {ProdShiftDataId}", id);
                throw new RepositoryException($"An error occurred while checking if production shift data exists with ID '{id}'. Please try again later.", ex);
            }
        }

        private List<ProdShiftDataModel> ConvertDataTableToList(DataTable dataTable)
        {
            List<ProdShiftDataModel> prodShiftDataList = new List<ProdShiftDataModel>();

            foreach (DataRow row in dataTable.Rows)
            {
                prodShiftDataList.Add(ConvertDataRowToProdShiftDataModel(row));
            }

            return prodShiftDataList;
        }

        private ProdShiftDataModel ConvertDataRowToProdShiftDataModel(DataRow row)
        {
            return new ProdShiftDataModel
            {
                Id = Convert.ToInt32(row["id"]),
                WorkOrderNo = Convert.ToString(row["work_order_no"]),
                ProdCode = Convert.ToString(row["prod_code"]),
                LotNo = Convert.ToString(row["lot_no"]),
                ProductionDate = Convert.IsDBNull(row["production_date"]) ? (DateTime?)null : Convert.ToDateTime(row["production_date"]),
                ExpiryDate = Convert.IsDBNull(row["expiry_date"]) ? (DateTime?)null : Convert.ToDateTime(row["expiry_date"]),
                UserName = Convert.ToString(row["user_name"]),
                ShiftNo = Convert.ToString(row["shift_no"]),
                CustCode = Convert.ToString(row["cust_code"]),
                DevideCode = Convert.ToString(row["devide_code"]),
                QtyToPack = Convert.IsDBNull(row["qty_to_pack"]) ? (decimal?)null : Convert.ToDecimal(row["qty_to_pack"]),
                WholeUom = Convert.ToString(row["whole_uom"]),
                CreatedBy = Convert.ToString(row["created_by"]),
                CreatedDate = Convert.IsDBNull(row["created_date"]) ? (DateTime?)null : Convert.ToDateTime(row["created_date"]),
                UpdatedBy = Convert.ToString(row["updated_by"]),
                UpdatedDate = Convert.IsDBNull(row["updated_date"]) ? (DateTime?)null : Convert.ToDateTime(row["updated_date"])
            };
        }

        private ProdShiftDataModel ConvertDataTableToSingleObject(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
            {
                return null;
            }

            DataRow row = dataTable.Rows[0];
            return ConvertDataRowToProdShiftDataModel(row);
        }
    }
}
