namespace Tech.CodeGeneration
{
    public interface IGeneratedCode<out TResult>
    {
         TResult Execute(params object[] parameterValues);
         TResult Execute(string mainMethodName, params object[] parameterValues);
    }
}
