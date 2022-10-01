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
    private int _maxRadiusLoot = 5;

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

    public void TryAddTouchPoint(InputAction.CallbackContext context)
    {
        Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit) && context.performed == true && _pathHandler.IsNewPointAvailable() == true)
        {
            if (hit.transform != _gridHolder.transform && hit.collider.TryGetComponent(out Loot loot) == false
                || hit.collider.TryGetComponent(out Loot loot2) && loot2.IsLooted == true)
                return;

            if (hit.collider.TryGetComponent(out Loot lootObject))
            {
                lootObject.ScheduleInPathPoints();
                MakePathToLootObject(lootObject);
                LootWasLastPoint(true);
                return;
            }

            LootWasLastPoint(false);
            AddPathPoint(hit.point);
        }
    }

    private void AddPathPoint(Vector3 hitPosition)
    {
        Vector3 difference = hitPosition - _gridHolder.Offset;

        _targetCoordinate = new Vector2Int(CalculateCoordinate((int)difference.x),
            CalculateCoordinate((int)difference.z));

        Vector3 playerCoordinateDifference = _pathCreator.transform.position - _gridHolder.Offset;

        Node playerNode = _grid.GetNode(CalculateCoordinate((int)playerCoordinateDifference.x),
            CalculateCoordinate((int)playerCoordinateDifference.z));

        _pathHandler.AddPoint(_targetCoordinate, playerNode);
    }

    private void MakePathToLootObject(Loot loot)
    {
        Vector3 closestPoint = _collider.ClosestPoint(loot.transform.position);

        Vector3 nodePosition = closestPoint - _gridHolder.Offset;

        Node pointNode = _grid.GetNode(CalculateCoordinate((int)nodePosition.x),
                CalculateCoordinate((int)nodePosition.z));

        Vector3 playerCoordinateDifference = _pathCreator.transform.position - _gridHolder.Offset;

        int repeat = 0;

        while (pointNode.IsOccupied == true && repeat < _maxRadiusLoot)
        {
            Vector2Int node = new Vector2Int(CalculateCoordinate((int)nodePosition.x),
                CalculateCoordinate((int)nodePosition.z));

            Vector2Int freeNode = _grid.GetNearestFreeNode(node);

            _targetCoordinate = freeNode;

            pointNode = _grid.GetNode(CalculateCoordinate((int)playerCoordinateDifference.x),
                CalculateCoordinate((int)playerCoordinateDifference.z));

            repeat++;
        }

        _pathHandler.AddPoint(_targetCoordinate, pointNode);
    }

    private int CalculateCoordinate(int coordinateOnAxis)
    {
        return (int)(coordinateOnAxis / _gridHolder.NodeSize);
    }

    private void FindPath(Vector2Int target)
    {
        _grid.SetNewTarget(target);
    }

    public Node GetTargetNode()
    {
        return _grid.GetNode(_targetCoordinate);
    }
}