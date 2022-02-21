using System;
using System.Collections.Generic;
using Miscellaneous;
using UnityEngine;

namespace LoadManagement
{
    public class LoadClient : MonoBehaviour
    {
        private List<Vector2> _viewDistanceMask;
        private LoadServer _server;
        private Vector2 _position;
        private int _viewDistance;

        public void Init(LoadServer server)
        {
            _viewDistanceMask = new List<Vector2>();
            // 8 rn is perfect, will see after all the generation part...
            // Although I want to implement much more async stuff
            _viewDistance = 8;
            _server = server;
            CreateViewDistanceMask();
            SendRequest(Vector2.zero);
        }

        private void Update()
        {
            Vector3 position = transform.position;
            Vector2 currentPositionV2 = new Vector2(
                (int) (Math.Abs(position.x) / 16f + .5f) * Mathw.IntSign((int) position.x),
                (int) (Math.Abs(position.z) / 16f + .5f) * Mathw.IntSign((int) position.z)
            );
            if (_position != currentPositionV2) SendRequest(currentPositionV2);
        }

        private void SendRequest(Vector2 currentPosition)
        {
            _position = currentPosition;
            HashSet<Vector2> hashSet = new HashSet<Vector2>();
            for (int i = 0; i < _viewDistanceMask.Count; i++)
                hashSet.Add(new Vector2(
                    _viewDistanceMask[i].x + (int) _position.x,
                    _viewDistanceMask[i].y + (int) _position.y
                ));
            _server.ChunkRequest(hashSet);
        }

        private void CreateViewDistanceMask()
        {
            _viewDistanceMask.Clear();
            int max = _viewDistance * 2 + 1;
            for (int i = 0; i < max; i++)
            for (int j = 0; j < max; j++)
                _viewDistanceMask.Add(new Vector2(i - _viewDistance, j - _viewDistance));
        }
    }
}