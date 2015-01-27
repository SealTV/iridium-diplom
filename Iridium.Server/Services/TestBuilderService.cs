namespace Iridium.Server.Services
{
    using System;
    using System.Linq;

    using Iridium.Utils.Data;

    using Tech.CodeGeneration;
    using Tech.CodeGeneration.Compilers;

    public class TestBuilderService
    {
       

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
                                              "Console.WriteLine(\"@struckt = {0}\", @struckt.Value);" +
                                              "return 52222;";
                    const int max = 25;
                    int[] array = { 2, 4, 5 };
                    Class @class = new Class()
                    {
                        Working = false
                    };
                    Struckt struckt = new Struckt()
                    {
                        Value = 15
                    };

                    SharedData.Enemy enemy = new SharedData.Enemy(1, new SharedData.Point(1, 1));

                    var code = CodeGenerator.CreateCode<int>(sandbox, CS.Compiler,
                                                             sourceCode, 
                                                             new []{"Iridium.Utils"},
                                                             new[] { "Iridium.Utils.dll" }, 
                                                             CodeParameter.Create("max", max),
                                                             CodeParameter.Create("array", array),
                                                             CodeParameter.Create("@class", @class),
                                                             CodeParameter.Create("struckt", struckt),
                                                             CodeParameter.Create("enemy", enemy));
                    var value = code.Execute(max, array, @class, struckt, enemy);
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
    public struct Struckt
    {
        public int Value { get; set; }
    }
}
