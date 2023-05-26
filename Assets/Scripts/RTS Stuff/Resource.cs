using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : Selectable
{
    public Resources resourceType;

    [SerializeField]
    private uint    amount = 1000,
                    limit = 2000,
                    regeneration = 60;

    [SerializeField]
    private bool destroyResourceAtZero = true;

    public HexTile location;

    private void Awake()
    {
        if (regeneration > 0)
            StartCoroutine(Regenerate());
    }
    private IEnumerator Regenerate()
    {
        while(true)
        {
            yield return new WaitForSeconds(regeneration);

            if(amount < limit)
                ++amount;
        }
    }
    public int Extract()
    {
        if (amount == 0)
            return 0;

        --amount;

        if(amount == 0 && destroyResourceAtZero)
            Destroy(gameObject, 0.1f);

        return 1;
    }
    public override Action[] GetActions()
    {
        return new Action[0];
    }
}
