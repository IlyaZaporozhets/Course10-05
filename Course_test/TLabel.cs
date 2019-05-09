using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Course_test
{
    public class TLabel
    {
        public Label text = new Label();
        public TLabel(string defaultValue) {
            text.ForeColor = Color.White;
            text.Text = defaultValue;
            text.Size = new Size(100, 30);
            text.TextAlign = ContentAlignment.MiddleLeft;
        }
    }
}
