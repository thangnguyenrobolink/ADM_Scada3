using ADM_Scada.Cores.Models;
using ADM_Scada.Cores.PubEvent;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADM_Scada.Modules.User.ViewModels
{
    public class UserStatusRegionViewModel : BindableBase
    {
        private IEventAggregator _ea;
        private UserModel currentUser;
        private string userName = "Guest";
        private int userLevel = 0;

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
            UserName = currentUser.FullName;
            UserLevel = currentUser.Level;
        }
    }
}
