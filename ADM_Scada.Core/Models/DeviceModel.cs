using ADM_Scada.Cores.PlcService;

namespace ADM_Scada.Cores.Models
{
    public class DeviceModel
    {
        public int Id { get; set; }
        public string IpAddress { get; set; }
        public string Name { get; set; }
        public int Port { get; set; }
        public DeviceStatus Status { get; set; }
    }
}
