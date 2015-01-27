namespace Iridium.Server.Games
{
    using System;

    public static class GamesFactory
    {
        public static Game GetGame(int gameId)
        {
            switch ((GamesType)gameId)
            {
                case GamesType.StarWars:
                    return new StarWars();
                default:
                    return null;
            }
        }
    }

    public enum GamesType
    {
        StarWars = 1
    }
}
