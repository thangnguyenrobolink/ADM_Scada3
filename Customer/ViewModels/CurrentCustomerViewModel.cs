using ADM_Scada.Core.Models;
using ADM_Scada.Cores.PubEvent;
using ADM_Scada.Modules.Report.ViewModels;
using Prism.Events;
using Prism.Mvvm;
using System;

namespace Customer.ViewModels
{
    public class CurrentCustomerViewModel : BindableBase
    {
        private IEventAggregator _ea;
        private CustomerModel currentCustomer;

        public CustomerModel CurrentCustomer { get => currentCustomer ?? ProductionInfoViewModel.currentCust; set => SetProperty(ref currentCustomer, value); }
        public CurrentCustomerViewModel(IEventAggregator ea)
        {   
            currentCustomer = CustomerDatabaseViewModel.currentCus;
            _ea = ea;
            _ = _ea.GetEvent<CustomerChangeEvent>().Subscribe(UpdateCustomer);
        }

        private void UpdateCustomer(CustomerModel curUser)
        {

            CurrentCustomer = curUser;
            RaisePropertyChanged(nameof(CurrentCustomer));
        }
    }
}
