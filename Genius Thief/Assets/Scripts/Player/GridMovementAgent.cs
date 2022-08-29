using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovementAgent : MonoBehaviour
{
    [SerializeField] private GridHolder _gridHolder;
    [SerializeField] private float _speed;

    [SerializeField] private FlowFieldPathfinding _pathfinding;

    private WaitForFixedUpdate _updateTime = new WaitForFixedUpdate();
    private Coroutine _movement;
    private Node _targetNode;
    private const float _tolerance = 0.4f;

    private void Start()
    {
        _gridHolder.SetPath += MoveToTarget;
    }

    private void OnDisable()
    {
        _gridHolder.SetPath -= MoveToTarget;
    }

    private void MoveToTarget(Node firstNode)
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

            float distance = (target - transform.position).magnitude;

            if (distance < _tolerance)
            {
                _targetNode = _targetNode.NextNode;

                yield return null;
            }

            Vector3 direction = (target - transform.position).normalized;
            Vector3 delta = direction * (_speed * Time.deltaTime);
            transform.Translate(delta);

            if (_gridHolder.GetTargetNode() == _targetNode)
                _targetNode.NextNode = null;

            yield return _updateTime;
        }
    }
}