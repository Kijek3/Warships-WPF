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

    public EBoardRect RectAt(int height, int width)
    {
        return _shipsBoard[height, width];
    }

    public void SetRectAt(int height, int width, EBoardRect type)
    {
        _shipsBoard[height, width] = type;
    }
    
    public EBoardRect DrawnRectAt(int height, int width)
    {
        return _drawnBoard[height, width];
    }

    public void SetDrawnRectAt(int height, int width, EBoardRect type)
    {
        _drawnBoard[height, width] = type;
    }

    public bool CanPlaceAt(int height, int width)
    {
        return _shipsBoard[height, width] != EBoardRect.Ship;
    }
}