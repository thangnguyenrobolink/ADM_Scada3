using ADM_Scada.Core.Models;
using ADM_Scada.Core.Respo;
using ADM_Scada.Cores.PubEvent;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ADM_Scada.Module.Report.ViewModels
{
    public class CurrentSessionDetailViewModel : BindableBase
    {
        //event
        #region
        private IEventAggregator ea;

        #endregion
        // Database 
        #region
        private readonly WeighSessionDRepository weighSessionDRepository;
        public ObservableCollection<WeighSessionDModel> WeighSessionD { get => weighSessionD; set { SetProperty(ref weighSessionD, value); } }

        private ObservableCollection<WeighSessionDModel> weighSessionD;
        #endregion
        public CurrentSessionDetailViewModel(IEventAggregator ea, WeighSessionDRepository weighSessionDRepository)
        {
            this.weighSessionDRepository = weighSessionDRepository;
            this.ea = ea;
            this.ea.GetEvent<UpdateSessionDetailChangeEvent>().Subscribe(UpdateDetailSession);
            UpdateDetailSession();
        }

        private async void UpdateDetailSession()
        {
            try
            {
                WeighSessionD = new ObservableCollection<WeighSessionDModel>((IEnumerable<WeighSessionDModel>)await 
                    weighSessionDRepository.GetBySessionCode(Modules.Report.ViewModels.ProductionInfoViewModel.currentSession.SessionCode)) 
                    ?? new ObservableCollection<WeighSessionDModel>();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
