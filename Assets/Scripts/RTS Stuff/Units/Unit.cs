using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Selectable
{
    [SerializeField]
    protected float moveSpeed = 1;

    [SerializeField]
    protected int   health      = 100,
                    maxHealth   = 100,
                    attack      = 10,
                    defense     = 10;

    [SerializeField]
    protected float buildSpeed      = 1,
                    influenceSpeed  = 1,
                    influenceRange  = 1,
                    attackSpeed     = 1,
                    attackRange     = 1;

    protected Vector3   lastPosition,
                        targetPosition;

    protected float distanceMoved;
//    protected bool moving;

    private IEnumerator moving;

    protected override void Awake()
    {
        animator = GetComponent<Animator>();
        moving = Moving();
    }

    public IEnumerator NavigateTo(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
        StopCoroutine(moving);
        yield return StartCoroutine(moving);
    }
    public IEnumerator NavigateTo(Selectable target)
    {
        targetPosition = target.transform.position;
        StopCoroutine(moving);
        yield return StartCoroutine(moving);
    }

    protected IEnumerator Moving()
    {
        animator.SetBool("Swimming", true);
        do
        {
            //            lastPosition = transform.position;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * moveSpeed);
            //            distanceMoved = Vector3.Distance(lastPosition, transform.position);
            yield return null;
        }
        while (transform.position != targetPosition);// (distanceMoved > 0.001f);

        animator.SetBool("Swimming", false);
    }

    //  Not sure if I still need these

/*    public override Action[] GetActions()
    {
        return actions;
    }
    public override bool QueueAction(Action toQueue)
    {
        toQueue.whatHappens.Invoke();
        return true;
    }*/
}
