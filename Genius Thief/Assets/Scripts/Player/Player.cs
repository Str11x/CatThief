using UnityEngine;

public class Player : MonoBehaviour
{
    public int PointsCount { get; private set; } = 0;

    public void AddCount()
    {
        PointsCount++;
    }
}