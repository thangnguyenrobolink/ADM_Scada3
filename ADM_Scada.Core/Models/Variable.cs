using System.ComponentModel;

namespace ADM_Scada.Cores.Models
{
    public class VariableModel : INotifyPropertyChanged
    {
        private double _value;
        private string message;

        public int Id { get; set; }
        public int DeviceId { get; set; }
        public int Type { get; set; }
        public int Area { get; set; }
        public int Address { get; set; }
        public string Name { get; set; }
        public string Module { get; set; }
        public string Unit { get; set; }
        public string Message { get => message; set { message = value; OnPropertyChanged(nameof(Message)); } }
        public double Value { get => _value; set { _value = value; OnPropertyChanged(nameof(Value)); }}
        public string Purpose { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
