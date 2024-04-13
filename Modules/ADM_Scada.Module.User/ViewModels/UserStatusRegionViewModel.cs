using ADM_Scada.Core.Models;
using ADM_Scada.Cores.PubEvent;
using Prism.Events;
using Prism.Mvvm;

namespace ADM_Scada.Modules.User.ViewModels
{
    public class UserStatusRegionViewModel : BindableBase
    {
        private IEventAggregator _ea;
        private UserModel currentUser = UserLoginViewModel.currentUser;
        private string userName = UserLoginViewModel.currentUser?.UserName;
        private int userLevel =(UserLoginViewModel.currentUser?.UserGroup) ?? 0;

        public string UserName
        {
            get => userName;
            set => SetProperty(ref userName, value);
        }
        public int UserLevel
        {
            get => userLevel;
            set => SetProperty(ref userLevel, value);
        }

        public UserStatusRegionViewModel(IEventAggregator ea)
        {
            _ea = ea;
            _ = _ea.GetEvent<UserLoginEvent>().Subscribe(UpdateUser);
        }

        private void UpdateUser(UserModel curUser)
        {
            UserModule.CurrentUser = curUser;
            currentUser = curUser;
            UserName = currentUser.UserName;
            UserLevel = currentUser.UserGroup;
        }
    }
}
