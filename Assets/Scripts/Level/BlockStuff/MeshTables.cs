using System;
using System.Collections.Generic;
using UnityEngine;
using static Miscellaneous.Constants;

namespace Level.BlockStuff
{
    public static class MeshTables
    {
        /*
         * Order: X+- Y+- Z+-
         * X+: East  \ X-: West
         * Y+: Top   \ Y-: Bottom
         * Z+: North \ Z-: South
         *
         * Documentation in "MineClone.drawio"
         */
        public static readonly Vector3[] FaceCheck =
        {
            // Side facing ...
            new Vector3(0, 1, 0), // ... Top
            new Vector3(0, -1, 0), // ... Bottom
            new Vector3(0, 0, 1), // ... North
            new Vector3(0, 0, -1), // ... South
            new Vector3(1, 0, 0), // ... East
            new Vector3(-1, 0, 0) // ... West
        };

        private static readonly Vector3[] TMPFullBlockVertex =
        {
            new Vector3(0, 0, 0), // 0
            new Vector3(0, 0, 1), // 1
            new Vector3(1, 0, 0), // 2
            new Vector3(1, 0, 1), // 3
            new Vector3(0, 1, 0), // 4
            new Vector3(0, 1, 1), // 5
            new Vector3(1, 1, 0), // 6
            new Vector3(1, 1, 1) // 7
        };

        private static readonly int[][] TMPFullBlockTriangles =
        {
            // Side facing ...
            new[] {4, 5, 6, 6, 5, 7}, // ... Top
            new[] {0, 2, 1, 1, 2, 3}, // ... Bottom
            new[] {1, 3, 5, 5, 3, 7}, // ... North
            new[] {0, 4, 2, 2, 4, 6}, // ... South
            new[] {2, 6, 3, 3, 6, 7}, // ... East
            new[] {0, 1, 4, 4, 1, 5} // ... West
        };

        public static void Mesh(
            ref List<int> triangles, ref List<Vector3> vertex, ref List<Vector2> uvs,
            ref int vertexIndex, Vector3 position, Block block, int face
        )
        {
            TriangleListFromFace(block.MeshType, face, ref vertexIndex, ref triangles);
            VertexListFromFace(block.MeshType, face, position, ref vertex);
            UVsListFromFace(block.MeshType, face, block.BlockName, ref uvs);
        }

        private static void VertexListFromFace(MeshType meshType, int face, Vector3 pos, ref List<Vector3> list)
        {
            switch (meshType)
            {
                case MeshType.WFullBlock:
                    for (int i = 0; i < TMPFullBlockTriangles[face].Length; i++)
                        list.Add(TMPFullBlockVertex[TMPFullBlockTriangles[face][i]] + pos);
                    break;
            }
        }

        private static void TriangleListFromFace(MeshType meshType, int face, ref int vertexIndex, ref List<int> list)
        {
            switch (meshType)
            {
                case MeshType.WFullBlock:
                    for (int i = 0; i < TMPFullBlockTriangles[face].Length; i++)
                    {
                        list.Add(vertexIndex);
                        vertexIndex++;
                    }

                    break;
            }
        }

        private static void UVsListFromFace(MeshType meshType, int face, BlockName textureIndex, ref List<Vector2> list)
        {
            switch (meshType)
            {
                case MeshType.WFullBlock:
                    for (int i = 0; i < TMPFullBlockTriangles[face].Length; i++)
                        switch (face)
                        {
                            case 0:
                            case 1:
                                list.Add(new Vector2(
                                        TMPFullBlockVertex[TMPFullBlockTriangles[face][i]].x / TextureWidth +
                                        1f / TextureWidth * face,
                                        TMPFullBlockVertex[TMPFullBlockTriangles[face][i]].z / TextureHeight +
                                        1f / TextureHeight * (int) textureIndex
                                    )
                                );
                                break;

                            case 2:
                            case 3:
                                list.Add(new Vector2(
                                        TMPFullBlockVertex[TMPFullBlockTriangles[face][i]].x / TextureWidth +
                                        1f / TextureWidth * face,
                                        TMPFullBlockVertex[TMPFullBlockTriangles[face][i]].y / TextureHeight +
                                        1f / TextureHeight * (int) textureIndex
                                    )
                                );
                                break;

                            case 4:
                            case 5:
                                list.Add(new Vector2(
                                        TMPFullBlockVertex[TMPFullBlockTriangles[face][i]].z / TextureWidth +
                                        1f / TextureWidth * face,
                                        TMPFullBlockVertex[TMPFullBlockTriangles[face][i]].y / TextureHeight +
                                        1f / TextureHeight * (int) textureIndex
                                    )
                                );
                                break;
                        }

                    break;
            }
        }
    }
}