using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using System;

public class PointWay : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _player;

    public bool IsLiftedObject { get; private set; }

    public event Action DirectionCreated;

    public void CreatePath(InputAction.CallbackContext context)
    {
        IsLiftedObject = false;

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        bool hasHit = Physics.Raycast(ray, out hit);

        if (hit.collider.TryGetComponent(out LiftedObject reward))
            IsLiftedObject = true;

        if (hasHit)
            SetDestination(hit.point);
    }

    private void SetDestination(Vector3 target)
    {
        _player.SetDestination(target);
        DirectionCreated?.Invoke();
    }
}