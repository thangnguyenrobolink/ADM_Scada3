using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ADM_Scada.Core.Models;
using Serilog;

namespace ADM_Scada.Core.Respo
{
    public class WeighSessionRepository : RepositoryBase, IDataRepository<WeighSessionModel>
    {
        public WeighSessionRepository() { }

        public async Task<List<WeighSessionModel>> GetAll()
        {
            try
            {
                string query = "SELECT * FROM [dbo].[weigh_session]";
                DataTable dataTable = await ExecuteQueryAsync(query);
                return ConvertDataTableToList(dataTable);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving all weigh sessions.");
                throw new RepositoryException("An error occurred while retrieving all weigh sessions. Please try again later.", ex);
            }
        }

        public async Task<WeighSessionModel> GetByName(string sessionCode)
        {
            try
            {
                string query = "SELECT * FROM [dbo].[weigh_session] WHERE session_code = @SessionCode";
                Dictionary<string, object> parameters = new Dictionary<string, object> { { "@SessionCode", sessionCode } };
                DataTable dataTable = await ExecuteQueryAsync(query, parameters);
                return ConvertDataTableToSingleObject(dataTable);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving weigh session by session code: {SessionCode}", sessionCode);
                throw new RepositoryException($"An error occurred while retrieving weigh session by session code '{sessionCode}'. Please try again later.", ex);
            }
        }

        public async Task<int> Create(WeighSessionModel weighSession)
        {
            try
            {
                string query = @"INSERT INTO [dbo].[weigh_session] (session_code, start_time, end_time, cust_id, 
                                cust_name, cust_address, boat_id, so_number, qty_counted, qty_order_weigh, 
                                qty_tare_weigh, qty_weighed, gap, document_no, shift_data_id, 
                                user_id, device_code, status_code, created_date, created_by, updated_date, updated_by) 
                                 VALUES (@SessionCode, @StartTime, @EndTime, @CustId, @CustName, @CustAddress, @BoatId, 
                                @SoNumber, @QtyCounted, @QtyOrderWeigh, @QtyTareWeigh, @QtyWeighed, @Gap, @DocumentNo, 
                                @ShiftDataId, @UserId, @DeviceCode, @StatusCode, @CreatedDate, @CreatedBy, @UpdatedDate, @UpdatedBy)";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@SessionCode", weighSession.SessionCode },
                    { "@StartTime", weighSession.StartTime },
                    { "@EndTime", weighSession.EndTime },
                    { "@CustId", weighSession.CustId },
                    { "@CustName", weighSession.CustName },
                    { "@CustAddress", weighSession.CustAddress },
                    { "@BoatId", weighSession.BoatId },
                    { "@SoNumber", weighSession.SoNumber },
                    { "@QtyCounted", weighSession.QtyCounted },
                    { "@QtyOrderWeigh", weighSession.QtyOrderWeigh },
                    { "@QtyTareWeigh", weighSession.QtyTareWeigh },
                    { "@QtyWeighed", weighSession.QtyWeighed },
                    { "@Gap", weighSession.Gap },
                    { "@DocumentNo", weighSession.DocumentNo },
                    { "@ShiftDataId", weighSession.ShiftDataId },
                    { "@UserId", weighSession.UserId },
                    { "@DeviceCode", weighSession.DeviceCode },
                    { "@StatusCode", weighSession.StatusCode },
                    { "@CreatedDate", weighSession.CreatedDate },
                    { "@CreatedBy", weighSession.CreatedBy },
                    { "@UpdatedDate", weighSession.UpdatedDate },
                    { "@UpdatedBy", weighSession.UpdatedBy }
                };
                return await ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while creating weigh session with session code: {SessionCode}", weighSession.SessionCode);
                throw new RepositoryException($"An error occurred while creating weigh session with session code '{weighSession.SessionCode}'. Please try again later.", ex);
            }
        }

        public async Task<bool> Update(WeighSessionModel weighSession)
        {
            try
            {
                string query = @"UPDATE [dbo].[weigh_session] 
                         SET start_time = @StartTime, end_time = @EndTime, cust_id = @CustId, 
                             cust_name = @CustName, cust_address = @CustAddress, boat_id = @BoatId, 
                             so_number = @SoNumber, qty_counted = @QtyCounted, qty_order_weigh = @QtyOrderWeigh, 
                             qty_tare_weigh = @QtyTareWeigh, qty_weighed = @QtyWeighed, gap = @Gap, 
                             document_no = @DocumentNo, shift_data_id = @ShiftDataId, user_id = @UserId, 
                             device_code = @DeviceCode, status_code = @StatusCode, created_date = @CreatedDate, 
                             created_by = @CreatedBy, updated_date = @UpdatedDate, updated_by = @UpdatedBy 
                         WHERE id = @Id";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@Id", weighSession.Id },
                    { "@StartTime", weighSession.StartTime },
                    { "@EndTime", weighSession.EndTime },
                    { "@CustId", weighSession.CustId },
                    { "@CustName", weighSession.CustName },
                    { "@CustAddress", weighSession.CustAddress },
                    { "@BoatId", weighSession.BoatId },
                    { "@SoNumber", weighSession.SoNumber },
                    { "@QtyCounted", weighSession.QtyCounted },
                    { "@QtyOrderWeigh", weighSession.QtyOrderWeigh },
                    { "@QtyTareWeigh", weighSession.QtyTareWeigh },
                    { "@QtyWeighed", weighSession.QtyWeighed },
                    { "@Gap", weighSession.Gap },
                    { "@DocumentNo", weighSession.DocumentNo },
                    { "@ShiftDataId", weighSession.ShiftDataId },
                    { "@UserId", weighSession.UserId },
                    { "@DeviceCode", weighSession.DeviceCode },
                    { "@StatusCode", weighSession.StatusCode },
                    { "@CreatedDate", weighSession.CreatedDate },
                    { "@CreatedBy", weighSession.CreatedBy },
                    { "@UpdatedDate", weighSession.UpdatedDate },
                    { "@UpdatedBy", weighSession.UpdatedBy }
                };
                return await ExecuteNonQueryAsync(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while updating weigh session with ID: {WeighSessionId}", weighSession.Id);
                throw new RepositoryException($"An error occurred while updating weigh session with ID '{weighSession.Id}'. Please try again later.", ex);
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                string query = "DELETE FROM [dbo].[weigh_session] WHERE id = @Id";
                Dictionary<string, object> parameters = new Dictionary<string, object> { { "@Id", id } };
                return await ExecuteNonQueryAsync(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while deleting weigh session with ID: {WeighSessionId}", id);
                throw new RepositoryException($"An error occurred while deleting weigh session with ID '{id}'. Please try again later.", ex);
            }
        }

        public async Task<WeighSessionModel> GetById(int id)
        {
            try
            {
                string query = "SELECT * FROM [dbo].[weigh_session] WHERE id = @Id";
                Dictionary<string, object> parameters = new Dictionary<string, object> { { "@Id", id } };
                DataTable dataTable = await ExecuteQueryAsync(query, parameters);
                return ConvertDataTableToSingleObject(dataTable);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving weigh session by ID: {WeighSessionId}", id);
                throw new RepositoryException($"An error occurred while retrieving weigh session by ID '{id}'. Please try again later.", ex);
            }
        }

        public async Task<List<WeighSessionModel>> GetFiltered(params Func<WeighSessionModel, bool>[] filters)
        {
            try
            {
                string query = "SELECT * FROM [dbo].[weigh_session]";
                DataTable dataTable = await ExecuteQueryAsync(query);
                List<WeighSessionModel> weighSessions = ConvertDataTableToList(dataTable);

                foreach (var filter in filters)
                {
                    weighSessions = weighSessions.Where(filter).ToList();
                }

                return weighSessions;
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
                string query = "SELECT COUNT(*) FROM [dbo].[weigh_session]";
                return Convert.ToInt32(await ExecuteScalarAsync(query));
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
                string query = "SELECT COUNT(*) FROM [dbo].[weigh_session] WHERE id = @Id";
                Dictionary<string, object> parameters = new Dictionary<string, object> { { "@Id", id } };
                int count = Convert.ToInt32(await ExecuteScalarAsync(query, parameters));
                return count > 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while checking if weigh session exists with ID: {WeighSessionId}", id);
                throw new RepositoryException($"An error occurred while checking if weigh session exists with ID '{id}'. Please try again later.", ex);
            }
        }

        public async Task<WeighSessionModel> GetLast()
        {
            try
            {
                string query = "SELECT TOP 1 * FROM [dbo].[weigh_session] ORDER BY start_time DESC";
                DataTable dataTable = await ExecuteQueryAsync(query);
                return ConvertDataTableToSingleObject(dataTable);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving the last weigh session.");
                throw new RepositoryException("An error occurred while retrieving the last weigh session. Please try again later.", ex);
            }
        }

        private List<WeighSessionModel> ConvertDataTableToList(DataTable dataTable)
        {
            List<WeighSessionModel> weighSessions = new List<WeighSessionModel>();

            foreach (DataRow row in dataTable.Rows)
            {
                weighSessions.Add(ConvertDataRowToWeighSessionModel(row));
            }

            return weighSessions;
        }

        private WeighSessionModel ConvertDataRowToWeighSessionModel(DataRow row)
        {
            return new WeighSessionModel
            {
                Id = Convert.ToInt32(row["id"]),
                SessionCode = Convert.ToString(row["session_code"]),
                StartTime = Convert.IsDBNull(row["start_time"]) ? (DateTime?)null : Convert.ToDateTime(row["start_time"]),
                EndTime = Convert.IsDBNull(row["end_time"]) ? (DateTime?)null : Convert.ToDateTime(row["end_time"]),
                CustId = Convert.IsDBNull(row["cust_id"]) ? (int?)null : Convert.ToInt32(row["cust_id"]),
                CustName = Convert.ToString(row["cust_name"]),
                CustAddress = Convert.ToString(row["cust_address"]),
                BoatId =  Convert.ToString(row["boat_id"]),
                SoNumber = Convert.ToString(row["so_number"]),
                QtyCounted = Convert.IsDBNull(row["qty_counted"]) ? (int?)null : Convert.ToInt32(row["qty_counted"]),
                QtyOrderWeigh = Convert.IsDBNull(row["qty_order_weigh"]) ? (decimal?)null : Convert.ToDecimal(row["qty_order_weigh"]),
                QtyTareWeigh = Convert.IsDBNull(row["qty_tare_weigh"]) ? (decimal?)null : Convert.ToDecimal(row["qty_tare_weigh"]),
                QtyWeighed = Convert.IsDBNull(row["qty_weighed"]) ? (decimal?)null : Convert.ToDecimal(row["qty_weighed"]),
                Gap = Convert.IsDBNull(row["gap"]) ? (decimal?)null : Convert.ToDecimal(row["gap"]),
                DocumentNo = Convert.ToString(row["document_no"]),
                ShiftDataId = Convert.IsDBNull(row["shift_data_id"]) ? (int?)null : Convert.ToInt32(row["shift_data_id"]),
                UserId = Convert.IsDBNull(row["user_id"]) ? (int?)null : Convert.ToInt32(row["user_id"]),
                DeviceCode = Convert.ToString(row["device_code"]),
                StatusCode = Convert.ToString(row["status_code"]),
                CreatedDate = Convert.IsDBNull(row["created_date"]) ? (DateTime?)null : Convert.ToDateTime(row["created_date"]),
                CreatedBy = Convert.ToString(row["created_by"]),
                UpdatedDate = Convert.IsDBNull(row["updated_date"]) ? (DateTime?)null : Convert.ToDateTime(row["updated_date"]),
                UpdatedBy = Convert.ToString(row["updated_by"])
            };
        }

        private WeighSessionModel ConvertDataTableToSingleObject(DataTable dataTable)
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
