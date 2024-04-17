using System;
using System.ComponentModel;

namespace ADM_Scada.Core.Models
{
    // Variable class representing the variable table
    public class VariableModel
    {
        
        private string _value;
        public int Id { get; set; }
        public int? DeviceId { get; set; }
        public int? Type { get; set; }
        public int? Area { get; set; }
        public int? ByteAddress { get; set; }
        public int? BitAddress { get; set; }
        public string Name { get; set; }
        public string Module { get; set; }
        public string Unit { get; set; }
        public string Message { get; set; }
        public string Value { get => _value; set { _value = value; OnPropertyChanged(nameof(Value)); } }
        public string Purpose { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
