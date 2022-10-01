using System;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    public int PointsCount { get; private set; } = 0;

    public event Action AddedCoin;

    internal void AddPoints()
    {
        PointsCount++;
        AddedCoin?.Invoke();
    }
}