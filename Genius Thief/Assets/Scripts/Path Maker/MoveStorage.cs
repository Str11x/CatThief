using System.Collections.Generic;
using UnityEngine;

public class MoveStorage
{
    private List<Vector3> _movementPoints = new List<Vector3>();
    private List<Vector3[]> _movementSteps = new List<Vector3[]>();

    public int MovementPointsCount => _movementPoints.Count;
    public int MovementStepsCount => _movementSteps.Count;

    public void AddMovementPoint(Vector3 newPoint)
    {
        _movementPoints.Add(newPoint);
    }

    public Vector3 GetMovementPoint(int index)
    {
        return _movementPoints[index];
    }

    public Vector3 GetLastMovementPoint()
    {
        return _movementPoints[_movementPoints.Count - 1];
    }

    public void Save()
    {
        Vector3[] newPathPoints = new Vector3[_movementPoints.Count];
        _movementPoints.CopyTo(newPathPoints);
        _movementSteps.Add(newPathPoints);
    }

    public void SetPreviousState()
    {
        int penultimateIndex = 2;

        if (_movementSteps.Count >= penultimateIndex)
        {
            Vector3[] lastArray = _movementSteps[_movementSteps.Count - 1];
            Vector3[] penultimateArray = _movementSteps[_movementSteps.Count - penultimateIndex];

            int amountDeleteElements = lastArray.Length - penultimateArray.Length;
            int startRangeDeleteIndex = _movementPoints.Count - amountDeleteElements;
            _movementPoints.RemoveRange(startRangeDeleteIndex, amountDeleteElements);
        }

        _movementSteps.RemoveAt(_movementSteps.Count - 1);
    }

    public void ClearMovementPoints()
    {
        _movementPoints.Clear();
    }

    public Vector3 GetFinishPoint()
    {
        Vector3[] lastList = _movementSteps[_movementSteps.Count - 1];
        Vector3 finishPoint = lastList[lastList.Length - 1];

        return finishPoint;
    }

    public Vector3 GetPointFromPreviousFinish(int index)
    {
        return _movementSteps[_movementSteps.Count - 1][index];
    }

    public int GetMovementStepsArrayLength()
    {
        return _movementSteps[_movementSteps.Count - 1].Length;
    }
}