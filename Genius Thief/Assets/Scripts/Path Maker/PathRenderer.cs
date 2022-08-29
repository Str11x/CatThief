using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(PointWay))]
[RequireComponent(typeof(LineRenderer))]
public class PathRenderer : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _player;
    [SerializeField] private float _heightStartPoint = 1;
    [SerializeField] private float _initialLineWidth = 0.15f;
    [SerializeField] private float _initialEndLineWidth = 0.30f;

    private LineRenderer _lineRenderer;
    private PointWay _pointWay;

    private int _initialPositionCount = 0;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        ResetLineRenderer();

        _pointWay = GetComponent<PointWay>();
        _pointWay.DirectionCreated += DrawPath;
    }

    private void OnDisable()
    {
        _pointWay.DirectionCreated -= DrawPath;
    }

    private void ResetLineRenderer()
    {
        _lineRenderer.startWidth = _initialLineWidth;
        _lineRenderer.endWidth = _initialEndLineWidth;
        _lineRenderer.positionCount = _initialPositionCount;
    }
    
    public void DrawPath()
    {
        if (_pointWay.IsLiftedObject)
        {
         
        }

        _lineRenderer.positionCount = _player.path.corners.Length;
        _lineRenderer.SetPosition(0, new Vector3(_player.transform.position.x, 1, _player.transform.position.z));

        if (_player.path.corners.Length < 2)
            return;

        StartCoroutine(AnimateLine());
    }

    private IEnumerator AnimateLine()
    {
        float segmentDuration = 1f / _lineRenderer.positionCount;

        for (int i = 0; i < _lineRenderer.positionCount - 1; i++)
        {
            float startTime = Time.time;

            Vector3 startPosition = _player.path.corners[i];
            Vector3 endPosition = _player.path.corners[i + 1];

            Vector3 position = startPosition;
            while (position != endPosition)
            {
                float t = (Time.time - startTime) / segmentDuration;
                position = Vector3.Lerp(startPosition, endPosition, t);

                for (int nextPoint = i + 1; nextPoint < _lineRenderer.positionCount; nextPoint++)
                    _lineRenderer.SetPosition(nextPoint, position);

                yield return null;
            }
        }
    }
}
