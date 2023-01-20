using System;
using System.Windows.Media;

namespace Warships;

public enum EBoardRect
{
    Hidden,
    HiddenFocused,
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
        return rect switch
        {
            EBoardRect.Hidden => Brushes.Gray,
            EBoardRect.HiddenFocused => Brushes.LightGray,
            EBoardRect.PlacementReady => Brushes.Green,
            EBoardRect.PlacementError => Brushes.Firebrick,
            EBoardRect.Empty => Brushes.DarkSlateGray,
            EBoardRect.Ship => Brushes.Black,
            EBoardRect.Hit => Brushes.Black,
            _ => Brushes.White
        };
    }
}