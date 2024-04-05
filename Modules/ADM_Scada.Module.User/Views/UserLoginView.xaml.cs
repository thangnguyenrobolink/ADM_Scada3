using System.Windows.Controls;

namespace ADM_Scada.Modules.User.Views
{
    /// <summary>
    /// Interaction logic for UserLoginView
    /// </summary>
    public partial class UserLoginView : UserControl
    {
        public UserLoginView()
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
