using System;
using System.Reflection;

namespace Tech.CodeGeneration.Internals
{
    internal class CodeProxy : MarshalByRefObject
    {
        public CodeProxy(string assemblyLocation)
        {
            _delegate = DynamicAssembly.LoadAndCreateDynamicDelegate(assemblyLocation);
        }


        public object Execute(params object[] parameterValues)
        {
            try
            {
                return _delegate.DynamicInvoke(parameterValues);
            }
            catch (TargetInvocationException x)
            {
                throw x.InnerException;
            }
        }


        private readonly Delegate _delegate;
    }
}
