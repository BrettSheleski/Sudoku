using System;
using System.Threading;
using System.Threading.Tasks;
using c = System.Console;

namespace Sudoku.App.Console
{
    class ConsoleSudoku
    {
        const char topLeftCorner = '╔';
        const char bottomLeftCorner = '╚';
        const char topRightCorner = '╗';
        const char bottomRightCorner = '╝';
        const char vertical = '║';
        const char horizontal = '═';
        const char topCap = '╦';
        const char bottomCap = '╩';
        const char leftCap = '╠';
        const char rightCap = '╣';
        const char cross = '╬';
        const char space = ' ';
        const char tab = '\t';

        public Sudoku Sudoku { get; private set; }


        void NewPuzzle()
        {
            int row = this.SelectedCell.RowIndex, col = this.SelectedCell.ColumnIndex;

            this.Sudoku = Sudoku.NewPuzzle();

            this.SelectedCell = this.Sudoku.Rows[row].Cells[col];
        }

        public Cell SelectedCell { get; private set; }
        public bool IsSolving { get => _isSolving; set => _isSolving = value; }

        public ConsoleSudoku()
        {
            this.Sudoku = Sudoku.NewPuzzle();

            this.SelectedCell = Sudoku.Rows[0].Cells[0];
        }

        public void MoveUp()
        {
            if (SelectedCell.RowIndex > 0)
            {
                SelectedCell = Sudoku.Rows[SelectedCell.RowIndex - 1].Cells[SelectedCell.ColumnIndex];
            }
        }

        public void MoveDown()
        {
            if (SelectedCell.RowIndex < 8)
            {
                SelectedCell = Sudoku.Rows[SelectedCell.RowIndex + 1].Cells[SelectedCell.ColumnIndex];
            }
        }

        public void MoveLeft()
        {
            if (SelectedCell.ColumnIndex > 0)
            {
                SelectedCell = Sudoku.Rows[SelectedCell.RowIndex].Cells[SelectedCell.ColumnIndex - 1];
            }
        }

        public void MoveRight()
        {
            if (SelectedCell.ColumnIndex < 8)
            {
                SelectedCell = Sudoku.Rows[SelectedCell.RowIndex].Cells[SelectedCell.ColumnIndex + 1];
            }
        }


        public void DrawBoard()
        {
            c.SetCursorPosition(0, 0);
            c.CursorVisible = false;

            c.WriteLine();
            c.WriteLine("Move Up/Down/Left/Right using the arrow keys.");
            c.WriteLine("Enter a number to input the value desired.");
            c.WriteLine();

            Cell cell;

            int dings;

            for (int row = 0; row < 9; ++row)
            {
                
                switch (row)
                {
                    case 0:
                        c.Write(topLeftCorner);
                        WriteRepeat(horizontal, 7);
                        c.Write(topCap);
                        WriteRepeat(horizontal, 7);
                        c.Write(topCap);
                        WriteRepeat(horizontal, 7);
                        c.Write(topRightCorner);


                        

                        c.WriteLine();
                        break;

                    case 3:
                        c.Write(leftCap);
                        WriteRepeat(horizontal, 7);
                        c.Write(cross);
                        WriteRepeat(horizontal, 7);
                        c.Write(cross);
                        WriteRepeat(horizontal, 7);
                        c.Write(rightCap);
                        c.Write(tab);
                        c.Write(bottomLeftCorner);
                        WriteRepeat(horizontal, 19);
                        c.Write(bottomRightCorner);
                        c.WriteLine();
                        break;
                    case 6:
                        c.Write(leftCap);
                        WriteRepeat(horizontal, 7);
                        c.Write(cross);
                        WriteRepeat(horizontal, 7);
                        c.Write(cross);
                        WriteRepeat(horizontal, 7);
                        c.WriteLine(rightCap);
                        break;
                }


                for (int col = 0; col < 9; ++col)
                {
                    dings = 0;

                    if (!this.Sudoku.Rows[row].IsValid)
                        ++dings;

                    if (!this.Sudoku.Columns[col].IsValid)
                        ++dings;

                    if (!this.Sudoku.Rows[row].Cells[col].Box.IsValid)
                        ++dings;

                  


                    switch (col)
                    {
                        case 0:
                            c.Write(vertical);
                            break;
                        case 3:
                        case 6:
                            c.Write(space);
                            c.Write(vertical);
                            break;
                    }

                    c.Write(" ");

                    

                    cell = this.Sudoku.Rows[row].Cells[col];

                    

                    if (cell == SelectedCell)
                    {
                        c.BackgroundColor = System.ConsoleColor.White;
                        c.ForegroundColor = System.ConsoleColor.Black;
                    }
                    else
                    {
                        c.BackgroundColor = System.ConsoleColor.Black;
                        c.ForegroundColor = cell.IsClue ? System.ConsoleColor.DarkGray : System.ConsoleColor.White;
                    }

                    switch (dings)
                    {
                        case 1:
                            c.ForegroundColor = System.ConsoleColor.Red;
                            break;
                        case 2:
                            c.ForegroundColor = System.ConsoleColor.Magenta;
                            break;
                        case 3:
                            c.ForegroundColor = System.ConsoleColor.DarkRed;
                            break;
                    }

                    if (cell.Value.HasValue)
                    {
                        c.Write(cell.Value.ToString());
                    }
                    else
                    {
                        c.Write(' ');
                    }

                    c.BackgroundColor = System.ConsoleColor.Black;
                    c.ForegroundColor = System.ConsoleColor.White;
                }

                

                c.Write(space);
                c.Write(vertical);

                switch (row)
                {
                    case 0:
                        c.Write(tab);
                        c.Write("  Available Numbers");
                        break;
                    case 1:
                        c.Write(tab);
                        c.Write(topLeftCorner);
                        WriteRepeat(horizontal, 19);
                        c.Write(topRightCorner);
                        break;
                    
                    case 2:
                        c.Write(tab);
                        c.Write(vertical);
                        c.Write(space);
                        var candidates = SelectedCell.GetCandidates();

                        for (int i = 1; i < 10; ++i)
                        {
                            c.ForegroundColor = candidates.Contains(i) ? System.ConsoleColor.White : System.ConsoleColor.DarkGray;

                            c.Write(i);

                            c.ForegroundColor = System.ConsoleColor.White;
                            c.Write(space);
                        }
                        c.Write(vertical);
                        break;
                    case 3:
                        
                        break;
                }

                c.WriteLine();
            }

            c.Write(bottomLeftCorner);
            WriteRepeat(horizontal, 7);
            c.Write(bottomCap);
            WriteRepeat(horizontal, 7);
            c.Write(bottomCap);
            WriteRepeat(horizontal, 7);
            c.WriteLine(bottomRightCorner);

            c.WriteLine();
            c.WriteLine("Q: Quit  N: New Game  S: Solve");

        }

        static void WriteRepeat(char ch, int times)
        {
            while (times > 0)
            {
                c.Write(ch);
                --times;
            }
        }

        public async Task GetInputAsync()
        {
            var key = c.ReadKey(true);

            switch (key.KeyChar)
            {
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    SelectedCell.Value = int.Parse(key.KeyChar.ToString());
                    break;
                case 'Q':
                case 'q':
                    return;
                case 'S':
                case 's':
                    await SolvePuzzleAsync();
                    break;

                case 'N':
                case 'n':
                    NewPuzzle();
                    break;
            }

            switch (key.Key)
            {
                case System.ConsoleKey.LeftArrow:
                    MoveLeft();
                    break;
                case System.ConsoleKey.RightArrow:
                    MoveRight();
                    break;
                case System.ConsoleKey.DownArrow:
                    MoveDown();
                    break;
                case System.ConsoleKey.UpArrow:
                    MoveUp();
                    break;
                case System.ConsoleKey.Delete:
                case System.ConsoleKey.Backspace:
                    SelectedCell.Value = null;
                    break;
            }

            DrawBoard();
            await GetInputAsync();
        }

        private bool _isSolving;

        private async Task SolvePuzzleAsync()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(1));

            var solution = await this.Sudoku.GetSolutionAsync(cancellationTokenSource.Token);

            

            if (solution != null)
            {
//                int row = this.SelectedCell.RowIndex, col = this.SelectedCell.ColumnIndex;


                for(int row = 0; row < 9; ++row)
                {
                    for(int col = 0; col < 0; ++col)
                    {
                        this.Sudoku[row][col] = solution[row][col];
                    }
                }


                //this.SelectedCell = this.Sudoku.Rows[row].Cells[col];
            }
        }
    }
}
