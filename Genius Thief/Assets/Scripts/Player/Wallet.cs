using System;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    public int MarkersCount { get; private set; }
    public int PointsCount { get; private set; } = 0;

    public event Action CoinAdded;
    public event Action MarkerTouched;

    public void AddPoints()
    {
        PointsCount++;
        CoinAdded?.Invoke();
    }

    public void AddMarkerCount()
    {
        MarkersCount++;
        MarkerTouched?.Invoke();
    }
}