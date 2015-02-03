namespace Iridium.Server.Games
{
    using System;
    using System.Collections.Generic;

    using Tech.CodeGeneration;

    public abstract class Game
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public bool RunCode(string intput, string codeSource, out string[] results)
        {
            using (var sandbox = new Sandbox(IridiumMasterServer.Configuration.ServerProperties.FilePath))
                try
                {
                    List<string> enemyList;
                    var result = ProcessCode(codeSource, intput, sandbox, out enemyList);
                    results = enemyList.ToArray();
                    return result;
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                    Logger.Error(e.Message);
                }

            results = new string[0];
            return false;
        }

        protected abstract bool ProcessCode(string codeSource, string intput, Sandbox sandbox, out List<string> enemyList);
    }
}
