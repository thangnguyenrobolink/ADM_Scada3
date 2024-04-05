using ADM_Scada.Modules.Plc.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace ADM_Scada.Modules.Plc
{
    public class PlcModule : IModule
    {
        //Region config 
        #region
        private readonly IRegionManager _regionManager;
        #endregion
        public PlcModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;

        }
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<SettingView>();
            containerRegistry.RegisterForNavigation<DashBoardView>();
        }
    }
}