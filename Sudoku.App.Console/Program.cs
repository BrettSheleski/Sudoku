using System;
using System.Threading.Tasks;

namespace Sudoku.App.Console
{
    class Program
    {

        static async Task Main(string[] args)
        {
            ConsoleSudoku p = new ConsoleSudoku();

            p.DrawBoard();

            await p.GetInputAsync();
        }
    }
}
