using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabDetectionRange : MonoBehaviour
{
    [SerializeField]
    private Crab crab;

    [SerializeField]
    private SphereCollider detectionRange;

    private void OnTriggerEnter(Collider other)
    {
        Villager villager = other.GetComponent<Villager>();

        if (villager)
        {
            crab.SetTarget(villager.GetComponent<Selectable>());
            detectionRange.radius = 0;
            return;
        }
        Building building = other.GetComponent<Building>();

        if (building)
        {
            crab.SetTarget(building.GetComponent<Selectable>());
            detectionRange.radius = 0;
            return;
        }
    }
}
