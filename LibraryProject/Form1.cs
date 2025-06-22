using LibraryProject.Main;
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

namespace LibraryProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //this.roundedLabel1.CornerRaius = 20;
            // Insert 100 books, 50 readers, and 200 transactions
            InsertBooks(100);
            InsertReaders(50);
            InsertTransactions(200);
        }

        private void roundedLabel1_Click(object sender, EventArgs e)
        {
            
        }
        private void InsertBooks(int count)
        {
            using (var conn = Database.getConnection())
            {
                conn.Open();
                for (int i = 1; i <= count; i++)
                {
                    var cmd = new SqlCommand(
                        @"INSERT INTO Books (Title, Author, ISBN, TotalQuantity, AddedDate, LastRestockedDate, AvailableQuantity)
                  VALUES (@Title, @Author, @ISBN, @TotalQuantity, @AddedDate, @LastRestockedDate, @AvailableQuantity)", conn);

                    cmd.Parameters.AddWithValue("@Title", $"Book Title {i}");
                    cmd.Parameters.AddWithValue("@Author", $"Author {i}");
                    cmd.Parameters.AddWithValue("@ISBN", $"ISBN-{1000000000 + i}");
                    cmd.Parameters.AddWithValue("@TotalQuantity", 10 + (i % 10));
                    cmd.Parameters.AddWithValue("@AddedDate", DateTime.Now.AddDays(-i));
                    cmd.Parameters.AddWithValue("@LastRestockedDate", DateTime.Now.AddDays(-i / 2));
                    cmd.Parameters.AddWithValue("@AvailableQuantity", 5 + (i % 5));
                    cmd.ExecuteNonQuery();
                }
            }
        }
        private void InsertReaders(int count)
        {
            using (var conn = Database.getConnection())
            {
                conn.Open();
                for (int i = 1; i <= count; i++)
                {
                    var cmd = new SqlCommand(
                        @"INSERT INTO Readers (FullName, Email, Phone, RegisterDate, IsActive)
                  VALUES (@FullName, @Email, @Phone, @RegisterDate, @IsActive)", conn);

                    cmd.Parameters.AddWithValue("@FullName", $"Reader {i}");
                    cmd.Parameters.AddWithValue("@Email", $"reader{i}@example.com");
                    cmd.Parameters.AddWithValue("@Phone", $"555-000-{i:D4}");
                    cmd.Parameters.AddWithValue("@RegisterDate", DateTime.Now.AddDays(-i * 2));
                    cmd.Parameters.AddWithValue("@IsActive", i % 2 == 0);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        private void InsertTransactions(int count)
        {
            var rand = new Random();
            List<int> bookIds = new List<int>();
            List<int> readerIds = new List<int>();

            using (var conn = Database.getConnection())
            {
                conn.Open();

                // Fetch all valid BookIDs
                using (var cmd = new SqlCommand("SELECT BookID FROM Books", conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        bookIds.Add(reader.GetInt32(0));

                // Fetch all valid ReaderIDs
                using (var cmd = new SqlCommand("SELECT ReaderID FROM Readers", conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        readerIds.Add(reader.GetInt32(0));

                // Now insert transactions
                for (int i = 1; i <= count; i++)
                {
                    int bookId = bookIds[rand.Next(bookIds.Count)];
                    int readerId = readerIds[rand.Next(readerIds.Count)];
                    DateTime borrowDate = DateTime.Now.AddDays(-rand.Next(1, 100));
                    DateTime dueDate = borrowDate.AddDays(14);
                    DateTime? returnDate = (i % 3 == 0) ? (DateTime?)borrowDate.AddDays(rand.Next(1, 20)) : null;
                    bool isReturn = returnDate.HasValue;

                    var cmd = new SqlCommand(
                        @"INSERT INTO Transactions (ReaderID, BookID, BorrowDate, ReturnDate, IsReturn, DueDate)
                  VALUES (@ReaderID, @BookID, @BorrowDate, @ReturnDate, @IsReturn, @DueDate)", conn);

                    cmd.Parameters.AddWithValue("@ReaderID", readerId);
                    cmd.Parameters.AddWithValue("@BookID", bookId);
                    cmd.Parameters.AddWithValue("@BorrowDate", borrowDate);
                    cmd.Parameters.AddWithValue("@DueDate", dueDate);
                    cmd.Parameters.AddWithValue("@IsReturn", isReturn);
                    if (returnDate.HasValue)
                        cmd.Parameters.AddWithValue("@ReturnDate", returnDate.Value);
                    else
                        cmd.Parameters.AddWithValue("@ReturnDate", DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
