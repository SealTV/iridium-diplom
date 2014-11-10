namespace Iridium.Server.Protocol
{
    public struct Destination
    {
        public readonly Direction Direction;
        public readonly int Id;

        public Destination(Direction direction, int id)
        {
            this.Direction = direction;
            this.Id = id;
        }
    }

}