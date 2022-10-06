using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Suite _defaultSuite;
    [SerializeField] Wallet _wallet;

    private Suite [] _allSuites;

    public int MarkersCount { get; private set; }

    public event Action StartedMovement;
    public event Action TouchedMarker;

    private void Awake()
    {
        _allSuites = GetComponentsInChildren<Suite>();

        foreach (Suite suite in _allSuites)
        {
            if (suite != _defaultSuite)
                suite.gameObject.SetActive(false);
        }     
    }

    public void AddCount()
    {
        _wallet.AddPoints();
    }

    public int GetBalance()
    {
        return _wallet.PointsCount;
    }

    public void AddMarkerCount()
    {
        MarkersCount++;
        TouchedMarker?.Invoke();
    }

    public void StartAnimation()
    {
        StartedMovement?.Invoke();
    }
}