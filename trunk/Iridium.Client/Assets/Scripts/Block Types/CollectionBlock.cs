using System;

namespace Scripts.Block_Types
{
    using Scripts.Blocks;

    public class CollectionBlock : Block
    {
        public string Name;
        public override float GetHeight()
        {
            throw new NotImplementedException();
        }

        public override float GetWidth()
        {
            throw new NotImplementedException();
        }

        public override void Stretch()
        {
            base.Stretch();
        }

        public override void ReSortingLayers(int layer)
        {
            throw new NotImplementedException();
        }

        public override string GetCode()
        {
            return this.Name;
        }
    }
}
