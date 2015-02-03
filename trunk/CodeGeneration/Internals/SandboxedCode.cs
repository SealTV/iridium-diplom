namespace Tech.CodeGeneration.Internals
{
    internal class SandboxedCode<TResult> : IGeneratedCode<TResult>
    {
        internal SandboxedCode(CodeProxy codeProxy)
        {
            _codeProxy = codeProxy;
        }

        internal SandboxedCode(string mainMethodName, CodeProxy codeProxy)
        {
            _codeProxy = codeProxy;
            _mainMethodName = mainMethodName;
        }


        public TResult Execute(params object[] parameterValues)
        {
            return (TResult)_codeProxy.Execute(parameterValues);
        }

        public TResult Execute(string mainMethodName, params object[] parameterValues)
        {
            return (TResult)_codeProxy.Execute(mainMethodName, parameterValues);
        }


        private readonly CodeProxy _codeProxy;
        private readonly string _mainMethodName;
    }
}
