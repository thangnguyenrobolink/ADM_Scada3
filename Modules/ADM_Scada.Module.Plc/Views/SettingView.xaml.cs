using System.Windows.Controls;

namespace ADM_Scada.Modules.Plc.Views
{
    /// <summary>
    /// Interaction logic for SettingView
    /// </summary>
    public partial class SettingView : UserControl
    {
        public SettingView()
        {
            InitializeComponent();
        }

        private void RowStyle_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                // Clear the selected item
                dataGrid.SelectedItem = null;
            }
        }

        private void DataGrid_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                // Clear the selected item
                dataGrid.SelectedItem = null;
            }
        }

        private void DataGrid_LostFocus_1(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                // Clear the selected item
                dataGrid.SelectedItem = null;
            }
        }

        private void DataGrid_LostFocus_2(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                // Clear the selected item
                dataGrid.SelectedItem = null;
            }
        }
    }
}
