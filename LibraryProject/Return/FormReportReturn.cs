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
            this.Load += FormReportReturn_Load;
        }


        private void FormReportReturn_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSet1.Transactions' table. You can move, or remove it, as needed.
            this.transactionsTableAdapter.Fill(this.dataSet1.Transactions);
            // 1. Create and fill the DataSet
            var dataSet = new DataSet1(); // or DataSetBook, depending on your setup
            var adapter = new DataSet1TableAdapters.TransactionsTableAdapter();
            adapter.Fill(dataSet.Transactions); // This should now include ReaderName and BookTitle

            // 2. Set up the ReportDataSource
            var transactionsTable = dataSet.Transactions as DataTable; // Explicitly cast to DataTable to resolve ambiguity
            _Return = transactionsTable; // Store the data for later use in filtering
            var rds = new ReportDataSource("DataSetReturnHistrory", transactionsTable);
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);

            // 3. Configure the ReportViewer
            reportViewer1.LocalReport.ReportEmbeddedResource = "LibraryProject.Return.Report_Book_Return.rdlc";

            // 4. Refresh the report
            reportViewer1.RefreshReport();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            // Get filter values
            string readerName = txtReaderName.Text.Trim();
            string bookTitle = txtBookTitle.Text.Trim();

            // Build filter expression
            List<string> filters = new List<string>();
            if (!string.IsNullOrEmpty(readerName))
                filters.Add($"FullName LIKE '%{readerName.Replace("'", "''")}%'");
            if (!string.IsNullOrEmpty(bookTitle))
                filters.Add($"Title LIKE '%{bookTitle.Replace("'", "''")}%'");

            string filterExpression = string.Join(" AND ", filters);

            // Filter the DataTable
            DataView dv = new DataView(_Return);
            dv.RowFilter = filterExpression;

            // Rebind filtered data to the report
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(
                new ReportDataSource("DataSetReturnHistrory", dv.ToTable())
            );
            reportViewer1.RefreshReport();
        }
    }
}
