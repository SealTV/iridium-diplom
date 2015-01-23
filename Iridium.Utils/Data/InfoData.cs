namespace Iridium.Utils.Data
{
    using System;

    [Serializable]
    public class GameData
    {
        public int Id { get; private set; }
        public string Description { get; private set; }
        public string PictureName { get; private set; }
        public int[] LevelsId { get; private set; }

        public GameData(int id, string description, int[] levelsId, string pictureName)
        {
            this.PictureName = pictureName;
            this.LevelsId = levelsId;
            this.Description = description;
            this.Id = id;
        }
    }

    [Serializable]
    public class LevelData
    {
        public int GameId { get; private set; }
        public int Id { get; private set; }
        public string Description { get; private set; }

        public LevelData(int gameId, int id, string description)
        {
            this.Description = description;
            this.Id = id;
            this.GameId = gameId;
        }
    }
}
