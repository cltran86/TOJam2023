using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabSpawner : MonoBehaviour
{
    [SerializeField]
    private Crab crab;

    [SerializeField]
    private float spawnRate = 0.1f;

    private void Start()
    {
        StartCoroutine(Spawning());
    }

    private IEnumerator Spawning()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f / spawnRate);
            Instantiate(crab, transform);
        }
    }
}
