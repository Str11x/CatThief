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
    private int _defaultLayer;
    private int _playerLayer;

    public static event Action StartedRobbery;

    public bool IsStartMove { get; private set; } = false;

    private void Start()
    {
        FieldOfViewCalculate.GameIsLost += StopMove;

        _defaultLayer = LayerMask.NameToLayer("Default");
        _playerLayer = LayerMask.NameToLayer("Player");
        gameObject.layer = _defaultLayer;
        transform.position = _pathCreator.transform.position;
    }

    private void OnDisable()
    {
        FieldOfViewCalculate.GameIsLost -= StopMove;
    }

    private IEnumerator Move()
    {
        int nextPoint = 0;
        float maxDistanceDelta = 0.1f;

        while (nextPoint < _pathHandler.GetAllPathPoints())
        {
            transform.LookAt(_pathHandler.GetPathPoint(nextPoint));

            while (transform.position != _pathHandler.GetPathPoint(nextPoint))
            {
                transform.position = Vector3.MoveTowards(transform.position, _pathHandler.GetPathPoint(nextPoint), maxDistanceDelta);

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

        StopMove();
        _movement = StartCoroutine(Move());

        gameObject.layer = _playerLayer;
    }
}