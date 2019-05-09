using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Course_test
{
    class Figure 
    {
        public Block[] figureShape = new Block[3];

        private Point loc = new Point(120,0);

        Random rand = new Random();

        public Figure()
        {
            loc.X = 120;loc.Y = 0;
            for(int i=0;i<3;i++)
            {
                figureShape[i] = new Block(loc,rand.Next(0,5));
                loc.Y += 40;
            }
        }
        public void Rotate()
        {
            var tmp = figureShape[0].block.BackgroundImage;
            figureShape[0].block.BackgroundImage = figureShape[1].block.BackgroundImage;
            figureShape[1].block.BackgroundImage = figureShape[2].block.BackgroundImage;
            figureShape[2].block.BackgroundImage = tmp;
        }
    }
}
