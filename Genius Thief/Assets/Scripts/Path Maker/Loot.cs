using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Loot : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private RewardObjects _rewardObjectsService;

    private int _pickedUpItems;

    public bool IsLooted { get; private set; }

    private void Start()
    {
        _rewardObjectsService = GetComponentInParent<RewardObjects>();

        _pickedUpItems = LayerMask.NameToLayer("Picked up items");

        _meshRenderer = GetComponent<MeshRenderer>();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.AddCount();
            _meshRenderer.enabled = false;
            gameObject.layer = _pickedUpItems;
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