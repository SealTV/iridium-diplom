using System;
using System.Collections.Generic;

namespace Tech.CodeGeneration
{
    public interface ICompiler
    {
        string CompileAssemblyFile(string sourceCode,
                                IEnumerable<string> usingNamespaces,
                                Type returnType,
                                IEnumerable<string> referencedAssemblies,
                                IEnumerable<CodeParameter> parameters,
                                string assemblyDirectory);

        Delegate CompileDelegate(string sourceCode,
                                IEnumerable<string> usingNamespaces,
                                Type returnType,
                                IEnumerable<string> referencedAssemblies, 
                                IEnumerable<CodeParameter> parameters);
    }
}
