using System;
using UnityEngine;

public class PathHandler : MonoBehaviour
{
    [SerializeField] private GridMovementAgent _pathCreator;
    [SerializeField] private PlayerMovementAgent _playerMovementAgent;
    [SerializeField] private Exit _exit;
    [SerializeField] private TimeService _menu;

    private MoveStorage _storage;

    public event Action<Vector2Int> PointPlanned;
    public event Action PointsAdded;
    public event Action<Vector3> PathCreated;
    public event Action MovedToPreviousState;
    public event Action<Vector3> CreatedPathToExit;
    public event Action StartedMove;

    private void Awake()
    {
        _storage = new MoveStorage();
    }
    public void AddPoint(Vector2Int newPoint, Node playerPosition)
    {
        PointPlanned?.Invoke(newPoint);
        _pathCreator.MoveToTarget(playerPosition);
    }

    public void ClearRendererPoints()
    {
        StartedMove?.Invoke();
    }

    public void AddPathToExit(Vector3 exitPosition)
    {
        CreatedPathToExit?.Invoke(exitPosition);
    }

    public void AddNewNodePoint(Vector3 newPoint)
    {
        _storage.AddMovementPoint(newPoint);
        PointsAdded?.Invoke();
    }

    public Vector3 GetPathPoint(int index)
    {
        if(_storage.MovementPointsCount < index && index < 0)
            throw new ArgumentOutOfRangeException();
        
        return _storage.GetMovementPoint(index);   
    }

    public void CreateFinishMarker()
    {
        PathCreated?.Invoke(_storage.GetLastMovementPoint());
    }

    public void SaveNewPointsState()
    {
        _storage.Save();
    }

    public void BackToPreviousState()
    {
        if (_storage.MovementStepsCount == 0 && _pathCreator.transform.position == _pathCreator.StartPosition || 
            _storage.MovementStepsCount < 0 || _menu.IsInteractWithMenu)
            return;

        _storage.SetPreviousState();
        MovedToPreviousState?.Invoke();
    }

    public Vector3 GetPreviousPathFinishPoint()
    {
        if (_storage.MovementStepsCount == 0)
        {
            _storage.ClearMovementPoints();
            return _pathCreator.StartPosition;
        }

        return _storage.GetFinishPoint();
    }

    public Vector3 GetPointFromPreviousState(int index)
    {
        return _storage.GetPointFromPreviousFinish(index);
    }

    public int GetRedrawPoints()
    {
        return _storage.GetMovementStepsArrayLength();
    }

    public int GetAllPathPoints()
    {
        return _storage.MovementPointsCount;
    }

    public Vector3 GetExitPosition()
    {
        return _exit.transform.position;
    }

    public bool IsPlayerMove()
    {
        return _playerMovementAgent.IsStartMove;
    }

    public bool IsNewPointAvailable()
    {
        if (_playerMovementAgent.IsStartMove || _exit.IsPlayerPlannedExit || _menu.IsInteractWithMenu)
            return false;

        return true;
    }

    public bool IsInteractWithMenu()
    {
        return _menu.IsInteractWithMenu;
    }
}