using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Tech.CodeGeneration.Internals
{
    internal static class DynamicAssembly
    {
        public static Delegate CreateDelegate(Assembly dynamicAssembly)
        {
            var dynamicObject = dynamicAssembly.CreateInstance(DYNAMIC_CLASS_NAME);
            if (dynamicObject == null)
                throw new TypeLoadException(DYNAMIC_CLASS_NAME);

            var dynamicMethod = dynamicObject.GetType().GetMethod(DYNAMIC_METHOD_NAME);
            if (dynamicMethod == null)
                throw new MissingMethodException(DYNAMIC_METHOD_NAME);

            return GetDelegate(dynamicObject, dynamicMethod);
        }


        public static Assembly CompileDynamicAssembly(string assemblySourceCode, 
            CodeDomProvider compiler, 
            Func<CodeDomProvider, CompilerResults> compileProc)
        {
            Assembly asm;
            if (!_assemblyCach.TryGetValue(assemblySourceCode, out asm))
            {
                var compileResult = compileProc(compiler);

                asm = compileResult.CompiledAssembly;
                _assemblyCach.Add(assemblySourceCode, asm);
            }
            return asm;
        }


        public static Delegate LoadAndCreateDynamicDelegate(string assemblyLocation)
        {
            var assembly = Assembly.LoadFrom(assemblyLocation);
            return CreateDelegate(assembly);
        }


        private static Delegate GetDelegate(object target, MethodInfo method)
        {
            var parameters = method.GetParameters()
                  .Select(p => p.ParameterType)
                  .Concat(new[] { method.ReturnType })
                  .ToArray();
            var delegateType = Expression.GetDelegateType(parameters);
            return Delegate.CreateDelegate(delegateType, target, method);
        }


        private static readonly Dictionary<string, Assembly> _assemblyCach =
            new Dictionary<string, Assembly>();

        public const string DYNAMIC_CLASS_NAME = "DynamicClass";
        public const string DYNAMIC_METHOD_NAME = "DynamicMethod";
    }
}
