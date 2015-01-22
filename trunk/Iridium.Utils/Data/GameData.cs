namespace Iridium.Utils.Data
{
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
}
