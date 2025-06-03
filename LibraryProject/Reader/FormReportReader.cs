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

namespace LibraryProject.Reader
{
    public partial class FormReportReader : Form
    {
        private DataTable _readers;
        public FormReportReader(DataTable data)
        {
            InitializeComponent(); // ✅ Make sure this is first
            _readers = data;

            this.Load += FormReportReader_Load; // ✅ Ensure report loads when form is loaded
        }

        private void FormReportReader_Load(object sender, EventArgs e)
        {
            LoadData(_readers);
        }
        private void LoadData(DataTable books)
        {
            // Set the embedded report path (your .rdlc file must be marked as "Embedded Resource")
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "LibraryProject.Reader.ReportReader.rdlc";

            // Clear previous data sources if any
            this.reportViewer1.LocalReport.DataSources.Clear();

            // Add new data source (DataSet name must match the one defined in RDLC)
            ReportDataSource reportDataSource = new ReportDataSource("DataSetReader", books);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource);

            // Refresh the report
            this.reportViewer1.RefreshReport();
        }
    }
}
