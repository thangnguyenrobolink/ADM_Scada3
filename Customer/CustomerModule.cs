using ADM_Scada.Core;
using Customer.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace ADM_Scada.Modules.Customer
{
    public class CustomerModule : IModule
    {
        private readonly IRegionManager _regionManager;
        public CustomerModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;

        }
        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion(RegionNames.CustomerRegion, typeof(CurrentCustomerView));
            // Register your view in CustomerRegion2
            _regionManager.RegisterViewWithRegion(RegionNames.CustomerRegion2, typeof(CurrentCustomerView));
            _regionManager.RegisterViewWithRegion(RegionNames.CustomerDatabaseRegion, typeof(CustomerDatabaseView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<CustomerDatabaseView>();
        }

    }
}