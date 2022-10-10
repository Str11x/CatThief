using System;
using UnityEngine;

public class PlayerAnimationState : MonoBehaviour
{
    public bool IsRun { get; private set; }

    public event Action StartedMovement;
    public void StartAnimation()
    {
        StartedMovement?.Invoke();
    }

    public void SavePlayerRun(bool state)
    {
        IsRun = state;
    }
}
