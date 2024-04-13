using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using ADM_Scada.Core.Models;
using Serilog;

namespace ADM_Scada.Core.Respo
{
    public class DeviceRepository : RepositoryBase, IDataRepository<DeviceModel>
    {
        public async Task<List<DeviceModel>> GetAll()
        {
            try
            {
                string query = "SELECT * FROM [dbo].[device]";
                DataTable dataTable = await ExecuteQueryAsync(query);
                return ConvertDataTableToList(dataTable);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving all devices.");
                throw new RepositoryException("An error occurred while retrieving all devices. Please try again later.", ex);
            }
        }
        public async Task<DeviceModel> GetByName(string deviceName)
        {
            try
            {
                string query = "SELECT * FROM [dbo].[device] WHERE device_name = @DeviceName";
                Dictionary<string, object> parameters = new Dictionary<string, object> { { "@DeviceName", deviceName } };
                DataTable dataTable = await ExecuteQueryAsync(query, parameters);
                return ConvertDataTableToSingleObject(dataTable);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving device by name: {DeviceName}", deviceName);
                throw new RepositoryException($"An error occurred while retrieving device '{deviceName}'. Please try again later.", ex);
            }
        }

        public async Task<List<DeviceModel>> GetFiltered(params Func<DeviceModel, bool>[] filters)
        {
            try
            {
                string whereClause = string.Join(" AND ", Array.ConvertAll(filters, filter => $"({filter})"));
                string query = $"SELECT * FROM [dbo].[device] WHERE {whereClause}";
                DataTable dataTable = await ExecuteQueryAsync(query);
                return ConvertDataTableToList(dataTable);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving filtered devices.");
                throw new RepositoryException("An error occurred while retrieving filtered devices. Please try again later.", ex);
            }
        }

        public async Task<int> Count()
        {
            try
            {
                string query = "SELECT COUNT(*) FROM [dbo].[device]";
                object result = await ExecuteScalarAsync(query);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while counting devices.");
                throw new RepositoryException("An error occurred while counting devices. Please try again later.", ex);
            }
        }
        public async Task<DeviceModel> GetById(int id)
        {
            try
            {
                string query = "SELECT * FROM [dbo].[device] WHERE id = @Id";
                Dictionary<string, object> parameters = new Dictionary<string, object> { { "@Id", id } };
                DataTable dataTable = await ExecuteQueryAsync(query, parameters);
                return ConvertDataTableToSingleObject(dataTable);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving device by ID: {Id}", id);
                throw new RepositoryException($"An error occurred while retrieving device with ID '{id}'. Please try again later.", ex);
            }
        }

        public async Task<int> Create(DeviceModel entity)
        {
            try
            {
                string query = @"INSERT INTO [dbo].[device] (device_name, ip_address, port, created_date, created_by, updated_date, updated_by) 
                                 VALUES (@DeviceName, @IPAddress, @Port, @CreatedDate, @CreatedBy, @UpdatedDate, @UpdatedBy)";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@DeviceName", entity.DeviceName },
                    { "@IPAddress", entity.IpAddress },
                    { "@Port", entity.Port },
                    { "@CreatedDate", DateTime.Now },
                    { "@CreatedBy", entity.CreatedBy },
                    { "@UpdatedDate", DateTime.Now },
                    { "@UpdatedBy", entity.UpdatedBy }
                };

                return await ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while creating device.");
                throw new RepositoryException($"An error occurred while creating device. Please try again later.", ex);
            }
        }

        public async Task<bool> Update(DeviceModel entity)
        {
            try
            {
                string query = @"UPDATE [dbo].[device] 
                                 SET device_name = @DeviceName, ip_address = @IPAddress, port = @Port, 
                                     created_date = @CreatedDate, created_by = @CreatedBy, 
                                     updated_date = @UpdatedDate, updated_by = @UpdatedBy 
                                 WHERE id = @Id";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@Id", entity.Id },
                    { "@DeviceName", entity.DeviceName },
                    { "@IPAddress", entity.IpAddress },
                    { "@Port", entity.Port },
                    { "@CreatedDate", entity.CreatedDate },
                    { "@CreatedBy", entity.CreatedBy },
                    { "@UpdatedDate", DateTime.Now },
                    { "@UpdatedBy", entity.UpdatedBy }
                };

                return await ExecuteNonQueryAsync(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while updating device with ID: {Id}", entity.Id);
                throw new RepositoryException($"An error occurred while updating device with ID '{entity.Id}'. Please try again later.", ex);
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                string query = "DELETE FROM [dbo].[device] WHERE id = @Id";
                Dictionary<string, object> parameters = new Dictionary<string, object> { { "@Id", id } };
                return await ExecuteNonQueryAsync(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while deleting device with ID: {Id}", id);
                throw new RepositoryException($"An error occurred while deleting device with ID '{id}'. Please try again later.", ex);
            }
        }

        public async Task<bool> Exists(int id)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM [dbo].[device] WHERE id = @Id";
                Dictionary<string, object> parameters = new Dictionary<string, object> { { "@Id", id } };
                int count = Convert.ToInt32(await ExecuteScalarAsync(query, parameters));
                return count > 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while checking if device exists with ID: {Id}", id);
                throw new RepositoryException($"An error occurred while checking if device exists with ID '{id}'. Please try again later.", ex);
            }
        }

        // Additional methods can be added as needed

        private List<DeviceModel> ConvertDataTableToList(DataTable dataTable)
        {
            List<DeviceModel> deviceList = new List<DeviceModel>();

            foreach (DataRow row in dataTable.Rows)
            {
                deviceList.Add(ConvertDataRowToDeviceModel(row));
            }

            return deviceList;
        }

        private DeviceModel ConvertDataRowToDeviceModel(DataRow row)
        {
            return new DeviceModel
            {
                Id = Convert.ToInt32(row["id"]),
                DeviceName = Convert.ToString(row["device_name"]),
                IpAddress = Convert.ToString(row["ip_address"]),
                Port = Convert.ToString(row["port"]),
                CreatedDate = Convert.IsDBNull(row["created_date"]) ? (DateTime?)null : Convert.ToDateTime(row["created_date"]),
                CreatedBy = Convert.ToString(row["created_by"]),
                UpdatedDate = Convert.IsDBNull(row["updated_date"]) ? (DateTime?)null : Convert.ToDateTime(row["updated_date"]),
                UpdatedBy = Convert.ToString(row["updated_by"])
            };
        }

        private DeviceModel ConvertDataTableToSingleObject(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
            {
                return null;
            }
            DataRow row = dataTable.Rows[0];
            return ConvertDataRowToDeviceModel(row);
        }
    }
}
