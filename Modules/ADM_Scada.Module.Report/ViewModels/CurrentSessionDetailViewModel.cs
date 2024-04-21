using ADM_Scada.Core.Models;
using ADM_Scada.Core.Respo;
using ADM_Scada.Cores.PubEvent;
using ADM_Scada.Modules.Report.ViewModels;
using LiveCharts;
using LiveCharts.Defaults;
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
        private bool isSessionWorking;
        private bool isSessionEnded;
        public bool IsSessionWorking { get => CurrentSession.StatusCode == "S"; set => SetProperty(ref isSessionWorking, value); }
        public bool IsSessionEnded { get => CurrentSession.StatusCode != "S"; set => SetProperty(ref isSessionEnded, value); }
        private ObservableCollection<WeighSessionDModel> weighSessionD;
        private WeighSessionModel weighSession = ProductionInfoViewModel.currentSession ?? new WeighSessionModel();
        private WeighSessionDModel currentDetailWeight = new WeighSessionDModel();


        public WeighSessionDModel CurrentDetailWeight { get => currentDetailWeight; set => SetProperty(ref currentDetailWeight, value); }
        public WeighSessionModel CurrentSession
        {
            get => weighSession;
            set
            {
                _ = SetProperty(ref weighSession, value);
                IsSessionWorking = CurrentSession.StatusCode == "S";
                IsSessionEnded = CurrentSession.StatusCode != "S";
                UpdateDetailSession();
                RaisePropertyChanged(nameof(IsSessionWorking));
                RaisePropertyChanged(nameof(IsSessionEnded));
            }
        }
        public ChartValues<ObservablePoint> WeighSessionDValue { get; set; }
        #endregion
        public CurrentSessionDetailViewModel(IEventAggregator ea, WeighSessionDRepository weighSessionDRepository)
        {
            this.weighSessionDRepository = weighSessionDRepository;
            this.ea = ea;
            _ = this.ea.GetEvent<CurrentSessionChangeEvent>().Subscribe(UpdateCurrentSession);
        }

        private void UpdateCurrentSession(WeighSessionModel obj)
        {
            CurrentSession = obj;
        }

        private ChartValues<ObservablePoint> ConvertToChartValues(string propertyName)
        {
            var chartValues = new ChartValues<ObservablePoint>();

            foreach (var sessionD in WeighSessionD)
            {
                var yValue = sessionD.GetType().GetProperty(propertyName).GetValue(sessionD);
                double a = Convert.ToDouble(yValue);
                chartValues.Add(new ObservablePoint(sessionD.Id, a));
            }

            return chartValues;
        }
        private async void UpdateDetailSession()
        {
            try
            {
                WeighSessionD = new ObservableCollection<WeighSessionDModel>((IEnumerable<WeighSessionDModel>)await
                    weighSessionDRepository.GetBySessionCode(CurrentSession.SessionCode))
                    ?? new ObservableCollection<WeighSessionDModel>();

                RaisePropertyChanged(nameof(WeighSessionD));
                WeighSessionDValue = new  ChartValues<ObservablePoint>();
                WeighSessionDValue = ConvertToChartValues("CurrentWeigh");
                RaisePropertyChanged(nameof(WeighSessionDValue));
            }
            catch (Exception ex)
            {
            }
        }

    }
}
