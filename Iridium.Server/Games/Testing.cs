namespace Iridium.Server.Games
{
    using System.Collections.Generic;

    using Tech.CodeGeneration;
    using Tech.CodeGeneration.Compilers;

    public class Testing : Game
    {
        protected override bool ProcessCode(string codeSource, string intput, Sandbox sandbox, out List<string> enemyList)
        {
            throw new System.NotImplementedException();
        }

        protected bool ProcessCode(string codeSource, string mainMethodName, string intput, Sandbox sandbox, out List<string> enemyList)
        {
            int value = 100;
            var code = CodeGenerator.CreateCode<int>(sandbox, CSExtend.Compiler, codeSource, mainMethodName,
                                                         new[] { "Iridium.Utils" },
                                                         new[] { "Iridium.Utils.dll" },
                                                         CodeParameter.Create("k", value));
            //((Code<int>)code)

            enemyList = new List<string>();
            return false;
        }

        public void Run()
        {
            List<string> enemyList;
            using (var sandbox = new Sandbox())
            {
                this.ProcessCode(str, "Main", string.Empty, sandbox, out enemyList);
            }
        }

        private string str = @"public int Main(int k)
{
    for (int i = 0; i < 5; i++)
    {
        Console.WriteLine(i);
    }
    Console.WriteLine(Str());
    return k + 10;
}

public string Str()
{
    return " + "\"Hello world!\"" + ";" +
"\n}";

    }
}
