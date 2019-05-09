using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Course_test
{
    public class Block
    {
        public PictureBox block = new PictureBox();

        private string[] colors = new string[]
        {
            "GreenJevel", "RedJevel", "PurpleJevel","YellowJevel", "BlueJevel", "OrangeJevel"
            //"GreenJevel","GreenJevel","GreenJevel","GreenJevel","GreenJevel","GreenJevel"
        };
       
        public Block()
        {
            block.BackgroundImage = Properties.Resources.NOIMAGE;
            block.BackgroundImage.Tag = "white";
            block.BackColor = Color.White;
        }
        public Block(Point loc,int order)
        {
            var color = colors[order];
            block.BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject(color);
            block.BackgroundImage.Tag = color;
            block.Size = new Size(40,40);
            block.Location = loc;
        }

        public void MoveStep(Point way)
        {
            block.Location = new Point(block.Location.X + way.X, block.Location.Y + way.Y);
        }
    }
}
