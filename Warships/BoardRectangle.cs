using System;
using System.Diagnostics;
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

        if (GameManager.GetInstance().GameState is EGameState.FirstPlayerPlacingShips or EGameState.SecondPlayerPlacingShips)
        {
            if (_board.IsNextShipPlacable(y, x) == true)
            {
                foreach (var (ty, tx) in _board.ShipToPlace()!.ShipTiles(Tuple.Create(y,x), _board))
                {
                    _board.SetDrawnRectAt(ty ,tx, EBoardRect.PlacementReady);
                }
            }
            else if (_board.IsNextShipPlacable(y, x) == false)
            {
                foreach (var (ty, tx) in _board.ShipToPlace()!.ShipTiles(Tuple.Create(y,x), _board))
                {
                    _board.SetDrawnRectAt(ty ,tx, EBoardRect.PlacementError);
                }
            }
            else
            {
                return;
            }

            RedrawBoard(_board);
        }
    }
    
    private void OnMouseLeave(object sender, MouseEventArgs e)
    {
        if (e.OriginalSource is not System.Windows.Shapes.Rectangle) return;
            
        var y = Id / _board.BoardProperties.Width;
        var x = Id % _board.BoardProperties.Width;

        if (GameManager.GetInstance().GameState is EGameState.FirstPlayerPlacingShips or EGameState.SecondPlayerPlacingShips)
        {
            if (_board.ShipToPlace() != null)
            {
                foreach (var (ty, tx) in _board.ShipToPlace()!.ShipTiles(Tuple.Create(y,x), _board))
                {
                    _board.SetDrawnRectAt(ty ,tx, _board.RectAt(ty, tx));
                }
            }
            else
            {
                return;
            }

            RedrawBoard(_board);
        }
    }

    private void OnMouseLeftButtonDown(object sender, MouseEventArgs e)
    {
        if (e.OriginalSource is not System.Windows.Shapes.Rectangle) return;
            
        var y = Id / _board.BoardProperties.Width;
        var x = Id % _board.BoardProperties.Width;

        if (GameManager.GetInstance().GameState is EGameState.FirstPlayerPlacingShips or EGameState.SecondPlayerPlacingShips)
        {
            if (_board.IsNextShipPlacable(y, x) == true)
            {
                foreach (var (ty, tx) in _board.ShipToPlace()!.ShipTiles(Tuple.Create(y,x), _board))
                {
                    _board.SetRectAt(ty ,tx, EBoardRect.Ship);
                    _board.SetDrawnRectAt(ty, tx, EBoardRect.Ship);
                }
                _board.PlaceNextShip(y ,x);
            }
            else
            {
                return;
            }

            RedrawBoard(_board);
        }
    }
}