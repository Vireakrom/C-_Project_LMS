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
using LibraryProject.Borrow;
using LibraryProject.Main;

namespace LibraryProject.Return
{
    public partial class ReturnBookcs : UserControl
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataView dv;

        public ReturnBookcs()
        {
            InitializeComponent();
        }

        private void BtnShow_Click(object sender, EventArgs e)
        {
            using (var conn = Database.getConnection())
            {
                try
                {
                    conn.Open();

                    var cmd = new SqlCommand();
                    var conditions = new List<string>();
                    string sql = @"
                SELECT 
                    t.TransactionID,
                    r.FullName AS ReaderName,
                    r.Email AS ReaderEmail,
                    b.Title AS BookTitle,
                    b.ISBN AS BookISBN,
                    t.BorrowDate,
                    t.DueDate

                FROM Transactions t
                INNER JOIN Readers r ON t.ReaderID = r.ReaderID
                INNER JOIN Books b ON t.BookID = b.BookID
                WHERE 1=1
            ";

                    // Filtering
                    if (!string.IsNullOrWhiteSpace(txt_id.Text))
                    {
                        string idSearch = $"%{txt_id.Text.Trim()}%";
                        conditions.Add("t.TransactionID LIKE @ID COLLATE SQL_Latin1_General_CP1_CI_AS");
                        cmd.Parameters.AddWithValue("@ID", idSearch);
                    }
                    if (!string.IsNullOrWhiteSpace(txt_reader_name.Text))
                    {
                        string readerSearch = $"%{txt_reader_name.Text.Trim()}%";
                        conditions.Add("r.FullName LIKE @Reader COLLATE SQL_Latin1_General_CP1_CI_AS");
                        cmd.Parameters.AddWithValue("@Reader", readerSearch);
                    }
                    if (!string.IsNullOrWhiteSpace(txtTitle.Text))
                    {
                        string bookSearch = $"%{txtTitle.Text.Trim()}%";
                        conditions.Add("b.Title LIKE @Book COLLATE SQL_Latin1_General_CP1_CI_AS");
                        cmd.Parameters.AddWithValue("@Book", bookSearch);
                    }

                    // Always show not returned
                    conditions.Add("t.isReturn = 0");

                    // Add conditions to SQL
                    if (conditions.Count > 0)
                        sql += " AND " + string.Join(" AND ", conditions);

                    sql += " ORDER BY t.TransactionID";

                    cmd.CommandText = sql;
                    cmd.Connection = conn;

                    da = new SqlDataAdapter(cmd);
                    ds = new DataSet();
                    da.Fill(ds);
                    dv = ds.Tables[0].DefaultView;
                    dataGridViewBorrow.DataSource = dv;


                    // Optional: set headers
                    if (dataGridViewBorrow.Columns.Contains("TransactionID"))
                        dataGridViewBorrow.Columns["TransactionID"].HeaderText = "Transaction ID";

                    if (dataGridViewBorrow.Columns.Contains("ReaderName"))
                        dataGridViewBorrow.Columns["ReaderName"].HeaderText = "Reader Name";

                    if (dataGridViewBorrow.Columns.Contains("ReaderEmail"))
                        dataGridViewBorrow.Columns["ReaderEmail"].HeaderText = "Reader Email";

                    if (dataGridViewBorrow.Columns.Contains("BookTitle"))
                        dataGridViewBorrow.Columns["BookTitle"].HeaderText = "Book Title";

                    if (dataGridViewBorrow.Columns.Contains("BookISBN"))
                        dataGridViewBorrow.Columns["BookISBN"].HeaderText = "Book ISBN";

                    if (dataGridViewBorrow.Columns.Contains("BorrowDate"))
                        dataGridViewBorrow.Columns["BorrowDate"].HeaderText = "Borrow Date";

                    if (dataGridViewBorrow.Columns.Contains("DueDate"))
                        dataGridViewBorrow.Columns["DueDate"].HeaderText = "Due Date";
                    if (dataGridViewBorrow.Columns.Contains("BookISBN"))



                    dataGridViewBorrow.ClearSelection();

                    if (ds.Tables[0].Rows.Count == 0)
                        MessageBox.Show("No results found.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Search failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void BtnReturn_Click(object sender, EventArgs e)
        {
            if (dataGridViewBorrow.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a transaction to return.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var conn = Database.getConnection())
            {
                conn.Open();
                try
                {
                    // Get the selected transaction ID
                    int transactionID = Convert.ToInt32(dataGridViewBorrow.SelectedRows[0].Cells["TransactionID"].Value);
                    // 1. Check if exists
                    int bookID;
                    string checkSql = "SELECT BookID FROM Transactions WHERE TransactionID = @transactionID";
                    using (var checkCmd = new SqlCommand(checkSql, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@transactionID", transactionID);
                        using (var reader = checkCmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                // Transaction does not exist -> warning
                                MessageBox.Show("This transaction does not exist.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            else
                            {
                                bookID = Convert.ToInt16(reader["BookID"]);
                            }
                        }
                    }
                    // 2. Delete the transaction
                    string deleteSql = "UPDATE Transactions SET IsReturn = 1, ReturnDate = @ReturnDate WHERE TransactionID = @transactionID";
                    using (var deleteCmd = new SqlCommand(deleteSql, conn))
                    {
                        deleteCmd.Parameters.AddWithValue("@ReturnDate", DateTime.Now);
                        deleteCmd.Parameters.AddWithValue("@transactionID", transactionID);
                        int rowsAffected = deleteCmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            string updateSql = "UPDATE Books SET AvailableQuantity = AvailableQuantity + 1 WHERE BookID = @BookID";
                            using (var updateCmd = new SqlCommand(updateSql, conn))
                            {
                                
                                updateCmd.Parameters.AddWithValue("@BookID", bookID);



                                int rowsUpdated = updateCmd.ExecuteNonQuery();
                                if (rowsUpdated > 0)
                                {
                                    // Successfully updated the book's quantity
                                    MessageBox.Show("Book return successfully.");

                                    BtnShow_Click(null, null); // Refresh the borrow list
                                }
                                else
                                {
                                    MessageBox.Show("Failed to update book quantity.");
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Failed to return book.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Return Book: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            // Get the parent form and cast it to MainForm1
            MainForm mainForm = this.FindForm() as MainForm;

            if (mainForm != null)
            {
                mainForm.HideContentPanel(); // Call the method from the main form
            }
        }

        private void BtnReport_Click(object sender, EventArgs e)
        {
            using (var conn = Database.getConnection())
            {
                conn.Open();

                string sql = @"
                SELECT 
                    t.TransactionID,
                    r.FullName AS FullName,
                    r.Email AS Email,
                    b.Title AS Title,
                    b.ISBN AS ISBN,
                    t.BorrowDate,
                    t.ReturnDate,
                    t.DueDate
                FROM Transactions t
                INNER JOIN Readers r ON t.ReaderID = r.ReaderID
                INNER JOIN Books b ON t.BookID = b.BookID
                WHERE t.isReturn = 1
            ";

                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                    // Open the report form and pass the data
                    FormReportReturn reportForm = new FormReportReturn(dt);
                    reportForm.ShowDialog();
                
            }
        }
    }
}
