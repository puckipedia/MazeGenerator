using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DrawMaze
{
    public partial class Form1 : Form
    {
        public Maze m;

        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            DrawMaze();
            base.OnPaint(e);
        }

        protected override void OnClientSizeChanged(EventArgs e)
        {
            int width = (int)Math.Floor(this.ClientSize.Width / (double)Robot.mazesize / 2) * 2;
            int height = (int)Math.Floor(this.ClientSize.Height / (double)Robot.mazesize / 2) * 2;

            if (width % 2 == 0)
                width--;
            if (height % 2 == 0)
                height--;

            m = new Maze(width, height);
            m.RandomlyGenerate();
            Refresh();
            base.OnClientSizeChanged(e);
        }

        public void DrawMaze()
        {
            color_black = new SolidBrush(Color.Black);
            color_white = new SolidBrush(Color.White);
            color_green = new SolidBrush(Color.FromArgb(92,248,49));
            color_purple = new SolidBrush(Color.Purple);
            graphics = this.CreateGraphics();
            var index = m.RandomGenStep();
            while (index != null)
            {
                int z = 0;
                int theta = 0;
                for (int x = index.Item1 - 1; x <= index.Item1 + 1; x++)
                {
                    for (int y = index.Item2 - 1; y <= index.Item2 + 1; y++)
                    {
                        DrawMazeBlock(x, y);
                    }
                }

                index = m.RandomGenStep();
            }

            int rx = m.g.Random.Next(m.Width);
            int ry = m.g.Random.Next(m.Height);
            while (m.MazeData[rx, ry]) {
                rx = m.g.Random.Next(m.Width);
                ry = m.g.Random.Next(m.Height);
            }

            var robot = new Robot(m, 0, 1, graphics);
            robot.TargetLocation = new Tuple<int, int>(rx, ry);

            robot.TurnRight();

            IEnumerable<Tuple<int, int>> path = null;

            try
            {
                robot.Copy();
            }
            catch(Robot.FoundPathException e)
            {
                path = e.Path;
            }
            if (path == null)
                return;

            foreach (var point in path)
            {
                DrawMazeBlock(color_green, point.Item1, point.Item2);
            }
        }

        private SolidBrush color_purple;
        private SolidBrush color_black;
        private SolidBrush color_white;
        private SolidBrush color_green;
        private Graphics graphics;

        public void DrawMazeBlock(int x, int y)
        {   
            if (x >= m.Width || y >= m.Height || x < 0 || y < 0)
                return;

            if (m.g.Visited[x, y] || m.MazeData[x, y])
                graphics.FillRectangle(m.MazeData[x, y] ? color_black : color_white, x * Robot.mazesize, y * Robot.mazesize, Robot.mazesize, Robot.mazesize);
        }

        public void DrawMazeBlock(SolidBrush color, int x, int y)
        {
            if (x >= m.Width || y >= m.Height || x < 0 || y < 0)
                return;

            graphics.FillRectangle(color, x * Robot.mazesize, y * Robot.mazesize, Robot.mazesize, Robot.mazesize);
        }
    }
}
