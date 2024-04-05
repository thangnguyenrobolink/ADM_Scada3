using System.Windows.Controls;

namespace ADM_Scada.Modules.Product.Views
{
    /// <summary>
    /// Interaction logic for CurrentProductView
    /// </summary>
    public partial class CurrentProductView : UserControl
    {
        public CurrentProductView()
        {
            InitializeComponent();
        }

        private void ToggleButton_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            Infocard.Height = 80;
        }

        private void ToggleButton_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            Infocard.Height = 200;
        }
    }
}
