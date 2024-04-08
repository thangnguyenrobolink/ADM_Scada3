using ADM_Scada.Core.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ADM_Scada.Cores.PlcService
{
    public interface IPLCCommunicationService
    {
        List<DeviceModel> Devices { get; }
        Task<VariableModel> GetPLCValue(VariableModel a);
        Task<bool> SetPLCValue(VariableModel a);
        Task<bool> ConnectDevice(DeviceModel a);
        DeviceStatus GetPlcStatusById(int plcid);
        ObservableCollection<VariableModel> GetAllVariables();
        ObservableCollection<DeviceModel> GetAllDevices();

        ObservableCollection<VariableModel> GetVariablesByModule(string module);
        ObservableCollection<VariableModel> GetVariablesByPurpose(string purpose);

        VariableModel GetVariableById(int id);
        VariableModel GetVariableByName(string name);
        Task<bool> UpdateVariable(VariableModel variable);
        Task<bool> UpdatePlcDevice(DeviceModel plc);
    }
}
