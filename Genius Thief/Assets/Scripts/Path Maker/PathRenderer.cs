using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(PointWay))]
[RequireComponent(typeof(LineRenderer))]
public class PathRenderer : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _player;
    [SerializeField] private GameObject _startPointPath;

    private LineRenderer _lineRenderer;
    private PointWay _pointWay;

    private float _initialLineWidth = 0.15f;
    private float _initialEndLineWidth = 0.30f;
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
            _lineRenderer.startColor = Color.green;
            _lineRenderer.endColor = Color.cyan;
        }          


        _lineRenderer.positionCount = _player.path.corners.Length;
        _lineRenderer.SetPosition(0, _startPointPath.gameObject.transform.position);

        if (_player.path.corners.Length < 2)
            return;

        for (int i = 1; i < _player.path.corners.Length; i++)
        {
            Vector3 pointPosition = new Vector3(_player.path.corners[i].x, _player.path.corners[i].y, _player.path.corners[i].z);

            _lineRenderer.SetPosition(i, pointPosition);
        }

        //ResetLineRenderer();
    }
}
