using System.Windows.Media;

namespace Warships;

public class Board
{
    public int Height { get; set; }
    public int Width { get; set; }

    private EBoardRect[,] _shipsBoard;
    private EBoardRect[,] _drawnBoard;

    public Board(int height, int width)
    {
        Height = height;
        Width = width;
        SetUpBoard();
    }

    private void SetUpBoard()
    {
        _shipsBoard = new EBoardRect[Height, Width];
        _drawnBoard = new EBoardRect[Height, Width];
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                _shipsBoard[i, j] = EBoardRect.Empty;
                _drawnBoard[i, j] = EBoardRect.Empty;
            }
        }
    }

    public EBoardRect RectAt(int y, int x)
    {
        return _shipsBoard[y, x];
    }

    public void SetRectAt(int y, int x, EBoardRect type)
    {
        _shipsBoard[y, x] = type;
    }
    
    public EBoardRect DrawnRectAt(int y, int x)
    {
        return _drawnBoard[y, x];
    }

    public void SetDrawnRectAt(int y, int x, EBoardRect type)
    {
        _drawnBoard[y, x] = type;
    }

    public bool CanPlaceAt(int y, int x)
    {
        return _shipsBoard[y, x] != EBoardRect.Ship;
    }
}