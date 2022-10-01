using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GridHolder : MonoBehaviour
{
    [SerializeField] private int _gridWidth;
    [SerializeField] private int _gridHeight;
    [SerializeField] private float _nodeSize;

    private Grid _grid;
    private float _offsetNumber = 0.5f;
    private float _sizeCorrection = 0.1f;

    public float NodeSize => _nodeSize;
    public Vector3 Offset { get; private set; }
    public int PlaneHeight { get; private set; } = 1;


    private void Awake()
    {
        float width = _gridWidth * _nodeSize;
        float height = _gridHeight * _nodeSize;

        transform.localScale = new Vector3(width * _sizeCorrection, PlaneHeight, height * _sizeCorrection);

        Offset = transform.position - new Vector3(width, 0, height) * _offsetNumber;

        _grid = new Grid(_gridWidth, _gridHeight, Offset, _nodeSize);
    }

    private void OnValidate()
    {
        float width = _gridWidth * _nodeSize;
        float height = _gridHeight * _nodeSize;

        transform.localScale = new Vector3(width * _sizeCorrection, PlaneHeight, height * _sizeCorrection);

        Offset = transform.position - new Vector3(width, 0, height) * _offsetNumber;
    }

    public Grid GetGrid()
    {
        return _grid;
    } 
}