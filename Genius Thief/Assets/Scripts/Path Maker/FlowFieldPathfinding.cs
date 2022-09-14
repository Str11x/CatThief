using System.Collections;
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
        {
            node.ResetWeight();
        }

        Queue<Vector2Int> queue = new Queue<Vector2Int>();

        queue.Enqueue(_target);
        _grid.GetNode(_target).PathWeight = 0;

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
                    neighbourNode.NextNode = currentNode;
                    neighbourNode.PathWeight = weightToTarget;
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

        bool hasRightNode = rightCoordinate.x < _grid.Width && _grid.GetNode(rightCoordinate).IsOccupied != true;
        bool hasLeftNode = leftCoordinate.x >= 0 && _grid.GetNode(leftCoordinate).IsOccupied != true;
        bool hasUpNode = upCoordinate.y < _grid.Height && _grid.GetNode(upCoordinate).IsOccupied != true;
        bool hasDowntNode = downCoordinate.y >= 0 && _grid.GetNode(downCoordinate).IsOccupied != true;

        if (hasRightNode)
            yield return rightCoordinate;
        if (hasLeftNode)
            yield return leftCoordinate;
        if (hasUpNode)
            yield return upCoordinate;
        if (hasDowntNode)
            yield return downCoordinate;
    }

    public Vector2Int GetFreeNeighbour(Vector2Int coordinate)
    {
        bool isFoundNode = false;
        int additionalCoordinate = 1;

        while(isFoundNode == false)
        {
            Vector2Int rightCoordinate = coordinate + new Vector2Int(additionalCoordinate, 0);
            bool hasRightNode = rightCoordinate.x < _grid.Width && _grid.GetNode(rightCoordinate).IsOccupied != true;
            if (hasRightNode)
            {
                isFoundNode = true;
                return rightCoordinate;        
            }
                

            Vector2Int leftCoordinate = coordinate + new Vector2Int(-additionalCoordinate, 0);
            bool hasLeftNode = leftCoordinate.x >= 0 && _grid.GetNode(leftCoordinate).IsOccupied != true;
            if (hasLeftNode)
            {
                isFoundNode = true;
                return leftCoordinate;
            }
                

            Vector2Int upCoordinate = coordinate + new Vector2Int(0, additionalCoordinate);
            bool hasUpNode = upCoordinate.y < _grid.Height && _grid.GetNode(upCoordinate).IsOccupied != true;
            if (hasUpNode)
            {
                isFoundNode = true;
                return upCoordinate;
            }
                

            Vector2Int downCoordinate = coordinate + new Vector2Int(0, -additionalCoordinate);
            bool hasDowntNode = downCoordinate.y >= 0 && _grid.GetNode(downCoordinate).IsOccupied != true;
            if (hasDowntNode)
            {
                isFoundNode = true;
                return downCoordinate;
            }

            additionalCoordinate++;
        }

        return coordinate;
    }
}