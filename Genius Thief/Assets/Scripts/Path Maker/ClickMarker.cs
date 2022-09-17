using TMPro;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class ClickMarker : MonoBehaviour 
{
    [SerializeField] private TextMeshProUGUI _stepRenderer;
    [SerializeField] private Material _exitMaterial;

    private MeshRenderer _meshRenderer;
    private Vector3 _exitPosition;
    private float _exitDistance = 0.4f;
    private string _lastMarkerName = "END";
    private Vector3 _endButtonScale = new Vector3(1.8f, 0.02f, 1.8f);

    public bool IsFinishMarker { get; private set; }

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();

        if ((_exitPosition - transform.position).sqrMagnitude < _exitDistance)
        {
            IsFinishMarker = true;
            _meshRenderer.material = _exitMaterial;
            transform.localScale = _endButtonScale;
            _stepRenderer.text = _lastMarkerName;           
        }           
    }

    private void OnTriggerEnter(Collider other)
    {
        int nextPoint = 1;

        if(int.TryParse(_stepRenderer.text, out int result))
        {
            if (other.TryGetComponent(out Player player) && (player.PointsCount + nextPoint) == result)
            {
                player.AddCount();
                _meshRenderer.material = _exitMaterial;
            }
        }             
    }
    public void AddStep(int stepNumber)
    {
        _stepRenderer.text = stepNumber.ToString();
    }

    public void SpecifyExit(Vector3 position)
    {
        _exitPosition = position;
    }
}