using System;
using System.Collections;
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
        FieldOfViewCalculate.GameIsLost += StopMove;

        transform.position = _pathCreator.transform.position;
    }

    private void OnDisable()
    {
        FieldOfViewCalculate.GameIsLost -= StopMove;
    }

    private IEnumerator Move()
    {
        int nextPoint = 0;

        while (nextPoint < _pathHandler.GetAllPathPoints())
        {
            transform.LookAt(_pathHandler.GetPathPoint(nextPoint));

            while (transform.position != _pathHandler.GetPathPoint(nextPoint))
            {
                transform.position = Vector3.MoveTowards(transform.position, _pathHandler.GetPathPoint(nextPoint), 0.1f);

                yield return _updateTime;
            }

            nextPoint++;
        }
    }

    private void StopMove()
    {
        if (_movement != null)
            StopCoroutine(_movement);
    }

    public void MoveToTargets()
    {
        if (_pathHandler.IsInteractWithMenu())
            return;

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
}