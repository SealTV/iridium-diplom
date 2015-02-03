namespace Iridium.Server.Services
{
    using System.IO;
    using System.Collections.Generic;

    using IridiumDatabase;

    public class LevelsDataProvider
    {
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private const string DefaultPath = "Data";
        private static readonly Dictionary<string, string> levelData = new Dictionary<string, string>();

        public static string GetLevelData(level_data level_data)
        {
            string result;
            if (levelData.TryGetValue(level_data.path, out result))
            {
                return result;
            }
            string path = IridiumMasterServer.Configuration.ServerProperties.FilePath + Path.AltDirectorySeparatorChar + DefaultPath + Path.AltDirectorySeparatorChar + level_data.path;
            try
            {
                using (var stream = new FileStream(path, FileMode.Open))
                using (var reader = new StreamReader(stream))
                {
                    var data = reader.ReadToEnd();
                    levelData.Add(level_data.path, data);
                }
            }
            catch (IOException e)
            {
                Logger.Error(e);
                return null;
            }

            return levelData[level_data.path];
        }
    }
}
