using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Course_test
{
    public class TPanel
    {
        public Panel panel;
        public TPanel()
        {
            panel = new Panel();
            panel.Size = new Size(400, 530);
            panel.BackColor = Color.Black;
        }
    }
}
