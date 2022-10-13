using System.Collections.Generic;
using UnityEngine;

public class FlowFieldPathfinding
{
    private Grid _grid;
    private Vector2Int _target;
    private float _pathWeight = 1;

    public FlowFieldPathfinding (Grid grid)
    {
        _grid = grid;
    }

    public void UpdateField(Vector2Int target)
    {
        _target = target;

        foreach (Node node in _grid.EnumerateAllNodes())
            node.ResetWeight();

        Queue<Vector2Int> queue = new Queue<Vector2Int>();

        queue.Enqueue(_target);
        _grid.GetNode(_target).SetWeight(0);

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            Node currentNode = _grid.GetNode(current);
            float weightToTarget = currentNode.PathWeight + _pathWeight;

            foreach(Vector2Int neighbour in GetNeighbours(current))
            {
                Node neighbourNode = _grid.GetNode(neighbour);

                if(weightToTarget < neighbourNode.PathWeight)
                {
                    neighbourNode.SetNextNode(currentNode);
                    neighbourNode.SetWeight(weightToTarget);
                    queue.Enqueue(neighbour);
                }
            }
        }
    }

    public IEnumerable<Vector2Int> GetNeighbours(Vector2Int coordinate)
    {
        Vector2Int rightCoordinate = coordinate + Vector2Int.right;
        Vector2Int leftCoordinate = coordinate + Vector2Int.left;
        Vector2Int upCoordinate = coordinate + Vector2Int.up;
        Vector2Int downCoordinate = coordinate + Vector2Int.down;

        if (IsRightNodeSuitable(rightCoordinate))
            yield return rightCoordinate;
        if (IsLeftNodeSuitable(leftCoordinate))
            yield return leftCoordinate;
        if (IsUpNodeSuitable(upCoordinate))
            yield return upCoordinate;
        if (IsDownNodeSuitable(downCoordinate))
            yield return downCoordinate;
    }

    public Vector2Int GetNearestFreeNeighbour(Vector2Int coordinate)
    {
        int distance = 1;
        int maximumDistance = 10;

        while(distance < maximumDistance)
        {
            Vector2Int rightCoordinate = coordinate + new Vector2Int(distance, 0);
            if (IsRightNodeSuitable(rightCoordinate))
                return rightCoordinate;                    

            Vector2Int leftCoordinate = coordinate + new Vector2Int(-distance, 0);
            if (IsLeftNodeSuitable(leftCoordinate))
                return leftCoordinate;              

            Vector2Int upCoordinate = coordinate + new Vector2Int(0, distance);
            if (IsUpNodeSuitable(upCoordinate))
                return upCoordinate;               

            Vector2Int downCoordinate = coordinate + new Vector2Int(0, -distance);
            if (IsDownNodeSuitable(downCoordinate))
                return downCoordinate;

            distance++;
        }

        return coordinate;
    }

    private bool IsRightNodeSuitable(Vector2Int rightCoordinate)
    {
        bool hasRightNode = rightCoordinate.x < _grid.Width && _grid.GetNode(rightCoordinate).IsOccupied != true;

        return hasRightNode;
    }

    private bool IsLeftNodeSuitable(Vector2Int leftCoordinate)
    {
        bool hasLeftNode = leftCoordinate.x >= 0 && _grid.GetNode(leftCoordinate).IsOccupied != true;

        return hasLeftNode;
    }

    private bool IsUpNodeSuitable(Vector2Int upCoordinate)
    {
        bool hasUpNode = upCoordinate.y < _grid.Height && _grid.GetNode(upCoordinate).IsOccupied != true;

        return hasUpNode;
    }

    private bool IsDownNodeSuitable(Vector2Int downCoordinate)
    {
        bool hasDownNode = downCoordinate.y >= 0 && _grid.GetNode(downCoordinate).IsOccupied != true;

        return hasDownNode;
    }
}