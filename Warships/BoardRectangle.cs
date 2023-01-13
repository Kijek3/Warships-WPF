using System;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Warships;

public class BoardRectangle
{
    public BoardRectangle(Board board, int rectangleSize, SolidColorBrush colorBrush, int id, Action<Board> redrawBoard)
    {
        _board = board;
        Id = id;
        RedrawBoard = redrawBoard;
        Rectangle = new Rectangle
        {
            Width = rectangleSize,
            Height = rectangleSize,
            Fill = colorBrush,
            StrokeThickness = 1,
            Stroke = StrokeColor,
            Uid = id.ToString(),
        };
        
        Rectangle.MouseEnter += OnMouseEnter;
        Rectangle.MouseLeave += OnMouseLeave;
        Rectangle.MouseLeftButtonDown += OnMouseLeftButtonDown;
    }
    
    public Rectangle Rectangle;

    private Board _board;
    private Action<Board> RedrawBoard;
    private int Id;
    private static readonly SolidColorBrush StrokeColor = Brushes.Azure;
    
    private void OnMouseEnter(object sender, MouseEventArgs e)
    {
        if (e.OriginalSource is not System.Windows.Shapes.Rectangle) return;
            
        var y = Id / _board.BoardProperties.Width;
        var x = Id % _board.BoardProperties.Width;
            
        if (_board.RectAt(y, x) == EBoardRect.Empty)
        {
            _board.SetDrawnRectAt(y, x, EBoardRect.PlacementReady);
            RedrawBoard(_board);
        } 
        else if (_board.RectAt(y, x) == EBoardRect.Ship)
        {
            _board.SetDrawnRectAt(y, x, EBoardRect.PlacementError);
            RedrawBoard(_board);
        }
    }
    
    private void OnMouseLeave(object sender, MouseEventArgs e)
    {
        if (e.OriginalSource is not System.Windows.Shapes.Rectangle) return;
            
        var y = Id / _board.BoardProperties.Width;
        var x = Id % _board.BoardProperties.Width;

        if (_board.RectAt(y, x) == EBoardRect.Empty)
        {
            _board.SetDrawnRectAt(y, x, EBoardRect.Empty);
            RedrawBoard(_board);
        } 
        else if (_board.RectAt(y, x) == EBoardRect.Ship)
        {
            _board.SetDrawnRectAt(y, x, EBoardRect.Ship); 
            RedrawBoard(_board);
        }
    }

    private void OnMouseLeftButtonDown(object sender, MouseEventArgs e)
    {
        if (e.OriginalSource is not System.Windows.Shapes.Rectangle) return;
            
        var y = Id / _board.BoardProperties.Width;
        var x = Id % _board.BoardProperties.Width;

        if (_board.CanPlaceAt(y, x))
        {
            _board.SetRectAt(y, x, EBoardRect.Ship);
            RedrawBoard(_board);
        }
    }
}