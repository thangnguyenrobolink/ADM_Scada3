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
        private readonly WeighSessionDRepository weighSessionDRepository;
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
                RaisePropertyChanged(nameof(IsSessionWorking));
                RaisePropertyChanged(nameof(IsSessionEnded));
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
            _ = this.ea.GetEvent<NewBagEvent>().Subscribe(UpdateCurrentBag);
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
        private async void UpdateCurrentBag()
        {
            if (IsSessionEnded) return;
            WeighSessionDModel newDetail = new WeighSessionDModel();
            try
            {
                // Create a new instance of WeighSessionModel with initial value
                newDetail.Barcode = "123456";
                newDetail.CurrentWeigh = (decimal?)10.0;
                newDetail.ProdCode = CurrentShiftInfo.ProdCode;
                newDetail.ProdD365Code = ProductionInfoViewModel.currentProduct.HashCode;
                newDetail.ProdFullName = CurrentShiftInfo.ProdCode;
                newDetail.ProductionDate = DateTime.Now;
                newDetail.StartTime = CurrentSession.StartTime;
                newDetail.EndTime = DateTime.Now;

                newDetail.SessionCode = CurrentSession.SessionCode;
                newDetail.ShiftDataId = CurrentShiftInfo.Id;

                newDetail.QtyCounted = CurrentDetailWeight.QtyCounted + 1;
                newDetail.QtyWeighed = CurrentDetailWeight.QtyWeighed + newDetail.CurrentWeigh;

                newDetail.UpdatedDate = DateTime.Now; // Set the created date to the current time
                newDetail.UpdatedBy = UserLoginViewModel.currentUser.UserName; // Set the created by to the current user
                newDetail.CreatedDate = DateTime.Now; // Set the created date to the current time
                newDetail.CreatedBy = UserLoginViewModel.currentUser.UserName;
                int b = await weighSessionDRepository.Create(newDetail);
               
                if (b != -1)
                {
                    CurrentDetailWeight = newDetail;
                    CurrentDetailWeight.Id = b;
                }
                ea.GetEvent<UpdateSessionDetailChangeEvent>().Publish();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while record a new detail weigh");
            }
        }

        private void UpdateCurrentSession(WeighSessionModel obj)
        {
            CurrentSession = obj;
        }
    }
}
