using ADM_Scada.Cores.Models;
using ADM_Scada.Cores.PubEvent;
using Prism.Events;
using Prism.Mvvm;

namespace ADM_Scada.Modules.Report.ViewModels
{
    public class ShiftInfoViewModel : BindableBase
    {
        private IEventAggregator _ea;
        private ProductionHistoryModel currentShift;
        private ProductionHistoryModel ProductName = new ProductionHistoryModel();

        public ProductionHistoryModel CurrentShift { get => currentShift; set => SetProperty(ref currentShift, value); }
        public ShiftInfoViewModel(IEventAggregator ea)
        {
            _ea = ea;
            CurrentShift = ProductionInfoViewModel.currentShift;
            _ = _ea.GetEvent<ShiftInfoChangeEvent>().Subscribe(UpdateProduct);
        }

        private void UpdateProduct(ProductionHistoryModel curShift)
        {
            CurrentShift = curShift;
            RaisePropertyChanged(nameof(CurrentShift));
        }
    }
}
