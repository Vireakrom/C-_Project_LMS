using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibraryProject.Main
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            Database.getConnection();
            
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            contentPanel.Controls.Clear();
            dashBoard1.Dock = DockStyle.Fill;
            contentPanel.Controls.Add(dashBoard1);

        }
        public void HideContentPanel()
        {
            contentPanel.Controls.Clear();
            dashBoard1.Dock = DockStyle.Fill;
            contentPanel.Controls.Add(dashBoard1);
        }
        private void BooksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            contentPanel.Controls.Clear();
            contentPanel.Controls.Add(books1);
            books1.Dock = DockStyle.Fill;
        }

        private void ReadersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            contentPanel.Controls.Clear();
            contentPanel.Controls.Add(reader1);
            reader1.Dock = DockStyle.Fill;
        }

        private void BorrowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            contentPanel.Controls.Clear();
            contentPanel.Controls.Add(userControlBorrow1);
            userControlBorrow1.Dock = DockStyle.Fill;
        }

        private void ReturnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            contentPanel.Controls.Clear();
            contentPanel.Controls.Add(returnBookcs1);
            returnBookcs1.Dock = DockStyle.Fill;
        }

        private void DashboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            contentPanel.Controls.Clear();
            Dashboard.DashBoard dashBoard1 = new Dashboard.DashBoard();
            contentPanel.Controls.Add(dashBoard1);
            dashBoard1.Dock = DockStyle.Fill;

        }
    }
}
