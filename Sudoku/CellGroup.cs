using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Sudoku
{

    public class CellGroup : Observable
    {
        public Cell[] Cells { get; } = new Cell[9];

        public int? this[int index]
        {
            get => this.Cells[index].Value;
            set => this.Cells[index].Value = value;
        }

        private bool _isValid = true;

        public bool IsValid { get => _isValid; private set => Set(ref _isValid, value); }

        public int Index { get; internal set; }

        internal void UpdateIsValid()
        {
            HashSet<int> enteredValues = new HashSet<int>();

            for (int i = 0; i < Cells.Length; ++i)
            {
                if (Cells[i]?.Value.HasValue == true)
                {
                    if (enteredValues.Contains(Cells[i].Value.Value))
                    {
                        IsValid = false;
                        return;
                    }

                    enteredValues.Add(Cells[i].Value.Value);
                }
            }

            IsValid = true;
        }
    }
}