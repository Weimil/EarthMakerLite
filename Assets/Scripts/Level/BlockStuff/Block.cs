namespace Level.BlockStuff
{
    public class Block
    {
        public BlockName BlockName { get; }
        public MeshType MeshType { get; }

        public Block(BlockName blockType, MeshType meshType)
        {
            BlockName = blockType;
            MeshType = meshType;
        }
    }

    public enum BlockName
    {
        Debug,
        Bedrock,
        Stone,
        Dirt,
        Grass,
        Sand
    }

    public enum MeshType
    {
        Debug,
        WFullBlock
    }

    public class BlockType
    {
        public readonly Block Bedrock = new Block(BlockName.Bedrock, MeshType.WFullBlock);
        public readonly Block Stone = new Block(BlockName.Stone, MeshType.WFullBlock);
        public readonly Block Dirt = new Block(BlockName.Dirt, MeshType.WFullBlock);
        public readonly Block Grass = new Block(BlockName.Grass, MeshType.WFullBlock);
        public readonly Block Sand = new Block(BlockName.Sand, MeshType.WFullBlock);
    }
}