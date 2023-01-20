namespace Warships;

public enum EGameState
{
    WelcomeScreen,
    FirstPlayerBoardGeneration,
    SecondPlayerBoardGeneration,
    FirstPlayerPlacingShips,
    SecondPlayerPlacingShips,
    FirstPlayerTurn,
    SecondPlayerTurn,
    FirstPlayerPlacingNext,
    SecondPlayerPlacingNext,
    FirstPlayerTurnNext,
    SecondPlayerTurnNext,
    PlayerWaiting,
    FirstPlayerWon,
    SecondPlayerWon
}

public static class EGameStateExtensions
{
    public static bool IsFirstPlayerBoard(this EGameState state)
    {
        return state switch
        {
            EGameState.FirstPlayerBoardGeneration => true,
            EGameState.FirstPlayerPlacingShips => true,
            EGameState.FirstPlayerTurn => false,
            EGameState.SecondPlayerBoardGeneration => false,
            EGameState.SecondPlayerPlacingShips => false,
            EGameState.SecondPlayerTurn => true,
            _ => true
        };
    }
}