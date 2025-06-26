using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using LibraryProject.Main;

namespace LibraryProject.Dashboard
{
    public partial class DashBoard : UserControl
    {
        public DashBoard()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();

            EnableDoubleBuffering(this);

            getTotalBooks();
            getTotalReader();
            getTotalBookBorrow();
            getTotalBookReturn();
        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            // You can remove this if not used
        }

        private void getTotalBooks()
        {
            using (var conn = Database.getConnection())
            using (var cmd = new SqlCommand("SELECT SUM(TotalQuantity) FROM Books WHERE TotalQuantity > 0", conn))
            {
                try
                {
                    conn.Open();
                    int totalBooks = (int)cmd.ExecuteScalar();
                    lbTotalBooks.Text += totalBooks.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching total books: " + ex.Message);
                }
            }
        }

        private void getTotalReader()
        {
            using (var conn = Database.getConnection())
            using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Readers WHERE IsActive = 1", conn))
            {
                try
                {
                    conn.Open();
                    int totalReaders = (int)cmd.ExecuteScalar();
                    lbTotalReaders.Text += totalReaders.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching total readers: " + ex.Message);
                }
            }
        }

        private void getTotalBookBorrow()
        {
            using (var conn = Database.getConnection())
            using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Transactions WHERE IsReturn = 0", conn))
            {
                try
                {
                    conn.Open();
                    int totalBorrow = (int)cmd.ExecuteScalar();
                    lbTotalBookBorrow.Text += totalBorrow.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching total borrowed books: " + ex.Message);
                }
            }
        }

        private void getTotalBookReturn()
        {
            using (var conn = Database.getConnection())
            using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Transactions WHERE IsReturn = 1", conn))
            {
                try
                {
                    conn.Open();
                    int totalReturn = (int)cmd.ExecuteScalar();
                    lbTotalBookReturn.Text += totalReturn.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching total returned books: " + ex.Message);
                }
            }
        }

        private void EnableDoubleBuffering(Control control)
        {
            var doubleBufferPropertyInfo = control.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (doubleBufferPropertyInfo != null)
            {
                doubleBufferPropertyInfo.SetValue(control, true, null);
            }
            foreach (Control child in control.Controls)
            {
                EnableDoubleBuffering(child);
            }
        }
    }
}
