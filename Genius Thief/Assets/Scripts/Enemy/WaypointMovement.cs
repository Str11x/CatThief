using System.Collections;
using UnityEngine;

public class WaypointMovement : MonoBehaviour
{
    [SerializeField] private Transform _path;

    private float _rotationSpeed = 6f;
    private Transform[] _points;
    private int _currentPoint;
    private string _stopMovement = "StopMovement";
    private int _delayStopMovement = 1;
    private Coroutine _movementCoroutine;

    public float Speed { get; private set; } = 1;

    private void Start()
    {
        Exit.LevelCompleted += DelayStopMovement;
        FieldOfViewCalculate.GameIsLost += StopMovement;

        _points = new Transform[_path.childCount];

        for (int i = 0; i < _path.childCount; i++)
        {
            _points[i] = _path.GetChild(i);
        }

        if(_movementCoroutine != null)
        {
            StopCoroutine(_movementCoroutine);
            _movementCoroutine = StartCoroutine(MoveToPathPoints());
        }
        else
        {
            _movementCoroutine = StartCoroutine(MoveToPathPoints());
        }
    }

    private void OnDestroy()
    {
        Exit.LevelCompleted -= DelayStopMovement;
        FieldOfViewCalculate.GameIsLost -= StopMovement;
    }

    private void DelayStopMovement()
    {
        Invoke(_stopMovement, _delayStopMovement);
    }

    private void StopMovement()
    {
        if(_movementCoroutine != null)
            StopCoroutine(_movementCoroutine);
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