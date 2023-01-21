using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Timer = System.Timers.Timer;

namespace Warships;

public class BoardRectangle
{
    public BoardRectangle(Board board, int rectangleSize, SolidColorBrush colorBrush, int id, Action<Board> redrawBoard)
    {
        _board = board;
        Id = id;
        Y = Id / _board.BoardProperties.Width;
        X = Id % _board.BoardProperties.Width;
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
        Rectangle.MouseRightButtonDown += OnMouseRightButtonDown;
    }
    
    public Rectangle Rectangle;

    private Board _board;
    private Action<Board> RedrawBoard;
    private int Id;
    private int X, Y;
    private static readonly SolidColorBrush StrokeColor = Brushes.Azure;
    
    private void OnMouseEnter(object sender, MouseEventArgs e)
    {
        if (e.OriginalSource is not System.Windows.Shapes.Rectangle) return;

        if (GameManager.GetInstance().GameState is EGameState.FirstPlayerPlacingShips or EGameState.SecondPlayerPlacingShips)
        {
            if (_board.IsNextShipPlaceable(Y, X) == true)
            {
                foreach (var (ty, tx) in _board.ShipToPlace()!.ShipTiles(Tuple.Create(Y, X), _board))
                {
                    _board.SetDrawnRectAt(ty ,tx, EBoardRect.PlacementReady);
                }
            }
            else if (_board.IsNextShipPlaceable(Y, X) == false)
            {
                foreach (var (ty, tx) in _board.ShipToPlace()!.ShipTiles(Tuple.Create(Y, X), _board))
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
        else if (GameManager.GetInstance().GameState is EGameState.FirstPlayerTurn or EGameState.SecondPlayerTurn)
        {
            if (_board.DrawnRectAt(Y, X) is EBoardRect.Hidden)
            {
                _board.SetDrawnRectAt(Y ,X, EBoardRect.HiddenFocused);
                RedrawBoard(_board);
            }
        }
    }
    
    private void OnMouseLeave(object sender, MouseEventArgs e)
    {
        if (e.OriginalSource is not System.Windows.Shapes.Rectangle) return;

        if (GameManager.GetInstance().GameState is EGameState.FirstPlayerPlacingShips or EGameState.SecondPlayerPlacingShips)
        {
            if (_board.ShipToPlace() != null)
            {
                foreach (var (ty, tx) in _board.ShipToPlace()!.ShipTiles(Tuple.Create(Y, X), _board))
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
        else if (GameManager.GetInstance().GameState is EGameState.FirstPlayerTurn or EGameState.SecondPlayerTurn)
        {
            if (_board.DrawnRectAt(Y, X) is EBoardRect.HiddenFocused)
            {
                _board.SetDrawnRectAt(Y ,X, EBoardRect.Hidden);
                RedrawBoard(_board);
            }
        }
    }

    private void OnMouseLeftButtonDown(object sender, MouseEventArgs e)
    {
        if (e.OriginalSource is not System.Windows.Shapes.Rectangle) return;

        if (GameManager.GetInstance().GameState is EGameState.FirstPlayerPlacingShips or EGameState.SecondPlayerPlacingShips)
        {
            if (_board.IsNextShipPlaceable(Y, X) == true)
            {
                foreach (var (ty, tx) in _board.ShipToPlace()!.ShipTiles(Tuple.Create(Y, X), _board))
                {
                    _board.SetRectAt(ty ,tx, EBoardRect.Ship);
                    _board.SetDrawnRectAt(ty, tx, EBoardRect.Ship);
                }
            }
            else
            {
                return;
            }

            RedrawBoard(_board);
            _board.PlaceNextShip(Y, X);
        }
        else if (GameManager.GetInstance().GameState is EGameState.FirstPlayerTurn or EGameState.SecondPlayerTurn)
        {
            if (_board.DrawnRectAt(Y, X) != EBoardRect.HiddenFocused) return;
            if (_board.RectAt(Y, X) is EBoardRect.Ship)
            {
                _board.SetDrawnRectAt(Y, X, EBoardRect.Hit);
                _board.HandleShipHit(Y, X);
            } 
            else if (_board.RectAt(Y, X) is EBoardRect.Empty)
            {
                _board.SetDrawnRectAt(Y ,X, EBoardRect.Empty);
            }
            else
            {
                return;
            }

            RedrawBoard(_board);

            var nextState = _board.AllShipsSunk() 
                ? GameManager.GetInstance().GameState is EGameState.FirstPlayerTurn
                    ? EGameState.FirstPlayerWon
                    : EGameState.SecondPlayerWon
                : GameManager.GetInstance().GameState is EGameState.FirstPlayerTurn
                    ? EGameState.SecondPlayerTurnNext
                    : EGameState.FirstPlayerTurnNext;
            
            GameManager.GetInstance().GameState = EGameState.PlayerWaiting;
            
            GameManager.GetInstance().ChangeStateAfterDelay(nextState);
        }
    }
    
    private void OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (GameManager.GetInstance().GameState is EGameState.FirstPlayerPlacingShips or EGameState.SecondPlayerPlacingShips)
        {
            if (_board.ShipToPlace() != null)
            {
                foreach (var (ty, tx) in _board.ShipToPlace()!.ShipTiles(Tuple.Create(Y, X), _board))
                {
                    _board.SetDrawnRectAt(ty, tx, _board.RectAt(ty, tx));
                }
                
                var currentOri = _board.ShipToPlace()!.Orientation;
                _board.ShipToPlace()!.Orientation = currentOri == EShipOrientation.Horizontal
                    ? EShipOrientation.Vertical
                    : EShipOrientation.Horizontal;
                
                RedrawBoard(_board);
            }
        }
    }
}