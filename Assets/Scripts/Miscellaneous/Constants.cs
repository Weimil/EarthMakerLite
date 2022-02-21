using Level.ChunkStuff;
using Level.RegionStuff;
using UnityEngine;

namespace Miscellaneous
{
    public class Constants : MonoBehaviour
    {
        public const int Octaves = 4;
        public const int ChunkWidth = 16;
        public const int ChunkHeight = 256;
        public const int RegionWidth = 16;
        public const int TextureWidth = 6;
        public const int TextureHeight = 8;
        public const float Seed = 50;
        public const float Scale = 128f;

        public static readonly Chunk ChunkPrefab = Resources.Load<Chunk>("Prefabs/ChunkPrefab");
        public static readonly Region RegionPrefab = Resources.Load<Region>("Prefabs/RegionPrefab");
    }
}