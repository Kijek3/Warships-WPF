using System;
using System.Linq;

namespace Warships;

public class Ship
{
    public EShipType Type { get; set; }
    public EShipOrientation Orientation { get; set; }
    public Tuple<int, int>? StartPosition { get; set; }

    public Ship(EShipType type)
    {
        this.Type = type;
        this.Orientation = EShipOrientation.Vertical;
    }
    
    public Tuple<int,int>[] ShipTiles(Tuple<int, int> start, Board board)
    {
        var tiles = new Tuple<int, int>[Type.Length()];
        var yMult = Orientation == EShipOrientation.Vertical ? 1 : 0;
        var xMult = Orientation == EShipOrientation.Horizontal ? 1 : 0;
        for (int i = 0; i < Type.Length(); i++)
        {
            tiles[i] = Tuple.Create<int, int>(i * yMult + start.Item1, i * xMult + start.Item2);
        }

        return
            tiles
                .Where(t => t.Item1 >= 0 && t.Item1 < board.BoardProperties.Height && t.Item2 >= 0 &&
                            t.Item2 < board.BoardProperties.Width)
                .ToArray();
    }

    public Tuple<int, int>[]? FinalShipTiles()
    {
        if (StartPosition == null)
        {
            return null;
        }
        var tiles = new Tuple<int, int>[Type.Length()];
        var yMult = Orientation == EShipOrientation.Vertical ? 1 : 0;
        var xMult = Orientation == EShipOrientation.Horizontal ? 1 : 0;
        for (int i = 0; i < Type.Length(); i++)
        {
            tiles[i] = Tuple.Create<int, int>(i * yMult + StartPosition.Item1, i * xMult + StartPosition.Item2);
        }

        return tiles;
    }
}