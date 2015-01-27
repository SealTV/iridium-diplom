namespace Iridium.Utils.Data
{
    using System;

    public static class SharedData
    {
        [Serializable]
        public class GameData
        {
            public int Id { get; private set; }
            public string Description { get; private set; }
            public string PictureName { get; private set; }

            public GameData(int id, string description, string pictureName = null)
            {
                this.PictureName = pictureName;
                this.Description = description;
                this.Id = id;
            }
        }
    }
}
