using ADM_Scada.Cores.Models;
using ADM_Scada.Cores.PubEvent;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ADM_Scada.Modules.Report.ViewModels
{
    public class ProductionInfoViewModel : BindableBase
    {
        private readonly IEventAggregator eventAggregator;
        public static ProductionHistoryModel currentShift = new ProductionHistoryModel() {ProductId = 0, CustomerId =0, UserId =0, Shift=1, WO = "000", LOT = "L000", TimeStamp = DateTime.Now.Date };
        public ProductionHistoryModel CurrentShift
        {
            get => currentShift ?? new ProductionHistoryModel();
            set => SetProperty(ref currentShift, value);
        }
        public DelegateCommand ChangeShiftInfoCommand { get; private set; }
        private void ChangeShiftInfo()
        {
            // Validation: Check if login credentials are valid
            if (CurrentShift.LOT == null || CurrentShift.WO == null|| CurrentShift.TimeStamp == null)
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
