using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sudoku.App.Mobile
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            Entry tb;

            BoxView border;

            var vm = new MainPageViewModel();

            this.BindingContext = vm;

            
            for (int i = 0; i < 9; ++i)
            {
                SudokuGrid.ColumnDefinitions.Add(new ColumnDefinition());
                SudokuGrid.RowDefinitions.Add(new RowDefinition());

                border = new BoxView
                {
                    BackgroundColor = Color.Red.MultiplyAlpha(0.3),
                    BindingContext = vm.Sudoku.Rows[i]
                };

                SudokuGrid.Children.Add(border);
                Grid.SetRow(border, i);
                Grid.SetColumnSpan(border, 9);
                border.SetBinding(BoxView.IsVisibleProperty, new Binding
                {
                     Path= $"IsValid",
                     Converter = InvertBoolConverter.Instance
                });

                border = new BoxView
                {
                    BackgroundColor = Color.Red.MultiplyAlpha(0.3),
                    BindingContext = vm.Sudoku.Columns[i]
                };

                SudokuGrid.Children.Add(border);
                Grid.SetColumn(border, i);
                Grid.SetRowSpan(border, 9);
                border.SetBinding(BoxView.IsVisibleProperty, new Binding
                {
                    Path = $"IsValid",
                    Converter = InvertBoolConverter.Instance
                });


                border = new BoxView
                {
                    BackgroundColor = Color.Red.MultiplyAlpha(0.3),
                    BindingContext = vm.Sudoku.Boxes[i]
                };

                SudokuGrid.Children.Add(border);
                Grid.SetRowSpan(border, 3);
                Grid.SetColumnSpan(border, 3);
                Grid.SetRow(border, (i / 3) * 3);
                Grid.SetColumn(border, (i % 3) * 3);

                border.SetBinding(BoxView.IsVisibleProperty, new Binding
                {
                    Path = $"IsValid",
                    Converter = InvertBoolConverter.Instance
                });
            }

            for (int r = 0; r < 9; ++r)
            {
                for (int c = 0; c < 9; ++c)
                {
                    tb = new Entry
                    {
                        Margin = new Thickness(5),
                        Keyboard = Keyboard.Numeric,
                        BackgroundColor = Color.White.MultiplyAlpha(0.8),
                        BindingContext = vm.Sudoku[r].Cells[c]
                    };

                    SudokuGrid.Children.Add(tb);
                    Grid.SetRow(tb, r);
                    Grid.SetColumn(tb, c);
                    tb.SetBinding(Entry.TextProperty, new Binding()
                    {
                        Path = "Value"
                    });
                }
            }
        }
    }
}
