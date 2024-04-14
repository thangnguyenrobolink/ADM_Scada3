using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Windows.Controls;

namespace ADM_Scada.Modules.Report.Views
{
    /// <summary>
    /// Interaction logic for ProductionInfoView
    /// </summary>
    public partial class ProductionInfoView : UserControl
    {
        private const string Filename = "E:\\work\\ADM_Scada\\Modules\\ADM_Scada.Module.Report\\CrystalReport1.rpt";

        public ProductionInfoView()
        {
            InitializeComponent();
            LoadReport();
        }

        private void LoadReport()
        {
            try
            {
                // Load your Crystal Report file
                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(Filename);

                // Ensure ReportViewer and ViewerCore are not null before setting the ReportSource
                if (ReportViewer != null && ReportViewer.ViewerCore != null)
                {
                    ReportViewer.ViewerCore.ReportSource = reportDocument;
                }
                else
                {
                    // Handle the case where ReportViewer or ViewerCore is null
                    // You can log an error, display a message to the user, or take other appropriate action
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                // Handle any exceptions that occur during report loading
                // You can log the exception, display an error message, or take other appropriate action
            }
        }
    }
}
