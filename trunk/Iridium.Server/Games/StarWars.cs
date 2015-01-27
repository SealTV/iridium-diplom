namespace Iridium.Server.Games
{
    using System;
    using System.Linq;

    using Iridium.Utils.Data;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using Tech.CodeGeneration;
    using Tech.CodeGeneration.Compilers;

    public class StarWars : Game
    {
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public bool RunCode(string intput, string output, string codeSource, out string[] results)
        {
            var json = JsonConvert.DeserializeObject<JToken>(intput);
            var enemiesData = json["enemies"].ToObject<string[]>();

            var enemies = new Enemy[enemiesData.Length];
            for (int i = 0; i < enemies.Length; i++)
            {
                var enemy = JsonConvert.DeserializeObject<JToken>(enemiesData[i]);

                enemies[i] = new Enemy()
                {
                    Id = enemy["id"].ToObject<int>(),
                    Position = new Point()
                    {
                        X = enemy["position"]["x"].ToObject<float>(),
                        Y = enemy["position"]["y"].ToObject<float>()
                    },
                    Name = enemy["name"].ToString(),
                    Speed = enemy["speed"].ToObject<int>(),
                    Health = enemy["health"].ToObject<int>()
                };
            }
            try
            {
                    
                using (var sandbox = new Sandbox())
                {
                    var code = CodeGenerator.CreateCode<int>(sandbox, CS.Compiler, codeSource, null, null,
                                                               CodeParameter.Create("enemies", enemies));

                    var codeResult = code.Execute(enemies);

                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            finally
            {
                results = null;
            }
            return false;
        }

        public bool Run(int[] inputs, int[] output, string bodySource)
        {
            bool result = false;
            try
            {
                using (var sandbox = new Sandbox())
                {
                    var code = CodeGenerator.CreateCode<int[]>(sandbox, CS.Compiler, bodySource, null, null,
                                                               CodeParameter.Create("inputs", inputs));
                    var codeResult = code.Execute(inputs);
                    if (codeResult.Length == output.Length)
                    {
                        result = !codeResult.Where((t, i) => t != output[i]).Any();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return result;
        }
    }
}