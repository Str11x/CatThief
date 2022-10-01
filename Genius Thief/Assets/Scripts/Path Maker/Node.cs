using UnityEngine;

public class Node
{
    public Node NextNode;
    public bool IsOccupied;

    public float PathWeight;
    public Vector3 Position { get; private set; }

    public Node(Vector3 position)
    {
        Position = position;
    }

    public void ResetWeight()
    {
        PathWeight = float.MaxValue;
    }
}