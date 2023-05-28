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

    public void RemoveUnit(Unit toRemove)
    {
        units.Remove(toRemove);
    }

    public int HowManyUnits()
    {
        return units.Count;
    }
}
