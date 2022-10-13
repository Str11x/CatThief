using System.Collections;
using UnityEngine;

public class GridMovementAgent : MonoBehaviour
{
    private const float Tolerance = 1f;

    [SerializeField] private GridHolder _gridHolder;
    [SerializeField] private PathPointCatcher _pathPointCatcher;
    [SerializeField] private PathHandler _pathHandler;
    [SerializeField] private float _speed;

    private WaitForFixedUpdate _updateTime = new WaitForFixedUpdate();
    private Coroutine _movement;
    private Node _targetNode;
    
    public Vector3 StartPosition { get; private set; }

    private void Start()
    {
        StartPosition = transform.position;
        _pathHandler.MovedToPreviousState += MoveToPreviousPath;
    }

    private void OnDestroy()
    {
        _pathHandler.MovedToPreviousState -= MoveToPreviousPath;
    }

    private IEnumerator Move()
    {
        while (_targetNode != null)
        {
            Vector3 target = _targetNode.Position;

            _pathHandler.AddNewNodePoint(target);

            float distance = (target - transform.position).magnitude;

            if (distance < Tolerance)
            {
                _targetNode = _targetNode.NextNode;

                if (_pathPointCatcher.GetTargetNode() == _targetNode)
                    _targetNode.SetNextNode(null);

                continue;
            }

            Vector3 direction = (target - transform.position).normalized;
            Vector3 delta = direction * (_speed * Time.deltaTime);
            transform.Translate(delta);                        

            yield return _updateTime;
        }
        
        _pathHandler.SaveNewPointsState();
    }

    public void MoveToPreviousPath()
    {
        transform.position = _pathHandler.GetPreviousPathFinishPoint();
    }

    public void MoveToTarget(Node firstNode)
    {
        _targetNode = firstNode;

        if (_targetNode == null)
            return;           

        if (_movement != null)
            StopCoroutine(_movement);
        _movement = StartCoroutine(Move());
    }
}