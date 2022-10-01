using System;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public bool IsPlayerPlannedExit { get; private set; }

    public static event Action LevelCompleted;

    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
            LevelCompleted?.Invoke();          
        else
            IsPlayerPlannedExit = true;
    }

    public void OnTriggerExit(Collider other)
    {
        IsPlayerPlannedExit = false;
    }
}