using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Loot : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private RewardObjects _rewardObjectsService;
    private ParticleSystem _rewardDefaultEffect;

    private int _pickedUpItems;

    public bool IsLooted { get; private set; }

    private void Start()
    {
        _rewardDefaultEffect = GetComponentInChildren<ParticleSystem>();
        _rewardObjectsService = GetComponentInParent<RewardObjects>();

        _pickedUpItems = LayerMask.NameToLayer("Picked up items");

        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Wallet playerWallet))
        {
            playerWallet.AddPoints();
            _meshRenderer.enabled = false;
            gameObject.layer = _pickedUpItems;

            _rewardDefaultEffect.gameObject.SetActive(false);
            _rewardObjectsService.DoPickupEffect(transform.position);
        }
    }

    public void ScheduleInPathPoints()
    {
        IsLooted = true;
        _rewardObjectsService.AddNewLoot(this);
    }

    public void DeleteFromSchedulePathPoints()
    {
        IsLooted = false;
    }
}