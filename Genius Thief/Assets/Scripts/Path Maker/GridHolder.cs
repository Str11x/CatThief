using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GridHolder : MonoBehaviour
{
    [SerializeField] private PathCreator _pathCreator;
    [SerializeField] private PathHandler _pathHandler;
    [SerializeField] private int _gridWidth;
    [SerializeField] private int _gridHeight;
    [SerializeField] private float _nodeSize;

    private Vector2Int _targetCoordinate;
    private Grid _grid;
    private Camera _camera;

    private float _offsetNumber = 0.5f;
    private float _sizeCorrection = 0.1f;

    public Vector3 Offset { get; private set; }
    public int PlaneHeight { get; private set; } = 1;

    private void Awake()
    {
        _pathHandler.PointPlanned += FindPath;
        _camera = Camera.main;

        float width = _gridWidth * _nodeSize;
        float height = _gridHeight * _nodeSize;

        transform.localScale = new Vector3(width * _sizeCorrection, PlaneHeight, height * _sizeCorrection);

        Offset = transform.position - new Vector3(width, 0, height) * _offsetNumber;

        _grid = new Grid(_gridWidth, _gridHeight, Offset, _nodeSize);
    }

    private void OnValidate()
    {
        float width = _gridWidth * _nodeSize;
        float height = _gridHeight * _nodeSize;

        transform.localScale = new Vector3(width * _sizeCorrection, PlaneHeight, height * _sizeCorrection);

        Offset = transform.position - new Vector3(width, 0, height) * _offsetNumber;
    }

    private void OnDisable()
    {
        _pathHandler.PointPlanned -= FindPath;
    }

    public void AddPathPoint(InputAction.CallbackContext context)
    {
        Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit) && context.performed == true)
        {
            if (hit.transform != transform)
                return;

            Vector3 hitPosition = hit.point;
            Vector3 difference = hitPosition - Offset;

            _targetCoordinate = new Vector2Int(CalculateCoordinate((int)difference.x), 
                CalculateCoordinate((int)difference.z));
            
            Vector3 playerCoordinateDifference = _pathCreator.transform.position - Offset;

            Node playerNode = _grid.GetNode(CalculateCoordinate((int)playerCoordinateDifference.x), 
                CalculateCoordinate((int)playerCoordinateDifference.z));

            _pathHandler.AddPoint(_targetCoordinate, playerNode);
            //_grid.SetNewTarget(_targetCoordinate);
            //SetPath?.Invoke(playerNode);
        }
    }

    private int CalculateCoordinate(int coordinateOnAxis)
    {
        return (int)(coordinateOnAxis / _nodeSize);
    }
  

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        if (_grid == null)
            return;

        foreach (Node node in _grid.EnumerateAllNodes())
        {
            if (node.NextNode == null)
                continue;
            
            if (node.IsOccupied == true)
                continue;

            Vector3 start = node.Position;
            Vector3 end = node.NextNode.Position;

            Vector3 direction = end - start;

            start -= direction * 0.25f;
            end -= direction * 0.75f;

            Gizmos.DrawLine(start, end);
            //Gizmos.DrawLine(node.Position, new Vector3(node.Position.x, node.Position.y + 1, node.Position.z));
            Gizmos.DrawSphere(end, 0.1f);
        }
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
