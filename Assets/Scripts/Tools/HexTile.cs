using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TerrainType
{
    Water,
    Grass,
    Stone,
    Iron,
    Gold
}

public class HexTile : MonoBehaviour
{
    [SerializeField]
    public Vector2Int gridCoordinates;

    [SerializeField]
    public Resource resource;

    [SerializeField]
    public Building builtHere;

    public List<Unit> unitsStandingHere;

    [SerializeField]
    public TerrainType terrainType;

    public Vector3 AddUnit(Unit toAdd)
    {
//        if(unitsStandingHere.Count >= 7)
  //      {
            //  This tile is full, send them to an adjacent tile?
    //    }
        unitsStandingHere.Add(toAdd);
        return RecalculateUnitPositions(toAdd);
    }

    public void RemoveUnit(Unit toRemove)
    {
        unitsStandingHere.Remove(toRemove);
        RecalculateUnitPositions();
    }

    private Vector3 RecalculateUnitPositions(Unit beingAdded = null)
    {
        if (unitsStandingHere.Count == 0)
            return Vector3.zero;

        if (unitsStandingHere.Count == 1)
        {
//            unitsStandingHere[0].MoveToPosition(transform.position);
            return transform.position;
        }
        return transform.position;
    }
}
