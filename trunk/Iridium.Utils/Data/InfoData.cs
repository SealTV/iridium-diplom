namespace Iridium.Utils.Data
{
    using System;

    [Serializable]
    public class GameData
    {
        public int Id { get; private set; }
        public string Description { get; private set; }
        public string PictureName { get; private set; }
        public int LevelsCount { get; private set; }

        public GameData(int id, string description, int levelsCount, string pictureName = null)
        {
            this.PictureName = pictureName;
            this.LevelsCount = levelsCount;
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
