using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Sudoku
{
    public class Cell : Observable
    {
        private int? _value;

        public int? Value
        {
            get => _value;
            set
            {
                _value = value;
                Box.UpdateIsValid();
                Row.UpdateIsValid();
                Column.UpdateIsValid();
                OnPropertyChanged();
            }
        }

        public CellGroup Row { get; internal set; }

        public CellGroup Column { get; internal set; }

        public CellGroup Box { get; internal set; }

        public HashSet<int> GetCandidates()
        {
            HashSet<int> candidates = new HashSet<int>(Enumerable.Range(1, 9));

            if (Value.HasValue)
                candidates.Remove(Value.Value);

            foreach (var val in Box.Cells.Where(c => c.Value.HasValue).Select(c => c.Value.Value))
            {
                candidates.Remove(val);
            }

            foreach (var val in Row.Cells.Where(c => c.Value.HasValue).Select(c => c.Value.Value))
            {
                candidates.Remove(val);
            }

            foreach (var val in Column.Cells.Where(c => c.Value.HasValue).Select(c => c.Value.Value))
            {
                candidates.Remove(val);
            }

            return candidates;
        }


        public int RowIndex { get; internal set; }
        public int ColumnIndex { get; internal set; }
        public int BoxIndex { get; internal set; }
    }
}