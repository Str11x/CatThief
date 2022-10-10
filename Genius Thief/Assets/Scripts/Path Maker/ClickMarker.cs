using TMPro;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class ClickMarker : MonoBehaviour 
{
    private const string LastMarkerName = "END";

    [SerializeField] private TextMeshProUGUI _stepRenderer;
    [SerializeField] private Material _exitMaterial;
    [SerializeField] private MeshRenderer _beam;

    private MeshRenderer _meshRenderer;
    private Vector3 _exitPosition;
    private float _exitDistance = 0.4f;
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
            _stepRenderer.text = LastMarkerName;           
        }           
    }

    private void OnTriggerEnter(Collider other)
    {
        int nextPoint = 1;
        Wallet playerWallet;

        if(int.TryParse(_stepRenderer.text, out int result))
        {
            if (other.TryGetComponent(out PlayerSuite player) && 
                ((playerWallet = player.GetComponent<Wallet>()).MarkersCount + nextPoint) == result)
            {
                playerWallet.AddMarkerCount();
                _meshRenderer.material = _exitMaterial;
                _beam.enabled = false;
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