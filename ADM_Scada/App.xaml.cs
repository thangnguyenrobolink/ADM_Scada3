using ADM.Scada.Modules.Static;
using ADM_Scada.Cores.PlcService;
using ADM_Scada.Cores.Regions;
using ADM_Scada.Modules.Customer;
using ADM_Scada.Modules.Plc;
using ADM_Scada.Modules.Product;
using ADM_Scada.Modules.Report;
using ADM_Scada.Modules.User;
using ADM_Scada.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System.Windows;
using System.Windows.Controls;

namespace ADM_Scada
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            _ = containerRegistry.RegisterSingleton<IPLCCommunicationService, PlcCommunicateService>();
        }
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            _ = moduleCatalog.AddModule<PlcModule>();
            _ = moduleCatalog.AddModule<UserModule>();
            _ = moduleCatalog.AddModule<ReportModule>();
            _ = moduleCatalog.AddModule<StaticModule>();
            _ = moduleCatalog.AddModule<CustomerModule>();
            _ = moduleCatalog.AddModule<ProductModule>();
        }
        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);
            regionAdapterMappings.RegisterMapping(typeof(StackPanel), Container.Resolve<StackPanelRegionAdapter>());
        }
    }
}
