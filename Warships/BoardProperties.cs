namespace Warships;

public class BoardProperties
{
    public BoardProperties(int height, int width)
    {
        Height = height;
        Width = width;
    }
    
    public int Width { get; set; }
    public int Height { get; set; }
}