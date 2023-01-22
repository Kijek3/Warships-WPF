using Warships;

namespace WarshipsTest;

[TestFixture]
public class GameManagerTests
{
    private bool _didInvoke = false;

    private void OnGameStateChanged()
    {
        _didInvoke = true;
    }
    
    [Test]
    public void GameManagerInit()
    {
        Assert.Multiple(
            () =>
            {
                Assert.That(GameManager.GetInstance(), Is.Not.Null);
                Assert.That(GameManager.GetInstance().GameState, Is.EqualTo(EGameState.FirstPlayerBoardGeneration));
                Assert.That(GameManager.GetInstance().IsBeginningTurns, Is.True);
            }
        );
    }
    
    [Test]
    [TestCase(EGameState.PlayerWaiting)]
    [TestCase(EGameState.FirstPlayerTurn)]
    [TestCase(EGameState.SecondPlayerPlacingShips)]
    [TestCase(EGameState.FirstPlayerTurnNext)]
    [TestCase(EGameState.SecondPlayerBoardGeneration)]
    public void GameStateChangeInvoke(EGameState newState)
    {
        _didInvoke = false;
        GameManager.GetInstance().GameStateChanged += OnGameStateChanged;
        GameManager.GetInstance().GameState = newState;
        Assert.That(_didInvoke, Is.True);
    }
}