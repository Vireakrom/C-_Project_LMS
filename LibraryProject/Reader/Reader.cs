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

namespace LibraryProject.Reader
{
    public partial class Reader : UserControl
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataView dv;

        public Reader()
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
                    string sql = "SELECT * FROM Readers WHERE 1=1";

                    cmd.Parameters.Clear();

                    if (!string.IsNullOrWhiteSpace(txtName.Text))
                    {
                        string nameText = $"%{txtName.Text.Trim()}%";
                        conditions.Add("FullName LIKE @FullName COLLATE SQL_Latin1_General_CP1_CI_AS");
                        cmd.Parameters.AddWithValue("@Fullname", nameText);
                    }
                    if (!string.IsNullOrWhiteSpace(txtEmail.Text))
                    {
                        string nameText = $"%{txtEmail.Text.Trim()}%";
                        conditions.Add("Email LIKE @Email COLLATE SQL_Latin1_General_CP1_CI_AS");
                        cmd.Parameters.AddWithValue("@Email", nameText);
                    }
                    if (!string.IsNullOrWhiteSpace(txtPhone.Text))
                    {
                        string nameText = $"%{txtPhone.Text.Trim()}%";
                        conditions.Add("Phone LIKE @Phone COLLATE SQL_Latin1_General_CP1_CI_AS");
                        cmd.Parameters.AddWithValue("@Phone", nameText);
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

        private void BtnClose_Click(object sender, EventArgs e)
        {
            // Get the parent form and cast it to MainForm1
            MainForm mainForm = this.FindForm() as MainForm;

            if (mainForm != null)
            {
                mainForm.HideContentPanel(); // Call the method from the main form
            }
        }

        int selectedReaderID = 0;
        private void DataGridViewReader_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewReader.Rows[e.RowIndex];

                selectedReaderID = Convert.ToInt32(row.Cells["ReaderID"].Value);
                txtName.Text = row.Cells["Fullname"].Value?.ToString();
                txtEmail.Text = row.Cells["Email"].Value?.ToString();
                txtPhone.Text = row.Cells["Phone"].Value?.ToString();
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();

            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Please enter an Email.");
                return;
            }

            using (var conn = Database.getConnection())
            {
                try
                {
                    conn.Open();

                    // 1. Check if ISBN exists
                    string checkSql = "SELECT Fullname FROM Readers WHERE Email = @Email AND isActive =1";
                    using (var checkCmd = new SqlCommand(checkSql, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@Email", email);

                        using (var reader = checkCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string existingName = reader["Fullname"].ToString();
                                // email exists -> warning
                                MessageBox.Show("Reader "+existingName+" with this email already exists. Please use a different email.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                reader.Close();

                                // ISBN does not exist -> insert new book
                                string insertSql = @"
                            INSERT INTO Readers (FullName, Email, Phone)
                            VALUES (@FullName, @Email, @Phone)";

                                using (var insertCmd = new SqlCommand(insertSql, conn))
                                {
                                    
                                    insertCmd.Parameters.AddWithValue("@FullName", name);
                                    insertCmd.Parameters.AddWithValue("@Email", email);
                                    insertCmd.Parameters.AddWithValue("@Phone", phone);
       

                                    int rowsInserted = insertCmd.ExecuteNonQuery();
                                    if (rowsInserted > 0)
                                    {
                                        clearFields();
                                        MessageBox.Show("New reader added successfully.");
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
            txtName.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridViewReader.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a reader to update.");
                return;
            }

            using (var conn = Database.getConnection())
            {
                try
                {
                    conn.Open();
                    string sql = "UPDATE Readers SET FullName = @FullName, Email = @Email, Phone = @Phone WHERE ReaderID = @ID";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@FullName", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
                        cmd.Parameters.AddWithValue("@ID", selectedReaderID);

                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Reader updated successfully.");
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
                    MessageBox.Show("Error updating reader: " + ex.Message);
                }
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            clearFields();
            BtnShow_Click(null, null); // refresh table
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewReader.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a reader to remove.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("Are you sure you want to remove this reader?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
                return;


            try
            {
                using (var conn = Database.getConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE Readers SET isActive = 0 WHERE ReaderID = @readerID", conn);
                    cmd.Parameters.AddWithValue("@readerID", selectedReaderID);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Reader is removed.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // Refresh the DataGridView
                        Button1_Click(null, null); // Clear fields and refresh table
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

                FormReportReader reportForm = new FormReportReader(table);
                reportForm.ShowDialog();
            }
    }
}
