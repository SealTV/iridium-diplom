namespace Iridium.Server.Services
{
    using System.IO;
    using System.Collections.Generic;

    using IridiumDatabase;

    public class LavelsDataProvider
    {
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private const string DefaultPath = "Data";
        private static readonly Dictionary<string, string> levelData = new Dictionary<string, string>();

        public static string  GetLevelData(level_data level_data)
        {
            string result;
            if (levelData.TryGetValue(level_data.path, out result))
            {
                return result;
            }
            string path = DefaultPath + Path.AltDirectorySeparatorChar + level_data.path;
            using (var stream = new FileStream(path, FileMode.Open))
            using (var reader = new StreamReader(stream))
            {
                var data = reader.ReadToEnd();
                levelData.Add(level_data.path, data);
            }

            return levelData[level_data.path];
        }
    }
}
