namespace Assets.Scripts
{
    public interface IBlock
    {
        void OnChangeBlock(IBlock[] subBlocks);
        float GetHeight();
        float GetWidth();
        void Streach();
    }
}
