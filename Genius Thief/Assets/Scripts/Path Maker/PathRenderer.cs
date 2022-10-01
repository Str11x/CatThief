using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathHandler))]
public class PathRenderer : MonoBehaviour
{
    [SerializeField] private GridMovementAgent _pathCreator;
    [SerializeField] private ClickMarker _marker;
    [SerializeField] private Line _line;
    [SerializeField] private Exit _exit;
  
    private PathHandler _pathHandler; 
    private List<Line> _lines = new List<Line>();
    private List<ClickMarker> _markers = new List<ClickMarker>();
    private float _height = 0.1f;
    private int _lastIndexInPastPath = 0;

    private void Start()
    {
        _pathHandler = GetComponent<PathHandler>();
        _pathHandler.PointsAdded += RealTimeDrawPath;
        _pathHandler.PathCreated += CreateMarker;
        _pathHandler.MovedToPreviousState += RedrawPath;
        _pathHandler.StartedMove += RemovePoints;
    }

    private void OnDisable()
    {
        _pathHandler.PointsAdded -= RealTimeDrawPath;
        _pathHandler.PathCreated -= CreateMarker;
        _pathHandler.MovedToPreviousState -= RedrawPath;
        _pathHandler.StartedMove -= RemovePoints;
    }

    private void RemovePoints()
    {
        for (int i = 0; i < _lines.Count; i++)
        {
            Destroy(_lines[i].gameObject);
        }

        _lines.Clear(); ;
    }

    private void CreateMarker(Vector3 markerPosition)
    {
        ClickMarker newMarker = Instantiate(_marker, markerPosition + Vector3.up * _height, Quaternion.identity);
        newMarker.SpecifyExit(_pathHandler.GetExitPosition());
        newMarker.AddStep(_markers.Count + 1);

        _markers.Add(newMarker);
    }

    public void RealTimeDrawPath()
    {
        if (_pathHandler.IsPlayerMove() == true)
            return;

        Vector3 newPointPosition = _pathHandler.GetPathPoint(_lastIndexInPastPath) + Vector3.up * _height;

        Line lastLine = Instantiate(_line, newPointPosition, Quaternion.identity);

        _lines.Add(lastLine);
        _lastIndexInPastPath++;
    }

    public void RedrawPath()
    {
        RemovePoints();

        _lastIndexInPastPath = 0;

        Destroy(_markers[_markers.Count - 1].gameObject);
        _markers.RemoveAt(_markers.Count - 1);

        if (_pathCreator.transform.position == _pathCreator.StartPosition)
            return;

        for (int i = 0; i < _pathHandler.GetRedrawPoints(); i++)
        {
                Line lastLine = Instantiate(_line, _pathHandler.GetPointFromPreviousState(i) + Vector3.up * _height, Quaternion.identity);
                _lines.Add(lastLine);
                _lastIndexInPastPath++;
        }
    }  
}