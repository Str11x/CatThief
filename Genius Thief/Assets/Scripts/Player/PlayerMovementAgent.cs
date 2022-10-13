using System;
using System.Collections;
using UnityEngine;

public class PlayerMovementAgent : MonoBehaviour
{
    private const string DefaultLayer = "Default";
    private const string PlayerLayer = "Player";

    [SerializeField] private GridMovementAgent _pathCreator;
    [SerializeField] private PathHandler _pathHandler;
    [SerializeField] private Exit _exit;

    private WaitForFixedUpdate _updateTime = new WaitForFixedUpdate();
    private Coroutine _movement;
    private int _defaultLayer;
    private int _playerLayer;

    public static event Action RobberyStarted;

    public bool IsStartMove { get; private set; } = false;

    private void Start()
    {
        FieldOfViewCalculate.GameIsLost += StopMove;

        _defaultLayer = LayerMask.NameToLayer(DefaultLayer);
        _playerLayer = LayerMask.NameToLayer(PlayerLayer);
        gameObject.layer = _defaultLayer;
        transform.position = _pathCreator.transform.position;
    }

    private void OnDisable()
    {
        FieldOfViewCalculate.GameIsLost -= StopMove;
    }

    private IEnumerator Move()
    {
        gameObject.layer = _playerLayer;
        float maxDistanceDelta = 0.1f;

        for (int nextPoint = 0; nextPoint < _pathHandler.GetAllPathPoints(); nextPoint++)
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

        RobberyStarted?.Invoke();

        if (_exit.IsPlayerPlannedExit == false)
            _pathHandler.AddPathToExit(_exit.Position);

        IsStartMove = true;
        _pathHandler.ClearRendererPoints();

        StopMove();
        _movement = StartCoroutine(Move());      
    }
}