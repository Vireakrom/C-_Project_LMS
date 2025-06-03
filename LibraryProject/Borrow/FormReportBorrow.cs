using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibraryProject.Books;
using Microsoft.Reporting.WinForms;

namespace LibraryProject.Borrow
{
    public partial class FormReportBorrow : Form
    {
        private DataTable _borrow;
        public FormReportBorrow(DataTable data)
        {
            InitializeComponent(); // ✅ Make sure this is first
            _borrow = data;

            this.Load += FormReportBorrow_Load; // ✅ Ensure report loads when form is loaded

        }


        private void LoadData(DataTable transaction)
        {
            // Set the embedded report path (your .rdlc file must be marked as "Embedded Resource")
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "LibraryProject.Borrow.ReportBorrow.rdlc";

            // Clear previous data sources if any
            this.reportViewer1.LocalReport.DataSources.Clear();

            // Add new data source (DataSet name must match the one defined in RDLC)
            ReportDataSource reportDataSource = new ReportDataSource("DataSetBook",transaction);
            ReportDataSource reportDataSource1 = new ReportDataSource("DataSetReader", transaction);
            ReportDataSource reportDataSource2 = new ReportDataSource("DataSetTransaction", transaction);
            // Add the data sources to the report viewer
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource2);
            
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource);

            // Refresh the report
            this.reportViewer1.RefreshReport();
        }

        private void FormReportBorrow_Load(object sender, EventArgs e)
        {
            LoadData(_borrow); // Load the data when the form is loaded
        }
    }
}
