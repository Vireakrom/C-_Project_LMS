using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace LibraryProject
{
    public partial class FormReport : Form
    {
        private DataTable _books;

        public FormReport(DataTable data)
        {
            InitializeComponent(); // ✅ Make sure this is first
            _books = data;

            this.Load += FormReport_Load; // ✅ Ensure report loads when form is loaded
        }

        private void FormReport_Load(object sender, EventArgs e)
        {
            LoadData(_books);
        }

        private void LoadData(DataTable books)
        {
            // Set the embedded report path (your .rdlc file must be marked as "Embedded Resource")
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "LibraryProject.Books.BookReport.rdlc";

            // Clear previous data sources if any
            this.reportViewer1.LocalReport.DataSources.Clear();

            // Add new data source (DataSet name must match the one defined in RDLC)
            ReportDataSource reportDataSource = new ReportDataSource("DataSetBook", books);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource);

            // Refresh the report
            this.reportViewer1.RefreshReport();
        }
    }
}
