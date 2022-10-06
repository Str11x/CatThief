using System;
using System.Collections.Generic;
using UnityEngine;

public class RewardObjects : MonoBehaviour
{
    [SerializeField] private PathHandler _pathHandler;
    [SerializeField] private PathPointCatcher _pathPointCathcer;
    [SerializeField] private ParticleSystem _pickupEffect;

    private List<Loot> _lootedObjectsInPlan = new List<Loot>();
    private List<bool> _isLoot = new List<bool>();

    public Vector3 LastLootPosition { get; private set; }

    public event Action PickedupLoot;

    private void Start()
    {
        _pathHandler.MovedToPreviousState += RemoveLastLoot;
        _pathPointCathcer.LootWasLastPoint += AddNewLootBoolean;
    }

    private void OnDisable()
    {
        _pathHandler.MovedToPreviousState -= RemoveLastLoot;
        _pathPointCathcer.LootWasLastPoint -= AddNewLootBoolean;
    }

    private void AddNewLootBoolean(bool isLootInLastPoint)
    {
        _isLoot.Add(isLootInLastPoint);
    }

    private void RemoveLastLoot()
    {
        if(_lootedObjectsInPlan != null)
        {
            int penultimate = 1;
            int lastLoot = _lootedObjectsInPlan.Count - penultimate;

            if(_isLoot[_isLoot.Count - penultimate] == true)
            {
                _lootedObjectsInPlan[lastLoot].DeleteFromSchedulePathPoints();
                _lootedObjectsInPlan.RemoveAt(lastLoot);
                _isLoot.RemoveAt(_isLoot.Count - penultimate);
            }
            else
            {
                _isLoot.RemoveAt(_isLoot.Count - penultimate);
            }       
        }     
    }

    public void AddNewLoot(Loot newLoot)
    {
        _lootedObjectsInPlan.Add(newLoot);
    }

    public void DoPickupEffect(Vector3 position)
    {
        LastLootPosition = position;
        _pickupEffect.transform.position = position;
        _pickupEffect.Play();
        PickedupLoot?.Invoke();
    } 
}