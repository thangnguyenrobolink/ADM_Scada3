using ADM_Scada.Core.Models;
using ADM_Scada.Core.Respo;
using ADM_Scada.Cores.PubEvent;
using Prism.Events;
using S7.Net;
using S7.Net.Types;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace ADM_Scada.Cores.PlcService
{
    public enum DeviceStatus
    {
        NotPresent = 0,
        DisConnect = 1,
        Connecting = 2,
        Connected = 3
    }
    //public class DeviceChangedEventArgs : EventArgs
    //{
    //    public DeviceModel Device { get; }

    //    public DeviceChangedEventArgs(DeviceModel device)
    //    {
    //        Device = device;
    //    }
    //}

    //public class VariableChangedEventArgs : EventArgs
    //{
    //    public VariableModel Variable { get; }

    //    public VariableChangedEventArgs(VariableModel variable)
    //    {
    //        Variable = variable;
    //    }
    //}
    public class PlcCommunicateService : IPLCCommunicationService
    {
        // device variable
        #region
        private readonly DeviceRepository deviceRepository;
        private static ObservableCollection<DeviceModel> devices;
        #endregion
        /// Plc variable 
         #region
        private readonly VariableRepository variableRepository;
        private ObservableCollection<VariableModel> variables;
        #endregion

        #region
        private readonly DispatcherTimer timer;
        private readonly DispatcherTimer updateTimer;
        private IEventAggregator ea;

        public ObservableCollection<DeviceModel> Devices { get => devices; set => devices = value; }
        #endregion
        /// ///Init 
        public PlcCommunicateService(IEventAggregator ea, VariableRepository variableRepository, DeviceRepository deviceRepository)
        {
            this.variableRepository = variableRepository;
            this.deviceRepository = deviceRepository;
            this.ea = ea;
            _ = FetchInitialDataAsync();

            //updateTimer = new Timer(null, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));

            //Update device connection Status
            #region
            UpdatePlcStatus(null);
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5)
            };
            timer.Tick += (sender, e) => UpdatePlcStatus(null);
            timer.Start();
            #endregion

            //Update Variable by time
            #region
            updateTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(0.5)
            };
            updateTimer.Tick += (sender, e) => UpdatePlcStatus(null);
            //updateTimer.Start();
            #endregion
        }
        // Init data
        #region
        private void HandlePlcError(int index, PlcException ex)
        {
            switch (ex.ErrorCode)
            {
                case ErrorCode.ConnectionError:
                    devices[index].Status = DeviceStatus.DisConnect;
                    break;
                case ErrorCode.NoError:
                    devices[index].Status = DeviceStatus.Connected;
                    break;
                case ErrorCode.WrongCPU_Type:
                    devices[index].Status = DeviceStatus.NotPresent;
                    break;
                case ErrorCode.IPAddressNotAvailable:
                    devices[index].Status = DeviceStatus.NotPresent;
                    break;
                case ErrorCode.WrongVarFormat:
                    devices[index].Status = DeviceStatus.NotPresent;
                    break;
                case ErrorCode.WrongNumberReceivedBytes:
                    break;
                case ErrorCode.SendData:
                    break;
                case ErrorCode.ReadData:
                    break;
                case ErrorCode.WriteData:
                    break;
                default:
                    devices[index].Status = DeviceStatus.NotPresent;
                    break;
            }
            ea.GetEvent<PlcStatusChangeEvent>().Publish(Devices[index]);
            Log.Error(ex, $"PLC {devices[index].DeviceName} error");
            ShowErrorMessage($"An error occurred while working with plc {devices[index].DeviceName} . Please try again later.");
        }
        private void HandleDataFetchError(string dataType, Exception ex)
        {
            Log.Error(ex, $"An error occurred while loading {dataType}");
            ShowErrorMessage($"An error occurred while loading {dataType}. Please try again later.");
        }
        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private async Task FetchVariableAsync()
        {
            try
            {
                variables = new ObservableCollection<VariableModel>(await variableRepository.GetAll());
            }
            catch (Exception ex)
            {
                HandleDataFetchError("All Variables", ex);
            }
        }
        private async Task FetchDeviceAsync()
        {
            ///// Get database
            try
            {
                Devices = new ObservableCollection<DeviceModel>(await deviceRepository.GetAll());
                UpdateDataItemsByPlc();
            }
            catch (Exception ex)
            {
                HandleDataFetchError("All Devices", ex);
            }
            ////
            for (int i = 0; i<Devices.Count; i++)
            {
                Devices[i].plc = new Plc(CpuType.S71200, Devices[i].IpAddress, 0, 1);
                Devices[i].Status = DeviceStatus.Connecting;
                try
                {
                    await Devices[i].plc.OpenAsync();
                    Devices[i].Status = GetPlcStatusById(Devices[i].Id);
                }
                catch (PlcException a)
                {
                    switch (a.ErrorCode)
                    {
                        case ErrorCode.ConnectionError:
                            Devices[i].Status = DeviceStatus.DisConnect;
                            break;
                        default:
                            Devices[i].Status = DeviceStatus.NotPresent;
                            break;
                    }
                }
            }
        }

        private async Task FetchInitialDataAsync()
        {
            try
            {
                await FetchVariableAsync();
            }
            catch
            { }
            try
            {
                await FetchDeviceAsync();
            }
            catch { }
           
        }
        #endregion

        // Device Connect and status
        #region
        public async Task<bool> ConnectDevice(DeviceModel plcdevice)
        {
            int index = Devices.IndexOf(Devices.FirstOrDefault(x => x.Id == plcdevice.Id));
            if (index == -1) return false;
            try
            {

                Devices[index].Status = DeviceStatus.Connecting;
                ea.GetEvent<PlcStatusChangeEvent>().Publish(Devices[index]);
                await Devices[index].plc.OpenAsync();
                Devices[index].Status = DeviceStatus.Connected;
                ea.GetEvent<PlcStatusChangeEvent>().Publish(Devices[index]);
            }
            catch (PlcException a)
            {
                HandlePlcError(index, a);
                return false;
            }
            return true;
        }
        public bool DisConnectDevice(DeviceModel plcdevice)
        {
            int index = devices.IndexOf(devices.FirstOrDefault(x => x.Id == plcdevice.Id));
            if (index == -1) return false;
            try
            {
                devices[index].plc.Close();
                devices[index].Status = DeviceStatus.DisConnect;
            }
            catch (PlcException a)
            {
                HandlePlcError(index, a);
                return false;
            }
            return true;
        }
        private void UpdatePlcStatus(object state)
        {
            if (devices != null)
            {
                //Update plc status
                for (int index = 0 ; index < Devices.Count(); index++)
                { 
                    var newStatus = GetPlcStatusById(Devices[index].Id);
                    if (Devices[index].Status != newStatus)
                    {
                        Devices[index].Status = newStatus;
                        ea.GetEvent<PlcStatusChangeEvent>().Publish(Devices[index]);
                    }
                }
            }
        }
        public DeviceStatus GetPlcStatusById(int id)
        {
            int index = devices.IndexOf(devices.FirstOrDefault(x => x.Id == id));
            try
            {
                if (devices[index].plc.IsConnected)
                {
                    return DeviceStatus.Connected;
                }
                else
                {
                    return DeviceStatus.DisConnect;
                }

            }
            catch (PlcException a)
            {
                switch (a.ErrorCode)
                {
                    case ErrorCode.ConnectionError:
                        return DeviceStatus.DisConnect;
                    default:
                        return DeviceStatus.NotPresent;
                }
            }
        }
        #endregion

        /// Get Devices database
        #region        
        public ObservableCollection<DeviceModel> GetAllDevices()
        {
            try
            {
                return devices ?? new ObservableCollection<DeviceModel>(Devices);
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show($"Error while getting all Devices: {ex.Message}");
                return null;
            }
        }
        public async Task<bool> UpdatePlcDevice(DeviceModel plcdevice)
        {
            // Update to database
            try
            {
                int index = Devices.IndexOf(Devices.FirstOrDefault(x => x.Id == plcdevice.Id));
                if (index == -1) return false;
                _ = await deviceRepository.Update(plcdevice);
                Devices[index].plc.Close();
                Devices[index] = plcdevice;
                Devices[index].Status = DeviceStatus.NotPresent;
                ea.GetEvent<PlcStatusChangeEvent>().Publish(Devices[index]);
                Devices[index].plc = new Plc(CpuType.S71200, Devices[index].IpAddress, 0, 1);
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show(ex.Message);
                return false;
            }

            //_ = await ConnectDevice(plcdevice);
            return true;
        }
        #endregion

        /// Variables Database
        #region
        public ObservableCollection<VariableModel> GetAllVariables()
        {
            try
            {
                return new ObservableCollection<VariableModel>(variables);
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show($"Error while getting all variables: {ex.Message}");
                return null;
            }
        }
        public VariableModel GetPLCValue(VariableModel a)
        {
            return variables.FirstOrDefault(x => x.Id == a.Id) ?? a;
        }
        public VariableModel GetVariableById(int id)
        {
            VariableModel variable;
            try
            {
                variable = variables.FirstOrDefault(v => v.Id == id);
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show($"Error while getting variable by ID: {ex.Message}");
                return null;
            }
            return variable;
        }
        public VariableModel GetVariableByName(string name)
        {
            try
            {
                return variables.FirstOrDefault(v => v.Name == name);
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show($"Error while getting variable by name: {ex.Message}");
                return null;
            }
        }
        public ObservableCollection<VariableModel> GetVariablesByModule(string module)
        {
            try
            {
                var variablesInModule = new ObservableCollection<VariableModel>(
                    variables.Where(v => v.Module == module)
                );

                return variablesInModule;
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show($"Error while getting variables by module: {ex.Message}");
                return null;
            }
        }
        public ObservableCollection<VariableModel> GetVariablesByPlc(DeviceModel device)
        {
            try
            {
                var variablesInPlc = new ObservableCollection<VariableModel>(
                    variables.Where(v => v.DeviceId == device.Id)
                );

                return variablesInPlc;
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show($"Error while getting variables by Plc: {ex.Message}");
                return null;
            }
        }
        public ObservableCollection<VariableModel> GetVariablesByPurpose(string purpose)
        {
            try
            {
                var variablesWithPurpose = new ObservableCollection<VariableModel>(
                    variables.Where(v => v.Purpose == purpose)
                );

                return variablesWithPurpose;
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show($"Error while getting variables by purpose: {ex.Message}");
                return null;
            }
        }
        public async Task<bool> UpdateVariable(VariableModel variable)
        {
            try
            {
                return await variableRepository.Update(variable);
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show($"Error while updating variable: {ex.Message}");
                return false;
            }
        }
        public void UpdateDataItemsByPlc()
        {
            for (int i =0; i < Devices.Count(); i++)
            {
                // get variable by plc
                ObservableCollection<VariableModel> vars = GetVariablesByPlc(Devices[i]);
                Devices[i].DataItems = new List<DataItem>();
                foreach (VariableModel var in vars)
                {
                    DataItem dataItem = new DataItem
                    {
                        DataType = DataType.DataBlock, // Update with appropriate data type
                        VarType = (VarType)var.Type, // Update with appropriate variable type
                        DB = var.Area ?? 0, // Update with appropriate data block number
                        BitAdr = (byte)var.BitAddress, // Update with appropriate address
                        Count = 1, // Update with appropriate count
                        StartByteAdr = (int)var.ByteAddress // Update with appropriate start byte address
                    };

                    Devices[i].DataItems.Add(dataItem);
                }
            }
        }
        #endregion

        // Variable connect PLC
        #region
        public async Task<bool> SetPLCValue(VariableModel a)
        {
            int index = devices.IndexOf(devices.FirstOrDefault(x => x.Id == a.DeviceId));
            if (devices[index].Status == DeviceStatus.Connected)
            {
                try
                {
                    await devices[index].plc.WriteAsync(DataType.DataBlock, (int)a.Area, (int)a.ByteAddress, a.Value);
                }
                catch (PlcException b)
                {
                    _ = MessageBox.Show($"Set Variable {a.Name} from PLC { devices[index].DeviceName} fail! Error {b.ErrorCode}");
                    HandlePlcError(index, b);
                    return false;
                }
            }
            else return false;
            return true;
        }
        public async Task<bool> ReadVariablesByPlc(DeviceModel device)
        {
            try
            {
                await device.plc.ReadMultipleVarsAsync(device.DataItems); // Read multiple variables in one command
                return true;
            }
            catch (Exception ex)
            {
                // Handle read error
                // For example: Log the error or show a message to the user
                Log.Error(ex, "Error reading variables from PLC");
                return false;
            }
        }
        #endregion

    }
}