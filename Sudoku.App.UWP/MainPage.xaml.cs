using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Sudoku.App.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.Puzzle = new Sudoku();

            this.SudokuGrid.DataContext = this.Puzzle;

            TextBox tb;

            Border border;

            for(int i = 0; i < 9; ++i)
            {
                SudokuGrid.ColumnDefinitions.Add(new ColumnDefinition());
                SudokuGrid.RowDefinitions.Add(new RowDefinition());

                border = new Border
                {
                    BorderBrush = new SolidColorBrush(Windows.UI.Colors.Red),
                    BorderThickness = new Thickness(3),
                    CornerRadius = new CornerRadius(5)
                };

                //SudokuGrid.Children.Add(border);
                Grid.SetRow(border, i);
                Grid.SetColumnSpan(border, 9);
                

                border = new Border
                {
                    BorderBrush = new SolidColorBrush(Windows.UI.Colors.Blue),
                    BorderThickness = new Thickness(3),
                    CornerRadius = new CornerRadius(5)
                };

                //SudokuGrid.Children.Add(border);
                Grid.SetColumn(border, i);
                Grid.SetRowSpan(border, 9);

                border = new Border
                {
                    BorderBrush = new SolidColorBrush(Windows.UI.Colors.Black),
                    BorderThickness = new Thickness(3),
                    CornerRadius = new CornerRadius(0)
                };

                SudokuGrid.Children.Add(border);
                Grid.SetRowSpan(border, 3);
                Grid.SetColumnSpan(border, 3);
                Grid.SetRow(border, (i / 3) * 3);
                Grid.SetColumn(border, (i % 3) * 3);

            }

            for (int r = 0; r < 9; ++r)
            {

                for(int c = 0; c < 9; ++c)
                {
                    tb = new TextBox();
                    SudokuGrid.Children.Add(tb);
                    tb.Margin = new Thickness(5);
                    Grid.SetRow(tb, r);
                    Grid.SetColumn(tb, c);

                    tb.SetBinding(TextBox.TextProperty, new Binding()
                    {
                        Path = new PropertyPath($"Rows[{r}].Cells[{c}].Value")
                    });
                }
            }
        }

    }
}
