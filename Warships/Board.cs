using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Warships;

public class Board
{
    public BoardProperties BoardProperties;
    public List<Tuple<int, int>> RedrawBuffer { get; set; }

    private EBoardRect[,] _shipsBoard;
    private EBoardRect[,] _drawnBoard;

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
}