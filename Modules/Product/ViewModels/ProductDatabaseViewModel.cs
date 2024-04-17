using ADM_Scada.Core.ExcelService;
using ADM_Scada.Core.Models;
using ADM_Scada.Core.Respo;
using ADM_Scada.Cores.PubEvent;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ADM_Scada.Modules.Product.ViewModels
{
    public class ProductDatabaseViewModel : BindableBase
    {
        private string filMarket;
        private string filName;
        private string filCode;
        private string filDes;
        private ProductModel resProduct;
        public static ProductModel currentPro;
        public string FileName { get; set; }
        public ObservableCollection<ProductModel> FullProducts { get; set; }
        private ObservableCollection<ProductModel> products;
        public ObservableCollection<ProductModel> Products
        {
            get => products;
            set => SetProperty(ref products, value);
        }
        public ProductModel CurrentPro
        {
            get => currentPro ?? new ProductModel();
            set => SetProperty(ref currentPro, value);
        }
        public ProductModel ResProduct { get => resProduct; set => SetProperty(ref resProduct, value); }
        public string FilMarket { get => filMarket; set { SetProperty(ref filMarket, value); FilterData(); } }
        public string FilName { get => filName; set { SetProperty(ref filName, value); FilterData(); } }
        public string FilCode { get => filCode; set { SetProperty(ref filCode, value); FilterData(); } }
        public string FilDes { get => filDes; set { SetProperty(ref filDes, value); FilterData(); } }

        private readonly ProductRepository ProductRepository;

        public DelegateCommand<ProductModel> EditCommand { get; private set; }
        public DelegateCommand<ProductModel> DeleteCommand { get; private set; }
        public DelegateCommand ImageBrowseCommand { get; private set; }
        public DelegateCommand ExportExcelCommand { get; private set; }
        public DelegateCommand<ProductModel> ChangeProductCommand { get; private set; }
        public DelegateCommand AddProductCommand { get; set; }
        private void Edit(ProductModel selectedModel)
        {
            // Handle the Edit button click
            if (selectedModel != null)
            {
                Task task1 = Task.Run(async () =>
                {
                    _ = await ProductRepository.Update(selectedModel);
                    FullProducts = new ObservableCollection<ProductModel>((IEnumerable<ProductModel>)await ProductRepository.GetAll());
                    FilterData();
                });
                task1.Wait();
            }
        }
        private void Delete(ProductModel selectedModel)
        {
            if (selectedModel != null)
            {
                _ = FullProducts.Remove(selectedModel);
                _ = ProductRepository.Delete(selectedModel.Id);
                _ = Task.Run(async () => FullProducts = new ObservableCollection<ProductModel>((IEnumerable<ProductModel>)await ProductRepository.GetAll()));
                FilterData();
            }
        }
        private bool CanDelete(ProductModel selectedModel)
        {
            // CanExecute condition for DeleteCommand
            return FullProducts.Count > 1;
        }

        private async void AddProduct()
        {
            // Validation: Check if required properties are not empty
            if (string.IsNullOrWhiteSpace(ResProduct.Market)
                || string.IsNullOrWhiteSpace(ResProduct.ProdName)
                || string.IsNullOrWhiteSpace(ResProduct.ProdFullName)
                || string.IsNullOrWhiteSpace(ResProduct.ProdCode))
            {
                // Handle validation error (e.g., show a message)
                // You can also throw an exception or handle it based on your application's requirements.
                MessageBox.Show("Please fill in all required fields.");
                return;
            }
            if (string.IsNullOrWhiteSpace(ResProduct.Ingredient))
            {
                ResProduct.Ingredient = "N/A";
            }
            int newUserId = await ProductRepository.Create(ResProduct);

            // Set the generated Id to the new user
            ResProduct.Id = newUserId;

            // Add the new user to the Users collection
            FullProducts.Add(ResProduct);
            RaisePropertyChanged(nameof(FullProducts));
            FilterData();
            ResProduct = new ProductModel();
            RaisePropertyChanged(nameof(ResProduct));

        }
        private void ChangeProduct(ProductModel SelectedCus)
        {
            // Validation: Check if login credentials are valid
            if (SelectedCus == null || CurrentPro.Id == SelectedCus.Id)
            {
                // Handle validation error (e.g., show a message)
                MessageBox.Show("Please Select Product!");
                return;
            }

            // Check if a user with the provided login name and password exists
            _ = MessageBox.Show("Change Product successful!");
            CurrentPro = SelectedCus;
            // You can add additional logic here, such as navigation to the main application screen
            eventAggregator.GetEvent<ProductChangeEvent>().Publish(CurrentPro);
        }
        private void FilterData()
        {
            // Perform filtering based on your criteria
            IEnumerable<ProductModel> filteredProducts = FullProducts;

            // Apply filters based on the provided properties
            if (!string.IsNullOrEmpty(FilMarket))
                filteredProducts = filteredProducts.Where(Product => Product.Market.Contains(FilMarket));
            if (!string.IsNullOrEmpty(FilName))
                filteredProducts = filteredProducts.Where(Product => Product.ProdName.Contains(FilName));
            if (!string.IsNullOrEmpty(FilCode))
                filteredProducts = filteredProducts.Where(Product => Product.ProdCode.Contains(FilCode));
            if (!string.IsNullOrEmpty(FilCode))
                filteredProducts = filteredProducts.Where(Product => Product.ProdCode.Contains(FilCode));

            // Update the Products collection with the filtered results
            Products = new ObservableCollection<ProductModel>(filteredProducts);
            RaisePropertyChanged(nameof(Products));
        }
        private readonly IEventAggregator eventAggregator;
        public ProductDatabaseViewModel(IEventAggregator ed)
        {
            eventAggregator = ed;
           
            ProductRepository = new ProductRepository();
            Products = new ObservableCollection<ProductModel>();
            ResProduct = new ProductModel();
            _ = Task.Run(async () => { FullProducts = new ObservableCollection<ProductModel>((IEnumerable<ProductModel>)await ProductRepository.GetAll()); Products = FullProducts; });

            EditCommand = new DelegateCommand<ProductModel>(Edit);
            DeleteCommand = new DelegateCommand<ProductModel>(Delete, CanDelete).ObservesProperty(() => FullProducts.Count);
            AddProductCommand = new DelegateCommand(AddProduct);
            ChangeProductCommand = new DelegateCommand<ProductModel>(ChangeProduct);
            ExportExcelCommand = new DelegateCommand(ExportExcel);
        }

        private void ExportExcel()
        {
            var excelExporter = new ExcelExporter();
            excelExporter.ExportToExcel(Products, FileName);
        }
    }
}
