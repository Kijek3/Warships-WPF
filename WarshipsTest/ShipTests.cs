using Warships;
using Moq;

namespace WarshipsTest;

[TestFixture]
public class ShipTests
{
    private Ship _currentShip;
    
    [Test]
    [TestCase(EShipType.Quadruple)]
    [TestCase(EShipType.Triple)]
    [TestCase(EShipType.Double)]
    [TestCase(EShipType.Single)]
    public void ShipInit(EShipType type)
    {
        _currentShip = new Ship(type);
        Assert.Multiple( () => 
        {
            Assert.That(_currentShip.Type, Is.EqualTo(type));
            Assert.That(_currentShip.Orientation, Is.EqualTo(EShipOrientation.Vertical));
            Assert.That(_currentShip.IsSunk, Is.False);
            Assert.That(_currentShip.StartPosition, Is.Null);
        });
    }

    [Test]
    public void ShipTiles([Values]EShipType type, [Values]EShipOrientation orientation, [Values(0,2)]int y, [Values(0,2)]int x)
    {
        _currentShip = new Ship(type)
        {
            Orientation = orientation,
            StartPosition = Tuple.Create(y, x)
        };

        var properties = new BoardProperties(10, 10);
        
        var board = new Mock<Board>(10, 10);
        board.SetupGet(b => b.BoardProperties).Returns(properties);

        var resultList = new List<Tuple<int, int>>();
        for (int i = 0; i < type.Length(); i++)
        {
            if (orientation == EShipOrientation.Vertical)
            {
                resultList.Add(Tuple.Create(y+i, x));
            }
            else
            {
                resultList.Add(Tuple.Create(y, x+i));
            }
        }

        Assert.Multiple(() =>
        {
            Assert.That(_currentShip.ShipTiles(Tuple.Create(y, x), board.Object), Is.EqualTo(resultList));
            Assert.That(_currentShip.FinalShipTiles(), Is.Not.Null);
            Assert.That(_currentShip.ShipTiles(Tuple.Create(y, x), board.Object), Is.EqualTo(_currentShip.FinalShipTiles()!));
        });
    }
}