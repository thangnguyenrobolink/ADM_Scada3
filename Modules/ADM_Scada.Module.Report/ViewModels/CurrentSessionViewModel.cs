using ADM_Scada.Core.Models;
using ADM_Scada.Cores.PlcService;
using ADM_Scada.Cores.PubEvent;
using ADM_Scada.Modules.Report.ViewModels;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using BarcodeLib;
using ADM_Scada.Core.Respo;
using ADM_Scada.Modules.User.ViewModels;
using Serilog;
using System.Collections.ObjectModel;
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
        private WeighSessionDRepository weighSessionDRepository;
        #endregion

        //Config Command 
        #region
        #endregion

        //Command execute method
        #region
        public DelegateCommand EndSessionCommand { get; private set; }

        #endregion

        //UI Vcariables
        #region

        private WeighSessionModel weighSession = ProductionInfoViewModel.currentSession ?? new WeighSessionModel();
        public WeighSessionModel CurrentSession
        {
            get => weighSession;
            set
            {
                _ = SetProperty(ref weighSession, value);
                IsSessionWorking = CurrentSession.StatusCode == "S";
                IsSessionEnded = CurrentSession.StatusCode != "S";
                UpdateSessionDetailChangeEvent();
                
                RaisePropertyChanged(nameof(IsSessionWorking));
                RaisePropertyChanged(nameof(IsSessionEnded));
            }
        }

        private async void UpdateSessionDetailChangeEvent()
        {
            DetailSessions = new ObservableCollection<WeighSessionDModel>((IEnumerable<WeighSessionDModel>)await weighSessionDRepository.GetBySessionCode(weighSession.SessionCode))
                ?? new ObservableCollection<WeighSessionDModel>();
            CurrentDetailWeight = DetailSessions.LastOrDefault();
            RaisePropertyChanged(nameof(BarCodeImage));
            RaisePropertyChanged(nameof(CurrentDetailWeight));
        }

        private ObservableCollection<WeighSessionDModel> detailSessions = new ObservableCollection<WeighSessionDModel>();
        public ObservableCollection<WeighSessionDModel> DetailSessions
        {
            get => detailSessions;
            set
            {
                _ = SetProperty(ref detailSessions, value);
            }
        }
        private bool isSessionWorking;
        private bool isSessionEnded;
        private ProdShiftDataModel currentShiftInfo = ProductionInfoViewModel.currentShift;
        public ProdShiftDataModel CurrentShiftInfo { get => currentShiftInfo; set => SetProperty(ref currentShiftInfo, value); }

        public bool IsSessionWorking { get => CurrentSession.StatusCode == "S"; set => SetProperty(ref isSessionWorking, value); }
        public bool IsSessionEnded { get => CurrentSession.StatusCode != "S"; set => SetProperty(ref isSessionEnded, value); }

        public BitmapImage BarCodeImage
        {
            get
            {
                  Barcode b = new Barcode
                {
                    IncludeLabel = false,
                    Height = 250
                };
                BitmapImage bitmap = new BitmapImage();
                if (CurrentDetailWeight.Barcode == null) return bitmap;
                Image img = b.Encode(TYPE.CODE128, CurrentDetailWeight.Barcode,
                                        System.Drawing.Color.Black,
                                        System.Drawing.Color.White, 10 * 10 * CurrentDetailWeight.Barcode.Length, 250);
                MemoryStream ms = new MemoryStream();
                img.Save(ms, ImageFormat.Bmp);//save image in memory my buffer byte
                byte[] buffer = ms.GetBuffer();
                //Create new MemoryStream that has the contents of buffer
                MemoryStream bufferPasser = new MemoryStream(buffer);

                //I create a new BitmapImage to work with

                bitmap.BeginInit();
                bitmap.StreamSource = bufferPasser;
                bitmap.EndInit();
                return bitmap;//I set the source of the image control type as the new //BitmapImage created earlier.                 
            }
        }
        #endregion

        // PLC Variable
        #region
        private WeighSessionDModel currentDetailWeight = new WeighSessionDModel();


        public WeighSessionDModel CurrentDetailWeight { get => currentDetailWeight; set => SetProperty(ref currentDetailWeight, value); }

        #endregion

        //
        #region
        #endregion


        public CurrentSessionViewModel(IEventAggregator ea, IPLCCommunicationService plc, WeighSessionDRepository weighSessionDRepository)
        {
            //Init
            #region
            this.weighSessionDRepository = weighSessionDRepository;
            this.ea = ea;
            _ = this.ea.GetEvent<CurrentSessionChangeEvent>().Subscribe(UpdateCurrentSession);
            _ = this.ea.GetEvent<ShiftInfoChangeEvent>().Subscribe(UpdateCurrentShiftInfo);
            #endregion
            EndSessionCommand = new DelegateCommand(EndSession);
        }

        private void UpdateCurrentShiftInfo(ProdShiftDataModel obj)
        {
            CurrentShiftInfo = obj;
        }

        private void EndSession()
        {
            ea.GetEvent<EndSessionCommandEvent>().Publish();
        }
        private void UpdateCurrentSession(WeighSessionModel obj)
        {
            CurrentSession = obj;

        }
    }
}
