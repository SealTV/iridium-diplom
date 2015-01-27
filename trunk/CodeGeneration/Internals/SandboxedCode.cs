namespace Tech.CodeGeneration.Internals
{
    internal class SandboxedCode<TResult> : IGeneratedCode<TResult>
    {
        internal SandboxedCode(CodeProxy codeProxy)
        {
            _codeProxy = codeProxy;
        }


        public TResult Execute(params object[] parameterValues)
        {
            return (TResult)_codeProxy.Execute(parameterValues);
        }


        private readonly CodeProxy _codeProxy;
    }
}
