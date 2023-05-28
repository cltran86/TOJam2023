using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : Singleton<UnitManager>
{
    [SerializeField]
    private List<Unit> units;

    public void AddUnit(Unit toAdd)
    {
        units.Add(toAdd);
    }

    public void DestroyUnit(Unit toRemove)
    {
        units.Remove(toRemove);

        Destroy(toRemove.gameObject);

        if (units.Count == 0 && BuildingManager.Instance.HowManyBuildings() == 0)
            LevelManager.Instance.GameOver();
    }

    public int HowManyUnits()
    {
        return units.Count;
    }
}
