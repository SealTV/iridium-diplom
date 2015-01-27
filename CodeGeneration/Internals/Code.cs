using System;
using System.Reflection;

namespace Tech.CodeGeneration.Internals
{
    internal class Code<TResult> : IGeneratedCode<TResult>
    {
        internal Code(Delegate aDelegate)
        {
            _delegate = aDelegate;
        }

        public TResult Execute(params object[] parameterValues)
        {
            try
            {
                return (TResult)_delegate.DynamicInvoke(parameterValues);
            }
            catch (TargetInvocationException x)
            {
                throw x.InnerException;
            }
        }


        private readonly Delegate _delegate;        
    }
}
