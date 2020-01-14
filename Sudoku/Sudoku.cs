using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sudoku
{
    public class Sudoku
    {
        private Sudoku(int[] values)
        {
            Initialize(values.Select(x => (int?)x).ToArray());
        }

        public Sudoku()
        {
            Initialize(new int?[81]);
        }

        public async Task<Sudoku> GetSolutionAsync(CancellationToken cancellationToken)
        {
            int?[] values = Rows.SelectMany(r => r.Cells.Select(c => c.Value)).ToArray();

            CancellationTokenSource completionTokenSource = new CancellationTokenSource();

            int[] solutionValues = await TrySolutionAsync(values, 0, completionTokenSource, cancellationToken);

            if (solutionValues == null)
                return null;

            return new Sudoku(solutionValues);
        }

        void WriteValues(int?[] values)
        {
            for (int r = 0; r < 9; ++r)
            {
                if (r == 3 || r == 6)
                    Debug.WriteLine("-------------------------");

                for (int c = 0; c < 9; ++c)
                {
                    if (c == 3 || c == 6)
                        Debug.Write("|");

                    Debug.Write(" " + values[(r * 9) + c]?.ToString() ?? " " + " ");
                }
                Debug.WriteLine("");

            }
        }

        private async Task<int[]> TrySolutionAsync(int?[] values, int index, CancellationTokenSource completionTokenSource, CancellationToken cancellationToken)
        {


            if (index >= 81)
            {
                // signify we've found a solution
                completionTokenSource.Cancel();

                return values.Select(i => i.Value).ToArray();
            }

            int nextIndex = index + 1;

            if (values[index].HasValue)
            {
                return await TrySolutionAsync(values, nextIndex, completionTokenSource, cancellationToken);
            }
            else
            {
                HashSet<int> possibleValues = new HashSet<int>(Enumerable.Range(1, 9));

                possibleValues.ExceptWith(GetRow(values, index));

                if (cancellationToken.IsCancellationRequested || completionTokenSource.IsCancellationRequested)
                    return null;

                possibleValues.ExceptWith(GetColumn(values, index));

                if (cancellationToken.IsCancellationRequested || completionTokenSource.IsCancellationRequested)
                    return null;

                possibleValues.ExceptWith(GetBox(values, index));

                if (cancellationToken.IsCancellationRequested || completionTokenSource.IsCancellationRequested)
                    return null;

                if (possibleValues.Count == 0)
                    return null;

                List<Task<int[]>> tasks = new List<Task<int[]>>(possibleValues.Count);

                foreach (int value in possibleValues)
                {
                    int?[] valuesCopy = new int?[81];

                    Array.Copy(values, 0, valuesCopy, 0, values.Length);

                    valuesCopy[index] = value;

                    tasks.Add(Task.Run(() =>
                    {
                        //Trace.WriteLine($"{index} / {value}");
                        return TrySolutionAsync(valuesCopy, nextIndex, completionTokenSource, cancellationToken);
                    }));
                }

                await Task.WhenAll(tasks);

                return tasks.Where(t => t.Result != null).Select(t => t.Result).FirstOrDefault();
            }
        }

        public static IEnumerable<int> GetBox(int?[] values, int index)
        {
            int? v;

            int rowStartIndex = (index / 27) * 3;
            int colStartIndex = ((index % 9) / 3) * 3;

            foreach (var r in Enumerable.Range(rowStartIndex, 3))
            {
                foreach (var c in Enumerable.Range(colStartIndex, 3))
                {
                    v = values[(9 * r) + c];

                    if (v.HasValue)
                        yield return v.Value;
                }
            }
        }

        public static IEnumerable<int> GetColumn(int?[] values, int index)
        {
            int columnIndex = index % 9;

            foreach (var i in Enumerable.Range(0, 9).Select(i => columnIndex + (i * 9)))
            {
                if (values[i].HasValue)
                    yield return values[i].Value;
            }
        }

        public static IEnumerable<int> GetRow(int?[] values, int index)
        {
            int rowIndex = index / 9;

            foreach (var i in Enumerable.Range(0, 9).Select(i => (rowIndex * 9) + i))
            {
                if (values[i].HasValue)
                    yield return values[i].Value;
            }

        }

        private void Initialize(int?[] values)
        {
            for (int i = 0; i < 9; ++i)
            {
                Rows[i] = new CellGroup()
                {
                    Index = i
                };

                Columns[i] = new CellGroup()
                {
                    Index = i
                };

                Boxes[i] = new CellGroup()
                {
                    Index = i
                };
            }

            int boxIndex;
            int boxGroupIndex;
            Cell cell;
            for (int column = 0; column < 9; ++column)
            {
                for (int row = 0; row < 9; ++row)
                {
                    boxIndex = ((column / 3) * 3) + (row / 3);
                    boxGroupIndex = ((column % 3) * 3) + (row % 3);

                    cell = new Cell();

                    cell.ColumnIndex = column;
                    cell.RowIndex = row;
                    cell.BoxIndex = boxIndex;

                    Rows[row].Cells[column] = cell;
                    Columns[column].Cells[row] = cell;
                    Boxes[boxIndex].Cells[boxGroupIndex] = cell;

                    cell.Row = Rows[row];
                    cell.Column = Columns[column];
                    cell.Box = Boxes[boxIndex];

                    cell.Value = values[row * 9 + column];
                }
            }
        }

        public CellGroup[] Rows { get; } = new CellGroup[9];
        public CellGroup[] Columns { get; } = new CellGroup[9];
        public CellGroup[] Boxes { get; } = new CellGroup[9];

        public CellGroup this[int row]
        {
            get => this.Rows[row];
        }
    }
}