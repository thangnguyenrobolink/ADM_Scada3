using ADM_Scada.Core.Models;
using ADM_Scada.Core.Respo;
using ADM_Scada.Cores.PubEvent;
using ADM_Scada.Modules.User.ViewModels;
using Customer.ViewModels;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ADM_Scada.Modules.Report.ViewModels
{
    public class ProductionInfoViewModel : BindableBase
    {
        // Database
        #region
        private ObservableCollection<WeighSessionModel> sessions;
        private readonly ProductRepository productRepository = new ProductRepository();
        private readonly CustomerRepository customerRepository = new CustomerRepository();
        private readonly WeighSessionRepository weighSessionRepository = new WeighSessionRepository();
        private readonly WeighSessionDRepository weighSessionDRepository = new WeighSessionDRepository();

        public ObservableCollection<ProductModel> WeighSession { get => weighSession; set => SetProperty(ref weighSession, value); }
        public ObservableCollection<CustomerModel> WeighSessionD { get => weighSessionD; set => SetProperty(ref weighSessionD, value); }

        private ObservableCollection<ProductModel> weighSession;
        private ObservableCollection<CustomerModel> weighSessionD;
        public ObservableCollection<WeighSessionModel> Sessions { get => sessions; private set => SetProperty(ref sessions, value); }

        public ObservableCollection<ProductModel> FullProducts { get => fullProducts; set => SetProperty(ref fullProducts, value); }
        public ObservableCollection<CustomerModel> FullCustomers { get => fullCustomers; set => SetProperty(ref fullCustomers, value); }

        private ObservableCollection<ProductModel> fullProducts;
        private ObservableCollection<CustomerModel> fullCustomers;
        #endregion

        // Event aggr
        #region
        private readonly IEventAggregator eventAggregator;
        #endregion

        // UI variable
        #region
        public static ProdShiftDataModel currentShift = new ProdShiftDataModel();
        public ProdShiftDataModel CurrentShift
        {
            get => currentShift ?? new ProdShiftDataModel();
            set => SetProperty(ref currentShift, value);
        }
        //
        public static WeighSessionModel currentSession = new WeighSessionModel();
        public WeighSessionModel CurrentSession
        {
            get => currentSession ?? new WeighSessionModel();
            set
            {
                SetProperty(ref currentSession, value);
                UpdateSessionProperties();
            }
        }
        private void UpdateCurrentSession()
        { }

        private void UpdateSessionProperties()
        {
            IsSessionWorking = CurrentSession.StatusCode == "S";
            IsSessionEnded = CurrentSession.StatusCode != "S";
        }
        private bool isSessionWorking;
        private bool isSessionEnded;
        

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
        private void ChangeShiftInfo()
        {
            // Validation: Check if login credentials are valid
            if (CurrentShift.LotNo == null || CurrentShift.WorkOrderNo == null || CurrentShift.UpdatedDate == null)
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
                FetchLastSessionAsync()
            );
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
