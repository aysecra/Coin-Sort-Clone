using CoinSortClone.Structs;
using UnityEngine;

namespace CoinSortClone.Logic
{
    public static class ObjectDetector
    {
        public static bool CalculateIsHitToObject(DetectableObjectData objectData,
            ScreenToWorldPointData screenToWorldPointData)
        {
            return IsHitMesh(objectData, screenToWorldPointData);
        }

        private static bool IsHitMesh(DetectableObjectData objectData, ScreenToWorldPointData screenToWorldPoint)
        {
            var vertices = objectData.Mesh.vertices;
            var triangles = objectData.Mesh.triangles;

            int triangleCount = triangles.Length / 3;
            for (int i = 0; i < triangleCount; i++)
            {
                var p1 = Calculator.LocalPointToWorld(vertices[triangles[i * 3]], objectData.DetectableTransform);
                var p2 = Calculator.LocalPointToWorld(vertices[triangles[i * 3 + 1]], objectData.DetectableTransform);
                var p3 = Calculator.LocalPointToWorld(vertices[triangles[i * 3 + 2]], objectData.DetectableTransform);

                if (Calculator.TriangleIntersect(p1, p2, p3, screenToWorldPoint)) return true;
            }

            return false;
        }
    }
}