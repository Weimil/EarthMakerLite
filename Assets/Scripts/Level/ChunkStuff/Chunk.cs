using System.Collections.Generic;
using UnityEngine.Rendering;
using Level.BlockStuff;
using Unity.Mathematics;
using UnityEngine;
using static Miscellaneous.Constants;

namespace Level.ChunkStuff
{
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    [RequireComponent(typeof(MeshFilter))]
    public class Chunk : MonoBehaviour
    {
        private Block[,,] _blocks;
        private MeshCollider _meshCollider;
        private MeshRenderer _meshRenderer;
        private MeshFilter _meshFilter;
        private List<Vector3> _vertex;
        private List<Vector2> _uvs;
        private List<int> _triangles;
        private Vector3 _position;
        private int _vertexIndex;

        private void Awake()
        {
            _blocks = new Block[ChunkWidth, ChunkHeight, ChunkWidth];
            _meshCollider = GetComponent<MeshCollider>();
            _meshRenderer = GetComponent<MeshRenderer>();
            _meshFilter = GetComponent<MeshFilter>();
            _triangles = new List<int>();
            _vertex = new List<Vector3>();
            _uvs = new List<Vector2>();
            _position = transform.position;
            _vertexIndex = 0;
        }

        public void Init()
        {
            int[,] heightMap = CreateHeightMap();
            Population population = new Population();
            population.Populate(ref _blocks, heightMap);
            RenderChunk(heightMap);
        }

        private int[,] CreateHeightMap()
        {
            int[,] heightMap = new int[ChunkWidth + 2, ChunkWidth + 2];

            for (int x = 0; x < heightMap.GetLength(0); x++)
            for (int z = 0; z < heightMap.GetLength(1); z++)
            {
                float newZ = (z + _position.z + 0.001f) / Scale;
                float newX = (x + _position.x + 0.001f) / Scale;

                float totalDivisions = 0;
                float noiseValue = 0;
                float multiplier = 1;
                float divider = 1;

                for (int i = 0; i < Octaves; i++)
                {
                    noiseValue += divider * noise.snoise(new float3(newX * multiplier, Seed, newZ * multiplier));
                    totalDivisions += divider;
                    multiplier *= 2;
                    divider /= 2;
                }

                heightMap[x, z] = (int) (noiseValue / totalDivisions * 32 + 64);
            }

            return heightMap;
        }

        private void RenderChunk(int[,] heightMap)
        {
            const int offSet = 8;
            for (int x = 0; x < _blocks.GetLength(0); x++)
            for (int y = 0; y < _blocks.GetLength(1); y++)
            for (int z = 0; z < _blocks.GetLength(2); z++)
                if (_blocks[x, y, z] != null)
                    for (int face = 0; face < 6; face++)
                        if (IsVisible(x, y, z, face, heightMap))
                            MeshTables.Mesh(
                                ref _triangles, ref _vertex, ref _uvs, ref _vertexIndex,
                                new Vector3(x - offSet, y, z - offSet), _blocks[x, y, z], face
                            );
        }

        private bool IsVisible(int x, int y, int z, int face, int[,] heightMap)
        {
            int cX = x + (int) MeshTables.FaceCheck[face].x;
            int cY = y + (int) MeshTables.FaceCheck[face].y;
            int cZ = z + (int) MeshTables.FaceCheck[face].z;

            if (cX >= 0 && cX < ChunkWidth && cY >= 0 && cY < ChunkHeight && cZ >= 0 && cZ < ChunkWidth)
                return _blocks[cX, cY, cZ] == null;
            return heightMap[cX + 1, cZ + 1] < y;
        }

        public void InitMesh()
        {
            Mesh mesh = new Mesh
            {
                indexFormat = IndexFormat.UInt32,
                vertices = _vertex.ToArray(),
                triangles = _triangles.ToArray(),
                uv = _uvs.ToArray()
            };
            mesh.RecalculateNormals();
            _meshFilter.mesh = mesh;
            _meshCollider.sharedMesh = mesh;
        }
    }
}