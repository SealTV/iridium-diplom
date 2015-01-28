namespace Iridium.Server.Services
{
    using System;
    using System.Linq;

    using Iridium.Server.Games;
    using Iridium.Utils.Data;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using Tech.CodeGeneration;
    using Tech.CodeGeneration.Compilers;

    public class CodeBuildService
    {
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public bool RunCode(string jsonString, string code, out string[] results)
        {
            var json = JsonConvert.DeserializeObject<JToken>(jsonString);
            var game = GamesFactory.GetGame(json["game_id"].ToObject<int>());

            return game.RunCode(json["input"].ToString(), code, out results);
        }

        public void Run()
        {
            using (var sandbox = new Sandbox())
            {
                try
                {
                    const string sourceCode = "int i = 0;" +
                                              "while (i != 5)" +
                                              "{ " +
                                              "i++; " +
                                              "Console.WriteLine(i);" +
                                              "max++;" +
                                              "Console.WriteLine(array[0]);" +
                                              "}" +
                                              "foreach(var a in array){" +
                                              "Console.WriteLine(\"a = {0}\",a);" +
                                              "}" +
                                              "Console.WriteLine(\"@class = {0}\", @class.Working);" +
                                              "Console.WriteLine(\"@struct = {0}\", @struct.Value);" +
                                              "Console.WriteLine(\"@enemy = {0}\", @enemy.Id);" +
                                              "Console.WriteLine(\"@enemy = {0}\", @enemy.Position.X);" +
                                              "Console.WriteLine(\"@enemy = {0}\", @enemy.Position.Y);" +
                                              "return 52222;";
                    const int max = 25;
                    int[] array = { 2, 4, 5 };
                    Class @class = new Class()
                    {
                        Working = false
                    };

                    Struct @struct = new Struct()
                    {
                        Value = 15
                    };

                    Enemy enemy = new Enemy(1, new Point(1, 1));

                    var code = CodeGenerator.CreateCode<int>(sandbox, CS.Compiler,
                                                             sourceCode,
                                                             new[] { "Iridium.Utils" },
                                                             new[] { "Iridium.Utils.dll" },
                                                             CodeParameter.Create("max", max),
                                                             CodeParameter.Create("array", array),
                                                             CodeParameter.Create("@class", @class),
                                                             CodeParameter.Create("@struct", @struct),
                                                             CodeParameter.Create("enemy", enemy));

                    var value = code.Execute(max, array, @class, @struct, enemy);
                    Console.WriteLine(value);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
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
    
    public class Class : MarshalByRefObject
    {
        public bool Working { get; set; }
    }

    [Serializable]
    public struct Struct
    {
        public int Value { get; set; }
    }
}
