using System;
using System.Collections;
using UnityEngine;

public class FieldOfViewCalculate : MonoBehaviour
{
    [SerializeField] [Range(0, 360)] private float _angle;
    [SerializeField] private float _radius;
    [SerializeField] private Player _player;
    [SerializeField] private LayerMask _targetMask;
    [SerializeField] private LayerMask _obstructionMask;

    public bool CanSeePlayer { get; private set; }

    public LayerMask ObstructionMask => _obstructionMask;
    public float Radius => _radius;
    public float Angle => _angle;
    public Player Player => _player;

    public static event Action GameIsLost;

    private void Start()
    {
        StartCoroutine(SearchPlayer());
    }

    private IEnumerator SearchPlayer()
    {
        while (CanSeePlayer == false)
        {
            yield return new WaitForFixedUpdate();
            TryGetPlayerInView();
            if (CanSeePlayer)
                GameIsLost?.Invoke();
        }
    }

    private void TryGetPlayerInView()
    {
        float halfAngle = 0.5f;

        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, _radius, _targetMask);

        if(rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) <= _angle * halfAngle)
            {
                float distanceToTarget = Vector3.Distance(transform.position,target.position);

                if (Physics.Raycast(transform.position, directionToTarget, distanceToTarget, _obstructionMask) == false)
                    CanSeePlayer = true;
                else
                    CanSeePlayer = false;
            }
            else
            {
                CanSeePlayer = false;
            }
        }
        else if (CanSeePlayer)
        {
            CanSeePlayer = false;
        }
    }

    public Vector3 DirectionFromAngle(float angleInDegrees)
    {
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}