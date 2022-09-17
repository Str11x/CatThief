using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FieldOfViewCalculate))]
public class FieldOfViewRenderer : MonoBehaviour
{
    [SerializeField] private float _meshResolution;
    [SerializeField] private MeshFilter _viewMeshFilter;
    [SerializeField] private int _edgeResolveIterations;
    [SerializeField] private int _edgeDistanceTreshold;

    private FieldOfViewCalculate _fieldOfView;
    private Mesh _viewMesh;

    float halfAngle = 0.5f;

    private void Start()
    {
        _viewMesh = new Mesh();
        _viewMesh.name = "View Mesh";
        _viewMeshFilter.mesh = _viewMesh;
        _fieldOfView = GetComponent<FieldOfViewCalculate>();
        
        StartCoroutine(DrawField());
    }

    private IEnumerator DrawField()
    {
        int stepCount = Mathf.RoundToInt(_fieldOfView.Angle * _meshResolution);
        float stepAngleSize = _fieldOfView.Angle / stepCount;   

        while(_fieldOfView.CanSeePlayer == false)
        {
            RedrawNewMeshTriangles(stepCount, halfAngle, stepAngleSize);
            yield return null;
        }
    }

    private void RedrawNewMeshTriangles(float stepCount, float halfAngle, float stepAngleSize)
    {
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();

        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - _fieldOfView.Angle * halfAngle + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);

            if(i > 0)
            {
                bool edgeDistanceExceeded = Mathf.Abs(oldViewCast.Distance - newViewCast.Distance) > _edgeDistanceTreshold;
                if(oldViewCast.Hit != newViewCast.Hit || oldViewCast.Hit && newViewCast.Hit && edgeDistanceExceeded)
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);

                    if(edge.PointA != Vector3.zero)
                        viewPoints.Add(edge.PointA);
                    if (edge.PointB != Vector3.zero)
                        viewPoints.Add(edge.PointB);
                }
            }
            viewPoints.Add(newViewCast.Point);
            oldViewCast = newViewCast;
        }

        int trianglesIndex = 3;
        int initialVertexCount = 2;
        int vertexCount = viewPoints.Count + 1;

        Vector3[] verticles = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - initialVertexCount) * trianglesIndex];
        verticles[0] = Vector3.zero;

        for (int i = 0; i < vertexCount - 1; i++)
        {
            int nextVertex = 1;
            verticles[i + nextVertex] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - initialVertexCount)
            {
                triangles[i * trianglesIndex] = 0;
                triangles[i * trianglesIndex + nextVertex] = i + nextVertex;
                triangles[i * trianglesIndex + (nextVertex + nextVertex)] = i + (nextVertex + nextVertex);
            }
        }

        _viewMesh.Clear();
        _viewMesh.vertices = verticles;
        _viewMesh.triangles = triangles;
        _viewMesh.RecalculateNormals();
    }

    private ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 direction = _fieldOfView.DirectionFromAngle(globalAngle);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction, out hit, _fieldOfView.Radius, _fieldOfView.ObstructionMask))
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        else
            return new ViewCastInfo(false, transform.position + direction * _fieldOfView.Radius, hit.distance, globalAngle);
    }

    private EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.Angle;
        float maxAngle = maxViewCast.Angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for(int i = 0; i < _edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) * halfAngle;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDistanceExceeded = Mathf.Abs(minViewCast.Distance - newViewCast.Distance) > _edgeDistanceTreshold;

            if (newViewCast.Hit == minViewCast.Hit && edgeDistanceExceeded == false)
            {
                minAngle = angle;
                minPoint = newViewCast.Point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.Point;  
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }

    public struct ViewCastInfo
    {
        public bool Hit;
        public Vector3 Point;
        public float Distance;
        public float Angle;

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
        public Vector3 PointA;
        public Vector3 PointB;

        public EdgeInfo(Vector3 pointA, Vector3 pointB)
        {
            PointA = pointA;
            PointB = pointB;
        }
    }
}