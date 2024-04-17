
using ADM_Scada.Module.Report.Views;
using CrystalDecisions.CrystalReports.Engine;
using MaterialDesignThemes.Wpf;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ADM_Scada.Modules.Report.Views
{
    /// <summary>
    /// Interaction logic for ProductionInfoView
    /// </summary>
    public partial class ProductionInfoView : UserControl
    {
        public ProductionInfoView()
        {
            InitializeComponent();
            
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Get the parent window of the UserControl
            Window parentWindow = Window.GetWindow(this);

            if (parentWindow != null)
            {
                // Instantiate the window from the other project
                CrystalReportWindow otherWindow = new CrystalReportWindow();

                // Show the window as a dialog from the parent window
                otherWindow.Show();
            }
            else
            {
                MessageBox.Show("Unable to find parent window.");
            }
        }

    }
}
