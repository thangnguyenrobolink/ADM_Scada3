using ADM_Scada.Cores.PlcService;
using System;

namespace ADM_Scada.Core.Models
{
    // Device class representing the device table
    public class DeviceModel
    {
        public int Id { get; set; }
        public string DeviceName { get; set; }
        public string IpAddress { get; set; }
        public string Port { get; set; }
        public DeviceStatus Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
