using System;
using UnityEngine;

public class PlayerAnimationState : MonoBehaviour
{
    public bool IsRun { get; private set; }

    public event Action MovementStarted;
    public void StartAnimation()
    {
        MovementStarted?.Invoke();
    }

    public void SavePlayerRun(bool state)
    {
        IsRun = state;
    }
}