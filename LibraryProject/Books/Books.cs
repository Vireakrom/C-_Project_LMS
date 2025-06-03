using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibraryProject.Main;


namespace LibraryProject.Books
{
    public partial class Books : UserControl
    {

        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataView dv;

        public Books()
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

                    var conditions = new List<string>();
                    string sql = "SELECT * FROM Books WHERE 1=1";

                    cmd.Parameters.Clear();

                    if (!string.IsNullOrWhiteSpace(txtAuthor.Text))
                    {
                        string nameText = $"%{txtAuthor.Text.Trim()}%";
                        conditions.Add("Author LIKE @Author COLLATE SQL_Latin1_General_CP1_CI_AS");
                        cmd.Parameters.AddWithValue("@Author", nameText);
                    }
                    if (!string.IsNullOrWhiteSpace(txtISBN.Text))
                    {
                        string nameText = $"%{txtISBN.Text.Trim()}%";
                        conditions.Add("ISBN LIKE @ISBN COLLATE SQL_Latin1_General_CP1_CI_AS");
                        cmd.Parameters.AddWithValue("@ISBN", nameText);
                    }
                    if (!string.IsNullOrWhiteSpace(txtTitle.Text))
                    {
                        string nameText = $"%{txtTitle.Text.Trim()}%";
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
        }

        private void Books_Load(object sender, EventArgs e)
        {

        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            string isbn = txtISBN.Text.Trim();
            string title = txtTitle.Text.Trim();
            string author = txtAuthor.Text.Trim();

            if (string.IsNullOrWhiteSpace(isbn))
            {
                MessageBox.Show("Please enter an ISBN.");
                return;
            }

            using (var conn = Database.getConnection())
            {
                try
                {
                    conn.Open();

                    // 1. Check if ISBN exists
                    string checkSql = "SELECT TotalQuantity, AvailableQuantity FROM Books WHERE ISBN = @ISBN";
                    using (var checkCmd = new SqlCommand(checkSql, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@ISBN", isbn);

                        using (var reader = checkCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // ISBN exists -> update quantities
                                int totalQty = (int)reader["TotalQuantity"];
                                int availableQty = (int)reader["AvailableQuantity"];
                                reader.Close();

                                string updateSql = @"
                            UPDATE Books
                            SET TotalQuantity = @NewTotalQty,
                                AvailableQuantity = @NewAvailableQty,
                                LastRestockedDate = @RestockDate
                            WHERE ISBN = @ISBN";

                                using (var updateCmd = new SqlCommand(updateSql, conn))
                                {
                                    updateCmd.Parameters.AddWithValue("@NewTotalQty", totalQty + 1);
                                    updateCmd.Parameters.AddWithValue("@NewAvailableQty", availableQty + 1);
                                    updateCmd.Parameters.AddWithValue("@RestockDate", DateTime.Now);
                                    updateCmd.Parameters.AddWithValue("@ISBN", isbn);

                                    int rowsUpdated = updateCmd.ExecuteNonQuery();
                                    if (rowsUpdated > 0) { 
                                    clearFields();
                                    MessageBox.Show("Book quantities updated successfully."); }
                                    else
                                        MessageBox.Show("Failed to update book quantities.");
                                }
                            }
                            else
                            {
                                reader.Close();

                                // ISBN does not exist -> insert new book
                                string insertSql = @"
                            INSERT INTO Books (ISBN, Title, Author, TotalQuantity, AvailableQuantity, AddedDate, LastRestockedDate)
                            VALUES (@ISBN, @Title, @Author, 1, 1, @AddedDate, @RestockDate)";

                                using (var insertCmd = new SqlCommand(insertSql, conn))
                                {
                                    insertCmd.Parameters.AddWithValue("@ISBN", isbn);
                                    insertCmd.Parameters.AddWithValue("@Title", title);
                                    insertCmd.Parameters.AddWithValue("@Author", author);
                                    insertCmd.Parameters.AddWithValue("@AddedDate", DateTime.Now);
                                    insertCmd.Parameters.AddWithValue("@RestockDate", DateTime.Now);

                                    int rowsInserted = insertCmd.ExecuteNonQuery();
                                    if (rowsInserted > 0)
                                    {
                                        clearFields();
                                        MessageBox.Show("New book added successfully.");
                                    }
                                    else
                                        MessageBox.Show("Failed to add new book.");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error adding book: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        

        }

        int selectedBookID = -1;
        private void DataGridViewBook_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewBook.Rows[e.RowIndex];

                selectedBookID = Convert.ToInt32(row.Cells["BookID"].Value);
                txtTitle.Text = row.Cells["Title"].Value?.ToString();
                txtAuthor.Text = row.Cells["Author"].Value?.ToString();
                txtISBN.Text = row.Cells["ISBN"].Value?.ToString();
            }
        }

        private void clearFields()
        {
            txtTitle.Clear();
            txtAuthor.Clear();
            txtISBN.Clear();
        }
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridViewBook.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a book to update.");
                return;
            }

            using (var conn = Database.getConnection())
            {
                try
                {
                    conn.Open();
                    string sql = "UPDATE Books SET Title = @Title, Author = @Author, ISBN = @ISBN WHERE BookID = @ID ";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Title", txtTitle.Text.Trim());
                        cmd.Parameters.AddWithValue("@Author", txtAuthor.Text.Trim());
                        cmd.Parameters.AddWithValue("@ISBN", txtISBN.Text.Trim());
                        cmd.Parameters.AddWithValue("@ID", selectedBookID);

                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Book updated successfully.");
                            BtnShow_Click(null, null); // refresh table
                        }
                        else
                        {
                            MessageBox.Show("Update failed.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating book: " + ex.Message);
                }
            }
        

        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            clearFields();
            BtnShow_Click(null, null); // refresh table
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {

            if (dataGridViewBook.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a book to remove.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("Are you sure you want to remove this book?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
                return;

            string selectedISBN = dataGridViewBook.SelectedRows[0].Cells["ISBN"].Value.ToString();

            try
            {
                using (var conn = Database.getConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE Books SET TotalQuantity = 0, AvailableQuantity = 0 WHERE ISBN = @ISBN", conn);
                    cmd.Parameters.AddWithValue("@ISBN", selectedISBN);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Book is removed.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // Refresh the DataGridView
                        BtnClear_Click(null, null); // Clear fields and refresh table
                    }
                    else
                    {
                        MessageBox.Show("Book not found or could not be removed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during deletion: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (dv == null || dv.Count == 0)
            {
                MessageBox.Show("No data to generate report.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataTable table = dv.ToTable(); // Convert DataView to DataTable
            if (table.Rows.Count == 0)
            {
                MessageBox.Show("No data in report");
                return;
            }

            FormReport reportForm = new FormReport(table);
            reportForm.ShowDialog();
        }

        private void LbTitle_Click(object sender, EventArgs e)
        {

        }

        private void TxtISBN_TextChanged(object sender, EventArgs e)
        {

        }

        private void Isbn_Click(object sender, EventArgs e)
        {

        }

        private void Label3_Click(object sender, EventArgs e)
        {

        }

        private void TxtTitle_TextChanged(object sender, EventArgs e)
        {

        }

        private void TxtAuthor_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

