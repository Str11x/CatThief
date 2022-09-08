using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PathHandler : MonoBehaviour
{
    [SerializeField] private GridMovementAgent _pathCreator;
    [SerializeField] private GridHolder _gridholder;
    private Vector2Int _drawPoint;

    private Queue<Vector2Int> _targetCoordinates = new Queue<Vector2Int>();
    private List<Vector3> _movementPoints = new List<Vector3>();

    public event Action<Vector2Int> PointPlanned;
    public event Action PointsAdded;

    private void Awake()
    {
        _gridholder = GetComponent<GridHolder>();
    }

    public void AddPoint(Vector2Int newPoint, Node playerPosition)
    {
        PointPlanned?.Invoke(newPoint);

        _pathCreator.MoveToTarget(playerPosition);
        //_playerNodes.Enqueue(playerPoint);
        //_targetCoordinates.Enqueue(newPoint);
        //_drawPoint = newPoint;

        //PointAdded?.Invoke();
    }

    public Vector2Int GetPointCoordinate()
    {
        return _drawPoint;
    }

    public void AddNewNodePoint(Vector3 newPoint)
    {
        _movementPoints.Add(newPoint);
    }

    public Vector3 GetPathPoint(int index)
    {
        //if (_movementPoints != null && index < _movementPoints.Count)
        //{
            return _movementPoints[index];
        //}

        //return new Vector3();
    }

    public int GetPositionCount()
    {
        return _movementPoints.Count;
    }

    public Vector3 [] GetArray()
    {
        return _movementPoints.ToArray();
    }

    public void GiveFinishedList()
    {
        PointsAdded?.Invoke();
    }
}
