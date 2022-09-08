using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(PathHandler))]
public class PathRenderer : MonoBehaviour
{
    [SerializeField] private GridMovementAgent _pathCreator;
    [SerializeField] private GridHolder _gridHolder;
    [SerializeField] private ClickMarker _marker;
    [SerializeField] private float _heightStartPoint = 1;
    [SerializeField] private float _initialLineWidth = 0.30f;
    [SerializeField] private float _initialEndLineWidth = 0.30f;

    private PathHandler _pathHandler;
    private LineRenderer _lineRenderer;
    private Coroutine _currentCoroutine;
    private int _lastIndexInPastPath = 0;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _pathHandler = GetComponent<PathHandler>();
        _pathHandler.PointsAdded += DrawPath;

        ResetLineRenderer();
    }

    private void OnDisable()
    {
        _pathHandler.PointsAdded -= DrawPath;
    }

    private void ResetLineRenderer()
    {
        _lineRenderer.startWidth = _initialLineWidth;
        _lineRenderer.endWidth = _initialEndLineWidth;
        _lineRenderer.transform.position = _pathCreator.transform.position;
    }

    public void DrawPath()
    {     
        _lineRenderer.positionCount = _pathHandler.GetPositionCount();

        if (_currentCoroutine == null)
        {           
            _currentCoroutine = StartCoroutine(AnimateLine());
        }
        else
        {
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = StartCoroutine(AnimateLine());
        }
    }

    private IEnumerator AnimateLine()
    {
        float animationDuration = 2f;

        float segmentDuration = animationDuration / _lineRenderer.positionCount;

        for (int i = _lastIndexInPastPath; i < _lineRenderer.positionCount - 1; i++)
        {
            float startTime = Time.time;

            Vector3 startPosition = _pathHandler.GetPathPoint(i);
            Vector3 endPosition = _pathHandler.GetPathPoint(i + 1);

            Vector3 pos = startPosition;
            while (pos != endPosition)
            {
                float t = (Time.time - startTime) / segmentDuration;
                pos = Vector3.Lerp(startPosition, endPosition, t);

                for (int j = i + 1; j < _lineRenderer.positionCount; j++)
                    _lineRenderer.SetPosition(j, pos);

                yield return null;
            }

            _lastIndexInPastPath = i;
        }
    }
}