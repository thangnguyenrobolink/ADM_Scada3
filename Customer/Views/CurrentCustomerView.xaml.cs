using System.Windows;
using System.Windows.Controls;

namespace Customer.Views
{
    /// <summary>
    /// Interaction logic for CurrentCustomer
    /// </summary>
    public partial class CurrentCustomerView : UserControl
    {
        public CurrentCustomerView()
        {
            InitializeComponent();
            Infocard.Height = 200;
            Infocard.Margin = new Thickness(4, -74, 4, 4);
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            Infocard.Height = 200;
            Infocard.Margin = new Thickness(4, -74, 4, 4);
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            Infocard.Height = 120;
            Infocard.Margin = new Thickness(4, -100, 4, 4);
        }
    }
}
