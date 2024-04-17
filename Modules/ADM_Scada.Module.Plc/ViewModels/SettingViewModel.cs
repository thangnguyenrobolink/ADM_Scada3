using ADM_Scada.Core.Models;
using ADM_Scada.Cores.PlcService;
using ADM_Scada.Cores.PubEvent;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ADM_Scada.Modules.Plc.ViewModels
{
    public class SettingViewModel : BindableBase
    {
        // Event and service 
        #region
        private IEventAggregator ea;
        private IPLCCommunicationService pLCCommunicationService;
        #endregion

        // UI Variable
        #region
        private ObservableCollection<VariableModel> variables;
        private ObservableCollection<VariableModel> generalVariables;
        private ObservableCollection<VariableModel> infeedVariables;
        private ObservableCollection<VariableModel> outfeedVariables;
        private ObservableCollection<VariableModel> scaleVariables;
        private ObservableCollection<DeviceModel> devices;
        private ObservableCollection<string> status;
        private VariableModel selectedvar;
        private DeviceModel selectedDevice;
        public VariableModel SelectedVariable { get => selectedvar; set => SetProperty(ref selectedvar, value); }
        public DeviceModel SelectedDevice { get => selectedDevice; set => SetProperty(ref selectedDevice, value); }
        public ObservableCollection<VariableModel> Variables { get => variables; set => SetProperty(ref variables, value); }
        public ObservableCollection<VariableModel> GeneralVariables { get => generalVariables; set => SetProperty(ref generalVariables, value); }
        public ObservableCollection<VariableModel> InfeedVariables { get => infeedVariables; set => SetProperty(ref infeedVariables, value); }
        public ObservableCollection<VariableModel> OutfeedVariables { get => outfeedVariables; set => SetProperty(ref outfeedVariables, value); }
        public ObservableCollection<VariableModel> ScaleVariables { get => scaleVariables; set => SetProperty(ref scaleVariables, value); }
        public ObservableCollection<DeviceModel> Devices { get => devices; set => SetProperty(ref devices, value); }
        public ObservableCollection<string> Status { get => status; set => SetProperty(ref status, value); }
        #endregion

        // UI command
        #region
        public DelegateCommand<VariableModel> SetValueCommand { get; private set; }
        public DelegateCommand<VariableModel> CommentCommand { get; private set; }
        public DelegateCommand<DeviceModel> ConnectCommand { get; private set; }
        public DelegateCommand<DeviceModel> DisConnectCommand { get; private set; }
        public DelegateCommand<DeviceModel> UpdateDeviceCommand { get; private set; }
        #endregion

        // Command Execute 
        #region
        private void SetValue(VariableModel selectedVar)
        {
            _ = Task.Run(async () => await pLCCommunicationService.SetPLCValue(selectedVar));
        }
        private void Comment(VariableModel selectedVar)
        {
            if (selectedVar != null)
            {
                _ = pLCCommunicationService.UpdateVariable(selectedVar);
            }
        }
        private bool CanSetValue(VariableModel selectedVar)
        {
            // CanExecute condition for DeleteCommand
            return true;//selectedVar.Value != -1;
        }
        private void ConnectDevice(DeviceModel deviceModel)
        {
            _ = pLCCommunicationService.ConnectDevice(deviceModel);
        }
        private void DisConnectDevice(DeviceModel deviceModel)
        {
            pLCCommunicationService.DisConnectDevice(deviceModel);
        }
        private void UpdateDevice(DeviceModel deviceModel)
        {
            _ = pLCCommunicationService.UpdatePlcDevice(deviceModel);
        }
        #endregion
        public SettingViewModel(IPLCCommunicationService _pLCCommunicationService, IEventAggregator ea)
        {
            this.ea = ea;
            pLCCommunicationService = _pLCCommunicationService;
            _ = this.ea.GetEvent<PlcStatusChangeEvent>().Subscribe(UpdatePlcStatus);
            Variables = pLCCommunicationService.GetAllVariables();
            GeneralVariables = pLCCommunicationService.GetVariablesByModule("General");
            InfeedVariables = pLCCommunicationService.GetVariablesByModule("Infeed");
            OutfeedVariables = pLCCommunicationService.GetVariablesByModule("Outfeed");
            ScaleVariables = pLCCommunicationService.GetVariablesByModule("Scale");
            Devices = pLCCommunicationService.GetAllDevices();
            Status = new ObservableCollection<string>();
            foreach (DeviceModel device in Devices)
            {
                string status_in = device.Status.ToString();
                Status.Add(status_in);
            }
            CommentCommand = new DelegateCommand<VariableModel>(Comment);
            SetValueCommand = new DelegateCommand<VariableModel>(SetValue);
            ConnectCommand = new DelegateCommand<DeviceModel>(ConnectDevice);
            DisConnectCommand = new DelegateCommand<DeviceModel>(DisConnectDevice);
            UpdateDeviceCommand = new DelegateCommand<DeviceModel>(UpdateDevice);
        }

        private void UpdatePlcStatus(DeviceModel obj)
        {
            int index = Devices.IndexOf(Devices.FirstOrDefault(x => x.Id == obj.Id));
            if (index != -1)
            {
                Devices[index] = obj; // Update the item at the specified index
                Status[index] = obj.Status.ToString();
                RaisePropertyChanged(nameof(Status)); 
                RaisePropertyChanged(nameof(Devices)); // Notify the UI that the collection has changed
            }
        }
    }
}
