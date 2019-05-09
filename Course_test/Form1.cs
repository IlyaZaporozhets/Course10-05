using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Course_test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public void ChangePanel(MainPanel panel)
        {
            Controls.Clear();
            Controls.Add(panel);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Controls.Add(new MainMenuPanel(this));
        }
    }
}