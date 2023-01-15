namespace Warships;

public delegate void Notify();

public sealed class GameManager
{
    private GameManager()
    {
        GameState = EGameState.FirstPlayerBoardGeneration;
    }

    private static GameManager? _gameManager;
    private EGameState _gameState;

    public EGameState GameState
    {
        get => _gameState;
        set
        {
            _gameState = value;
            GameStateChanged?.Invoke();
        }
    }

    public static GameManager GetInstance()
    {
        if (_gameManager == null)
        {
            _gameManager = new GameManager();
        }

        return _gameManager;
    }

    public event Notify? GameStateChanged;
}