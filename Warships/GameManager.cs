namespace Warships;

public sealed class GameManager
{
    private GameManager()
    {
        GameState = EGameState.FirstPlayerPlacingShips;
    }

    private static GameManager? _gameManager;

    public EGameState GameState { get; set; }

    public static GameManager GetInstance()
    {
        if (_gameManager == null)
        {
            _gameManager = new GameManager();
        }

        return _gameManager;
    }
}