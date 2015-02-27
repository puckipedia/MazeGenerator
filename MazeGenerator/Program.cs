using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Maze m = new Maze(200, 200);
            m.RandomlyGenerate();

            for (var i = 0; i < m.Height; i++)
            {
                for (var j = 0; j < m.Width; j++)
                {
                    Console.Write(m.MazeData[j, i] ? '#' : ' ');
                }

                Console.WriteLine();
            }

            Console.ReadLine();
        }
    }
}
