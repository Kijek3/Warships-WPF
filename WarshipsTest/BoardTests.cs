using Warships;

namespace WarshipsTest;

[TestFixture(10, 10)]
[TestFixture(5, 8)]
[TestFixture(12, 4)]
public class BoardTests
{
    private Board _board;
    private readonly int _width;
    private readonly int _height;

    public BoardTests(int width, int height)
    {
        _width = width;
        _height = height;
    }
    
    [SetUp]
    public void Setup()
    {
        _board = new Board(_height, _width);
    }

    [Test]
    public void BoardInit()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_board.BoardProperties.Height, Is.EqualTo(_height));
            Assert.That(_board.BoardProperties.Width, Is.EqualTo(_width));
            Assert.That(_board.RedrawBuffer, Has.Count.EqualTo(_width * _height));
        });
    }

    [Test]
    public void RedrawBuffer()
    {
        _board.CleanRedrawBuffer();
        var expectedResult = new List<Tuple<int, int>>() {Tuple.Create(1, 2)};

        _board.SetRectAt(1, 2, EBoardRect.Ship);
        
        Assert.That(_board.RedrawBuffer, Is.EqualTo(expectedResult));
    }

    [Test]
    [TestCase(0, 0, EBoardRect.Ship)]
    [TestCase(1, 2, EBoardRect.Hit)]
    [TestCase(2, 1, EBoardRect.PlacementError)]
    public void RectAt(int x, int y, EBoardRect eBoardRect)
    {
        _board.CleanRedrawBuffer();
        var expectedResult = new List<Tuple<int, int>>() {Tuple.Create(y, x)};
        
        _board.SetRectAt(y, x, eBoardRect);
        
        Assert.Multiple(() =>
        {
            Assert.That(_board.RectAt(y, x), Is.EqualTo(eBoardRect));
            Assert.That(_board.RedrawBuffer, Is.EqualTo(expectedResult));
        });
    }

    [Test]
    [TestCase(0, 0, EBoardRect.Ship)]
    [TestCase(1, 2, EBoardRect.Hit)]
    [TestCase(2, 1, EBoardRect.PlacementError)]
    public void DrawnRectAt(int x, int y, EBoardRect eBoardRect)
    {
        _board.CleanRedrawBuffer();
        var expectedResult = new List<Tuple<int, int>>() {Tuple.Create(y, x)};
        
        _board.SetDrawnRectAt(y, x, eBoardRect);
        
        Assert.Multiple(() =>
        {
            Assert.That(_board.DrawnRectAt(y, x), Is.EqualTo(eBoardRect));
            Assert.That(_board.RedrawBuffer, Is.EqualTo(expectedResult));
        });
    }

    [Test]
    [TestCase(0, 0, EBoardRect.Ship, false)]
    [TestCase(0, 0, EBoardRect.Empty, true)]
    [TestCase(1, 2, EBoardRect.Ship, false)]
    [TestCase(1, 2, EBoardRect.Empty, true)]
    public void CanPlaceAt(int y, int x, EBoardRect eBoardRect, bool result)
    {
        _board.SetRectAt(y, x, eBoardRect);
        
        Assert.That(_board.CanPlaceAt(y, x), Is.EqualTo(result));
    }

    [Test]
    public void CleanRedrawBuffer()
    {
        _board.CleanRedrawBuffer();
        
        Assert.That(_board.RedrawBuffer, Has.Count.EqualTo(0));
    }

    [Test]
    public void ShipToPlace()
    {
        Assert.That(_board.ShipToPlace()?.Type, Is.EqualTo(EShipType.Quadruple));
        _board.PlaceNextShip(0, 0);
        Assert.That(_board.ShipToPlace()?.Type, Is.EqualTo(EShipType.Triple));
    }
}