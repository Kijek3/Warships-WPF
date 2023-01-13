namespace Warships;

public sealed class GameManager
{
    private GameManager() {}

    private static GameManager? _gameManager;

    public static GameManager GetInstance()
    {
        if (_gameManager == null)
        {
            _gameManager = new GameManager();
        }

        return _gameManager;
    }
}