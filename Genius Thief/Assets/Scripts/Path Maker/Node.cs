using UnityEngine;

public class Node
{
    public Node NextNode { get; private set; }
    public bool IsOccupied { get; private set; }
    public float PathWeight { get; private set; }
    public Vector3 Position { get; private set; }

    public Node(Vector3 position)
    {
        Position = position;
    }

    public void ResetWeight()
    {
        PathWeight = float.MaxValue;
    }

    public void SetWeight(float newWeight)
    {
        if (newWeight >= 0)
            PathWeight = newWeight;
    }

    public void SetNextNode(Node nextNode)
    {
        NextNode = nextNode;
    }

    public void SetOccupiedState(bool active)
    {
        IsOccupied = active;
    }
}