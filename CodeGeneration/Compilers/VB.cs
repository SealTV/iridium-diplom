using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.VisualBasic;
using Tech.CodeGeneration.Internals;

namespace Tech.CodeGeneration.Compilers
{
    public class VB : BaseCompiler<VBCodeProvider>, ICompiler
    {
        public static VB Compiler
        {
            get { return _instance; }
        }

        protected override string FormatAssemblySourceCode(string sourceCode,
            IEnumerable<string> usingNamespaces, Type returnType, 
            IEnumerable<CodeParameter> parameters)
        {
            var usings = usingNamespaces == null ? "" : 
                string.Join(Environment.NewLine, usingNamespaces.Select(
                    u => "Imports " + u));

            var codeParameters = string.Join(", ",
                parameters.Select(p => p.Name + " As " + p.ParameterType));

            var codeHeader = string.Format(CultureInfo.InvariantCulture,
                CODE_HEADER_FORMAT, DynamicAssembly.DYNAMIC_CLASS_NAME,
                returnType.FullName, DynamicAssembly.DYNAMIC_METHOD_NAME, codeParameters);

            var codeFooter = string.Format(CultureInfo.InvariantCulture, 
                CODE_FOOTER);

            return string.Format(CultureInfo.InvariantCulture,
                "{0}{1}{2}{3}", usings, codeHeader, sourceCode, codeFooter);
        }


        protected override int CodeHeaderHeight
        {
            get { return CODE_HEADER_FORMAT.Split('\n').Count(); }
        }


        private const string CODE_HEADER_FORMAT  = @"
Imports System
Public Class {0} 
    Public Function {2}({3}) As {1}
";
        private const string CODE_FOOTER = @"
        Return Nothing
    End Function
End Class";

        private VB() {}
        private static readonly VB _instance = new VB();
    }
}
