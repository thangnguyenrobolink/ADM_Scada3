using ADM_Scada.Core.Models;
using ADM_Scada.Cores.PubEvent;
using ADM_Scada.Modules.Report.ViewModels;
using Prism.Events;
using Prism.Mvvm;
using System;

namespace ADM_Scada.Modules.Product.ViewModels
{
    public class CurrentProductViewModel : BindableBase
    {
        private IEventAggregator _ea;
        private ProductModel currentProduct;

        public ProductModel CurrentProduct { get => currentProduct ?? ProductionInfoViewModel.currentProduct; set => SetProperty(ref currentProduct, value); }
        public CurrentProductViewModel(IEventAggregator ea)
        {
            _ea = ea;
            CurrentProduct = ProductDatabaseViewModel.currentPro;
            _ = _ea.GetEvent<ProductChangeEvent>().Subscribe(UpdateProduct);
        }

        private void UpdateProduct(ProductModel curProduct)
        {
            if (curProduct != null)
            {
                CurrentProduct = curProduct;
                CurrentProduct.UpdatedDate = DateTime.Now;
                RaisePropertyChanged(nameof(CurrentProduct));
            }
        }
    }
}
