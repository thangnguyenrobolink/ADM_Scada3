using System.Windows.Controls;

namespace Customer.Views
{
    /// <summary>
    /// Interaction logic for CustomerDatabaseView
    /// </summary>
    public partial class CustomerDatabaseView : UserControl
    {
        public CustomerDatabaseView()
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
