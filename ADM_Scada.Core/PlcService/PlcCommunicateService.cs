using ADM_Scada.Core.Models;
using ADM_Scada.Core.Respo;
using S7.Net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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
    public class DeviceChangedEventArgs : EventArgs
    {
        public DeviceModel Device { get; }

        public DeviceChangedEventArgs(DeviceModel device)
        {
            Device = device;
        }
    }

    public class VariableChangedEventArgs : EventArgs
    {
        public VariableModel Variable { get; }

        public VariableChangedEventArgs(VariableModel variable)
        {
            Variable = variable;
        }
    }
    public class PlcCommunicateService : IPLCCommunicationService
    {
        // device variable
        private static List<Plc> plcs;
        private readonly DeviceRepository deviceRepository;
        private static List<DeviceModel> devices;
        /// Plc variable 
        private List<VariableModel> variables;
        private List<VariableModel> statusVars;
        private List<VariableModel> errorVars;
        private List<int[]> Status;
        private List<int[]> Errors;
        private Timer updateTimer;

        #region
        private string apiUrl = "http://localhost:8000/webapp";
        private readonly HttpClient _httpClient = new HttpClient();

        private async Task<HttpResponseMessage> MakeApiRequestAsync()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Process successful response (see step 5)
                }
                else
                {
                    // Handle error (see step 6)
                }

                return response;
            }
            catch (Exception ex)
            {
                // Handle exceptions (see step 6)
                return null;
            }
        }
        #endregion


        private readonly VariableRepository variableRepository;
        //Event
        public event EventHandler<DeviceChangedEventArgs> DeviceStatusChanged;
        public event EventHandler<VariableChangedEventArgs> VariableValueChanged;

        private void OnDeviceStatusChanged(DeviceChangedEventArgs e)
        {
            DeviceStatusChanged?.Invoke(this, e);
        }
        private void OnVariableValueChanged(VariableChangedEventArgs e)
        {
            VariableValueChanged?.Invoke(this, e);
        }
        /// </summary>
        public List<DeviceModel> Devices => devices;
        private readonly DispatcherTimer timer;
        /// ///Init 
        public PlcCommunicateService()
        {
            variableRepository = new VariableRepository();
            deviceRepository = new DeviceRepository();
            devices = new List<DeviceModel>();
            plcs = new List<Plc>();
            _ = Task.Run(async () =>
            {
                variables = new List<VariableModel>(await variableRepository.GetAll());
                statusVars = new List<VariableModel>(variables.FindAll(x => x.Purpose == "StatusArray"));
                Status = new List<int[]>();
                errorVars = new List<VariableModel>(variables.FindAll(x => x.Purpose == "ErrorArray"));
                Errors = new List<int[]>();
                foreach (VariableModel aStatusVar in statusVars)
                {
                    int[] _status = new int[300];
                    for (int i = 0; i < 300; i++)
                    {
                        try
                        {
                            _status[i] = variables.FindIndex(x => x.Address == i && x.Purpose == "Status" && x.DeviceId == aStatusVar.DeviceId);
                        }
                        catch (Exception ex)
                        {
                            _ = MessageBox.Show(ex.Message);
                        }
                        Status.Add(_status);
                    }
                }
                foreach (VariableModel aErrorVar in errorVars)
                {
                    int[] _errors = new int[300];
                    for (int i = 0; i < 300; i++)
                    {
                        _errors[i] = variables.FindIndex(x => x.Address == i && x.Purpose == "Error" && x.DeviceId == aErrorVar.DeviceId);
                        Errors.Add(_errors);
                    }
                }
            });
            _ = Task.Run(async () =>
            {
                devices = new List<DeviceModel>(await deviceRepository.GetAll());
                foreach (DeviceModel device in devices)
                {
                    Plc plc = new Plc(CpuType.S71200, device.IpAddress, 0, 1);
                    device.Status = DeviceStatus.Connecting;
                    try
                    {
                        plc.Open();
                        device.Status = DeviceStatus.Connected;
                    }
                    catch (PlcException a)
                    {
                        switch (a.ErrorCode)
                        {
                            case ErrorCode.ConnectionError:
                                device.Status = DeviceStatus.DisConnect;
                                break;
                            default:
                                device.Status = DeviceStatus.NotPresent;
                                break;
                        }
                        plcs.Add(plc);
                    }
                }
            });
            //updateTimer = new Timer(null, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
            #region
            UpdatePlcData(null);
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(3)
            };
            timer.Tick += (sender, e) => UpdatePlcData(null);
            timer.Start();
            #endregion
        }

        private async void UpdatePlcData(object state)
        {
            //Update plc status
            await MakeApiRequestAsync();
            foreach (DeviceModel device in devices)
            {
                var newStatus = GetPlcStatusById(device.Id);
                if (device.Status != newStatus)
                {
                    device.Status = newStatus;
                    OnDeviceStatusChanged(new DeviceChangedEventArgs(device));
                }
            }
            // Example: Update status and error for plcs
            // Get list array variable
            for (int i = 0; i < statusVars?.Count; i++)
            {
                int count = 300;
                try
                {
                    if (statusVars[i].DeviceId >= plcs.Count) return;
                    byte[] byteArray = await plcs[(int)statusVars[i].DeviceId].ReadBytesAsync(DataType.DataBlock, (int)statusVars[i].Area, (int)statusVars[i].Address, count * 4);
                    // Assuming each real is 4 bytes
                    // Convert the byte array to an array of floats
                    float[] floatArray = new float[count];
                    for (int j = 0; j < count; j++)
                    {
                        floatArray[j] = BitConverter.ToSingle(byteArray, j * 4);
                        if (floatArray[j] != variables[Status[i][j]].Value)
                        {
                            variables[Status[i][j]].Value = floatArray[j];
                            OnVariableValueChanged(new VariableChangedEventArgs(variables[Status[i][j]]));
                        }
                    }
                }
                catch (PlcException b)
                {
                    Random random = new Random();
                    _ = MessageBox.Show($"Read Status Variables from PLC {errorVars[i].DeviceId} fail! Error {b.ErrorCode}");
                }
            }
            ///
            for (int i = 0; i < errorVars?.Count; i++)
            {
                int count = 300;
                try
                {
                    if (statusVars[i].DeviceId >= plcs.Count) return;
                    byte[] byteArray = await plcs[(int)errorVars[i].DeviceId].ReadBytesAsync(DataType.DataBlock, (int)errorVars[i].Area, (int)errorVars[i].Address, count * 4);
                    // Assuming each real is 4 bytes
                    // Convert the byte array to an array of floats
                    float[] floatArray = new float[count];
                    for (int j = 0; j < count; j++)
                    {
                        floatArray[j] = BitConverter.ToSingle(byteArray, j * 4);
                        if (floatArray[j] != variables[Status[i][j]].Value)
                        {
                            variables[Status[i][j]].Value = floatArray[j];
                            OnVariableValueChanged(new VariableChangedEventArgs(variables[Status[i][j]]));
                        }
                    }
                }
                catch (PlcException b)
                {
                    Random random = new Random();
                    _ = MessageBox.Show($"Read Error Variables from PLC {errorVars[i].DeviceId} fail! Error {b.ErrorCode}");
                }
            }
        }

        // Use to re connect Plc 
        public async Task<bool> UpdatePlcDevice(DeviceModel plcdevice)
        {
            // Update to database
            try
            {
                _ = await deviceRepository.Update(plcdevice);
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show(ex.Message);
                return false;
            }
            // Update in plcs list
            int index = devices.FindIndex(x => x.Id == plcdevice.Id);
            plcs[index] = new Plc(CpuType.S71200, plcdevice.IpAddress, 0, 1);
            _ = await ConnectDevice(plcdevice);
            return true;
        }
        public async Task<bool> ConnectDevice(DeviceModel plcdevice)
        {
            int index = devices.FindIndex(x => x == plcdevice);
            if (index == -1) return false;
            try
            {
                await plcs[index].OpenAsync();
            }
            catch (PlcException a)
            {
                switch (a.ErrorCode)
                {
                    case ErrorCode.ConnectionError:
                        devices[index].Status = DeviceStatus.DisConnect;
                        break;
                    case ErrorCode.NoError:
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
                _ = MessageBox.Show($"Fail to connect PLC {devices[index].DeviceName}");
                return false;
            }
            return true;
        }

        public DeviceStatus GetPlcStatusById(int id)
        {
            int index = devices.FindIndex(x => x.Id == id);
            if (index == -1 || index >= plcs.Count) return DeviceStatus.NotPresent;
            try
            {
                if (plcs[index].IsConnected)
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
                        devices[index].Status = DeviceStatus.DisConnect;
                        return DeviceStatus.DisConnect;
                    default:
                        devices[index].Status = DeviceStatus.NotPresent;
                        return DeviceStatus.NotPresent;
                }
            }
        }

        public async Task<VariableModel> GetPLCValue(VariableModel a)
        {
            DeviceModel device = devices.Find(x => x.Id == a.DeviceId);
            try
            {
                a.Value = (float?)(double)await plcs[(int)a.DeviceId].ReadAsync(DataType.DataBlock, (int)a.Area, (int)a.Address, VarType.DWord, 1, 0);
            }
            catch (PlcException b)
            {
                Random random = new Random();
                _ = MessageBox.Show($"Read Variable {a.Name} from PLC {device.DeviceName} fail! Error {b.ErrorCode}");
                a.Value = (float?)random.NextDouble();
            }
            return a;
        }

        public async Task<bool> SetPLCValue(VariableModel a)
        {
            DeviceModel device = devices.Find(x => x.Id == a.DeviceId);

            try
            {
                await plcs[(int)a.DeviceId].WriteAsync(DataType.DataBlock, (int)a.Area, (int)a.Address, a.Value);
            }
            catch (PlcException b)
            {
                _ = MessageBox.Show($"Set Variable {a.Name} from PLC {device.DeviceName} fail! Error {b.ErrorCode}");
                return false;
            }
            return true;
        }

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
        public ObservableCollection<DeviceModel> GetAllDevices()
        {
            try
            {
                return new ObservableCollection<DeviceModel>(Devices);
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show($"Error while getting all Devices: {ex.Message}");
                return null;
            }
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
    }
}