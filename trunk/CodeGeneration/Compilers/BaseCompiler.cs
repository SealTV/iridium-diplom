using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Tech.CodeGeneration.Internals;

namespace Tech.CodeGeneration.Compilers
{
    public abstract class BaseCompiler<TCodeDomProvider> 
        where TCodeDomProvider : CodeDomProvider, new()
    {
        public string CompileAssemblyFile(string sourceCode, 
            IEnumerable<string>        usingNamespaces,
            Type                       returnType,
            IEnumerable<string>        referencedAssemblies,
            IEnumerable<CodeParameter> parameters,
            string                     assemblyDirectory)
        {
            var assemblySourceCode = FormatAssemblySourceCode(sourceCode, 
                usingNamespaces, returnType, parameters);

            referencedAssemblies = AddParameterAssemblies(referencedAssemblies, parameters);

            using (var compiler = new TCodeDomProvider())
            {
                return CompileAssemblyFile(assemblySourceCode,
                    referencedAssemblies, assemblyDirectory, compiler);
            }
        }


        public Delegate CompileDelegate(string sourceCode,
            IEnumerable<string> usingNamespaces, 
            Type returnType,
            IEnumerable<string> referencedAssemblies,
            IEnumerable<CodeParameter> parameters)
        {
            var assemblyCode = FormatAssemblySourceCode(sourceCode, 
                usingNamespaces, returnType, parameters);

            referencedAssemblies = AddParameterAssemblies(referencedAssemblies, parameters);

            using (var compiler = new TCodeDomProvider())
            {
                var dynamicAssembly = DynamicAssembly.CompileDynamicAssembly(
                    assemblyCode, compiler,
                    c => CompileAssembly(assemblyCode,
                        referencedAssemblies, c, null));

                return DynamicAssembly.CreateDelegate(dynamicAssembly);
            }
        }


        private CompilerResults CompileAssembly(string assemblySourceCode,
            IEnumerable<string> referencedAssemblies,
            CodeDomProvider compiler,
            string assemblyDirectory)
        {
            var inMemmory = assemblyDirectory == null;
            var compileParams = new CompilerParameters
            {
                GenerateInMemory = inMemmory,
                TempFiles = inMemmory ? null : new TempFileCollection(assemblyDirectory),
            };
            if (referencedAssemblies != null)
                compileParams.ReferencedAssemblies.AddRange(referencedAssemblies.ToArray());

            var compileResult = compiler.CompileAssemblyFromSource(
                compileParams, assemblySourceCode);

            if (compileResult.Errors.Count > 0)
            {
                var msg = FormatErrors(compileResult);
                throw new InvalidOperationException(msg);
            }

            return compileResult;
        }


        protected abstract string FormatAssemblySourceCode(string sourceCode,
            IEnumerable<string> usingNamespaces, Type returnType,
            IEnumerable<CodeParameter> parameters);


        protected abstract int CodeHeaderHeight { get; }


        private string CompileAssemblyFile(string assemblySourceCode,
            IEnumerable<string> referencedAssemblies, string assemblyDirectory,
            CodeDomProvider compiler)
        {
            var compileResult = CompileAssembly(assemblySourceCode,
                referencedAssemblies, compiler, assemblyDirectory);
            return compileResult.PathToAssembly;
        }


        private string FormatErrors(CompilerResults results)
        {
            return results.Errors
                .Cast<object>()
                .Select((t, i) => results.Errors[i])
                .Where(e => !e.IsWarning)
                .Select(e => string.Format(CultureInfo.InvariantCulture,
                    "Line {0}, Column {1}, Error: {2}{3}",
                    e.Line - CodeHeaderHeight + 1, 
                    e.Column, e.ErrorText, Environment.NewLine))
                .Aggregate((a, e) => a + e);
        }


        private static IEnumerable<string> AddParameterAssemblies(
            IEnumerable<string> referencedAssemblies,
            IEnumerable<CodeParameter> parameters)
        {
            return parameters.Select(p => p.ParameterType.Assembly.Location)
                .Union(referencedAssemblies ?? new List<string>());
        }
    }
}
