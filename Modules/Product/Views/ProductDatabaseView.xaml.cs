using System.Windows.Controls;

namespace ADM_Scada.Modules.Product.Views
{
    /// <summary>
    /// Interaction logic for ProductDatabaseView
    /// </summary>
    public partial class ProductDatabaseView : UserControl
    {
        public ProductDatabaseView()
        {
            InitializeComponent();
        }

        private void DataGrid_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                // Clear the selected item
                dataGrid.SelectedItem = null;
            }
        }
    }
}
