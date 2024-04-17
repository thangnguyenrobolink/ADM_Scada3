using CrystalDecisions.CrystalReports.Engine;
using System.Windows;

namespace ADM_Scada.Module.Report.Views
{
    /// <summary>
    /// Interaction logic for PrismWindow1.xaml
    /// </summary>
    public partial class CrystalReportWindow : Window
    {
        private const string Filename = "E:\\work\\ADM_Scada\\Modules\\ADM_Scada.Module.Report\\CrystalReport1.rpt";

        public CrystalReportWindow()
        {

            InitializeComponent();
            reportViewer.Owner = this;
            ReportDocument reportDocument = new ReportDocument();
            //string Filename = System.AppDomain.CurrentDomain.BaseDirectory + "\\CrystalReport1.rpt";
            reportDocument.Load(Filename);
            reportViewer.ViewerCore.ReportSource = reportDocument;
        }
    }
}
