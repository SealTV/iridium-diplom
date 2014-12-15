namespace Assets.Scripts
{
    public interface IBlock
    {
        void OnChangeBlock(IBlock[] subBlocks);
        float GetHeight();
        void Streach();
    }
}
