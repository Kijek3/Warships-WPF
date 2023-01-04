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
        private Board _board;
        
        public MainWindow()
        {
            InitializeComponent();
            CreateBoard();
        }

        private void CreateBoard()
        {
            var height = 8;
            var width = 10;
            _board = new Board(height, width);
            RedrawBoard();
        }

        private void RedrawBoard()
        {
            var rectangleSize = 80;
            var globalId = 0;
            var strokeColor = Brushes.Azure;
            
            Canvas.Children.Clear();
            
            for (int i = 0; i < _board.Height; i++)
            {
                for (int j = 0; j < _board.Width; j++)
                {
                    Rectangle newRectangle = new Rectangle
                    {
                        Width = rectangleSize,
                        Height = rectangleSize,
                        Fill = _board.DrawnRectAt(i, j).RectColor(),
                        StrokeThickness = 1,
                        Stroke = strokeColor
                    };
                    
                    var id = globalId;
                    newRectangle.MouseEnter += (sender, e) => OnMouseEnter(sender, e, id);
                    newRectangle.MouseLeave += (sender, e) => OnMouseLeave(sender, e, id);
                    newRectangle.MouseLeftButtonDown += (sender, e) => OnMouseLeftButtonDown(sender, e, id);
                    
                    Canvas.SetTop(newRectangle, i * rectangleSize);
                    Canvas.SetLeft(newRectangle, j * rectangleSize);
                    
                    Canvas.Children.Add(newRectangle);
                    globalId += 1;
                }
            }
        }
        
        private void OnMouseEnter(object sender, MouseEventArgs e, int id)
        {
            if (e.OriginalSource is not Rectangle) return;
            
            var y = id / _board.Width;
            var x = id % _board.Width;
            
            if (_board.RectAt(y, x) == EBoardRect.Empty)
            {
                _board.SetDrawnRectAt(y, x, EBoardRect.PlacementReady);
                RedrawBoard();
            } 
            else if (_board.RectAt(y, x) == EBoardRect.Ship)
            {
                _board.SetDrawnRectAt(y, x, EBoardRect.PlacementError);
                RedrawBoard();
            }
        }
        
        private void OnMouseLeave(object sender, MouseEventArgs e, int id)
        {
            if (e.OriginalSource is not Rectangle) return;
            
            var y = id / _board.Width;
            var x = id % _board.Width;

            if (_board.RectAt(y, x) == EBoardRect.Empty)
            {
                _board.SetDrawnRectAt(y, x, EBoardRect.Empty);
                RedrawBoard();
            } 
            else if (_board.RectAt(y, x) == EBoardRect.Ship)
            {
                _board.SetDrawnRectAt(y, x, EBoardRect.Ship); 
                RedrawBoard();
            }
        }

        private void OnMouseLeftButtonDown(object sender, MouseEventArgs e, int id)
        {
            if (e.OriginalSource is not Rectangle) return;
            
            var y = id / _board.Width;
            var x = id % _board.Width;

            if (_board.CanPlaceAt(y, x))
            {
                _board.SetRectAt(y, x, EBoardRect.Ship);
                RedrawBoard();
            }
        }
    }
}