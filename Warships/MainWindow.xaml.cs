using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Warships
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Board _boardPlayer1;
        private Board _boardPlayer2; // For future players
        private const int RectangleSize = 80;
        
        public MainWindow()
        {
            InitializeComponent();
            CreateBoard();
        }

        private void CreateBoard()
        {
            var height = 10;
            var width = 10;
            _boardPlayer1 = new Board(height, width);
            RedrawBoard(_boardPlayer1);
        }
            
        private void RedrawBoard(Board board)
        {
            for (int i = 0; i < board.RedrawBuffer.Count; i++)
            {
                var y = board.RedrawBuffer[i].Item1;
                var x = board.RedrawBuffer[i].Item2;
                
                var id = y * board.BoardProperties.Width + x;

                SolidColorBrush colorBrush = board.DrawnRectAt(y, x).RectColor();
                BoardRectangle boardRectangle = new BoardRectangle(board, RectangleSize, colorBrush, id, RedrawBoard);
                
                Canvas.SetTop(boardRectangle.Rectangle, y * RectangleSize);
                Canvas.SetLeft(boardRectangle.Rectangle, x * RectangleSize);

                foreach (UIElement rect in Canvas.Children)
                {
                    if (rect.Uid == id.ToString())
                    {
                        Canvas.Children.Remove(rect);
                        break;
                    }
                }
                
                Canvas.Children.Add(boardRectangle.Rectangle);
            }
            board.RedrawBuffer.Clear();
        }
    }
}