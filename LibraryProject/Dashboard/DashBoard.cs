using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibraryProject.Main;

namespace LibraryProject.Dashboard
{

    public partial class DashBoard : UserControl
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataView dv;
        public DashBoard()
        {
            InitializeComponent();
            getTotalBooks();
            getTotalReader();
            getTotalBookBorrow();
            getTotalBookReturn();
        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void getTotalBooks()
        {
            using(var conn = Database.getConnection())
            {
                try
                {
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT COUNT(*) FROM Books WHERE TotalQuantity > 0";
                    int totalBooks = (int)cmd.ExecuteScalar();
                    lbTotalBooks.Text += totalBooks.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching total books: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        private void getTotalReader()
        {
            using (var conn = Database.getConnection())
            {
                try
                {
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT COUNT(*) FROM Readers WHERE isActive = 1";
                    int totalBooks = (int)cmd.ExecuteScalar();
                    lbTotalReaders.Text += totalBooks.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching total books: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        private void getTotalBookBorrow()
        {
            using (var conn = Database.getConnection())
            {
                try
                {
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT COUNT(*) FROM Transactions WHERE IsReturn = 0";
                    int totalBooks = (int)cmd.ExecuteScalar();
                    lbTotalBookBorrow.Text += totalBooks.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching total books: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        private void getTotalBookReturn()
        {
            using (var conn = Database.getConnection())
            {
                try
                {
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT COUNT(*) FROM Transactions WHERE IsReturn = 1";
                    int totalBooks = (int)cmd.ExecuteScalar();
                    lbTotalBookReturn.Text += totalBooks.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching total books: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}
