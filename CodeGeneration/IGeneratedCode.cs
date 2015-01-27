namespace Tech.CodeGeneration
{
    public interface IGeneratedCode<out TResult>
    {
         TResult Execute(params object[] parameterValues);
    }
}
