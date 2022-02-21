using System;
using Level.BlockStuff;
using Miscellaneous;

namespace Level.ChunkStuff
{
    public class Population
    {
        private readonly Random _random;

        public Population()
        {
            _random = new Random((int) Constants.Seed);
        }

        public void Populate(ref Block[,,] blocks, int[,] heightMap)
        {
            StoneLayer(ref blocks, ref heightMap);
            BedrockLayer(ref blocks);
            DirtGrassLayer(ref blocks, ref heightMap);
        }

        private void BedrockLayer(ref Block[,,] blocks)
        {
            for (int x = 0; x < blocks.GetLength(0); x++)
            for (int y = 0; y < 5; y++)
            for (int z = 0; z < blocks.GetLength(2); z++)
                if (_random.NextDouble() > y / 5f)
                    blocks[x, y, z] = new BlockType().Bedrock;
        }

        private void StoneLayer(ref Block[,,] blocks, ref int[,] heightMap)
        {
            for (int x = 0; x < blocks.GetLength(0); x++)
            for (int z = 0; z < blocks.GetLength(2); z++)
            for (int y = 0; y < heightMap[x + 1, z + 1]; y++)
                blocks[x, y, z] = new BlockType().Stone;
        }

        private void DirtGrassLayer(ref Block[,,] blocks, ref int[,] heightMap)
        {
            for (int x = 0; x < blocks.GetLength(0); x++)
            for (int z = 0; z < blocks.GetLength(2); z++)
            {
                blocks[x, heightMap[x + 1, z + 1], z] = new BlockType().Grass;
                for (int i = 0; i < _random.Next(2, 4);)
                {
                    i++;
                    if (blocks[x, heightMap[x + 1, z + 1] - i, z].BlockName != BlockName.Stone) continue;
                    blocks[x, heightMap[x + 1, z + 1] - i, z] = new BlockType().Dirt;
                }
            }
        }
    }
}