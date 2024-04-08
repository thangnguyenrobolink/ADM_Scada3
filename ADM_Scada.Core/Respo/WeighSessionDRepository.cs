using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ADM_Scada.Core.Models;
using Serilog;

namespace ADM_Scada.Core.Respo
{
    public class WeighSessionDRepository : RepositoryBase, IDataRepository<WeighSessionDModel>
    {
        public WeighSessionDRepository() { }

        public async Task<List<WeighSessionDModel>> GetAll()
        {
            try
            {
                string query = "SELECT * FROM [dbo].[weigh_session_d]";
                DataTable dataTable = await ExecuteQueryAsync(query);
                return ConvertDataTableToList(dataTable);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving all weigh sessions.");
                throw new RepositoryException("An error occurred while retrieving all weigh sessions. Please try again later.", ex);
            }
        }

        public async Task<WeighSessionDModel> GetByName(string s)
        {
            try
            {
                // Since there is no unique name for weigh sessions, this method may not be applicable.
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving weigh session by name: {Name}", s);
                throw new RepositoryException($"An error occurred while retrieving weigh session '{s}'. Please try again later.", ex);
            }
        }

        public async Task<int> Create(WeighSessionDModel entity)
        {
            try
            {
                string query = @"INSERT INTO [dbo].[weigh_session_d] (p_id, created_date, current_weigh, barcode, prod_code, prod_fullname, prod_d365_code, production_date, start_time, end_time, qty_counted, qty_weighed, gap, shift_data_id, user_id, device_id, p_status_code, created_by, updated_date, updated_by) 
                                 VALUES (@PId, @CreatedDate, @CurrentWeigh, @Barcode, @ProdCode, @ProdFullname, @ProdD365Code, @ProductionDate, @StartTime, @EndTime, @QtyCounted, @QtyWeighed, @Gap, @ShiftDataId, @UserId, @DeviceId, @PStatusCode, @CreatedBy, @UpdatedDate, @UpdatedBy)";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@PId", entity.PId },
                    { "@CreatedDate", entity.CreatedDate },
                    { "@CurrentWeigh", entity.CurrentWeigh },
                    { "@Barcode", entity.Barcode },
                    { "@ProdCode", entity.ProdCode },
                    { "@ProdFullname", entity.ProdFullName },
                    { "@ProdD365Code", entity.ProdD365Code },
                    { "@ProductionDate", entity.ProductionDate },
                    { "@StartTime", entity.StartTime },
                    { "@EndTime", entity.EndTime },
                    { "@QtyCounted", entity.QtyCounted },
                    { "@QtyWeighed", entity.QtyWeighed },
                    { "@Gap", entity.Gap },
                    { "@ShiftDataId", entity.ShiftDataId },
                    { "@UserId", entity.UserId },
                    { "@DeviceId", entity.DeviceId },
                    { "@PStatusCode", entity.PStatusCode },
                    { "@CreatedBy", entity.CreatedBy },
                    { "@UpdatedDate", entity.UpdatedDate },
                    { "@UpdatedBy", entity.UpdatedBy }
                };
                return await ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while creating weigh session.");
                throw new RepositoryException($"An error occurred while creating weigh session. Please try again later.", ex);
            }
        }

        public async Task<bool> Update(WeighSessionDModel entity)
        {
            try
            {
                string query = @"UPDATE [dbo].[weigh_session_d] 
                                 SET created_date = @CreatedDate, current_weigh = @CurrentWeigh, barcode = @Barcode, prod_code = @ProdCode, 
                                     prod_fullname = @ProdFullname, prod_d365_code = @ProdD365Code, production_date = @ProductionDate, 
                                     start_time = @StartTime, end_time = @EndTime, qty_counted = @QtyCounted, qty_weighed = @QtyWeighed, 
                                     gap = @Gap, shift_data_id = @ShiftDataId, user_id = @UserId, device_id = @DeviceId, 
                                     p_status_code = @PStatusCode, created_by = @CreatedBy, updated_date = @UpdatedDate, 
                                     updated_by = @UpdatedBy 
                                 WHERE id = @Id";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@Id", entity.Id },
                    { "@CreatedDate", entity.CreatedDate },
                    { "@CurrentWeigh", entity.CurrentWeigh },
                    { "@Barcode", entity.Barcode },
                    { "@ProdCode", entity.ProdCode },
                    { "@ProdFullname", entity.ProdFullName },
                    { "@ProdD365Code", entity.ProdD365Code },
                    { "@ProductionDate", entity.ProductionDate },
                    { "@StartTime", entity.StartTime },
                    { "@EndTime", entity.EndTime },
                    { "@QtyCounted", entity.QtyCounted },
                    { "@QtyWeighed", entity.QtyWeighed },
                    { "@Gap", entity.Gap },
                    { "@ShiftDataId", entity.ShiftDataId },
                    { "@UserId", entity.UserId },
                    { "@DeviceId", entity.DeviceId },
                    { "@PStatusCode", entity.PStatusCode },
                    { "@CreatedBy", entity.CreatedBy },
                    { "@UpdatedDate", entity.UpdatedDate },
                    { "@UpdatedBy", entity.UpdatedBy }
                };
                return await ExecuteNonQueryAsync(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while updating weigh session with ID: {Id}", entity.Id);
                throw new RepositoryException($"An error occurred while updating weigh session with ID '{entity.Id}'. Please try again later.", ex);
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                string query = "DELETE FROM [dbo].[weigh_session_d] WHERE id = @Id";
                Dictionary<string, object> parameters = new Dictionary<string, object> { { "@Id", id } };
                return await ExecuteNonQueryAsync(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while deleting weigh session with ID: {Id}", id);
                throw new RepositoryException($"An error occurred while deleting weigh session with ID '{id}'. Please try again later.", ex);
            }
        }

        public async Task<WeighSessionDModel> GetById(int id)
        {
            try
            {
                string query = "SELECT * FROM [dbo].[weigh_session_d] WHERE id = @Id";
                Dictionary<string, object> parameters = new Dictionary<string, object> { { "@Id", id } };
                DataTable dataTable = await ExecuteQueryAsync(query, parameters);
                return ConvertDataTableToSingleObject(dataTable);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving weigh session by ID: {Id}", id);
                throw new RepositoryException($"An error occurred while retrieving weigh session with ID '{id}'. Please try again later.", ex);
            }
        }

        public async Task<List<WeighSessionDModel>> GetFiltered(params Func<WeighSessionDModel, bool>[] filters)
        {
            try
            {
                // Constructing a WHERE clause from filters and executing the query
                string whereClause = string.Join(" AND ", filters.Select(filter => $"({filter})"));
                string query = $"SELECT * FROM [dbo].[weigh_session_d] WHERE {whereClause}";
                DataTable dataTable = await ExecuteQueryAsync(query);
                return ConvertDataTableToList(dataTable);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving filtered weigh sessions.");
                throw new RepositoryException("An error occurred while retrieving filtered weigh sessions. Please try again later.", ex);
            }
        }

        public async Task<int> Count()
        {
            try
            {
                string query = "SELECT COUNT(*) FROM [dbo].[weigh_session_d]";
                object result = await ExecuteScalarAsync(query);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while counting weigh sessions.");
                throw new RepositoryException("An error occurred while counting weigh sessions. Please try again later.", ex);
            }
        }

        public async Task<bool> Exists(int id)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM [dbo].[weigh_session_d] WHERE id = @Id";
                Dictionary<string, object> parameters = new Dictionary<string, object> { { "@Id", id } };
                int count = Convert.ToInt32(await ExecuteScalarAsync(query, parameters));
                return count > 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while checking if weigh session exists with ID: {Id}", id);
                throw new RepositoryException($"An error occurred while checking if weigh session exists with ID '{id}'. Please try again later.", ex);
            }
        }

        private List<WeighSessionDModel> ConvertDataTableToList(DataTable dataTable)
        {
            List<WeighSessionDModel> weighSessionList = new List<WeighSessionDModel>();

            foreach (DataRow row in dataTable.Rows)
            {
                weighSessionList.Add(ConvertDataRowToWeighSessionModel(row));
            }

            return weighSessionList;
        }

        private WeighSessionDModel ConvertDataRowToWeighSessionModel(DataRow row)
        {
            return new WeighSessionDModel
            {
                Id = Convert.ToInt32(row["id"]),
                PId = row["p_id"] != DBNull.Value ? Convert.ToInt32(row["p_id"]) : (int?)null,
                CreatedDate = row["created_date"] != DBNull.Value ? Convert.ToDateTime(row["created_date"]) : (DateTime?)null,
                CurrentWeigh = (row["current_weigh"] != DBNull.Value ? Convert.ToDecimal(row["current_weigh"]) : (decimal?)null),
                Barcode = row["barcode"] != DBNull.Value ? Convert.ToString(row["barcode"]) : null,
                ProdCode = row["prod_code"] != DBNull.Value ? Convert.ToString(row["prod_code"]) : null,
                ProdFullName = row["prod_fullname"] != DBNull.Value ? Convert.ToString(row["prod_fullname"]) : null,
                ProdD365Code = row["prod_d365_code"] != DBNull.Value ? Convert.ToString(row["prod_d365_code"]) : null,
                ProductionDate = row["production_date"] != DBNull.Value ? Convert.ToDateTime(row["production_date"]) : (DateTime?)null,
                StartTime = row["start_time"] != DBNull.Value ? Convert.ToDateTime(row["start_time"]) : (DateTime?)null,
                EndTime = row["end_time"] != DBNull.Value ? Convert.ToDateTime(row["end_time"]) : (DateTime?)null,
                QtyCounted = row["qty_counted"] != DBNull.Value ? Convert.ToInt32(row["qty_counted"]) : (int?)null,
                QtyWeighed = (row["qty_weighed"] != DBNull.Value ? Convert.ToDecimal(row["qty_weighed"]) : (decimal?)null),
                Gap = (row["gap"] != DBNull.Value ? Convert.ToDecimal(row["gap"]) : (decimal?)null),
                ShiftDataId = row["shift_data_id"] != DBNull.Value ? Convert.ToInt32(row["shift_data_id"]) : (int?)null,
                UserId = row["user_id"] != DBNull.Value ? Convert.ToInt32(row["user_id"]) : (int?)null,
                DeviceId = row["device_id"] != DBNull.Value ? Convert.ToString(row["device_id"]) : null,
                PStatusCode = row["p_status_code"] != DBNull.Value ? Convert.ToString(row["p_status_code"]) : null,
                CreatedBy = row["created_by"] != DBNull.Value ? Convert.ToString(row["created_by"]) : null,
                UpdatedDate = row["updated_date"] != DBNull.Value ? Convert.ToDateTime(row["updated_date"]) : (DateTime?)null,
                UpdatedBy = row["updated_by"] != DBNull.Value ? Convert.ToString(row["updated_by"]) : null
            };
        }

        private WeighSessionDModel ConvertDataTableToSingleObject(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
            {
                return null;
            }

            DataRow row = dataTable.Rows[0];
            return ConvertDataRowToWeighSessionModel(row);
        }
    }
}
