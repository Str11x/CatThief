using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovementAgent : MonoBehaviour
{
    [SerializeField] private GridHolder _gridHolder;
    [SerializeField] private PathPointCatcher _pathPointCatcher;
    [SerializeField] private float _speed;
    [SerializeField] private PathHandler _pathHandler;

    private WaitForFixedUpdate _updateTime = new WaitForFixedUpdate();
    private Coroutine _movement;
    private Node _targetNode;
    private const float _tolerance = 0.4f;
    
    public Vector3 StartPosition { get; private set; }

    private void Start()
    {
        StartPosition = transform.position;
        _pathHandler.MovedToPreviousState += MoveToPreviousPathPoint;
    }

    private void OnDestroy()
    {
        _pathHandler.MovedToPreviousState -= MoveToPreviousPathPoint;
    }

    public void MoveToPreviousPathPoint()
    {
        transform.position = _pathHandler.GetPreviousPathFinishPoint();
    }

    public void MoveToTarget(Node firstNode)
    {
        _targetNode = firstNode;

        if (_targetNode == null)
            return;

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
        while (_targetNode != null)
        {
            Vector3 target = _targetNode.Position;

            _pathHandler.AddNewNodePoint(target);

            float distance = (target - transform.position).magnitude;

            if (distance < _tolerance)
            {
                _targetNode = _targetNode.NextNode;

                yield return null;
            }

            Vector3 direction = (target - transform.position ).normalized;
            Vector3 delta = direction * (_speed * Time.deltaTime);
            transform.Translate(delta);

            if (_pathPointCatcher.GetTargetNode() == _targetNode)
                _targetNode.SetNextNode(null);                

            yield return _updateTime;
        }

        _pathHandler.CreateFinishMarker();        
        _pathHandler.SaveNewPointsState();        
    }
}