using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sudoku.Tests
{
    [TestClass]
    public class SudokuTests
    {
        [TestMethod]
        public void Sudoku_InitializedRowsAndCellsCorrect()
        {
            // Setup
            Sudoku s = new Sudoku();


            // Act
            int value = 0;
            for (int c = 0; c < 9; ++c)
            {
                for (int r = 0; r < 9; ++r)
                {
                    ++value;

                    s.Columns[c].Cells[r].Value = value;
                }
            }

            // Verify

            int[][] rows = Enumerable.Range(0, 9).Select(r =>
                Enumerable.Range(1, 9).Select(c => (r * 9) + c).ToArray()
            ).ToArray();

            int[][] cols = Enumerable.Range(1, 9).Select(c =>
                Enumerable.Range(0, 9).Select(r => (r * 9) + c).ToArray()
            ).ToArray();

            int[][] boxes = new int[][]
            {
                new int[]{1, 2, 3,
                    10, 11, 12,
                    19, 20, 21},

                new int[]{4, 5, 6,
                    13, 14, 15,
                    22, 23, 24 },

                new int[]{7, 8, 9,
                    16, 17, 18,
                    25, 26, 27 },



                new int[]{28, 29, 30,
                    37, 38, 39,
                    46, 47, 48},

                new int[]{31, 32, 33,
                    40, 41, 42,
                    49, 50, 51},

                new int[]{34, 35, 36,
                    43, 44, 45,
                    52, 53, 54 },

                new int[]{55, 56, 57,
                    64, 65, 66,
                    73, 74, 75},

                new int[]{58, 59, 60,
                    67, 68, 69,
                    76, 77, 78},

                new int[]{61, 62, 63,
                    70, 71, 72,
                    79, 80, 81 },
            };


            for (int r = 0; r < rows.Length; ++r)
            {
                //Trace.WriteLine($"[{string.Join(", ", boxes[r])}]");

                //Trace.WriteLine($"[{string.Join(", ", s.Boxes[r].Cells.Select(c => c.Value))}]");


                for (int c = 0; c < rows[r].Length; ++c)
                {
                    Assert.IsTrue(rows[r][c] == s.Columns[r].Cells[c].Value);
                    Assert.IsTrue(cols[c][r] == s.Rows[c].Cells[r].Value);
                    Assert.IsTrue(boxes[r][c] == s.Boxes[r].Cells[c].Value);
                }
            }

        }

        [TestMethod]
        public async Task Sudoku_GetSolution_Works()
        {
            // setup
            Sudoku s = new Sudoku();

            // act
            Sudoku solution = await s.GetSolutionAsync(CancellationToken.None);

            // verify
            Assert.IsNotNull(solution);
        }


    }
}
