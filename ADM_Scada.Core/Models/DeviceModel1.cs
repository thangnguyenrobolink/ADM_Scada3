using ADM_Scada.Cores.PlcService;
using S7.Net;
using S7.Net.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ADM_Scada.Core.Models
{
    // Device class representing the device table
    public class DeviceModel
    {
        private DeviceStatus status;

        public int Id { get; set; }
        public string DeviceName { get; set; }
        public string IpAddress { get; set; }
        public string Port { get; set; }
        public DeviceStatus Status { get => status; set { status = value; OnPropertyChanged(nameof(Status)); } }
        public System.DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public Plc plc { get; set; }
        public List<DataItem> DataItems { get; set; }
        public DeviceModel()
        {
            Id = 0;
            DeviceName = "new device";
            IpAddress = "127.0.0.1";
            Port = "9100";
            Status = DeviceStatus.NotPresent;
            CreatedDate = System.DateTime.Now;
            CreatedBy = "Guest";
            plc = new Plc(CpuType.S71200, IpAddress, 0, 1);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
