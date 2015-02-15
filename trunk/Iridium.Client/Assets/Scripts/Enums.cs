namespace Assets.Scripts
{
    public enum ConnectorType
    {
        None,
        Next,
        Value,
        Bool,
        Int,
        Float,
        Collection,
        List,
        Enemy
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
