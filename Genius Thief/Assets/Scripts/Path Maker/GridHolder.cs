using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GridHolder : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private int _gridWidth;
    [SerializeField] private int _gridHeight;
    [SerializeField] private float _nodeSize; 

    private Vector2Int _targetCoordinate;
    private Grid _grid;
    private Camera _camera;
    private Vector3 _offset;

    private int _planeHeight = 1;
    private float _offsetNumber = 0.5f;
    private float _sizeCorrection = 0.1f;

    public event UnityAction<Node> SetPath;

    private void Awake()
    {
        _camera = Camera.main;

        float width = _gridWidth * _nodeSize;
        float height = _gridHeight * _nodeSize;

        transform.localScale = new Vector3(width * _sizeCorrection, _planeHeight, height * _sizeCorrection);

        _offset = transform.position - new Vector3(width, 0, height) * _offsetNumber;

        _grid = new Grid(_gridWidth, _gridHeight, _offset, _nodeSize);

    }

    private void OnValidate()
    {
        float width = _gridWidth * _nodeSize;
        float height = _gridHeight * _nodeSize;

        transform.localScale = new Vector3(width * _sizeCorrection, _planeHeight, height * _sizeCorrection);

        _offset = transform.position - new Vector3(width, 0, height) * _offsetNumber;
    }

    public void SetNewPath(InputAction.CallbackContext context)
    {
        Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform != transform)
                return;

            Vector3 hitPosition = hit.point;
            Vector3 difference = hitPosition - _offset;

            int xCoordinate = (int)(difference.x / _nodeSize);
            int zCoordinate = (int)(difference.z / _nodeSize);

            _targetCoordinate = new Vector2Int(xCoordinate, zCoordinate);
            _grid.SetNewTarget(_targetCoordinate);

            Vector3 playerCoordinateDifference = _player.transform.position - _offset;

            int xPlayerCoordinate = (int)(playerCoordinateDifference.x / _nodeSize);
            int zPlayerCoordinate = (int)(playerCoordinateDifference.z / _nodeSize);

            Node playerNode = _grid.GetNode(xPlayerCoordinate, zPlayerCoordinate);
 
            SetPath?.Invoke(playerNode);
        }
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

    public Node GetTargetNode()
    {
        return _grid.GetNode(_targetCoordinate);
    }
}
