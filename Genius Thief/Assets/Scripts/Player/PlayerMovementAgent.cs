using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementAgent : MonoBehaviour
{
    [SerializeField] private GridMovementAgent _pathCreator;
    [SerializeField] private PathHandler _pathHandler;
    [SerializeField] private Exit _exit;

    private WaitForFixedUpdate _updateTime = new WaitForFixedUpdate();
    private Coroutine _movement;

    public static event Action StartedRobbery;

    public bool IsStartMove { get; private set; } = false;

    private void Start()
    {
        transform.position = _pathCreator.transform.position;
    }

    public void MoveToTargets()
    {
        StartedRobbery?.Invoke();

        if (_exit.IsPlayerPlannedExit == false)
            _pathHandler.AddPathToExit(_exit.transform.position);

        IsStartMove = true;
        _pathHandler.ClearRendererPoints();

        if (_movement != null)
        {
            StopCoroutine(_movement);
            _movement = StartCoroutine(Move());
        }
        else
        {
            _movement = StartCoroutine(Move());
        }
    }

    private IEnumerator Move()
    {
        int nextPoint = 0;

        while (nextPoint < _pathHandler.GetAllPathPoints())
        {
            while(transform.position != _pathHandler.GetPathPoint(nextPoint))
            {
                transform.position = Vector3.MoveTowards(transform.position, _pathHandler.GetPathPoint(nextPoint), 0.1f);

                yield return _updateTime;
            }

            nextPoint++;
        }
    }
}