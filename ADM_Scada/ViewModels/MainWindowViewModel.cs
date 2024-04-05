using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Windows.Threading;
using ADM_Scada.Core;
using ADM_Scada.Cores.PlcService;

namespace ADM_Scada.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string currentDate;
        private string currentTime;
        private DispatcherTimer timer;
        private readonly IRegionManager _regionManager;
        public string CurrentDate
        {
            get => currentDate ?? DateTime.Now.ToString("dd-MMM-yy");
            set => SetProperty(ref currentDate, value);
        }
        public string CurrentTime
        {
            get => currentTime ?? DateTime.Now.ToString("HH:mm:ss");
            set => SetProperty(ref currentTime, value);
        }
        public DelegateCommand<string> NavigateCommand { get; private set; }
        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
                _regionManager.RequestNavigate(RegionNames.ContentRegion, navigatePath);
        }
        private IPLCCommunicationService pLCCommunicationService;
        public MainWindowViewModel(IRegionManager regionManager, IPLCCommunicationService _pLCCommunicationService)
        {
            _regionManager = regionManager;
            pLCCommunicationService = _pLCCommunicationService;
            NavigateCommand = new DelegateCommand<string>(Navigate);
            #region
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += (sender, e) =>
            {
                CurrentDate = DateTime.Now.ToString("dd-MMM-yy");
                CurrentTime = DateTime.Now.ToString("HH:mm:ss");
            };
            timer.Start();
            #endregion
        }
    }
}
