using UnityEngine;

public partial class FieldOfViewRenderer
{
    private struct ViewCastInfo
    {
        public bool Hit { get; private set; }
        public Vector3 Point { get; private set; }
        public float Distance { get; private set; }
        public float Angle { get; private set; }

        public ViewCastInfo(bool hit, Vector3 point, float distance, float angle)
        {
            Hit = hit;
            Point = point;
            Distance = distance;
            Angle = angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 PointA { get; private set; }
        public Vector3 PointB { get; private set; }

        public EdgeInfo(Vector3 pointA, Vector3 pointB)
        {
            PointA = pointA;
            PointB = pointB;
        }
    }
}