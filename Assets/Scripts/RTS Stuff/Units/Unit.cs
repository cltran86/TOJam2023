using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Selectable
{
//    public HexTile standingOn;

    protected Animator animator;

    public bool unlocked;
//    public int[] costToTrain;

    [SerializeField]
    protected float moveSpeed = 1;

    protected Vector3 lastPosition,
                    targetPosition;
    protected float distanceMoved;
    protected bool moving;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void MoveToPosition(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;

        if (!moving)
            StartCoroutine(Moving());
    }

    public IEnumerator MoveToTile(HexTile toMoveTo)
    {
//        StopAllCoroutines();
        targetPosition = toMoveTo.transform.position;
        transform.LookAt(toMoveTo.transform, Vector3.up);
        yield return StartCoroutine(Moving());
    }

    protected IEnumerator Moving()
    {
        animator.SetBool("Walking", true);
        do
        {
            lastPosition = transform.position;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * moveSpeed);
            distanceMoved = Vector3.Distance(lastPosition, transform.position);
            yield return null;
        }
        while (distanceMoved > 0.001f);
        animator.SetBool("Walking", false);
    }
    public override Action[] GetActions()
    {
        return actions;
    }
    public override bool QueueAction(Action toQueue)
    {
        toQueue.whatHappens.Invoke();
        return true;
    }
}
