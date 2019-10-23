using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sudoku.App.Mobile
{
    public class MainPageViewModel : Model
    {
        public MainPageViewModel()
        {
            this.SolveCommand = new Command(async () => await SolveAsync());
            this.ClearPuzzleCommand = new Command(ClearPuzzle);
            this.NewPuzzleCommand = new Command(NewPuzzle);
        }

        public Sudoku Sudoku { get; } = new Sudoku();
        public Command SolveCommand { get; }
        public Command ClearPuzzleCommand { get; }
        public Command NewPuzzleCommand { get; }

        async Task SolveAsync()
        {
            var solution = await Sudoku.GetSolutionAsync(CancellationToken.None);

            if (solution != null)
            {
                for(int r = 0; r < solution.Rows.Length; ++r)
                {
                    for(int c = 0; c < solution.Columns.Length; ++c)
                    {
                        Sudoku.Rows[r].Cells[c].Value = solution.Rows[r].Cells[c].Value;
                    }
                }
            }
        }

        void NewPuzzle()
        {

        }

        void ClearPuzzle()
        {
            foreach(var cell in Sudoku.Rows.SelectMany(r => r.Cells))
            {
                cell.Value = null;
            }
        }
    }
}
