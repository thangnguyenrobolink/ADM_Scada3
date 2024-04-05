using ADM_Scada.Cores.PlcService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
