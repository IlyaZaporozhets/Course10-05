using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Course_test
{
    class MainMenuPanel : MainPanel
    {
        public MainMenuPanel(Control parent) : base(parent)
        {
            AddButtons("NewGame", "Help", "Exit");
            buttons["NewGame"].btn.Click += ButtonClick;
            buttons["Exit"].btn.Click += ButtonClick;
        }
        public override void ButtonClick(object sender, EventArgs e)
        {
            if (sender == this.buttons["NewGame"].btn)
            {
                (Parent as Form1)?.ChangePanel(new GameSessionPanel(Parent));
            }
            else if(sender == this.buttons["Exit"].btn)
            {
                (Parent as Form1)?.Close();
            }
        }
    }
}