using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace LibraryProject.Return
{
    public partial class FormReportReturn : Form
    {
        private DataTable _Return;
        public FormReportReturn(DataTable data)
        {
            InitializeComponent();
            _Return = data;

            this.Load += FormReportReturn_Load;
        }

        private void LoadData(DataTable transaction)
        {
            // Set the embedded report path (your .rdlc file must be marked as "Embedded Resource")
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "LibraryProject.Return.Report_Book_Return.rdlc";

            // Clear previous data sources if any
            this.reportViewer1.LocalReport.DataSources.Clear();

            // Add new data source (DataSet name must match the one defined in RDLC)
            ReportDataSource reportDataSource = new ReportDataSource("DataSetBook", transaction);
            ReportDataSource reportDataSource1 = new ReportDataSource("DataSetReader", transaction);
            ReportDataSource reportDataSource2 = new ReportDataSource("DataSetTransaction", transaction);
            // Add the data sources to the report viewer
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource2);

            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource);

            // Refresh the report
            this.reportViewer1.RefreshReport();
        }

        private void FormReportReturn_Load(object sender, EventArgs e)
        {
            LoadData(_Return); // Load the data when the form is loaded
        }
    }
}
