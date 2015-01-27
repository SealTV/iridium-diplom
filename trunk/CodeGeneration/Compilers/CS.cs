using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.CSharp;
using Tech.CodeGeneration.Internals;

namespace Tech.CodeGeneration.Compilers
{
    public class CS : BaseCompiler<CSharpCodeProvider>, ICompiler
    {
        public static CS Compiler
        {
            get { return _instance; }
        }

        protected override string FormatAssemblySourceCode(string sourceCode,
            IEnumerable<string> usingNamespaces, Type returnType, 
            IEnumerable<CodeParameter> parameters)
        {
            var usings = usingNamespaces == null ? "" : 
                string.Join(Environment.NewLine, usingNamespaces.Select(
                    u => "using " + u + ";"));

            var codeParameters = string.Join(", ", 
                parameters.Select(p => p.ParameterType + " " + p.Name));

            var codeHeader = string.Format(CultureInfo.InvariantCulture,
                CODE_HEADER_FORMAT, DynamicAssembly.DYNAMIC_CLASS_NAME,
                returnType.FullName, DynamicAssembly.DYNAMIC_METHOD_NAME, codeParameters);

            var codeFooter = string.Format(CultureInfo.InvariantCulture, 
                CODE_FOOTER_FORMAT, returnType.FullName);

            return string.Format(CultureInfo.InvariantCulture,
                "{0}{1}{2}{3}", usings, codeHeader, sourceCode, codeFooter);
        }


        protected override int CodeHeaderHeight
        {
            get { return CODE_HEADER_FORMAT.Split('\n').Count(); }
        }


        private const string CODE_HEADER_FORMAT  = @"
using System;
public class {0} 
{{
    public {1} {2}({3})
    {{
";
        private const string CODE_FOOTER_FORMAT = @"
        return default({0});
    }}
}}";

        private CS() {}
        private static readonly CS _instance = new CS();
    }
}
