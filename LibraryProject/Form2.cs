using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibraryProject
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'libraryA4DataSet.Books' table. You can move, or remove it, as needed.
            this.booksTableAdapter.Fill(this.libraryA4DataSet.Books,"%");
            this.reportViewer1.RefreshReport();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.booksTableAdapter.Fill(this.libraryA4DataSet.Books, "%" + textBox1.Text + "%");
            this.reportViewer1.RefreshReport();
            
        }
    }
}
