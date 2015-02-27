using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DrawMaze
{
    public class Robot
    {
        public static int mazesize = 25;

        enum Point {
            Left,
            Up,
            Right,
            Down
        };

        public bool WallInFront
        {
            get
            {
                try
                {
                    switch (_orientation)
                    {
                        case Point.Left:
                            return _maze.MazeData[_x - 1, _y];
                        case Point.Right:
                            return _maze.MazeData[_x + 1, _y];
                        case Point.Up:
                            return _maze.MazeData[_x, _y - 1];
                        default:
                            return _maze.MazeData[_x, _y + 1];
                    }
                }
                catch (IndexOutOfRangeException e)
                {
                    return true;
                }
            }
        }

        public void Copy()
        {
            if (WallInFront)
                return;

            var rbt = new Robot(this);
            rbt.MoveForward();
            rbt.Navigate();
        }

        private Robot(Robot other)
        {
            _path = new List<Tuple<int, int>>(other.Path);
            _x = other._x;
            _y = other._y;
            _maze = other._maze;
            _target = other._target;
            _start_orientation = _orientation = other._orientation;
            _graphics = other._graphics;
            _brush = other._brush;
            _random = other._random;
            RandomParse();
            _ori = 0;
        }

        public void RandomlyTurn()
        {
            switch (_ori++)
            {
                case 0:
                    _orientation = _or1;
                    break;
                case 1:
                    _orientation = _or2;
                    break;
                case 2:
                    _orientation = _or3;
                    break;
            }
        }

        public void MoveBackward()
        {

            switch (_orientation)
            {
                case Point.Left:
                    _x++;
                    break;
                case Point.Right:
                    _x--;
                    break;
                case Point.Up:
                    _y++;
                    break;
                default:
                    _y--;
                    break;
            }
        }

        public void MoveForward()
        {
            Thread.Sleep(10);
            switch (_orientation)
            {
                case Point.Left:
                    _x--;
                    break;
                case Point.Right:
                    _x++;
                    break;
                case Point.Up:
                    _y--;
                    break;
                default:
                    _y++;
                    break;
            }

            _path.Add(new Tuple<int, int>(_x, _y));

            if (_x < 0 || _x >= _maze.Width || _y < 0 || _y >= _maze.Height)
                Debug.Assert(1 == 0);

            _maze.g.Visited[_x, _y] = false;

            _graphics.FillRectangle(_brush, _x * mazesize + (mazesize / 4), _y * mazesize + (mazesize / 4), mazesize / 2, mazesize / 2);
        }

        public bool FoundEnd
        {
            get
            {
                if (_maze.MazeData[_maze.Width - 2, _maze.Height - 2])
                {
                    return _x == _maze.Width - 3 && _y == _maze.Height - 2;
                } else
                    return _x == _maze.Width - 2 && _y == _maze.Height - 2;
            }
        }

        public Robot(Maze m, int x, int y, Graphics g)
        {
            _maze = m;
            _x = x;
            _y = y;
            _orientation = Point.Up;
            _start_orientation = _orientation;
            _graphics = g;
            _brush = new SolidBrush(Color.FromArgb(128, 128, 128));
            _target = null;
            _path = new List<Tuple<int, int>>();
            _random = new Random();

            RandomParse();
            _ori = 0;
        }

        private void RandomParse()
        {
            List<Point> orientations = new List<Point>();
            while (orientations.Count < 3)
            {
                Point or = (Point)(((int)_orientation + 3 + _random.Next(3)) % 4);
                if (!orientations.Contains(or))
                    orientations.Add(or);
            }

            _or1 = orientations[0];
            _or2 = orientations[1];
            _or3 = orientations[2];
        }

        public IEnumerable<Tuple<int, int>> Path
        {
            get
            {
                return _path;
            }
        }

        public Tuple<int, int> Location
        {
            get
            {
                return new Tuple<int, int>(_x, _y);
            }
        }

        private Tuple<int, int> _target;

        private List<Tuple<int, int>> _path;

        public Tuple<int, int> TargetLocation
        {
            get
            {
                return _target;
            }

            set
            {
                _target = value;
            }
        }

        public void TurnLeft()
        {
            _orientation = (Point)((3 + (int)_orientation) % 4);
        }

        public void TurnRight()
        {
            _orientation = (Point)((5 + (int)_orientation) % 4);
        }

        public class FoundPathException : Exception
        {
            public IEnumerable<Tuple<int, int>> Path;
        }

        static public void SignalFoundPath(IEnumerable<Tuple<int, int>> path)
        {
            throw new FoundPathException() { Path = path };
        }











































        public void Navigate()
        {
            if (FoundEnd)
                SignalFoundPath(Path);

            TurnLeft();

            for (var i = 0; i < 3; i++)
            {
                Copy();
                TurnRight();
            }
        }































        






















        private int _x;
        private int _y;

        private Point _or1;
        private Point _or2;
        private Point _or3;
        private int _ori;

        private Point _orientation;
        private Random _random;
        private Point _start_orientation;
        private Maze _maze;
        private Graphics _graphics;
        private SolidBrush _brush;
    }

    class MazeFramework
    {
    }
}
