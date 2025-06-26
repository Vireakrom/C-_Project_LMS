using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibraryProject.Borrow
{
    public partial class FormQuantity : Form
    {
        int bookAvailableQuantity;
        public FormQuantity(string bookTitle,string readerName, int bookAvailableQuantity)
        {
            InitializeComponent();

            lbBookTitle.Text += bookTitle;
            lbReaderName.Text += readerName;
            this.bookAvailableQuantity = bookAvailableQuantity;
            txtQuantity.Text = "1"; // Default value for quantity
        }

        public int QuantityValue
        {
            get { return Quantity; }
        }
        int Quantity
        {
            get
            {
                int quantity;
                if (int.TryParse(txtQuantity.Text, out quantity))
                {
                    return quantity;
                }
                return 0; // Default value if parsing fails
            }
        }
        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            if (Quantity <= 0)
            {
                MessageBox.Show("Please enter a valid quantity.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Quantity > bookAvailableQuantity)
            {
                MessageBox.Show($"You can borrow a maximum of {bookAvailableQuantity} copies of this book.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            this.DialogResult = DialogResult.OK;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
