namespace Tech.CodeGeneration.Compilers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Microsoft.CSharp;

    using Tech.CodeGeneration.Internals;

    public class CSExtend: BaseCompiler<CSharpCodeProvider>, ICompiler
    {
        public static CSExtend Compiler
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
                CODE_HEADER_FORMAT, DynamicAssembly.DYNAMIC_CLASS_NAME);

            var codeFooter = string.Format(CultureInfo.InvariantCulture,
                CODE_FOOTER_FORMAT);

            return string.Format(CultureInfo.InvariantCulture,
                "{0}{1}{2}{3}", usings, codeHeader, sourceCode, codeFooter);
        }

        protected override string FormatAssemblySourceCode(string sourceCode, string mainMethodName,
            IEnumerable<string> usingNamespaces, Type returnType,
            IEnumerable<CodeParameter> parameters)
        {
            var usings = usingNamespaces == null ? "" :
                string.Join(Environment.NewLine, usingNamespaces.Select(
                    u => "using " + u + ";"));

            var codeParameters = string.Join(", ",
                parameters.Select(p => p.ParameterType + " " + p.Name));

            var codeHeader = string.Format(CultureInfo.InvariantCulture,
                CODE_HEADER_FORMAT, DynamicAssembly.DYNAMIC_CLASS_NAME);

            var codeFooter = CODE_FOOTER_FORMAT;

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
";
        private const string CODE_FOOTER_FORMAT = @"
}";

        private CSExtend() { }
        private static readonly CSExtend _instance = new CSExtend();
    }
}
