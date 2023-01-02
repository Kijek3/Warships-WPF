using System;
using System.Windows.Media;

namespace Warships;

public enum EBoardRect
{
    Hidden,
    PlacementReady,
    PlacementError,
    Empty,
    Ship,
    Hit
}

public static class EBoardRectExtensions
{
    public static SolidColorBrush RectColor(this EBoardRect rect)
    {
        switch(rect)
        {
            case EBoardRect.Hidden:
                return Brushes.Gray;
                break;
            case EBoardRect.PlacementReady:
                return Brushes.Lime;
                break;
            case EBoardRect.PlacementError:
                return Brushes.Firebrick;
                break;
            case EBoardRect.Empty:
                return Brushes.Blue;
                break;
            case EBoardRect.Ship:
                return Brushes.DarkSlateGray;
                break;
            case EBoardRect.Hit:
                return Brushes.Yellow;
                break;
            default:
                return Brushes.Gray;
        }
    }
}