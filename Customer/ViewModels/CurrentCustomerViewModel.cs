using ADM_Scada.Cores.Model;
using ADM_Scada.Cores.PubEvent;
using Prism.Events;
using Prism.Mvvm;
using System;

namespace Customer.ViewModels
{
    public class CurrentCustomerViewModel : BindableBase
    {
        private IEventAggregator _ea;
        private CustomerModel currentCustomer;
        private CustomerModel customerName = new CustomerModel();

        public CustomerModel CurrentCustomer { get => currentCustomer; set => SetProperty(ref currentCustomer, value); }
        public CurrentCustomerViewModel(IEventAggregator ea)
        {
            currentCustomer = CustomerDatabaseViewModel.currentCus;
            _ea = ea;
            _ = _ea.GetEvent<CustomerChangeEvent>().Subscribe(UpdateCustomer);
        }

        private void UpdateCustomer(CustomerModel curUser)
        {

            CurrentCustomer = curUser;
            CurrentCustomer.TimeStamp = DateTime.Now;
            RaisePropertyChanged(nameof(CurrentCustomer));
        }
    }
}
