using System;
using CoinSortClone.Structs;
using UnityEngine;

namespace CoinSortClone.Logic
{
    public static class Calculator
    {
        // Möller-Trumbore algorithm
        public static bool TriangleIntersect(Vector3 p1, Vector3 p2, Vector3 p3,
            ScreenToWorldPointData screenToWorldPoint)
        {
            // Vectors from p1 to p2/p3 (edges)
            Vector3 e1, e2;

            Vector3 p, q, t;
            float det, invDet, u, v;


            //Find vectors for two edges sharing vertex/point p1
            e1 = p2 - p1;
            e2 = p3 - p1;

            // calculating determinant 
            p = Vector3.Cross(screenToWorldPoint.Direction, e2);

            //Calculate determinat
            det = Vector3.Dot(e1, p);

            //if determinant is near zero, ray lies in plane of triangle otherwise not
            if (det > -Double.Epsilon && det < Double.Epsilon)
            {
                return false;
            }

            invDet = 1.0f / det;

            //calculate distance from p1 to ray origin
            t = screenToWorldPoint.Origin - p1;

            //Calculate u parameter
            u = Vector3.Dot(t, p) * invDet;

            //Check for ray hit
            if (u < 0 || u > 1)
            {
                return false;
            }

            //Prepare to test v parameter
            q = Vector3.Cross(t, e1);

            //Calculate v parameter
            v = Vector3.Dot(screenToWorldPoint.Direction, q) * invDet;

            //Check for ray hit
            if (v < 0 || u + v > 1)
            {
                return false;
            }

            if ((Vector3.Dot(e2, q) * invDet) > Double.Epsilon)
            {
                //ray does intersect
                return true;
            }

            // No hit at all
            return false;
        }

        public static Vector3 LocalPointToWorld(Vector3 p, Transform objectTransform) => objectTransform.localToWorldMatrix.MultiplyPoint3x4(p);
        
        private static Vector3 SolveEulerAngle(Transform objectTransform)
        {
            Vector3 result = objectTransform.eulerAngles;
            Vector3 prevAngle = result;


            if (Vector3.Dot(objectTransform.up, Vector3.up) >= 0f)
            {
                if (prevAngle.x >= 0f && prevAngle.x <= 90f)
                {
                    result.x = prevAngle.x;
                }

                if (prevAngle.x >= 270f && prevAngle.x <= 360f)
                {
                    result.x = prevAngle.x - 360f;
                }
            }

            if (Vector3.Dot(objectTransform.up, Vector3.up) < 0f)
            {
                if (prevAngle.x >= 0f && prevAngle.x <= 90f)
                {
                    result.x = 180 - prevAngle.x;
                }

                if (prevAngle.x >= 270f && prevAngle.x <= 360f)
                {
                    result.x = 180 - prevAngle.x;
                }
            }
            return result;
        }

        // public static Vector3 GetRotatedPosition(Vector3 position, Vector3 sinVector, Vector3 cosVector)
        // {
        //     // return new Vector3(position.x * cosVector.y * cosVector.z - position.y * sinVector.z + position.z * sinVector.y
        //     //     , position.x * sinVector.z + position.y * cosVector.x * cosVector.z - position.z * sinVector.x
        //     //     , -position.x * sinVector.y + position.y * sinVector.x + position.z * cosVector.x * cosVector.y);
        //
        //
        //     // Vector3 xRot = new Vector3(position.x, position.y * cosVector.x - position.z * sinVector.x,
        //     //     position.y * sinVector.x + position.z * cosVector.x);
        //     // Vector3 yRot = new Vector3(xRot.x * cosVector.y + xRot.z * sinVector.y, xRot.y,
        //     //     xRot.z * cosVector.y - xRot.x * sinVector.y);
        //     // Vector3 zRot = new Vector3(yRot.x * cosVector.z - yRot.y * sinVector.z,
        //     //     yRot.x * sinVector.z + yRot.y * cosVector.z, yRot.z);
        //     //
        //     // return zRot;
        //
        //     return new Vector3(position.x * cosVector.z * cosVector.y +
        //                        position.y * (-sinVector.z * cosVector.x + cosVector.z * sinVector.y * sinVector.x) +
        //                        position.z * (sinVector.z * sinVector.x + cosVector.z * sinVector.y * cosVector.x),
        //         position.x * sinVector.z * sinVector.y +
        //         position.y * (cosVector.z * cosVector.x + sinVector.z * sinVector.y * sinVector.x) +
        //         position.z * (-cosVector.z * sinVector.x + sinVector.z * sinVector.y * cosVector.x),
        //         position.x * (-sinVector.y) + position.y * cosVector.y * sinVector.x +
        //         position.z * cosVector.y * cosVector.x);
        // }

        public static Vector3 EulerAngleToSinVector(Transform objectTransform)
        {
            var angle = SolveEulerAngle(objectTransform) * Mathf.Deg2Rad;
            Vector3 sinRad = new Vector3(Mathf.Sin(angle.x), Mathf.Sin(angle.y), Mathf.Sin(angle.z));
            return sinRad;
        }

        public static Vector3 EulerAngleToCosVector(Transform objectTransform)
        {
            var angle = SolveEulerAngle(objectTransform) * Mathf.Deg2Rad;
            Vector3 cosRad = new Vector3(Mathf.Cos(angle.x), Mathf.Cos(angle.y), Mathf.Cos(angle.z));
            return cosRad;
        }
    }
}