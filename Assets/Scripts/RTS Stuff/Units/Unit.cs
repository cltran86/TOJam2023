using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Selectable
{
    [SerializeField]
    protected float moveSpeed = 1;

    [SerializeField]
    protected int   attack      = 10,
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
        moving = Moving();
        yield return StartCoroutine(moving);
    }
    public IEnumerator NavigateTo(Selectable target)
    {
        this.targetPosition = target.transform.position;
        StopCoroutine(moving);
        moving = Moving();
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
}
