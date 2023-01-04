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
            var strokeColor = Brushes.Azure;
            
            for (int i = 0; i < _board.RedrawBuffer.Count; i++)
            {
                var y = _board.RedrawBuffer[i].Item1;
                var x = _board.RedrawBuffer[i].Item2;
                
                var id = y * _board.Width + x;
                
                Rectangle newRectangle = new Rectangle
                {
                    Width = rectangleSize,
                    Height = rectangleSize,
                    Fill = _board.DrawnRectAt(y, x).RectColor(),
                    StrokeThickness = 1,
                    Stroke = strokeColor,
                    Uid = id.ToString()
                };
                
                newRectangle.MouseEnter += (sender, e) => OnMouseEnter(sender, e, id);
                newRectangle.MouseLeave += (sender, e) => OnMouseLeave(sender, e, id);
                newRectangle.MouseLeftButtonDown += (sender, e) => OnMouseLeftButtonDown(sender, e, id);
                
                Canvas.SetTop(newRectangle, y * rectangleSize);
                Canvas.SetLeft(newRectangle, x * rectangleSize);

                foreach (UIElement rect in Canvas.Children)
                {
                    if (rect.Uid == id.ToString())
                    {
                        Canvas.Children.Remove(rect);
                        break;
                    }
                }
                
                Canvas.Children.Add(newRectangle);
            }
            _board.RedrawBuffer.Clear();
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