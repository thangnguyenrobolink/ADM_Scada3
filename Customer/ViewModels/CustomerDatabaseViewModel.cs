using ADM_Scada.Core.ExcelService;
using ADM_Scada.Cores.Model;
using ADM_Scada.Cores.PubEvent;
using ADM_Scada.Modules.Customer.Repositories;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Customer.ViewModels
{
    public class CustomerDatabaseViewModel : BindableBase
    {
        private string filCompany;
        private string filName;
        private string filCode;
        private CustomerModel resCustomer;
        public static CustomerModel currentCus;
        public ObservableCollection<CustomerModel> FullCustomers { get; set; }
        private ObservableCollection<CustomerModel> customers;
        public ObservableCollection<CustomerModel> Customers
        {
            get => customers;
            set => SetProperty(ref customers, value);
        }
        public CustomerModel CurrentCus { get => currentCus ?? new CustomerModel() { Name = "N/A", Company = "N/A", Code = "0", Avatar = "9723582.jpg", }; set => SetProperty(ref currentCus, value); }
        public CustomerModel ResCustomer { get => resCustomer; set => SetProperty(ref resCustomer, value); }
        public string FilCompany { get => filCompany; set  { SetProperty(ref filCompany, value); FilterData(); } }
        public string FilName { get => filName; set  { SetProperty(ref filName, value); FilterData(); } }
        public string FilCode { get => filCode; set  { SetProperty(ref filCode, value); FilterData(); } }

        private readonly CustomerRepository customerRepository;

        public DelegateCommand<CustomerModel> EditCommand { get; private set; }
        public DelegateCommand<CustomerModel> DeleteCommand { get; private set; }
        public DelegateCommand ImageBrowseCommand { get; private set; }
        public DelegateCommand<CustomerModel> ChangeCustomerCommand { get; private set; }
        public DelegateCommand ExportExcelCommand { get; private set; }
        public DelegateCommand AddCustomerCommand { get; set; }
        public string FileName { get; set; }

        private void Edit(CustomerModel selectedModel)
        {
            // Handle the Edit button click
            if (selectedModel != null)
            {
                Task task1 = Task.Run(async () =>
                {
                    _ = await customerRepository.Update(selectedModel);
                    FullCustomers = new ObservableCollection<CustomerModel>((IEnumerable<CustomerModel>)await customerRepository.GetAll());
                    FilterData();
                });
                task1.Wait();
            }
        }
        private void Delete(CustomerModel selectedModel)
        {
            if (selectedModel != null)
            {
                _ = FullCustomers.Remove(selectedModel);
                _ = customerRepository.Delete(selectedModel.Id);
                _ = Task.Run(async () => FullCustomers = new ObservableCollection<CustomerModel>((IEnumerable<CustomerModel>)await customerRepository.GetAll()));
                FilterData();
            }
        }
        private bool CanDelete(CustomerModel selectedModel)
        {
            // CanExecute condition for DeleteCommand
            return FullCustomers.Count > 1;
        }
        private void ImageBrowse()
        {

            var openFileDialog1 = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;*.tiff;*.bmp|All files|*.*",
                InitialDirectory = "\\"
            };
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                List<string> ImagePathList = new List<string>
                {
                    openFileDialog1.FileName
                };
                ResCustomer.Avatar = ImagePathList[0];
            }
            RaisePropertyChanged(nameof(ResCustomer));
        }
        private async void AddCustomer()
        {
            // Validation: Check if required properties are not empty
            if (string.IsNullOrWhiteSpace(ResCustomer.Company)
                || string.IsNullOrWhiteSpace(ResCustomer.Name)
                || string.IsNullOrWhiteSpace(ResCustomer.Code))
            {
                // Handle validation error (e.g., show a message)
                // You can also throw an exception or handle it based on your application's requirements.
                MessageBox.Show("Please fill in all required fields.");
                return;
            }
            if (string.IsNullOrWhiteSpace(ResCustomer.Phone))
            {
                ResCustomer.Phone = "No Data";
            }
            if (string.IsNullOrWhiteSpace(ResCustomer.Email))
            {
                ResCustomer.Email = "No Data";
            }
            if (string.IsNullOrWhiteSpace(ResCustomer.Address))
            {
                ResCustomer.Address = "No Data";
            }
            int newUserId = await customerRepository.Create(ResCustomer);

            // Set the generated Id to the new user
            ResCustomer.Id = newUserId;

            // Add the new user to the Users collection
            FullCustomers.Add(ResCustomer);
            RaisePropertyChanged(nameof(FullCustomers));
            FilterData();
            ResCustomer = new CustomerModel();
            RaisePropertyChanged(nameof(ResCustomer));

        }
        private void ChangeCustomer(CustomerModel SelectedCus)
        {
            // Validation: Check if login credentials are valid
            if (SelectedCus == null || CurrentCus.Id == SelectedCus.Id)
            {
                // Handle validation error (e.g., show a message)
                MessageBox.Show("Please Select Customer!");
                return;
            }

            // Check if a user with the provided login name and password exists
            _ = MessageBox.Show("Change Customer successful!");
            CurrentCus = SelectedCus;
            // You can add additional logic here, such as navigation to the main application screen
            eventAggregator.GetEvent<CustomerChangeEvent>().Publish(CurrentCus);
        }
        private void FilterData()
        {
            // Perform filtering based on your criteria
            IEnumerable<CustomerModel> filteredCustomers = FullCustomers;

            // Apply filters based on the provided properties
            if (!string.IsNullOrEmpty(FilCompany))
                filteredCustomers = filteredCustomers.Where(customer => customer.Company.Contains(FilCompany));

            if (!string.IsNullOrEmpty(FilName))
                filteredCustomers = filteredCustomers.Where(customer => customer.Name.Contains(FilName));

            if (!string.IsNullOrEmpty(FilCode))
                filteredCustomers = filteredCustomers.Where(customer => customer.Code.Contains(FilCode));

            // Update the Customers collection with the filtered results
            Customers = new ObservableCollection<CustomerModel>(filteredCustomers);
            RaisePropertyChanged(nameof(Customers));
        }
        private readonly IEventAggregator eventAggregator;
        public CustomerDatabaseViewModel(IEventAggregator ed)
        {
            eventAggregator = ed;
            customerRepository = new CustomerRepository();
            Customers = new ObservableCollection<CustomerModel>();
            ResCustomer = new CustomerModel();
            _ = Task.Run(async () => { FullCustomers = new ObservableCollection<CustomerModel>((IEnumerable<CustomerModel>)await customerRepository.GetAll()); Customers = FullCustomers; }) ;
            
            EditCommand = new DelegateCommand<CustomerModel>(Edit);
            DeleteCommand = new DelegateCommand<CustomerModel>(Delete, CanDelete).ObservesProperty(() => FullCustomers.Count);
            AddCustomerCommand = new DelegateCommand(AddCustomer);
            ImageBrowseCommand = new DelegateCommand(ImageBrowse);
            ChangeCustomerCommand = new DelegateCommand<CustomerModel>(ChangeCustomer);
            ExportExcelCommand = new DelegateCommand(ExportExcel);
        }

        private void ExportExcel()
        {
            var excelExporter = new ExcelExporter();
            excelExporter.ExportToExcel(Customers, FileName);
        }
    }
}
