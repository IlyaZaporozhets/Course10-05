using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Course_test
{
    public class GameSessionPanel : MainPanel
    {
        TLabel jewels = new TLabel("JEWELS: 0");
        TLabel score = new TLabel("SCORE: 0");
        TLabel level = new TLabel("LEVEL: 0");
        
        public Panel GlassArea = new Panel();
        public Panel NextFigure = new Panel();

        private float delayMultiplier = 0.98f;
        private float scoreMultiplier = 1.0f;
        private int jewelsCount;
        private int gameScore;
        private int gameLevel;

        private bool _game;
        private bool _isMoving = false;
        private bool _BlocksTobeMoved = true;

        Figure currFigure = new Figure();
        Figure nextFigure = new Figure();

        public Block[,] blocks = new Block[6, 13];
        public GameSessionPanel(Control parent) : base(parent)
        {
            GlassArea.Size = new Size(240, 520);
            GlassArea.Location = new Point(5, 5);
            Controls.Add(GlassArea);
            BackgroundImage = Properties.Resources.gameBackGround;
            GlassArea.BackgroundImage = Properties.Resources.gameFieldBackGround;

            NextFigure.Size = new Size(40, 120);
            NextFigure.Location = new Point(310, 150);
            Controls.Add(NextFigure);

            Parent.KeyDown += KeyDown_Event;

            AddButtons("StartMove", "Back");
            buttons["StartMove"].btn.Location = new Point(280, 25);
            buttons["Back"].btn.Location = new Point(280, 75);
            buttons["StartMove"].btn.Click += ButtonClick;
            buttons["Back"].btn.Click += ButtonClick;

            jewels.text.Location = new Point(280,450);
            score.text.Location = new Point(280, 310);
            level.text.Location = new Point(280, 380);
            Controls.Add(jewels.text);
            Controls.Add(score.text);
            Controls.Add(level.text);
        }
        public override void ButtonClick(object sender, EventArgs e)
        {
            if (sender == buttons["StartMove"].btn)
            {
                if (!_game)
                {
                    Start();
                }
            }
            else if (sender == buttons["Back"].btn)
            {
                Stop();
                (Parent as Form1)?.ChangePanel(new MainMenuPanel(Parent));
            }
        }
        public override void Update()
        {
            _game = true;
            if (this.InvokeRequired)
            {
                Invoke((MethodInvoker)delegate ()
                { 
                    if (!_isMoving)
                    {
                        for(int i = 0; i < nextFigure.figureShape.Length; i++)
                        {
                            nextFigure.figureShape[i].block.Location = new Point(0, 40 * i);
                            NextFigure.Controls.Add(nextFigure.figureShape[i].block);
                        }
                        AddToPanel("figure");
                        _isMoving = true;
                    }
                    for (int i = 0; i < 3; i++)
                    {
                        currFigure.figureShape[i].MoveStep(new Point(0, 20));
                    }
                    if (isImpossibleToMove())
                    {
                        _isMoving = false;
                        for(int i = 0; i < currFigure.figureShape.Length; i++)
                        {
                            int x = currFigure.figureShape[i].block.Location.X / 40;
                            int y = currFigure.figureShape[i].block.Location.Y / 40;
                            blocks[x, y] = currFigure.figureShape[i];
                        }
                        refreshGameField();
                        findBlocksToDelete(blocks);
                        currFigure = new Figure();
                        for(int i = 0; i < 3; i++)
                        {
                            currFigure.figureShape[i].block.BackgroundImage = nextFigure.figureShape[i].block.BackgroundImage;
                        }
                        nextFigure = new Figure();
                        NextFigure.Controls.Clear();
                        AddToPanel("figure");
                        if (blocks[currFigure.figureShape[2].block.Location.X / 40, currFigure.figureShape[2].block.Location.Y / 40] != null)
                        {
                            Stop();
                            _game = false;
                            MessageBox.Show("You lost!");
                        }
                    }
                });
            }
        }
        private void UpdateInfo(List<Block> blocksToDelete)
        {
            jewelsCount += blocksToDelete.Count;
            gameScore += (int)Math.Round(blocksToDelete.Count * 100 * scoreMultiplier);
            if (gameScore / 2000 > gameLevel)
            {
                gameLevel++;
                scoreMultiplier *= 1.15f;
                SleepTime *= delayMultiplier;
            }
            jewels.text.Text = "JEWELS: " + jewelsCount;
            score.text.Text = "SCORE: " + gameScore;
            level.text.Text = "LEVEL: " + gameLevel;
        }
        private void MoveBlocksDown(Block[,] blocks)
        {
            for(int i = blocks.GetLength(1) - 2; i >= 0; i--)
            {
                for(int j=0;j< blocks.GetLength(0); j++)
                {
                    if (blocks[j, i + 1] == null && blocks[j,i]!=null)
                    {
                        blocks[j, i].MoveStep(new Point(0, 20));
                        blocks[j, i].MoveStep(new Point(0, 20));
                        blocks[j, i + 1] = blocks[j, i];
                        blocks[j, i] = null;
                        _BlocksTobeMoved = true;
                    }
                }
            }
        }
        private void findBlocksToDelete(Block[,] blocks)
        {
            List<Block> blocksToDelete = new List<Block>();
            _BlocksTobeMoved = true;
            for (int i = 0; i < blocks.GetLength(0); i++)
            {
                for (int j = 0; j < blocks.GetLength(1); j++)
                {
                    if (blocks[i, j] == null)
                    {
                        blocks[i, j] = new Block();
                    }
                }
            }

            //horizontals
            for (int col = 0; col < blocks.GetLength(0); col++)
            {
                int matches = 0;
                var blockImage = blocks[col, 0].block.BackgroundImage.Tag;
                for (int i = 0; i < blocks.GetLength(1); i++)
                {
                    if (blocks[col, i].block.BackgroundImage.Tag == blockImage)
                    {
                        matches++;
                    }
                    if (blocks[col, i].block.BackgroundImage.Tag != blockImage)
                    {
                         if (matches >= 3)
                        {
                            for (int j = i - matches; j < i; j++)
                            {
                                if (!blocksToDelete.Contains(blocks[col, j]) && blocks[col, j].block.BackColor != Color.White)
                                {
                                    blocksToDelete.Add(blocks[col, j]);
                                }
                            }
                        }
                        blockImage = blocks[col, i].block.BackgroundImage.Tag;
                        matches = 1;
                    }
                    if (i == blocks.GetLength(1) - 1)
                    {
                        if (matches >= 3)
                        {
                            for (int j = i - matches + 1; j <= i; j++)
                            {
                                if (!blocksToDelete.Contains(blocks[col, j]) && blocks[col, j].block.BackColor != Color.White)
                                {
                                    blocksToDelete.Add(blocks[col, j]);
                                }
                            }
                        }
                        matches = 0;
                    }
                } 
            }

            //verticles
            for (int row = 0; row < blocks.GetLength(1); row++)
            {
                int matches = 0;
                var blockImage = blocks[0, row].block.BackgroundImage.Tag;
                for (int i = 0; i < blocks.GetLength(0); i++)
                {
                    if (blocks[i, row].block.BackgroundImage.Tag == blockImage)
                    {
                        matches++;
                    }
                    if (blocks[i, row].block.BackgroundImage.Tag != blockImage)
                    {
                        if (matches >= 3)
                        {
                            for (int j = i - matches; j < i; j++)
                            {
                                if (!blocksToDelete.Contains(blocks[j, row]) && blocks[j, row].block.BackColor != Color.White)
                                {
                                    blocksToDelete.Add(blocks[j, row]);
                                }
                            }
                        }
                        blockImage = blocks[i, row].block.BackgroundImage.Tag;
                        matches = 1;
                    }
                    if (i == blocks.GetLength(0) - 1)
                    {
                        if (matches >= 3)
                        {
                            for (int j = i - matches + 1; j <= i; j++)
                            {
                                if (!blocksToDelete.Contains(blocks[j, row]) && blocks[j, row].block.BackColor != Color.White)
                                {
                                    blocksToDelete.Add(blocks[j, row]);
                                }
                            }
                        }
                        matches = 0;
                    }
                }
            }
            

            //diagonals
            for(int y = 0; y < blocks.GetLength(1); y++)
            {
                for(int x = 0; x < blocks.GetLength(0); x++)
                {
                    var blockImage = blocks[x, y].block.BackgroundImage.Tag;
                    if (blockImage.ToString() != "white")
                    {
                        int SubX = x + 1;
                        int SubY = y + 1;
                        int matches = 0;
                        while (SubX < blocks.GetLength(0) && SubY < blocks.GetLength(1) &&
                            blocks[SubX, SubY].block.BackgroundImage.Tag == blockImage)
                        {
                            matches++;
                            SubX++;
                            SubY++;
                        }
                        if (matches >= 2)
                        {
                            for (int i = 0; i <= matches; i++)
                            {
                                if (!blocksToDelete.Contains(blocks[x + i, y + i]) && blocks[x + i, y + i].block.BackColor != Color.White)
                                {
                                    blocksToDelete.Add(blocks[x + i, y + i]);
                                }
                            }
                        }
                    }
                }
            }
            for (int y = 0; y < blocks.GetLength(1); y++)
            {
                for (int x = 0; x < blocks.GetLength(0); x++)
                {
                    var blockImage = blocks[x, y].block.BackgroundImage.Tag;
                    if (blockImage.ToString() != "white")
                    {
                        int SubX = x - 1;
                        int SubY = y + 1;
                        int matches = 0;
                        while (SubX >=0 && SubY < blocks.GetLength(1) &&
                            blocks[SubX, SubY].block.BackgroundImage.Tag == blockImage)
                        {
                            matches++;
                            SubX--;
                            SubY++;
                        }
                        if (matches >= 2)
                        {
                            for (int i = 0; i <= matches; i++)
                            {
                                if (!blocksToDelete.Contains(blocks[x - i, y + i]) && blocks[x - i, y + i].block.BackColor != Color.White)
                                {
                                    blocksToDelete.Add(blocks[x - i, y + i]);
                                }
                            }
                        }
                    }
                }
            }
            if (blocksToDelete.Count > 0)
            {
                DeleteBlocks(blocksToDelete);
                while (_BlocksTobeMoved)
                {
                    _BlocksTobeMoved = false;
                    MoveBlocksDown(blocks);
                }
                for (int i = 0; i < blocks.GetLength(0); i++)
                {
                    for (int j = 0; j < blocks.GetLength(1); j++)
                    {
                        if (blocks[i, j] != null && blocks[i, j].block.BackColor == Color.White)
                        {
                            blocks[i, j] = null;
                        }
                    }
                }
                findBlocksToDelete(blocks);
            }
            for (int i = 0; i < blocks.GetLength(0); i++)
            {
                for (int j = 0; j < blocks.GetLength(1); j++)
                {
                    if (blocks[i, j] != null && blocks[i, j].block.BackColor==Color.White)
                    {
                        blocks[i, j] = null;
                    }
                }
            }
        }
        private void BlocksToDelArr(List<Block> blocksToDelete,int start,int end, int y)
        {
            for (int i = start; i <= end; i++)
            {
                if (!blocksToDelete.Contains(blocks[i, y]) && blocks[i, y].block.BackColor != Color.White)
                {
                    blocksToDelete.Add(blocks[i, y]);
                }
            }
        }
        private void DeleteBlocks(List<Block> blocksToDelete)
        {
            for(int i = 0; i < blocksToDelete.Count; i++)
            {
                GlassArea.Controls.Remove(blocksToDelete[i].block);
                blocks[blocksToDelete[i].block.Location.X / 40, blocksToDelete[i].block.Location.Y / 40] = null;
            }
            UpdateInfo(blocksToDelete);
        }
        private void refreshGameField()
        {
            GlassArea.Controls.Clear();
            AddToPanel("blocks");
        }
        private bool isImpossibleToMove()
        {
            return (currFigure.figureShape[2].block.Location.Y + 40 == GlassArea.Height ||
            blocks[currFigure.figureShape[2].block.Location.X / 40, currFigure.figureShape[2].block.Location.Y / 40 + 1] != null);
        }
        private void AddToPanel(string typeOfAdd)
        {
            switch (typeOfAdd)
            {
                case "figure":
                    for (int i = 0; i < currFigure.figureShape.Length; i++)
                    {
                        GlassArea.Controls.Add(currFigure.figureShape[i].block);
                    }
                    break;
                case "blocks":
                    for (int i = 0; i < blocks.GetLength(0); i++)
                    {
                        for (int j = 0; j < blocks.GetLength(1); j++)
                        {
                            if (blocks[i, j] != null)
                            {
                                GlassArea.Controls.Add(blocks[i, j].block);
                            }
                        }
                    }
                    break;
            }
        }

        public void KeyDown_Event(object sender, KeyEventArgs e)
        {
            if (_game)
            {
                bool _movable = true;
                switch (e.KeyCode)
                {
                    case Keys.D:
                        if (currFigure.figureShape[0].block.Location.X + 40 < GlassArea.Width)
                        {
                            for(int i = 0; i < 3; i++)
                            {
                                if(blocks[currFigure.figureShape[i].block.Location.X / 40 + 1, (currFigure.figureShape[i].block.Location.Y + 20) / 40] != null)
                                {
                                    _movable = false;
                                    break;
                                }
                            }
                            if (_movable)
                            {
                                for (int i = 0; i < 3; i++)
                                {
                                    currFigure.figureShape[i].MoveStep(new Point(40, 0));
                                }
                            }
                        }
                        break;
                    case Keys.A:
                        if (currFigure.figureShape[0].block.Location.X > 0)
                        {
                            for(int i = 0; i < 3; i++)
                            {
                                if(blocks[currFigure.figureShape[i].block.Location.X / 40 - 1, (currFigure.figureShape[i].block.Location.Y + 20) / 40] != null)
                                {
                                    _movable = false;
                                    break;
                                }
                            }
                            if (_movable)
                            {
                                for (int i = 0; i < 3; i++)
                                {
                                    currFigure.figureShape[i].MoveStep(new Point(-40, 0));
                                }
                            }
                        }
                        break;
                    case Keys.W:
                        currFigure.Rotate();
                        break;
                    case Keys.S:
                        if (currFigure.figureShape[2].block.Location.Y + 60 < GlassArea.Height &&
                            blocks[currFigure.figureShape[2].block.Location.X / 40, (currFigure.figureShape[2].block.Location.Y + 20) / 40 + 1] == null)
                        {
                            for (int i = 0; i < currFigure.figureShape.Length; i++)
                            {
                                currFigure.figureShape[i].MoveStep(new Point(0, 20));
                            }
                        }
                        break;
                    case Keys.P:
                        if (IsActive)
                        {
                            Stop();
                        }
                        else
                        {
                            Start();
                        }
                        break;
                }
            }
        }
    }
}



/*
            for (int y = 0; y < blocks.GetLength(1); y++)
            {
                for (int x = 0; x < blocks.GetLength(0); x++)
                {
                    if (blocks[x, y] != null)
                    {
                        var blockImage = blocks[x, y].block.BackgroundImage;
                        if (blockImage != null)
                        {
                            var SubX = x + 1;
                            var Count = 0;
                            if (blocks[SubX, y] != null || SubX<blocks.GetLength(0))
                            {
                                while (SubX < blocks.GetLength(0) && blocks[SubX, y].block.BackgroundImage == blockImage)
                                {
                                    Count++;
                                    SubX++;
                                }
                                if (Count >= 3)
                                {
                                    BlocksToDelArr(blocksToDelete, x, x + Count, y);
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        if (blocksToDelete.Count > 0)
                        {
                            DeleteBlocks(blocksToDelete);
                        }
                    }
                }
            }
*/