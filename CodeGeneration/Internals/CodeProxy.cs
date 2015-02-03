using System;
using System.Reflection;

namespace Tech.CodeGeneration.Internals
{
    internal class CodeProxy : MarshalByRefObject
    {

        public CodeProxy(string assemblyLocation, string methodName)
        {
            _delegate = DynamicAssembly.LoadAndCreateDynamicDelegate(assemblyLocation, methodName);
        }
        
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

        public object Execute(string mainMethodName, params object[] parameterValues)
        {
            try
            {
                return this._delegate.DynamicInvoke(parameterValues);
            }
            catch (TargetInvocationException x)
            {
                throw x.InnerException;
            }
        }

        private readonly Delegate _delegate;
    }
}
