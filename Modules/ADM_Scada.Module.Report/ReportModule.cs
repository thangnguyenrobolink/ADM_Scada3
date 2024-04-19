﻿using ADM_Scada.Core;
using ADM_Scada.Module.Report.Views;
using ADM_Scada.Modules.Report.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace ADM_Scada.Modules.Report
{
    public class ReportModule : IModule
    {
        //Region config 
        #region
        private readonly IRegionManager _regionManager;
        #endregion
        public static string UserName { get; internal set; }
        public static int UserLevel { get; internal set; }
        public ReportModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;

        }
        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion(RegionNames.ShiftRegion, typeof(ShiftInfoView));
            _regionManager.RegisterViewWithRegion(RegionNames.ShiftRegion2, typeof(ShiftInfoView));
            _regionManager.RegisterViewWithRegion(RegionNames.SessionStatus, typeof(CurrentSessionView));
            _regionManager.RegisterViewWithRegion(RegionNames.SessionStatus2, typeof(CurrentSessionView));
            _regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(ProductionInfoView));
            _regionManager.RegisterViewWithRegion(RegionNames.SessionDetail, typeof(CurrentSessionDetailView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ProductionInfoView>();
        }
    }
}