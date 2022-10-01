using System.Collections;
using UnityEngine;

public class WaypointMovement : MonoBehaviour
{
    private const string StopMovement = "StopMovement";

    [SerializeField] private Transform _path;

    private Coroutine _movement;
    private Transform[] _points;
    private float _rotationSpeed = 6f;
    private int _currentPoint;
    private int _delayStopMovement = 1;

    public float Speed { get; private set; } = 1;

    private void Start()
    {
        Exit.LevelCompleted += DelayStopMovement;
        FieldOfViewCalculate.GameIsLost += StopMovementCoroutine;

        _points = new Transform[_path.childCount];

        for (int i = 0; i < _path.childCount; i++)
        {
            _points[i] = _path.GetChild(i);
        }

        if(_movement != null)
        {
            StopCoroutine(_movement);
            _movement = StartCoroutine(MoveToPathPoints());
        }
        else
        {
            _movement = StartCoroutine(MoveToPathPoints());
        }
    }

    private void OnDestroy()
    {
        Exit.LevelCompleted -= DelayStopMovement;
        FieldOfViewCalculate.GameIsLost -= StopMovementCoroutine;
    }

    private void DelayStopMovement()
    {
        Invoke(StopMovement, _delayStopMovement);
    }

    private void StopMovementCoroutine()
    {
        if(_movement != null)
            StopCoroutine(_movement);
    }

    private IEnumerator MoveToPathPoints()
    {
        while (true)
        {
            Transform target = _points[_currentPoint];

            transform.position = Vector3.MoveTowards(transform.position, target.position, Speed * Time.deltaTime);

            Vector3 direction = target.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, _rotationSpeed * Time.deltaTime);

            if (transform.position == target.position)
            {
                _currentPoint++;

                if (_currentPoint >= _points.Length)
                {
                    _currentPoint = 0;
                }
            }

            yield return null;
        }    
    }
}