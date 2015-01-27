namespace Iridium.Server.Games
{
    public interface Game
    {
        bool RunCode(string intput, string output, string codeSource, out string[] results);
    }
}
