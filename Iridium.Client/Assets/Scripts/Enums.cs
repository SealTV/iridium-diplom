namespace Assets.Scripts
{
    public enum ConnectorType
    {
        Next,
        Value,
        Bool,
        Int,
        Float
    }

    public enum RequestState
    {
        NotValue,
        Waiting,
        Accepted
    }

    public enum MainMenuState
    {
        MainMenu,
        WaitingGames,
        Games,
        WeaitingLevels,
        Levels
    }
}
