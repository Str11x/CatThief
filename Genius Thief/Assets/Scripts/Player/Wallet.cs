using System;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    public int PointsCount { get; private set; } = 0;

    public event Action AddedCoin;

    public void AddPoints()
    {
        PointsCount++;
        AddedCoin?.Invoke();
    }
}