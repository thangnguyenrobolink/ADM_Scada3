using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ADM_Scada.Core.Models;
using Serilog;

namespace ADM_Scada.Core.Respo
{
    public class DeviceRepository : RepositoryBase, IDataRepository<DeviceModel>
    {
        public DeviceRepository() { }

        // Method to retrieve all devices
        public async Task<List<DeviceModel>> GetAll()
        {
            try
            {
                string query = "SELECT * FROM [dbo].[Device]";
                DataTable dataTable = await ExecuteQueryAsync(query);
                return ConvertDataTableToList(dataTable);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving all devices.");
                throw new RepositoryException("An error occurred while retrieving all devices. Please try again later.", ex);
            }
        }
        // Method to retrieve a device by name
        public async Task<DeviceModel> GetByName(string deviceName)
        {
            try
            {
                string query = "SELECT * FROM [dbo].[Device] WHERE DeviceName = @DeviceName";
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

        // Method to retrieve a device by ID
        public async Task<DeviceModel> GetById(int id)
        {
            try
            {
                string query = "SELECT * FROM [dbo].[Device] WHERE Id = @Id";
                Dictionary<string, object> parameters = new Dictionary<string, object> { { "@Id", id } };
                DataTable dataTable = await ExecuteQueryAsync(query, parameters);
                return ConvertDataTableToSingleObject(dataTable);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving device by ID: {DeviceId}", id);
                throw new RepositoryException($"An error occurred while retrieving device with ID '{id}'. Please try again later.", ex);
            }
        }

        // Method to create a new device
        public async Task<int> Create(DeviceModel device)
        {
            try
            {
                string query = @"INSERT INTO [dbo].[Device] (DeviceName, IpAddress, Port, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy) 
                                 VALUES (@DeviceName, @IpAddress, @Port, @CreatedDate, @CreatedBy, @UpdatedDate, @UpdatedBy)";

                // Set CreatedDate and UpdatedDate
                device.CreatedDate = DateTime.Now;
                device.UpdatedDate = DateTime.Now;

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@DeviceName", device.DeviceName },
                    { "@IpAddress", device.IpAddress },
                    { "@Port", device.Port },
                    { "@CreatedDate", device.CreatedDate },
                    { "@CreatedBy", device.CreatedBy },
                    { "@UpdatedDate", device.UpdatedDate },
                    { "@UpdatedBy", device.UpdatedBy }
                };
                return await ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while creating device: {DeviceName}", device.DeviceName);
                throw new RepositoryException($"An error occurred while creating device '{device.DeviceName}'. Please try again later.", ex);
            }
        }

        // Method to update a device
        public async Task<bool> Update(DeviceModel device)
        {
            try
            {
                string query = @"UPDATE [dbo].[Device] 
                         SET DeviceName = @DeviceName, IpAddress = @IpAddress, Port = @Port, 
                             CreatedDate = @CreatedDate, CreatedBy = @CreatedBy, 
                             UpdatedDate = @UpdatedDate, UpdatedBy = @UpdatedBy 
                         WHERE Id = @Id";

                // Set UpdatedDate
                device.UpdatedDate = DateTime.Now;

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@Id", device.Id },
                    { "@DeviceName", device.DeviceName },
                    { "@IpAddress", device.IpAddress },
                    { "@Port", device.Port },
                    { "@CreatedDate", device.CreatedDate },
                    { "@CreatedBy", device.CreatedBy },
                    { "@UpdatedDate", device.UpdatedDate },
                    { "@UpdatedBy", device.UpdatedBy }
                };
                return await ExecuteNonQueryAsync(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while updating device with ID: {DeviceId}", device.Id);
                throw new RepositoryException($"An error occurred while updating device with ID '{device.Id}'. Please try again later.", ex);
            }
        }

        // Method to delete a device
        public async Task<bool> Delete(int id)
        {
            try
            {
                string query = "DELETE FROM [dbo].[Device] WHERE Id = @Id";
                Dictionary<string, object> parameters = new Dictionary<string, object> { { "@Id", id } };
                return await ExecuteNonQueryAsync(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while deleting device with ID: {DeviceId}", id);
                throw new RepositoryException($"An error occurred while deleting device with ID '{id}'. Please try again later.", ex);
            }
        }

        // Method to retrieve filtered devices
        public async Task<List<DeviceModel>> GetFiltered(params Func<DeviceModel, bool>[] filters)
        {
            try
            {
                string whereClause = string.Join(" AND ", filters.Select(filter => $"({filter})"));
                string query = $"SELECT * FROM [dbo].[Device] WHERE {whereClause}";
                DataTable dataTable = await ExecuteQueryAsync(query);
                return ConvertDataTableToList(dataTable);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving filtered devices.");
                throw new RepositoryException("An error occurred while retrieving filtered devices. Please try again later.", ex);
            }
        }

        // Method to count the number of devices
        public async Task<int> Count()
        {
            try
            {
                string query = "SELECT COUNT(*) FROM [dbo].[Device]";
                object result = await ExecuteScalarAsync(query);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while counting devices.");
                throw new RepositoryException("An error occurred while counting devices. Please try again later.", ex);
            }
        }

        // Method to check if a device with the given ID exists
        public async Task<bool> Exists(int id)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM [dbo].[Device] WHERE Id = @Id";
                Dictionary<string, object> parameters = new Dictionary<string, object> { { "@Id", id } };
                int count = Convert.ToInt32(await ExecuteScalarAsync(query, parameters));
                return count > 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while checking if device exists with ID: {DeviceId}", id);
                throw new RepositoryException($"An error occurred while checking if device exists with ID '{id}'. Please try again later.", ex);
            }
        }

        // Method to convert DataTable to a list of DeviceModel objectsÁDASDASD
        private List<DeviceModel> ConvertDataTableToList(DataTable dataTable)
        {
            List<DeviceModel> deviceList = new List<DeviceModel>();

            foreach (DataRow row in dataTable.Rows)
            {
                deviceList.Add(ConvertDataRowToDeviceModel(row));
            }

            return deviceList;
        }

        // Method to convert DataRow to a DeviceModel object
        private DeviceModel ConvertDataRowToDeviceModel(DataRow row)
        {
            return new DeviceModel
            {
                Id = Convert.ToInt32(row["Id"]),
                DeviceName = Convert.ToString(row["DeviceName"]),
                IpAddress = Convert.ToString(row["IpAddress"]),
                Port = Convert.ToString(row["Port"]),
                CreatedDate = Convert.IsDBNull(row["CreatedDate"]) ? (DateTime?)null : Convert.ToDateTime(row["CreatedDate"]),
                CreatedBy = Convert.ToString(row["CreatedBy"]),
                UpdatedDate = Convert.IsDBNull(row["UpdatedDate"]) ? (DateTime?)null : Convert.ToDateTime(row["UpdatedDate"]),
                UpdatedBy = Convert.ToString(row["UpdatedBy"])
            };
        }

        // Method to convert DataTable to a single DeviceModel object
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
