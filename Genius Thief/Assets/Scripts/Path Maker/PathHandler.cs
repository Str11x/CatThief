using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PathHandler : MonoBehaviour
{
    [SerializeField] private GridMovementAgent _pathCreator;

    private List<Vector3> _movementPoints = new List<Vector3>();
    private List<Vector3[]> _movementSteps = new List<Vector3[]>();

    public event Action<Vector2Int> PointPlanned;
    public event Action PointsAdded;
    public event Action<Vector3> PathCreated;
    public event Action MovedToPreviousState;

    public void AddPoint(Vector2Int newPoint, Node playerPosition)
    {
        PointPlanned?.Invoke(newPoint);

        _pathCreator.MoveToTarget(playerPosition);
    }

    public void AddNewNodePoint(Vector3 newPoint)
    {
        _movementPoints.Add(newPoint);
        PointsAdded?.Invoke();
    }

    public Vector3 GetPathPoint(int index)
    {
        if(_movementPoints.Count < index && index < 0)
            throw new ArgumentOutOfRangeException();
        
        return _movementPoints[index];   
    }

    public void CreateFinishMarker()
    {
        PathCreated?.Invoke(_movementPoints[_movementPoints.Count - 1]);
    }

    public void SaveNewPointsState()
    {
        Vector3 [] newPathPoints = new Vector3 [_movementPoints.Count];

        _movementPoints.CopyTo(newPathPoints);
        _movementSteps.Add(newPathPoints);
    }

    public void BackToPreviousState()
    {
        if (_movementSteps.Count == 0 && _pathCreator.transform.position == _pathCreator.StartPosition || _movementSteps.Count < 0)
            return;

        if(_movementSteps.Count >= 2)
        {
            int amountDeleteElements = _movementSteps[_movementSteps.Count - 1].Length - _movementSteps[_movementSteps.Count - 2].Length;
            int startRangeDeleteIndex = _movementPoints.Count - amountDeleteElements;
            _movementPoints.RemoveRange(startRangeDeleteIndex, amountDeleteElements);
        }
        
        _movementSteps.RemoveAt(_movementSteps.Count - 1);
        MovedToPreviousState?.Invoke();
    }

    public Vector3 GetPreviousPathFinishPoint()
    {
        if (_movementSteps.Count == 0)
        {
            _movementPoints.Clear();
            return _pathCreator.StartPosition;
        }
            
        Vector3[] lasList = _movementSteps[_movementSteps.Count - 1];
        Vector3 finishPoint = lasList[lasList.Length - 1];

        return finishPoint;
    }

    public Vector3 GetPointFromPreviousState(int index)
    {
        return _movementSteps[_movementSteps.Count - 1][index];
    }

    public int GetRedrawPoints()
    {
        return _movementSteps[_movementSteps.Count - 1].Length;
    }
}