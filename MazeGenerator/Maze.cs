using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGenerator
{
    public class Maze
    {
        public bool[,] MazeData
        {
            get;
            set;
        }

        public int Width
        {
            get;
            private set;
        }

        public int Height
        {
            get;
            private set;
        }

        public enum Orientation
        {
            ToLeft,
            ToRight,
            ToTop,
            ToBottom
        }

        /// <summary>
        /// Creates a maze of wxh
        /// </summary>
        /// <param name="w">The width of the maze</param>
        /// <param name="h">The height of the maze</param>
        public Maze(int w, int h)
        {
            MazeData = new bool[w, h];
            for (var x = 0; x < w; x++)
                for (var y = 0; y < h; y++)
                    MazeData[x, y] = true;

            Width = w;
            Height = h;
        }

        public void RandomlyGenerate()
        {
            MazeGenerator g = new MazeGenerator();
            g.Visited = new bool[Width, Height];
            for (var i = 0; i < Width; i++)
                for (var j = 0; j < Height; j++)
                    g.Visited[i, j] = false;
            g.Random = new Random();

            List<Tuple<int, int, int, int>> ToTrack = new List<Tuple<int, int, int, int>>()
            {
                new Tuple<int, int, int, int>(g.Random.Next(Width), g.Random.Next(Height), -1, -1)
            };

            while (ToTrack.Count > 0)
            {
                var track = ToTrack[ToTrack.Count - 1];
                ToTrack.Remove(track);
                int x = track.Item1;
                int y = track.Item2;

                List<Tuple<int, int, int, int>> Neighbors = new List<Tuple<int, int, int, int>>(4)
                {
                    new Tuple<int, int, int, int>(x - 2, y, x - 1, y),
                    new Tuple<int, int, int, int>(x + 2, y, x + 1, y),
                    new Tuple<int, int, int, int>(x, y + 2, x, y + 1),
                    new Tuple<int, int, int, int>(x, y - 2, x, y - 1)
                };

                MazeData[x, y] = false;

                if (g.Visited[x, y])
                    continue;

                g.Visited[x, y] = true;

                if (track.Item3 >= 0)
                    MazeData[track.Item3, track.Item4] = false;

                while (Neighbors.Count > 0)
                {
                    var nb = g.Random.Next(Neighbors.Count);
                    var neighbor = Neighbors[nb];
                    Neighbors.Remove(neighbor);
                    if (neighbor.Item1 < 0 || neighbor.Item2 < 0 || neighbor.Item1 >= Width || neighbor.Item2 >= Height)
                        continue; // out of bounds

                    if (!g.Visited[neighbor.Item1, neighbor.Item2])
                    {
                        ToTrack.Add(neighbor);
                    }
                }
                _DrawMaze(x, y);
            }
        }

        private class MazeGenerator {
            public bool[,] Visited;
            public Random Random;
        }

        private void _TrackMaze(MazeGenerator gen, int x, int y)
        {
        }

        private void _DrawMaze(int cursorx = -1, int cursory = -1)
        {
#if DEBUG != DEBUG
            for (var i = 0; i < Height; i++)
            {
                for (var j = 0; j < Width; j++)
                {
                    if (j == cursorx && i == cursory)
                        Console.Write(MazeData[j, i] ? 'O' : '.');
                    else
                        Console.Write(MazeData[j, i] ? '#' : ' ');
                }
                Console.WriteLine();
            }
            Console.ReadLine();
#endif
        }
    }
}
