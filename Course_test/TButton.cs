using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Course_test
{
    public class TButton
    {
        public Button btn = new Button();
        public TButton()
        {
            btn.Size = new Size(100, 50);
            btn.ForeColor = Color.White;
        }
    }
}
