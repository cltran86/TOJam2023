using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : Selectable
{
    [SerializeField]
    private GameObject  abundant,
                        depleted;

    [SerializeField]
    private uint amount = 1000;//,
//                    limit = 2000,
  //                  regeneration = 60;

    protected override void Awake()
    {
        base.Awake();
        Deplete(amount == 0);
    }

    private void Deplete(bool isDepleted)
    {
        abundant.SetActive(!isDepleted);
        depleted.SetActive(isDepleted);
    }

    public bool HasMore()
    {
        return amount > 0;
    }
    public int Gather()
    {
        if (amount == 0)
            return 0;

        --amount;
        return 1;
    }

    //    [SerializeField]
    //  private bool destroyResourceAtZero = true;

    /*    protected override void Awake()
        {
            base.Awake();

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

    //        if(amount == 0 && destroyResourceAtZero)
      //          Destroy(gameObject, 0.1f);

            return 1;
        }*/
}
