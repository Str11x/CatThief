using System;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    public int MarkersCount { get; private set; }
    public int PointsCount { get; private set; } = 0;

    public event Action AddedCoin;
    public event Action TouchedMarker;

    public void AddPoints()
    {
        PointsCount++;
        AddedCoin?.Invoke();
    }
    public int GetBalance()
    {
        return PointsCount;
    }

    public void AddMarkerCount()
    {
        MarkersCount++;
        TouchedMarker?.Invoke();
    }
}