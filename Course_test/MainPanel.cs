using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace Course_test
{
    public class MainPanel : Control
    {
        private bool _isActive;
        public bool IsActive
        {
            get
            {
                return _isActive;
            }
        }

        //public TPanel panel = new TPanel();

        public Dictionary<string, TButton> buttons = new Dictionary<string, TButton>();

        public MainPanel(Control parent)
        {
            Parent = parent;
            Location = new Point(0,0);
            Size = new Size(Parent.Width, Parent.Height);
        }

        public void AddButtons(params string[] names)
        {
            int x = 150;
            int y = 100;
            foreach (string name in names){
                buttons.Add(name, new TButton());
                buttons[name].btn.Text = name;
                buttons[name].btn.Location = new Point(x,y);
                Controls.Add(buttons[name].btn);
                y += 75;
            }
        }

        public void Start()
        {
            _isActive = true;
            new Thread(Run).Start();
        }
        public void Stop()
        {
            _isActive = false;
        }

        private float sleepTime = 400;
        public float SleepTime
        {
            get
            {
                return sleepTime;
            }
            set
            {
                sleepTime = value;
            }
        }
        private void Run()
        {
            while (_isActive)
            {
                Thread.Sleep((int)sleepTime);

                Update();
            }
        }

        public virtual void Update()
        {

        }
        public virtual void ButtonClick(object sender, EventArgs e)
        {

        }
    }
}
