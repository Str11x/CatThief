using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PathHandler))]
public class PathPointCatcher : MonoBehaviour
{
    [SerializeField] private GridHolder _gridHolder;
    [SerializeField] private PathCreator _pathCreator;

    private PathHandler _pathHandler;
    private Camera _camera;
    private Vector2Int _targetCoordinate;
    private Grid _grid; 
    private Collider _collider;

    public event Action<bool> LootWasLastPoint;

    private void Start()
    {
        _pathHandler = GetComponent<PathHandler>();
        _camera = Camera.main;
        _grid = _gridHolder.GetGrid();
        _collider = _gridHolder.GetComponent<Collider>();

        _pathHandler.PointPlanned += FindPath;
        _pathHandler.CreatedPathToExit += AddPathPoint;
    }

    private void OnDisable()
    {
        _pathHandler.PointPlanned -= FindPath;
        _pathHandler.CreatedPathToExit -= AddPathPoint;
    }

    private void TryAddTouchPoint(Ray ray)
    {          
        if (Physics.Raycast(ray, out RaycastHit hit) && _pathHandler.IsNewPointAvailable() == true)
        {
            if (hit.transform != _gridHolder.transform && hit.collider.TryGetComponent(out Loot loot) == false
                || hit.collider.TryGetComponent(out Loot loot2) && loot2.IsLooted == true)
                return;

            if (hit.collider.TryGetComponent(out Loot lootObject))
            {
                lootObject.ScheduleInPathPoints();
                MakePathToLootObject(lootObject);
                LootWasLastPoint?.Invoke(true);
                return;
            }

            LootWasLastPoint?.Invoke(false);
            AddPathPoint(hit.point);
        }
    }

    private void AddPathPoint(Vector3 hitPosition)
    {
        _targetCoordinate = GetTargetPointWithOffset(hitPosition);       
        Node playerNode = GetPlayerNodeWithOffset();
        
        _pathHandler.AddPoint(_targetCoordinate, playerNode);
    }

    private void MakePathToLootObject(Loot loot)
    {
        Vector3 closestPoint = _collider.ClosestPoint(loot.transform.position);
        Vector2Int node = GetTargetPointWithOffset(closestPoint);

        Vector3 nodePosition = closestPoint - _gridHolder.Offset;
        Node pointNode = _grid.GetNode(GetCoordinateWithNodeSize((int)nodePosition.x),
                GetCoordinateWithNodeSize((int)nodePosition.z));

        Vector2Int freeNode = _grid.GetNearestFreeNode(node);

        _targetCoordinate = freeNode;
        pointNode = GetPlayerNodeWithOffset();

        _pathHandler.AddPoint(_targetCoordinate, pointNode);
    }

    private int GetCoordinateWithNodeSize(int coordinateOnAxis)
    {
        return (int)(coordinateOnAxis / _gridHolder.NodeSize);
    }

    private void FindPath(Vector2Int target)
    {
        _grid.SetNewTarget(target);
    }

    private Vector2Int GetTargetPointWithOffset(Vector3 hitPosition)
    {
        Vector3 difference = hitPosition - _gridHolder.Offset;

        Vector2Int targetPoint = new Vector2Int(GetCoordinateWithNodeSize((int)difference.x),
            GetCoordinateWithNodeSize((int)difference.z));

        return targetPoint;
    }

    private Node GetPlayerNodeWithOffset()
    {
        Vector3 playerCoordinateDifference = _pathCreator.transform.position - _gridHolder.Offset;

        Node node = _grid.GetNode(GetCoordinateWithNodeSize((int)playerCoordinateDifference.x),
            GetCoordinateWithNodeSize((int)playerCoordinateDifference.z));

        return node;
    }

    public void SetTouchPosition(InputAction.CallbackContext context)
    {
        Ray ray = _camera.ScreenPointToRay(Touchscreen.current.primaryTouch.position.ReadValue());

        if (context.performed == true)
            TryAddTouchPoint(ray);
    }

    public void SetClickPosition(InputAction.CallbackContext context)
    {
        Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (context.performed == true)
            TryAddTouchPoint(ray);
    }

    public Node GetTargetNode()
    {
        return _grid.GetNode(_targetCoordinate);
    }
}