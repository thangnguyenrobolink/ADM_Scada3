using ADM_Scada.Core.Models;
using ADM_Scada.Cores.PubEvent;
using Prism.Events;
using Prism.Mvvm;

namespace ADM_Scada.Modules.Report.ViewModels
{
    public class ShiftInfoViewModel : BindableBase
    {
        private IEventAggregator _ea;
        private ProdShiftDataModel currentShift;
        private ProdShiftDataModel ProductName = new ProdShiftDataModel();

        public ProdShiftDataModel CurrentShift { get => currentShift; set => SetProperty(ref currentShift, value); }
        public ShiftInfoViewModel(IEventAggregator ea)
        {
            _ea = ea;
            CurrentShift = ProductionInfoViewModel.currentShift;
            _ = _ea.GetEvent<ShiftInfoChangeEvent>().Subscribe(UpdateProduct);
        }

        private void UpdateProduct(ProdShiftDataModel curShift)
        {
            CurrentShift = curShift;
            RaisePropertyChanged(nameof(CurrentShift));
        }
    }
}
