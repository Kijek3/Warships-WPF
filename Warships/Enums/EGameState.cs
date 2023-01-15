namespace Warships;

public enum EGameState
{
    FirstPlayerBoardGeneration,
    SecondPlayerBoardGeneration,
    FirstPlayerPlacingShips,
    SecondPlayerPlacingShips,
    FirstPlayerTurn,
    SecondPlayerTurn,
    SecondPlayerPlacingNext,
    FirstPlayerTurnNext,
    SecondPlayerTurnNext,
}

public static class EGameStateExtensions
{
    public static bool IsFirstPlayer(this EGameState state)
    {
        return state switch
        {
            EGameState.FirstPlayerBoardGeneration => true,
            EGameState.FirstPlayerPlacingShips => true,
            EGameState.FirstPlayerTurn => true,
            EGameState.SecondPlayerBoardGeneration => false,
            EGameState.SecondPlayerPlacingShips => false,
            EGameState.SecondPlayerTurn => false,
            _ => true
        };
    }
}