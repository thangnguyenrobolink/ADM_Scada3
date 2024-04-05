using ADM_Scada.Cores.Models;
using ADM_Scada.Cores.PlcService;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ADM_Scada.Modules.Plc.ViewModels
{
    public class SettingViewModel : BindableBase
    {
        private IPLCCommunicationService pLCCommunicationService;
        private ObservableCollection<VariableModel> variables;
        private ObservableCollection<VariableModel> generalVariables;
        private ObservableCollection<VariableModel> infeedVariables;
        private ObservableCollection<VariableModel> outfeedVariables;
        private ObservableCollection<VariableModel> scaleVariables;
        private ObservableCollection<DeviceModel> devices;
        private VariableModel selectedvar;
        public VariableModel SelectedVariable { get => selectedvar; set => SetProperty(ref selectedvar, value); }
        public ObservableCollection<VariableModel> Variables { get => variables; set => SetProperty(ref variables, value);}
        public ObservableCollection<VariableModel> GeneralVariables { get => generalVariables; set => SetProperty(ref generalVariables, value);}
        public ObservableCollection<VariableModel> InfeedVariables { get => infeedVariables; set => SetProperty(ref infeedVariables, value);}
        public ObservableCollection<VariableModel> OutfeedVariables { get => outfeedVariables; set => SetProperty(ref outfeedVariables, value);}
        public ObservableCollection<VariableModel> ScaleVariables { get => scaleVariables; set => SetProperty(ref scaleVariables, value);}
        public ObservableCollection<DeviceModel> Devices { get => devices; set => SetProperty(ref devices, value);}
        public DelegateCommand<VariableModel> SetValueCommand { get; private set; }
        public DelegateCommand<VariableModel> CommentCommand { get; private set; }
        public DelegateCommand<DeviceModel> ConnectCommand { get; private set; }
        public DelegateCommand<DeviceModel> UpdateDeviceCommand { get; private set; }
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
            return selectedVar.Value != -1;
        }
        private void ConnectDevice(DeviceModel deviceModel)
        {
            _ = pLCCommunicationService.ConnectDevice(deviceModel);
        } 
        private void UpdateDevice(DeviceModel deviceModel)
        {
            _ = pLCCommunicationService.UpdatePlcDevice(deviceModel);
        }
        public SettingViewModel(IPLCCommunicationService _pLCCommunicationService)
        {
            pLCCommunicationService = _pLCCommunicationService;
            Variables = pLCCommunicationService.GetAllVariables();
            GeneralVariables = pLCCommunicationService.GetVariablesByModule("General");
            InfeedVariables = pLCCommunicationService.GetVariablesByModule("Infeed");
            OutfeedVariables = pLCCommunicationService.GetVariablesByModule("Outfeed");
            ScaleVariables = pLCCommunicationService.GetVariablesByModule("Scale");
            Devices = pLCCommunicationService.GetAllDevices();
            CommentCommand = new DelegateCommand<VariableModel>(Comment);
            SetValueCommand = new DelegateCommand<VariableModel>(SetValue);
            ConnectCommand = new DelegateCommand<DeviceModel>(ConnectDevice);
            UpdateDeviceCommand = new DelegateCommand<DeviceModel>(UpdateDevice);
        }
    }
}
