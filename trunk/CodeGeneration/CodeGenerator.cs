using System.Collections.Generic;
using System.Linq;
using Tech.CodeGeneration.Internals;
using Tech.CodeGeneration.Compilers;

namespace Tech.CodeGeneration
{
    public static class CodeGenerator
    {
        public static IGeneratedCode<TResult> CreateCode<TResult>(string sourceCode, 
            params CodeParameter[] parameters)
        {
            return CreateCode<TResult>(sourceCode, null, null, parameters);
        }


        public static IGeneratedCode<TResult> CreateCode<TResult>(string sourceCode, 
            IEnumerable<string> usingNamespaces, 
            IEnumerable<string> referencedAssemblies,
            params CodeParameter[] parameters)
        {
            return CreateCode<TResult>(CS.Compiler, sourceCode, 
                usingNamespaces, referencedAssemblies, parameters);
        }


        public static IGeneratedCode<TResult> CreateCode<TResult>(
            ICompiler compiler, 
            string sourceCode,
            IEnumerable<string> usingNamespaces,
            IEnumerable<string> referencedAssemblies,
            params CodeParameter[] parameters)
        {
            var d = compiler.CompileDelegate(sourceCode,
                usingNamespaces, typeof(TResult), referencedAssemblies, parameters);
            return new Code<TResult>(d);
        }


        public static IGeneratedCode<TResult> CreateCode<TResult>(Sandbox sandbox, 
            string sourceCode,
            params CodeParameter[] parameterInfos)
        {
            return CreateCode<TResult>(sandbox, sourceCode, null, null, parameterInfos);
        }


        public static IGeneratedCode<TResult> CreateCode<TResult>(Sandbox sandbox, 
            string sourceCode,
            IEnumerable<string> usingNamespaces,
            IEnumerable<string> referencedAssemblies,
            params CodeParameter[] parameterInfos)
        {
            return CreateCode<TResult>(sandbox, CS.Compiler, sourceCode,
                usingNamespaces, referencedAssemblies, parameterInfos);
        }


        public static IGeneratedCode<TResult> CreateCode<TResult>(Sandbox sandbox,
            ICompiler language, 
            string sourceCode,
            IEnumerable<string> usingNamespaces,
            IEnumerable<string> referencedAssemblies,
            params CodeParameter[] parameterInfos)
        {
            var asmLocation = language.CompileAssemblyFile(sourceCode,
                usingNamespaces, typeof(TResult), referencedAssemblies, parameterInfos,
                sandbox.ApplicationBase);

            return new SandboxedCode<TResult>(sandbox.CreateCodeProxy(asmLocation));
        }


        public static TResult ExecuteCode<TResult>(string sourceCode,
            params CodeParameter[] parameters)
        {
            var code = CreateCode<TResult>(sourceCode, parameters);
            return code.Execute(parameters.Select(p => p.Value).ToArray());
        }


        public static TResult ExecuteCode<TResult>(Sandbox sandbox,
            string sourceCode,
            IEnumerable<string> usingNamespaces,
            IEnumerable<string> referencedAssemblies,
            params CodeParameter[] parameters)
        {
            var code = CreateCode<TResult>(sandbox, CS.Compiler, sourceCode,
                usingNamespaces, referencedAssemblies, parameters);

            return code.Execute(parameters.Select(p => p.Value).ToArray());
        }


        public static TResult ExecuteCode<TResult>(Sandbox sandbox,
            ICompiler language,
            string sourceCode,
            IEnumerable<string> usingNamespaces,
            IEnumerable<string> referencedAssemblies,
            params CodeParameter[] parameters)
        {
            var code = CreateCode<TResult>(sandbox, language, sourceCode,
                usingNamespaces, referencedAssemblies, parameters);

            return code.Execute(parameters.Select(p => p.Value).ToArray());
        }
    }
}
