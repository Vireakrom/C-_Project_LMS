namespace LibraryProject.Main
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.dashboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.booksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.readersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.borrowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.returnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contentPanel = new System.Windows.Forms.Panel();
            this.dashBoard1 = new LibraryProject.Dashboard.DashBoard();
            this.returnBookcs1 = new LibraryProject.Return.ReturnBookcs();
            this.userControlBorrow1 = new LibraryProject.Borrow.UserControlBorrow();
            this.reader1 = new LibraryProject.Reader.Reader();
            this.books1 = new LibraryProject.Books.Books();
            this.menuStrip1.SuspendLayout();
            this.contentPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.DarkCyan;
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dashboardToolStripMenuItem,
            this.booksToolStripMenuItem,
            this.readersToolStripMenuItem,
            this.borrowToolStripMenuItem,
            this.returnToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(11, 3, 0, 3);
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menuStrip1.Size = new System.Drawing.Size(1304, 38);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // dashboardToolStripMenuItem
            // 
            this.dashboardToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.dashboardToolStripMenuItem.Name = "dashboardToolStripMenuItem";
            this.dashboardToolStripMenuItem.Size = new System.Drawing.Size(140, 45);
            this.dashboardToolStripMenuItem.Text = "Dashboard";
            this.dashboardToolStripMenuItem.Click += new System.EventHandler(this.DashboardToolStripMenuItem_Click);
            // 
            // booksToolStripMenuItem
            // 
            this.booksToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.booksToolStripMenuItem.Name = "booksToolStripMenuItem";
            this.booksToolStripMenuItem.Size = new System.Drawing.Size(88, 35);
            this.booksToolStripMenuItem.Text = "Books";
            this.booksToolStripMenuItem.Click += new System.EventHandler(this.BooksToolStripMenuItem_Click);
            // 
            // readersToolStripMenuItem
            // 
            this.readersToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.readersToolStripMenuItem.Name = "readersToolStripMenuItem";
            this.readersToolStripMenuItem.Size = new System.Drawing.Size(109, 35);
            this.readersToolStripMenuItem.Text = "Readers";
            this.readersToolStripMenuItem.Click += new System.EventHandler(this.ReadersToolStripMenuItem_Click);
            // 
            // borrowToolStripMenuItem
            // 
            this.borrowToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.borrowToolStripMenuItem.Name = "borrowToolStripMenuItem";
            this.borrowToolStripMenuItem.Size = new System.Drawing.Size(100, 35);
            this.borrowToolStripMenuItem.Text = "Borrow";
            this.borrowToolStripMenuItem.Click += new System.EventHandler(this.BorrowToolStripMenuItem_Click);
            // 
            // returnToolStripMenuItem
            // 
            this.returnToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.returnToolStripMenuItem.Name = "returnToolStripMenuItem";
            this.returnToolStripMenuItem.Size = new System.Drawing.Size(95, 35);
            this.returnToolStripMenuItem.Text = "Return";
            this.returnToolStripMenuItem.Click += new System.EventHandler(this.ReturnToolStripMenuItem_Click);
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.dashBoard1);
            this.contentPanel.Controls.Add(this.returnBookcs1);
            this.contentPanel.Controls.Add(this.userControlBorrow1);
            this.contentPanel.Controls.Add(this.reader1);
            this.contentPanel.Controls.Add(this.books1);
            this.contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentPanel.Location = new System.Drawing.Point(0, 38);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Size = new System.Drawing.Size(1304, 555);
            this.contentPanel.TabIndex = 2;
            // 
            // dashBoard1
            // 
            this.dashBoard1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dashBoard1.Location = new System.Drawing.Point(0, 0);
            this.dashBoard1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dashBoard1.Name = "dashBoard1";
            this.dashBoard1.Size = new System.Drawing.Size(1304, 555);
            this.dashBoard1.TabIndex = 0;
            // 
            // returnBookcs1
            // 
            this.returnBookcs1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.returnBookcs1.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.returnBookcs1.Location = new System.Drawing.Point(0, 0);
            this.returnBookcs1.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.returnBookcs1.Name = "returnBookcs1";
            this.returnBookcs1.Size = new System.Drawing.Size(1304, 555);
            this.returnBookcs1.TabIndex = 4;
            // 
            // userControlBorrow1
            // 
            this.userControlBorrow1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userControlBorrow1.Location = new System.Drawing.Point(0, 0);
            this.userControlBorrow1.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.userControlBorrow1.Name = "userControlBorrow1";
            this.userControlBorrow1.Size = new System.Drawing.Size(705, 758);
            this.userControlBorrow1.TabIndex = 3;
            // 
            // reader1
            // 
            this.reader1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reader1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.reader1.Location = new System.Drawing.Point(0, 0);
            this.reader1.Margin = new System.Windows.Forms.Padding(4);
            this.reader1.Name = "reader1";
            this.reader1.Size = new System.Drawing.Size(1304, 555);
            this.reader1.TabIndex = 2;
            // 
            // books1
            // 
            this.books1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.books1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.books1.Location = new System.Drawing.Point(0, 0);
            this.books1.Margin = new System.Windows.Forms.Padding(4);
            this.books1.Name = "books1";
            this.books1.Size = new System.Drawing.Size(1304, 555);
            this.books1.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1304, 593);
            this.Controls.Add(this.contentPanel);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contentPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem dashboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem booksToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem readersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem borrowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem returnToolStripMenuItem;
        private Books.Books books1;
        private System.Windows.Forms.Panel contentPanel;
        private Reader.Reader reader1;
        private Borrow.UserControlBorrow userControlBorrow1;
        private Return.ReturnBookcs returnBookcs1;
        private Dashboard.DashBoard dashBoard1;
    }
}