using System.IO;
using UnityEditor;
using UnityEngine;

namespace Game.DamageNumberGraphics
{
    public static class DamageNumberMeshAssetCreator
    {
        [MenuItem("Glider Game/Create/Damage Number Mesh")]
        public static void SaveMesh()
        {
// Number of quads
            // Number of quads
            int quadCount = 5;

            // Size of each quad
            float quadSize = 1f;
            float quadSpacingPer = 0.75f;
            // UV spacing between each quad
            float uvSpacing = 2f;

            // Vertices
            Vector3[] vertices = new Vector3[quadCount * 4];
            for (int i = 0; i < quadCount; i++)
            {
                vertices[i * 4 + 2] = new Vector3(i * quadSize*quadSpacingPer, 0, 0);
                vertices[i * 4 + 3] = new Vector3(i * quadSize*quadSpacingPer - quadSize, 0, 0);
                vertices[i * 4 + 0] = new Vector3(i * quadSize*quadSpacingPer - quadSize, quadSize, 0);
                vertices[i * 4 + 1] = new Vector3(i * quadSize*quadSpacingPer, quadSize, 0);
            }

            // Triangles
            int[] triangles = new int[quadCount * 6];
            for (int i = 0; i < quadCount; i++)
            {
                triangles[i * 6 + 5] = i * 4 + 3;
                triangles[i * 6 + 4] = i * 4 + 1;
                triangles[i * 6 + 3] = i * 4 + 0;
                triangles[i * 6 + 2] = i * 4 + 3;
                triangles[i * 6 + 1] = i * 4 + 2;
                triangles[i * 6 + 0] = i * 4 + 1;
            }

            // UVs
            Vector2[] uvs = new Vector2[vertices.Length];
            for (int i = 0; i < quadCount; i++)
            {
                uvs[i * 4 + 3] = new Vector2(i * uvSpacing, 0);
                uvs[i * 4 + 2] = new Vector2(i * uvSpacing + 1, 0);
                uvs[i * 4 + 1] = new Vector2(i * uvSpacing + 1, 1);
                uvs[i * 4 + 0] = new Vector2(i * uvSpacing, 1);
            }

            // Create mesh
            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            
            // Center the pivot of the mesh
            Vector3 center = mesh.bounds.center;
            Vector3[] meshVertices = mesh.vertices;
            for (int i = 0; i < meshVertices.Length; i++)
            {
                meshVertices[i] -= center;
            }
            mesh.vertices = meshVertices;
            mesh.RecalculateBounds();
            
            mesh.uv = uvs;

            // Save mesh to project folder
            AssetDatabase.CreateAsset(mesh, "Assets/Game/DamageNumberGraphics/DamageNumberMeshv3.asset");
            AssetDatabase.SaveAssets();
        }
    }
}