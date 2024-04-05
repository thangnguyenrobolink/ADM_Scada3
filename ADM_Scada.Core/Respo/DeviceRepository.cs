using ADM_Scada.Cores.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADM_Scada.Cores.Respo
{
    public class DeviceRepository : RepositoryBase, IDataRepository<DeviceModel>
    {
        public DeviceRepository(){ }

        public async Task<List<DeviceModel>> GetAll()
        {
            string query = "SELECT * FROM device";
            DataTable dataTable = await ExecuteQueryAsync(query);
            return ConvertDataTableToList(dataTable);
        }

        public async Task<DeviceModel> GetById(int deviceId)
        {
            string query = "SELECT * FROM device WHERE Id = @DeviceId";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@DeviceId", deviceId } };
            DataTable dataTable = await ExecuteQueryAsync(query, parameters);
            return ConvertDataTableToSingleObject(dataTable);
        }

        public async Task<int> Create(DeviceModel device)
        {
            string query = "INSERT INTO device (IpAddress, Name, Port) VALUES (@IpAddress, @Name, @Port)";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@IpAddress", device.IpAddress },
                { "@Name", device.Name },
                { "@Port", device.Port }
            };
            return await ExecuteNonQueryAsync(query, parameters);
        }

        public async Task<bool> Update(DeviceModel device)
        {
            string query = "UPDATE device SET IpAddress = @IpAddress, Name = @Name, Port = @Port WHERE Id = @DeviceId";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@DeviceId", device.Id },
                { "@IpAddress", device.IpAddress },
                { "@Name", device.Name },
                { "@Port", device.Port }
            };
            return await ExecuteNonQueryAsync(query, parameters) > 0;
        }

        public async Task<bool> Delete(int deviceId)
        {
            string query = "DELETE FROM device WHERE Id = @DeviceId";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@DeviceId", deviceId } };
            return await ExecuteNonQueryAsync(query, parameters) > 0;
        }

        public async Task<DeviceModel> Get(int deviceId)
        {
            string query = "SELECT * FROM device WHERE Id = @DeviceId";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@DeviceId", deviceId } };
            DataTable dataTable = await ExecuteQueryAsync(query, parameters);
            return ConvertDataTableToSingleObject(dataTable);
        }

        // Helper methods to convert DataTable to a list of objects
        private List<DeviceModel> ConvertDataTableToList(DataTable dataTable)
        {
            List<DeviceModel> deviceList = new List<DeviceModel>();

            foreach (DataRow row in dataTable.Rows)
            {
                DeviceModel device = new DeviceModel
                {
                    Id = Convert.ToInt32(row["Id"]),
                    IpAddress = Convert.ToString(row["IpAddress"]),
                    Name = Convert.ToString(row["Name"]),
                    Port = Convert.ToInt32(row["Port"])
                };

                deviceList.Add(device);
            }

            return deviceList;
        }

        private DeviceModel ConvertDataTableToSingleObject(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
            {
                return null;
            }

            DataRow row = dataTable.Rows[0];

            DeviceModel device = new DeviceModel
            {
                Id = Convert.ToInt32(row["Id"]),
                IpAddress = Convert.ToString(row["IpAddress"]),
                Name = Convert.ToString(row["Name"]),
                Port = Convert.ToInt32(row["Port"])
            };

            return device;
        }

        public Task<DeviceModel> GetByName(string s)
        {
            throw new NotImplementedException();
        }
    }
}
