using System;
using System.Collections.Generic;
using UnityEngine;

public class RewardObjects : MonoBehaviour
{
    [SerializeField] private PathHandler _pathHandler;
    [SerializeField] private PathPointCatcher _pathPointCathcer;
    [SerializeField] private ParticleSystem _pickupEffect;

    private List<Loot> _lootedObjectsInPlan = new List<Loot>();
    private List<bool> _pickedupLoot = new List<bool>();

    public Vector3 LastLootPosition { get; private set; }

    public event Action LootPickedup;

    private void Start()
    {
        _pathHandler.MovedToPreviousState += RemoveLastLoot;
        _pathPointCathcer.LootAddedInLastPoint += AddNewLootBoolean;
    }

    private void OnDisable()
    {
        _pathHandler.MovedToPreviousState -= RemoveLastLoot;
        _pathPointCathcer.LootAddedInLastPoint -= AddNewLootBoolean;
    }

    private void AddNewLootBoolean(bool isLootInLastPoint)
    {
        _pickedupLoot.Add(isLootInLastPoint);
    }

    private void RemoveLastLoot()
    {
        if(_lootedObjectsInPlan != null)
        {
            int penultimate = _pickedupLoot.Count - 1;
            int lastLootInPlan = _lootedObjectsInPlan.Count - 1;

            if(_pickedupLoot[penultimate] == true)
            {
                _lootedObjectsInPlan[lastLootInPlan].DeleteFromSchedulePathPoints();
                _lootedObjectsInPlan.RemoveAt(lastLootInPlan);
                _pickedupLoot.RemoveAt(penultimate);
            }
            else
            {
                _pickedupLoot.RemoveAt(penultimate);
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
        LootPickedup?.Invoke();
    } 
}