using System.Collections.Generic;
using UnityEngine;

public class RewardObjects : MonoBehaviour
{
    [SerializeField] private PathHandler _pathHandler;
    [SerializeField] private PathPointCatcher _pathPointCathcer;

    private List<Loot> _lootedObjectsInPlan = new List<Loot>();
    private List<bool> _isLastPointInLoot = new List<bool>();

    private void Start()
    {
        _pathHandler.MovedToPreviousState += RemoveLastLoot;
        _pathPointCathcer.LootWasLastPoint += AddNewLootBoolPoint;
    }

    private void OnDisable()
    {
        _pathHandler.MovedToPreviousState -= RemoveLastLoot;
        _pathPointCathcer.LootWasLastPoint -= AddNewLootBoolPoint;
    }

    public void AddNewLoot(Loot newLoot)
    {
        _lootedObjectsInPlan.Add(newLoot);
    }

    private void AddNewLootBoolPoint(bool isLootInLastPoint)
    {
        _isLastPointInLoot.Add(isLootInLastPoint);
    }

    public void RemoveLastLoot()
    {
        if(_lootedObjectsInPlan != null)
        {
            int penultimate = 1;
            int lastLoot = _lootedObjectsInPlan.Count - penultimate;

            if(_isLastPointInLoot[lastLoot] == true)
            {
                _lootedObjectsInPlan[lastLoot].DeleteFromSchedulePathPoints();
                _lootedObjectsInPlan.RemoveAt(lastLoot);
                _isLastPointInLoot.RemoveAt(lastLoot);
            }
            else
            {
                _isLastPointInLoot.RemoveAt(lastLoot);
            }       
        }     
    }
}