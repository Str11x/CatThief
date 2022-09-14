using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private FlowFieldPathfinding _pathFinding;
    private Node[,] _nodes; 

    private int _width;
    private int _height;
    private float _offsetNumber = 0.5f;
    private float _obstacleDistance = 0.25f;

    public int Width => _width;

    public int Height => _height;

    public Grid(int width, int height, Vector3 offset, float nodeSize)
    {
        _width = width;
        _height = height;

        _nodes = new Node[_width, _height];

        for (int line = 0; line < _nodes.GetLength(0); line++)
        {
            for (int column = 0; column < _nodes.GetLength(1); column++)
            {
                _nodes[line, column] = new Node(offset +
                    new Vector3(line + _offsetNumber, 0, column + _offsetNumber) * nodeSize);

                if (IsObstacle(_nodes[line, column]))
                    _nodes[line, column].IsOccupied = true;
            }
        }

        _pathFinding = new FlowFieldPathfinding(this);
    }

    public Node GetNode(int line, int column)
    {
        if (line < 0 || line > _width)
            return null;

        if (column < 0 || line > _height)
            return null;

        return _nodes[line, column];
    }

    public Node GetNode(Vector2Int coordinate)
    {
        return GetNode(coordinate.x, coordinate.y);
    }

    public IEnumerable<Node> EnumerateAllNodes()
    {
        for (int line = 0; line < _width; line++)
        {
            for (int column = 0; column < _height; column++)
            {
                yield return GetNode(line, column);
            }
        }
    }

    private bool IsObstacle(Node node)
    {
        if (Physics.Raycast(node.Position, new Vector3(node.Position.x, node.Position.y + 1, node.Position.z),
            out RaycastHit hit, _obstacleDistance))
        {
            if(hit.collider.TryGetComponent<Obstacle>(out Obstacle obstacle))
                return true;
        }

        return false;
    }

    public void SetNewTarget(Vector2Int target)
    {
        _pathFinding.UpdateField(target);
    }

    public Vector2Int GetFreeNode(Vector2Int node)
    {
        return _pathFinding.GetFreeNeighbour(node);
    }
}