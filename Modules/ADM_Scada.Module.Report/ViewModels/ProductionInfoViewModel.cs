using ADM_Scada.Core.Models;
using ADM_Scada.Cores.PubEvent;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Windows.Forms;

namespace ADM_Scada.Modules.Report.ViewModels
{
    public class ProductionInfoViewModel : BindableBase
    {
        private readonly IEventAggregator eventAggregator;
        public static ProdShiftDataModel currentShift = new ProdShiftDataModel() ;
        public ProdShiftDataModel CurrentShift
        {
            get => currentShift ?? new ProdShiftDataModel();
            set => SetProperty(ref currentShift, value);
        }
        public DelegateCommand ChangeShiftInfoCommand { get; private set; }
        private void ChangeShiftInfo()
        {
            // Validation: Check if login credentials are valid
            if (CurrentShift.LotNo == null || CurrentShift.WorkOrderNo == null || CurrentShift.UpdatedDate == null)
            {
                // Handle validation error (e.g., show a message)
                MessageBox.Show("Please Fill infomatrion!");
                return;
            }
            // Check if a user with the provided login name and password exists
            _ = MessageBox.Show("Change Infomation successful!");
            // You can add additional logic here, such as navigation to the main application screen
            eventAggregator.GetEvent<ShiftInfoChangeEvent>().Publish(CurrentShift);
        }
        public ProductionInfoViewModel(IEventAggregator ea)
        {
            eventAggregator = ea;
            ChangeShiftInfoCommand = new DelegateCommand(ChangeShiftInfo);
        }
    }
}
