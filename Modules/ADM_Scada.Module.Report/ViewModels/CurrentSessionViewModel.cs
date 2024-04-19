using ADM_Scada.Core.Models;
using ADM_Scada.Modules.Report.ViewModels;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ADM_Scada.Module.Report.ViewModels
{
    public class CurrentSessionViewModel : BindableBase
    {
        //event
        #region
        private IEventAggregator ea;

        #endregion

        // Database 
        #region
        #endregion

        //Config Command 
        #region
        #endregion

        //Command execute method
        #region
        #endregion

        //UI Vcariables
        #region
        private WeighSessionModel weighSessionModel = ProductionInfoViewModel.currentSession ?? new WeighSessionModel();
        public WeighSessionModel WeighSessionModel { get => weighSessionModel; set => weighSessionModel = value; }
        #endregion

        //
        #region
        #endregion

        //
        #region
        #endregion


        public CurrentSessionViewModel(IEventAggregator ea)
        {
            //Init
            #region
            this.ea = ea;

            #endregion

        }
    }
}
