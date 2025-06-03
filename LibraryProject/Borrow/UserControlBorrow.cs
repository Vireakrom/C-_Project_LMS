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
using System.Xml.Linq;
using LibraryProject.Main;

namespace LibraryProject.Borrow
{
    public partial class UserControlBorrow : UserControl
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataView dv;
        public UserControlBorrow()
        {
            InitializeComponent();
        }

        private void BtnShowBookAndReader_Click(object sender, EventArgs e)
        {
            using (var conn = Database.getConnection())
            {

                //Show all books
                try
                {
                    conn.Open();

                    var conditions = new List<string>();
                    string sql = "SELECT * FROM Books WHERE 1=1";

                    cmd.Parameters.Clear();


                    if (!string.IsNullOrWhiteSpace(txtBookName.Text))
                    {
                        string nameText = $"%{txtBookName.Text.Trim()}%";
                        conditions.Add("Title LIKE @Title COLLATE SQL_Latin1_General_CP1_CI_AS");
                        cmd.Parameters.AddWithValue("@Title", nameText);
                    }


                    conditions.Add("AvailableQuantity > 0");



                    sql += " AND " + string.Join(" AND ", conditions) + " ORDER BY Title";
                    cmd.CommandText = sql;
                    cmd.Connection = conn;
                    da = new SqlDataAdapter(cmd);
                    ds = new DataSet();
                    da.Fill(ds);
                    dv = ds.Tables[0].DefaultView;
                    dataGridViewBook.DataSource = dv;

                    dataGridViewBook.Columns["BookID"].Visible = false;
                    // Change column headers
                    if (dataGridViewBook.Columns.Contains("Title"))
                        dataGridViewBook.Columns["Title"].HeaderText = "Book Title";

                    if (dataGridViewBook.Columns.Contains("Author"))
                        dataGridViewBook.Columns["Author"].HeaderText = "Author Name";

                    if (dataGridViewBook.Columns.Contains("ISBN"))
                        dataGridViewBook.Columns["ISBN"].HeaderText = "ISBN Code";

                    if (dataGridViewBook.Columns.Contains("AvailableQuantity"))
                        dataGridViewBook.Columns["AvailableQuantity"].HeaderText = "In Stock";



                    dataGridViewBook.ClearSelection();

                    conn.Close();


                    if (dv.Count == 0)
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
            //Show all readers
            using (var conn = Database.getConnection())
            {
                try
                {
                    conn.Open();

                    var conditions = new List<string>();
                    string sql = "SELECT * FROM Readers WHERE 1=1";

                    cmd.Parameters.Clear();

                    if (!string.IsNullOrWhiteSpace(txtReaderName.Text))
                    {
                        string nameText = $"%{txtReaderName.Text.Trim()}%";
                        conditions.Add("FullName LIKE @FullName COLLATE SQL_Latin1_General_CP1_CI_AS");
                        cmd.Parameters.AddWithValue("@Fullname", nameText);
                    }


                    conditions.Add("isActive = 1");



                    sql += " AND " + string.Join(" AND ", conditions) + " ORDER BY FullName";
                    cmd.CommandText = sql;
                    cmd.Connection = conn;
                    da = new SqlDataAdapter(cmd);
                    ds = new DataSet();
                    da.Fill(ds);
                    dv = ds.Tables[0].DefaultView;
                    dataGridViewReader.DataSource = dv;

                    dataGridViewReader.Columns["ReaderID"].Visible = false;
                    dataGridViewReader.Columns["isActive"].Visible = false;
                    // Change column headers
                    if (dataGridViewReader.Columns.Contains("FullName"))
                        dataGridViewReader.Columns["FullName"].HeaderText = "Full Name";

                    if (dataGridViewReader.Columns.Contains("Email"))
                        dataGridViewReader.Columns["Email"].HeaderText = "Email";

                    if (dataGridViewReader.Columns.Contains("Phone"))
                        dataGridViewReader.Columns["Phone"].HeaderText = "Phone";

                    if (dataGridViewReader.Columns.Contains("RegisterDate"))
                        dataGridViewReader.Columns["RegisterDate"].HeaderText = "Register Date";



                    dataGridViewReader.ClearSelection();

                    conn.Close();


                    if (dv.Count == 0)
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

        private void BtnAdd_Click(object sender, EventArgs e)
        {


            if(dataGridViewBook.SelectedRows.Count == 0 || dataGridViewReader.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a book and a reader.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var conn = Database.getConnection())
            {
                try
                {
                    conn.Open();
                    int bookID = selectedBookID;
                    int readerID = selectedReaderID;
                    // 1. Check if is exists
                    string checkSql = "SELECT * FROM Transactions WHERE BookID = @bookid AND ReaderID = @readerid AND IsReturn = 0";
                    using (var checkCmd = new SqlCommand(checkSql, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@bookid", bookID);
                        checkCmd.Parameters.AddWithValue("@readerid", readerID);

                        using (var reader = checkCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                // email exists -> warning
                                MessageBox.Show("This borrowing transaction already exists. Please use a different email.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                reader.Close();

                                // ISBN does not exist -> insert new book
                                string insertSql = @"
                                                INSERT INTO Transactions (ReaderID, BookID, BorrowDate, ReturnDate, IsReturn, DueDate)
                                                VALUES (@ReaderID, @BookID, GETDATE(), NULL, 0, DATEADD(day, 14, GETDATE()))";


                                using (var insertCmd = new SqlCommand(insertSql, conn))
                                {

                                    insertCmd.Parameters.AddWithValue("@ReaderID", readerID);
                                    insertCmd.Parameters.AddWithValue("@BookID", bookID);


                                    int rowsInserted = insertCmd.ExecuteNonQuery();
                                    if (rowsInserted > 0)
                                    {
                                        // Update the book's available quantity
                                string updateSql = "UPDATE Books SET AvailableQuantity = AvailableQuantity - 1 WHERE BookID = @BookID";
                                using (var updateCmd = new SqlCommand(updateSql, conn))
                                {
                                    updateCmd.Parameters.AddWithValue("@BookID", bookID);
                                    int rowsUpdated = updateCmd.ExecuteNonQuery();
                                    if (rowsUpdated > 0)
                                    {
                                        // Successfully updated the book's quantity
                                        MessageBox.Show("Book borrowed successfully.");
                                        BtnShowBookAndReader_Click(null, null); // Refresh the book and reader lists
                                        ButtonShowBorrow_Click(null, null); // Refresh the borrow list
                                    }
                                    else
                                    {
                                        MessageBox.Show("Failed to update book quantity.");
                                    }
                                }

                                    }
                                    else
                                        MessageBox.Show("Failed to add new reader.");
                                }

                                
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error adding reader: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void clearFields()
        {
            txtBookName.Clear();
            txtReaderName.Clear();
            txtID.Clear();    
            
        }

int selectedBookID = -1;
        private void DataGridViewBook_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewBook.Rows[e.RowIndex];

                selectedBookID = Convert.ToInt32(row.Cells["BookID"].Value);
            }
        }
        int selectedReaderID = -1;
        private void DataGridViewReader_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewReader.Rows[e.RowIndex];

                selectedReaderID = Convert.ToInt32(row.Cells["ReaderID"].Value);

            }
        }

        private void ButtonShowBorrow_Click(object sender, EventArgs e)
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
                    b.Title AS BookTitle,
                    t.BorrowDate,
                    t.DueDate

                FROM Transactions t
                INNER JOIN Readers r ON t.ReaderID = r.ReaderID
                INNER JOIN Books b ON t.BookID = b.BookID
                WHERE 1=1
            ";

                    // Filtering
                    if (!string.IsNullOrWhiteSpace(txtID.Text))
                    {
                        string idSearch = $"%{txtID.Text.Trim()}%";
                        conditions.Add("t.TransactionID LIKE @ID COLLATE SQL_Latin1_General_CP1_CI_AS");
                        cmd.Parameters.AddWithValue("@ID", idSearch);
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

                    if (dataGridViewBorrow  .Columns.Contains("BookTitle"))
                        dataGridViewBorrow.Columns["BookTitle"].HeaderText = "Book Title";

                    if (dataGridViewBorrow.Columns.Contains("BorrowDate"))
                        dataGridViewBorrow.Columns["BorrowDate"].HeaderText = "Borrow Date";

                    if (dataGridViewBorrow.Columns.Contains("DueDate"))
                        dataGridViewBorrow.Columns["DueDate"].HeaderText = "Due Date";



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

        private void BtnClear_Click(object sender, EventArgs e)
        {
            clearFields();
            BtnShowBookAndReader_Click(null, null);
            ButtonShowBorrow_Click(null, null);
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
                    b.Title AS Title,
                    t.BorrowDate,
                    t.DueDate
                FROM Transactions t
                INNER JOIN Readers r ON t.ReaderID = r.ReaderID
                INNER JOIN Books b ON t.BookID = b.BookID
                WHERE t.isReturn = 0
            ";

                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No data to generate report.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    // Open the report form and pass the data
                    FormReportBorrow reportForm = new FormReportBorrow(dt);
                    reportForm.ShowDialog();
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if(dataGridViewBorrow.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a transaction to remove.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using(var conn = Database.getConnection())
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
                    string deleteSql = "DELETE FROM Transactions WHERE TransactionID = @transactionID";
                    using (var deleteCmd = new SqlCommand(deleteSql, conn))
                    {
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
                                    MessageBox.Show("Transaction removed successfully.");
                                    BtnShowBookAndReader_Click(null, null); // Refresh the book and reader lists
                                    ButtonShowBorrow_Click(null, null); // Refresh the borrow list
                                }
                                else
                                {
                                    MessageBox.Show("Failed to update book quantity.");
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Failed to remove transaction.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error removing transaction: " + ex.Message);
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
    }
}
