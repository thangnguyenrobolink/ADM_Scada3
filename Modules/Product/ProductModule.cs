using ADM_Scada.Core;
using ADM_Scada.Modules.Product.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace ADM_Scada.Modules.Product
{
    public class ProductModule : IModule
    {
        private readonly IRegionManager _regionManager;
        public ProductModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;

        }
        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion(RegionNames.ProductRegion, typeof(CurrentProductView));
            // Register your view in CustomerRegion2
            _regionManager.RegisterViewWithRegion(RegionNames.ProductRegion2, typeof(CurrentProductView));
            _regionManager.RegisterViewWithRegion(RegionNames.ProductDatabaseRegion, typeof(ProductDatabaseView));

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ProductDatabaseView>();
        }

    }
}