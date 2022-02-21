using System.Collections.Generic;
using System.Threading.Tasks;
using Level.ChunkStuff;
using static Miscellaneous.Constants;
using UnityEngine;

namespace Level.RegionStuff
{
    public class Region : MonoBehaviour
    {
        private Dictionary<Vector2, Chunk> _chunks;

        private void Awake()
        {
            _chunks = new Dictionary<Vector2, Chunk>();
        }

        public void InstantiateChunk(Vector2 vector)
        {
            Chunk chunk = Instantiate(
                ChunkPrefab,
                new Vector3(vector.x, 0, vector.y) * 16f,
                Quaternion.identity,
                transform
            );
            chunk.name = $"Chunk[{vector.x}|{vector.y}]";
            _chunks.Add(vector, chunk);
            LoadAsync(vector);
        }

        private async void LoadAsync(Vector2 vector)
        {
            await Task.Run(() => _chunks[vector].Init());
            _chunks[vector].InitMesh();
        }

        public void Unload(Vector2 vector)
        {
            if (!_chunks.ContainsKey(vector)) return;
            Destroy(_chunks[vector].gameObject);
            _chunks.Remove(vector);
        }

        public int ChunksCount()
        {
            return _chunks.Count;
        }
    }
}