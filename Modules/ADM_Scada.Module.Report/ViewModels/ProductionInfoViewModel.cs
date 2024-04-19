using ADM_Scada.Core.Models;
using ADM_Scada.Core.Respo;
using ADM_Scada.Cores.PubEvent;
using ADM_Scada.Modules.User.ViewModels;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ADM_Scada.Modules.Report.ViewModels
{
    public class ProductionInfoViewModel : BindableBase
    {
        // Database
        #region
        private ObservableCollection<WeighSessionModel> sessions;
        private ObservableCollection<WeighSessionModel> filtersessions;
        private readonly ProdShiftDataRepository prodShiftDataRepository = new ProdShiftDataRepository();
        private readonly ProductRepository productRepository = new ProductRepository();
        private readonly CustomerRepository customerRepository = new CustomerRepository();
        private readonly WeighSessionRepository weighSessionRepository = new WeighSessionRepository();
        private readonly WeighSessionDRepository weighSessionDRepository = new WeighSessionDRepository();
        private readonly DeviceRepository deviceRepository = new DeviceRepository();

        public ObservableCollection<WeighSessionDModel> WeighSessionD { get => weighSessionD; set { SetProperty(ref weighSessionD, value); FilterWeighSessionD = value; } }
        public ObservableCollection<WeighSessionDModel> FilterWeighSessionD { get => filterweighSessionD; set { SetProperty(ref filterweighSessionD, value); } }

        private ObservableCollection<WeighSessionDModel> weighSessionD;
        private ObservableCollection<WeighSessionDModel> filterweighSessionD;
        public ObservableCollection<WeighSessionModel> Sessions { get => sessions; private set { SetProperty(ref sessions, value); FilterSessions = value; } }
        public ObservableCollection<WeighSessionModel> FilterSessions { get => filtersessions; private set => SetProperty(ref filtersessions, value); }
        public ObservableCollection<DeviceModel> Devices { get => devices; set => SetProperty(ref devices, value); }
        public ObservableCollection<ProductModel> FullProducts { get => fullProducts; set => SetProperty(ref fullProducts, value); }
        public ObservableCollection<CustomerModel> FullCustomers { get => fullCustomers; set => SetProperty(ref fullCustomers, value); }

        private ObservableCollection<ProductModel> fullProducts;
        private ObservableCollection<DeviceModel> devices;
        private ObservableCollection<CustomerModel> fullCustomers;
        public List<string> DeviceNames { get => deviceNames; set => SetProperty(ref deviceNames, value); }
        #endregion

        // Event aggr
        #region
        private readonly IEventAggregator eventAggregator;
        private void UpdateCustomer(CustomerModel curUser)
        {

            if (CurrentCustomer != curUser)
            {
                CurrentCustomer = curUser;
                RaisePropertyChanged(nameof(CurrentCustomer));
            }
            RaisePropertyChanged(nameof(CurrentCustomer));
        }
        #endregion

        // UI variable
        #region
        //current customer
        public static CustomerModel currentCust;
        public CustomerModel CurrentCustomer
        {
            get => currentCust ?? new CustomerModel();
            set
            {
                _ = SetProperty(ref currentCust, value);
                eventAggregator?.GetEvent<CustomerChangeEvent>().Publish(currentCust);
                CurrentSession.CustId = CurrentCustomer.Id;
                CurrentSession.CustName = CurrentCustomer.CustName;
                CurrentSession.CustAddress = CurrentCustomer.CustAdd;
                RaisePropertyChanged(nameof(CurrentSession));
            }
        }
        //current product
        public static ProductModel currentProduct;
        public ProductModel CurrentProduct
        {
            get => currentProduct ?? new ProductModel();
            set
            {
                _ = SetProperty(ref currentProduct, value);
                eventAggregator?.GetEvent<ProductChangeEvent>().Publish(currentProduct);
                CurrentShift.ProdCode = CurrentProduct.ProdCode;
            }
        }
        // current So
        public static ProdShiftDataModel currentShift;
        public ProdShiftDataModel CurrentShift
        {
            get => currentShift ?? new ProdShiftDataModel();
            set
            {
                if (value != null)
                {
                    _ = SetProperty(ref currentShift, value);
                    eventAggregator?.GetEvent<ShiftInfoChangeEvent>().Publish(currentShift);
                    CurrentSession.SoNumber = CurrentShift.WorkOrderNo;
                    CurrentSession.QtyTareWeigh = CurrentShift.QtyTareWeigh;
                    CurrentSession.QtyOrderWeigh = CurrentShift.QtyOrderWeigh;
                    CurrentSession.ShiftDataId = CurrentShift.Id;
                    CurrentSession.UserId = UserLoginViewModel.currentUser.Id;
                    CurrentShift.UpdatedBy = UserLoginViewModel.currentUser.UserName;
                    RaisePropertyChanged(nameof(CurrentSession));
                }
            }
        }
        //Current session
        public static WeighSessionModel currentSession = new WeighSessionModel();
        public WeighSessionModel CurrentSession
        {
            get => currentSession ?? new WeighSessionModel();
            set
            {
                SetProperty(ref currentSession, value);
                eventAggregator?.GetEvent<CurrentSessionChangeEvent>().Publish(currentSession);
                UpdateSessionPropertiesAsync();
                RaisePropertyChanged(nameof(CurrentSession));
            }
        }

        private bool isSessionWorking;
        private bool isSessionEnded;
        private List<string> deviceNames;

        public bool IsSessionWorking { get => CurrentSession.StatusCode == "S"; set { SetProperty(ref isSessionWorking, value); RaisePropertyChanged(nameof(IsSessionWorking)); } }
        public bool IsSessionEnded { get => CurrentSession.StatusCode != "S"; set { SetProperty(ref isSessionEnded, value); RaisePropertyChanged(nameof(IsSessionEnded)); } }

        //Filter vareiables
        private string filSSCode;
        public string FilSSCode { get => filSSCode; set { SetProperty(ref filSSCode, value); FilterData(); } }
        private string filSCusName;
        public string FilSCusName { get => filSCusName; set { SetProperty(ref filSCusName, value); FilterData(); } }
        private string filSSSONo;
        public string FilSSSONo { get => filSSSONo; set { SetProperty(ref filSSSONo, value); FilterData(); } }
        private string filSSNoDoc;
        public string FilSSNoDoc { get => filSSNoDoc; set { SetProperty(ref filSSNoDoc, value); FilterData(); } }
        private void FilterData()
        {
            // Perform filtering based on your criteria
            IEnumerable<WeighSessionModel> _FilterWeighSession = Sessions;

            // Apply filters based on the provided properties
            if (!string.IsNullOrEmpty(FilSSCode))
                _FilterWeighSession = _FilterWeighSession.Where(x => x.SessionCode.Contains(FilSSCode));
            if (!string.IsNullOrEmpty(FilSCusName))
                _FilterWeighSession = _FilterWeighSession.Where(x => x.CustName.Contains(FilSCusName));
            if (!string.IsNullOrEmpty(FilSSSONo))
                _FilterWeighSession = _FilterWeighSession.Where(x => x.SoNumber.Contains(FilSSSONo));
            if (!string.IsNullOrEmpty(FilSSNoDoc))
                _FilterWeighSession = _FilterWeighSession.Where(x => x.DocumentNo.Contains(FilSSNoDoc));

            // Update the Products collection with the filtered results
            FilterSessions = new ObservableCollection<WeighSessionModel>(_FilterWeighSession);
            RaisePropertyChanged(nameof(FilterSessions));
        }
        private string filDetailName;
        public string FilPCode { get => filDetailName; set { SetProperty(ref filDetailName, value); FilterDData(); } }
        private string filDetailCode;
        public string FilBarCode { get => filDetailCode; set { SetProperty(ref filDetailCode, value); FilterDData(); } }
        private string filDetailPro;
        public string FilDetailPro { get => filDetailPro; set { SetProperty(ref filDetailPro, value); FilterDData(); } }

        private void FilterDData()
        {
            // Perform filtering based on your criteria
            IEnumerable<WeighSessionDModel> _FilterWeighSessionD = WeighSessionD;

            // Apply filters based on the provided properties
            if (!string.IsNullOrEmpty(FilPCode))
                _FilterWeighSessionD = _FilterWeighSessionD.Where(WeighSessionD => WeighSessionD.SessionCode.Contains(FilPCode));
            if (!string.IsNullOrEmpty(FilBarCode))
                _FilterWeighSessionD = _FilterWeighSessionD.Where(WeighSessionD => WeighSessionD.Barcode.Contains(FilBarCode));
            if (!string.IsNullOrEmpty(FilDetailPro))
                _FilterWeighSessionD = _FilterWeighSessionD.Where(WeighSessionD => WeighSessionD.ProdFullName.Contains(FilDetailPro));

            // Update the Products collection with the filtered results
            FilterWeighSessionD = (ObservableCollection<WeighSessionDModel>)_FilterWeighSessionD;
            RaisePropertyChanged(nameof(FilterWeighSessionD));
        }

        private string filDetailDate;
        public string FilDetailDate { get => filDetailDate; set { SetProperty(ref filDetailDate, value); FilterDData(); } }

        public DelegateCommand<WeighSessionModel> EditCommand { get; private set; }

        #endregion

        // Command config
        #region
        public DelegateCommand ChangeShiftInfoCommand { get; private set; }
        public DelegateCommand EndSessionCommand { get; private set; }
        public DelegateCommand StartSessionCommand { get; private set; }
        public DelegateCommand UpdateSessionCommand { get; private set; }
        public DelegateCommand<WeighSessionModel> CheckSessionCommand { get; private set; }
        public DelegateCommand UpdateSessionDetailCommand { get; private set; }

        #endregion

        //Command Execute
        #region
        public async Task<bool> CheckAndSaveOrUpdateProdShiftDataAsync(ProdShiftDataModel prodShiftData)
        {
            try
            {
                // Check if a record with the provided code exists
                var existingProdShiftData = await prodShiftDataRepository.GetByName(prodShiftData.WorkOrderNo);

                if (existingProdShiftData == null)
                {
                    CurrentShift.UpdatedBy = UserLoginViewModel.currentUser.UserName;
                    CurrentShift.CreatedBy = UserLoginViewModel.currentUser.UserName;
                    // Record does not exist, create a new one
                    await prodShiftDataRepository.Create(prodShiftData);
                }
                else
                {
                    // Record exists, update it
                    CurrentShift.UpdatedBy = UserLoginViewModel.currentUser.UserName;
                    await prodShiftDataRepository.Update(prodShiftData);
                }

                return true; // Success
            }
            catch (Exception ex)
            {
                Log.Error($"Error checking and saving/updating ProdShiftDataModel: {ex.Message}");
                return false; // Failure
            }
        }
        private void ChangeShiftInfo()
        {
            // Validation: Check if login credentials are valid
            if (CurrentShift.QtyOrderWeigh == null || CurrentShift.WorkOrderNo == null || CurrentShift.UpdatedDate == null)
            {
                // Handle validation error (e.g., show a message)
                MessageBox.Show("Please Fill infomatrion!");
                return;
            }
            // Check if a user with the provided login name and password exists
            _ = CheckAndSaveOrUpdateProdShiftDataAsync(CurrentShift);
            _ = MessageBox.Show("Change Infomation successful!");
            // You can add additional logic here, such as navigation to the main application screen
            eventAggregator.GetEvent<ShiftInfoChangeEvent>().Publish(CurrentShift);
        }
        private async void EndCurrentSession()
        {
            try
            {
                // Create a new instance of WeighSessionModel with initial values
                CurrentSession.EndTime = DateTime.Now;
                CurrentSession.UpdatedBy = UserLoginViewModel.currentUser.UserName; // Set the created by to the current user
                CurrentSession.StatusCode = "E";
                bool b = await weighSessionRepository.Update(CurrentSession);
                if (b)
                {
                    await FetchLastSessionAsync();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while starting a new session");

            }
            try
            {
                Sessions = new ObservableCollection<WeighSessionModel>(await weighSessionRepository.GetAll());
            }
            catch (Exception ex)
            {
                HandleDataFetchError("All Session ", ex);
            }
        }
        private async void StartNewSession()
        {

            bool b = await AddNewSession();
            try
            {
                Sessions = new ObservableCollection<WeighSessionModel>(await weighSessionRepository.GetAll());
            }
            catch (Exception ex)
            {
                HandleDataFetchError("All Session ", ex);
            }
        }
        private async void CheckSession(WeighSessionModel a)
        {
            try
            {
                bool b = await weighSessionRepository.SelectToPrint(a.SessionCode);
                WeighSessionD = new ObservableCollection<WeighSessionDModel>((IEnumerable<WeighSessionDModel>)await weighSessionDRepository.GetBySessionCode(a.SessionCode)) ?? new ObservableCollection<WeighSessionDModel>();
            }
            catch (Exception ex)
            {
                HandleDataFetchError("All Session Detail ", ex);
            }
        }
        #endregion

        //Database method
        #region
        private async Task<bool> AddNewSession()
        {
            try
            {
                // Create a new instance of WeighSessionModel with initial values
                var newSession = CurrentSession;
                newSession.SessionCode = $"ADMD{CurrentSession.BoatId}{(CurrentSession.Id + 1) % 10000000:D7}";
                newSession.EndTime = DateTime.Now;
                newSession.StartTime = DateTime.Now; // Set the start time to the current time
                newSession.StatusCode = "S"; // Assuming "S" represents the status for a started session
                newSession.UpdatedDate = DateTime.Now; // Set the created date to the current time
                newSession.UpdatedBy = UserLoginViewModel.currentUser.UserName; // Set the created by to the current user
                newSession.CreatedDate = DateTime.Now; // Set the created date to the current time
                newSession.CreatedBy = UserLoginViewModel.currentUser.UserName; // Set the created by to the current user
                                                                                // You may need to set other properties based on your requirements
                                                                                // Save the new session to the database
                int b = await weighSessionRepository.Create(newSession);
                if (b != -1)
                {
                    CurrentSession.Id = b;
                    await FetchLastSessionAsync();
                    return true; // Indicate success
                }
                else return false;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while starting a new session");
                return false; // Indicate failure
            }
        }
        #endregion
        private async void UpdateSessionPropertiesAsync()
        {
            IsSessionWorking = CurrentSession.StatusCode == "S";
            IsSessionEnded = CurrentSession.StatusCode != "S";
            CurrentCustomer = FullCustomers.FirstOrDefault(c => c.Id == CurrentSession.CustId);
            CurrentShift = await prodShiftDataRepository.GetByName(CurrentSession.SoNumber);
            CurrentProduct = FullProducts.FirstOrDefault(p => p.ProdCode == CurrentShift.ProdCode);

        }
        ///
        public ProductionInfoViewModel(IEventAggregator ea)
        {
            eventAggregator = ea;
            _ = eventAggregator.GetEvent<CustomerChangeEvent>().Subscribe(UpdateCustomer);
            _ = eventAggregator.GetEvent<EndSessionCommandEvent>().Subscribe(EndCurrentSession);
            _ = eventAggregator.GetEvent<NewBagEvent>().Subscribe(UpdateCurrentSession);
            try
            {
                InitializeCommands();
                _ = FetchInitialDataAsync();
            }
            catch (Exception ex)
            {
                HandleInitializationError(ex);
            }
        }

        private void UpdateCurrentSession()
        {
            decimal currentweight = 10;
            CurrentSession.QtyCounted = CurrentSession.QtyCounted + 1;
            CurrentSession.QtyWeighed = CurrentSession.QtyWeighed + currentweight;
            CurrentSession.QtyGoodWeigh = CurrentSession.QtyGoodWeigh + currentweight - CurrentSession.QtyTareWeigh;
            CurrentSession.Gap = CurrentShift.QtyOrderWeigh - CurrentSession.QtyWeighed;
            CurrentSession.UpdatedDate = DateTime.Now;
            CurrentSession.UpdatedBy = UserLoginViewModel.currentUser.UserName;
            CurrentSession = CurrentSession;

            EditSession(CurrentSession);
        }
        private async void EditSession(WeighSessionModel obj)
        {
            try
            {
                bool b = await weighSessionRepository.Update(CurrentSession);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while edit session");
            }
            try
            {
                Sessions = new ObservableCollection<WeighSessionModel>(await weighSessionRepository.GetAll());
            }
            catch (Exception ex)
            {
                HandleDataFetchError("All Session ", ex);
            }
        }
        //Init 
        #region
        private void InitializeCommands()
        {
            EditCommand = new DelegateCommand<WeighSessionModel>(EditSession);
            ChangeShiftInfoCommand = new DelegateCommand(ChangeShiftInfo);
            EndSessionCommand = new DelegateCommand(EndCurrentSession);
            StartSessionCommand = new DelegateCommand(StartNewSession);
            CheckSessionCommand = new DelegateCommand<WeighSessionModel>(CheckSession);
        }



        private async Task FetchInitialDataAsync()
        {
            await Task.WhenAll(
                FetchFullProductsAsync(),
                FetchFullCustomersAsync(),
                FetchSessionsAsync(),
                FetchLastSessionAsync(),
                FetchDevicesAsync());
        }

        private async Task FetchFullProductsAsync()
        {
            try
            {
                FullProducts = new ObservableCollection<ProductModel>(await productRepository.GetAll());
            }
            catch (Exception ex)
            {
                HandleDataFetchError("All Products", ex);
            }
        }

        private async Task FetchFullCustomersAsync()
        {
            try
            {
                FullCustomers = new ObservableCollection<CustomerModel>(await customerRepository.GetAll());
            }
            catch (Exception ex)
            {
                HandleDataFetchError("All Customers", ex);
            }
        }
        private async Task FetchDevicesAsync()
        {
            try
            {
                Devices = new ObservableCollection<DeviceModel>(await deviceRepository.GetAll());
                DeviceNames = Devices.Select(device => device.DeviceName).ToList();
            }
            catch (Exception ex)
            {
                HandleDataFetchError("All Devices", ex);
            }
        }

        private async Task FetchSessionsAsync()
        {
            try
            {
                Sessions = new ObservableCollection<WeighSessionModel>(await weighSessionRepository.GetAll());
            }
            catch (Exception ex)
            {
                HandleDataFetchError("All Session ", ex);
            }
        }

        private async Task FetchLastSessionAsync()
        {
            try
            {
                CurrentSession = await weighSessionRepository.GetLast() ?? new WeighSessionModel();
            }
            catch (Exception ex)
            {
                HandleDataFetchError("Lastest session", ex);
            }
        }

        private void HandleInitializationError(Exception ex)
        {
            Log.Error(ex, "An error occurred during ViewModel initialization");
            ShowErrorMessage("An error occurred while initializing the ViewModel. Please try again later.");
        }

        private void HandleDataFetchError(string dataType, Exception ex)
        {
            Log.Error(ex, $"An error occurred while fetching {dataType}");
            ShowErrorMessage($"An error occurred while fetching {dataType}. Please try again later.");
        }
        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        #endregion
    }
}
