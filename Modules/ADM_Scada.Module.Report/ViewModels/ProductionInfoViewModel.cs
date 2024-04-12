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
        private readonly ProdShiftDataRepository prodShiftDataRepository = new ProdShiftDataRepository();
        private readonly ProductRepository productRepository = new ProductRepository();
        private readonly CustomerRepository customerRepository = new CustomerRepository();
        private readonly WeighSessionRepository weighSessionRepository = new WeighSessionRepository();
        private readonly WeighSessionDRepository weighSessionDRepository = new WeighSessionDRepository();
        private readonly DeviceRepository deviceRepository = new DeviceRepository();

        public ObservableCollection<ProductModel> WeighSession { get => weighSession; set => SetProperty(ref weighSession, value); }
        public ObservableCollection<CustomerModel> WeighSessionD { get => weighSessionD; set => SetProperty(ref weighSessionD, value); }

        private ObservableCollection<ProductModel> weighSession;
        private ObservableCollection<CustomerModel> weighSessionD;
        public ObservableCollection<WeighSessionModel> Sessions { get => sessions; private set => SetProperty(ref sessions, value); }
        public ObservableCollection<DeviceModel> Devices { get => devices; set => SetProperty(ref devices, value); }
        public ObservableCollection<ProductModel> FullProducts { get => fullProducts; set => SetProperty(ref fullProducts, value); }
        public ObservableCollection<CustomerModel> FullCustomers { get => fullCustomers; set => SetProperty(ref fullCustomers, value); }

        private ObservableCollection<ProductModel> fullProducts;
        private ObservableCollection<DeviceModel> devices;
        private ObservableCollection<CustomerModel> fullCustomers;
        public List<string> DeviceNames { get => deviceNames; set => SetProperty(ref deviceNames , value); }
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
                UpdateCurrentSession();
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
                UpdateCurrentShift();
            }
        }
        // current So
        public static ProdShiftDataModel currentShift = new ProdShiftDataModel();
        public ProdShiftDataModel CurrentShift
        {
            get => currentShift ?? new ProdShiftDataModel();
            set
            {
                SetProperty(ref currentShift, value);
                eventAggregator?.GetEvent<ShiftInfoChangeEvent>().Publish(currentShift);
                UpdateCurrentSession();
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
            }
        }
        private void UpdateCurrentSession()
        { } 
        private void UpdateCurrentShift()
        { }

        private async void UpdateSessionPropertiesAsync()
        {
            IsSessionWorking = CurrentSession.StatusCode == "S";
            IsSessionEnded = CurrentSession.StatusCode != "S";
            CurrentCustomer = FullCustomers.FirstOrDefault(c => c.Id == CurrentSession.CustId);
            CurrentShift = await prodShiftDataRepository.GetByName(CurrentSession.SoNumber);
            CurrentProduct = FullProducts.FirstOrDefault(p => p.ProdCode == CurrentShift.ProdCode);
        }
        private bool isSessionWorking;
        private bool isSessionEnded;
        private List<string> deviceNames;

        public bool IsSessionWorking { get => CurrentSession.StatusCode == "S"; set => SetProperty(ref isSessionWorking, value); }
        public bool IsSessionEnded { get => CurrentSession.StatusCode != "S"; set => SetProperty(ref isSessionEnded, value); }
        #endregion

        // Command config
        #region
        public DelegateCommand ChangeShiftInfoCommand { get; private set; }
        public DelegateCommand EndSessionCommand { get; private set; }
        public DelegateCommand StartSessionCommand { get; private set; }
        public DelegateCommand UpdateSessionCommand { get; private set; }
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
                    // Record does not exist, create a new one
                    await prodShiftDataRepository.Create(prodShiftData);
                }
                else
                {
                    // Record exists, update it
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
            _ = MessageBox.Show("Change Infomation successful!");
            // You can add additional logic here, such as navigation to the main application screen
            eventAggregator.GetEvent<ShiftInfoChangeEvent>().Publish(CurrentShift);
        }
        private void EndCurrentSession()
        {
            throw new NotImplementedException();
        }
        private async void StartNewSession()
        {

            bool b = await AddNewSession();
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
                newSession.StartTime = DateTime.Now; // Set the start time to the current time
                newSession.StatusCode = "S"; // Assuming "S" represents the status for a started session
                newSession.UpdatedDate = DateTime.Now; // Set the created date to the current time
                newSession.UpdatedBy = UserStatusRegionViewModel.currentUser.UserName; // Set the created by to the current user
                newSession.CreatedDate = DateTime.Now; // Set the created date to the current time
                newSession.CreatedBy = UserStatusRegionViewModel.currentUser.UserName; // Set the created by to the current user
                                                                                       // You may need to set other properties based on your requirements


                // Save the new session to the database
                int b = await weighSessionRepository.Create(newSession);
                if (b != -1)
                {
                    CurrentSession.Id = b;
                    CurrentSession.SessionCode = $"ADMD{CurrentSession.BoatId}{CurrentSession.Id % 10000000:D7}";
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


        ///
        public ProductionInfoViewModel(IEventAggregator ea)
        {
            eventAggregator = ea;
            _ = eventAggregator.GetEvent<CustomerChangeEvent>().Subscribe(UpdateCustomer);
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
        //Init 
        #region
        private void InitializeCommands()
        {
            ChangeShiftInfoCommand = new DelegateCommand(ChangeShiftInfo);
            EndSessionCommand = new DelegateCommand(EndCurrentSession);
            StartSessionCommand = new DelegateCommand(StartNewSession);
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
