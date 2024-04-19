using ADM_Scada.Core;
using ADM_Scada.Core.Models;
using ADM_Scada.Modules.User.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace ADM_Scada.Modules.User
{
    public class UserModule : IModule
    {
        //Region config 
        #region
        private readonly IRegionManager _regionManager;
        #endregion
        public static UserModel CurrentUser;
        public UserModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;

        }
        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion(RegionNames.UserRegion, typeof(UserStatusRegion));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<UserLoginView>();
        }
    }
}