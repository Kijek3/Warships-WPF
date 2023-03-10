using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warships;

public class Board
{
    public virtual BoardProperties BoardProperties { get; }
    public List<Tuple<int, int>> RedrawBuffer { get; set; }
    
    private EBoardRect[,] _shipsBoard;
    private EBoardRect[,] _drawnBoard;
    private Ship[] _ships;
    private int _shipToPlaceIndex = 0;
    
    public Board(int height, int width)
    {
        BoardProperties = new BoardProperties(height, width);
        SetUpBoard();
    }

    private void SetUpBoard()
    {
        _shipsBoard = new EBoardRect[BoardProperties.Height, BoardProperties.Width];
        _drawnBoard = new EBoardRect[BoardProperties.Height, BoardProperties.Width];
        RedrawBuffer = new List<Tuple<int, int>>();
        for (int i = 0; i < BoardProperties.Height; i++)
        {
            for (int j = 0; j < BoardProperties.Width; j++)
            {
                _shipsBoard[i, j] = EBoardRect.Empty;
                _drawnBoard[i, j] = EBoardRect.Empty;
                RedrawBuffer.Add(Tuple.Create<int, int>(i, j));
            }
        }

        _ships = new Ship[]
        {
            new Ship(EShipType.Quadruple),
            new Ship(EShipType.Triple),
            new Ship(EShipType.Triple),
            new Ship(EShipType.Double),
            new Ship(EShipType.Double),
            new Ship(EShipType.Double),
            new Ship(EShipType.Single),
            new Ship(EShipType.Single),
            new Ship(EShipType.Single),
            new Ship(EShipType.Single)
        };
    }

    public EBoardRect RectAt(int y, int x)
    {
        return _shipsBoard[y, x];
    }

    public void SetRectAt(int y, int x, EBoardRect type)
    {
        _shipsBoard[y, x] = type;
        RedrawBuffer.Add(Tuple.Create<int, int>(y, x));
    }
    
    public EBoardRect DrawnRectAt(int y, int x)
    {
        return _drawnBoard[y, x];
    }

    public void SetDrawnRectAt(int y, int x, EBoardRect type)
    {
        _drawnBoard[y, x] = type;
        RedrawBuffer.Add(Tuple.Create<int, int>(y, x));
    }

    public bool CanPlaceAt(int y, int x)
    {
        return _shipsBoard[y, x] != EBoardRect.Ship;
    }

    public void CleanRedrawBuffer()
    {
        RedrawBuffer.Clear();
    }

    public void PlaceNextShip(int y, int x)
    {
        _ships[_shipToPlaceIndex].StartPosition = Tuple.Create(y, x);
        _shipToPlaceIndex += 1;

        if (_shipToPlaceIndex == _ships.Length)
        {
            var nextState = GameManager.GetInstance().GameState == EGameState.FirstPlayerPlacingShips
                ? EGameState.SecondPlayerPlacingNext
                : EGameState.FirstPlayerTurnNext;
            
            GameManager.GetInstance().GameState = EGameState.PlayerWaiting;
            
            GameManager.GetInstance().ChangeStateAfterDelay(nextState);
        }
    }
    
    public Ship? ShipToPlace()
    {
        if (_shipToPlaceIndex >= _ships.Length)
        {
            return null;
        }
        
        return _ships[_shipToPlaceIndex];
    }

    private bool IsRectPlaceable(int y, int x)
    {
        return new[]
        {
            Tuple.Create(y - 1, x - 1),
            Tuple.Create(y - 1, x),
            Tuple.Create(y - 1, x + 1),
            Tuple.Create(y, x - 1),
            Tuple.Create(y, x),
            Tuple.Create(y, x + 1),
            Tuple.Create(y + 1, x - 1),
            Tuple.Create(y + 1, x),
            Tuple.Create(y + 1, x + 1)
        }
            .Where(t => t.Item1 >= 0 && t.Item1 < BoardProperties.Height && t.Item2 >= 0 && t.Item2 < BoardProperties.Width)
            .Select(t => _shipsBoard[t.Item1, t.Item2])
            .All(r => r == EBoardRect.Empty);
    }

    public bool? IsNextShipPlaceable(int y, int x)
    {
        if (ShipToPlace() == null) return null;
        
        var next = ShipToPlace()!;
        var tiles = next.ShipTiles(Tuple.Create(y, x), this);
        
        return
            tiles.Count() == next.Type.Length() &&
            tiles.All(t => IsRectPlaceable(t.Item1, t.Item2));
    }

    public void SetupForTurns()
    {
        for (int i = 0; i < BoardProperties.Height; i++)
        {
            for (int j = 0; j < BoardProperties.Width; j++)
            {
                SetDrawnRectAt(i, j, EBoardRect.Hidden);
            }
        }
    }

    public void HandleShipHit(int y, int x)
    {
        var ship = _ships.First(s => s.FinalShipTiles()!.Contains(Tuple.Create(y, x)))!;
        if (ship.FinalShipTiles()!.All(t => DrawnRectAt(t.Item1, t.Item2) == EBoardRect.Hit))
        {
            ship.IsSunk = true;
            var tiles = new List<Tuple<int, int>>();
            foreach (var (ty, tx) in ship.FinalShipTiles()!)
            {
                tiles.Add(Tuple.Create<int, int>(ty - 1, tx - 1));
                tiles.Add(Tuple.Create<int, int>(ty - 1, tx));
                tiles.Add(Tuple.Create<int, int>(ty - 1, tx + 1));
                tiles.Add(Tuple.Create<int, int>(ty, tx - 1));
                tiles.Add(Tuple.Create<int, int>(ty, tx + 1));
                tiles.Add(Tuple.Create<int, int>(ty + 1, tx - 1));
                tiles.Add(Tuple.Create<int, int>(ty + 1, tx));
                tiles.Add(Tuple.Create<int, int>(ty + 1, tx + 1));
            }

            var outsideTiles = 
                tiles
                    .Distinct()
                    .Where(t => t.Item1 >= 0 && t.Item1 < BoardProperties.Height && t.Item2 >= 0 && t.Item2 < BoardProperties.Width)
                    .Where(t => RectAt(t.Item1, t.Item2) == EBoardRect.Empty);
            
            foreach (var (ty, tx) in outsideTiles)
            {
                SetDrawnRectAt(ty, tx, EBoardRect.Empty);
            }
        }
    }

    public bool AllShipsSunk()
    {
        return _ships.All(s => s.IsSunk);
    }
}