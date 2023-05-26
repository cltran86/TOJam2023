using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : Selectable
{
    public HexTile site;

    [SerializeField]
    protected int   productivity,
                    productivityCostToBuild,
                    productivitySpent;

    [SerializeField]
    protected bool holdsResources;

    [SerializeField]
    protected uint actionQueueLimit;
    protected List<Action> actionQueue;
    protected Action currentAction;
    protected bool busy;

    private bool interruptCurrentAction;

    [SerializeField]
    private GameObject rallyPoint;

    [SerializeField]
    public List<TerrainType> canBeBuiltOn;

    [SerializeField]
    public int[] cost;

    private void Awake()
    {
        actionQueue = new List<Action>();
    }

    public override void Select(bool select)
    {
        base.Select(select);
        rallyPoint.SetActive(select);
    }

    public bool Construct()
    {
        if (++productivitySpent == productivityCostToBuild)
            return true;
        else
            return false;
    }

    public override Action[] GetActions()
    {
        return actions;
    }
    public List<Action> GetQueuedActions()
    {
        return actionQueue;
    }
    private IEnumerator PerformQueuedActions()
    {
        busy = true;
        do
        {
            currentAction = actionQueue[0];
            do
            {
                yield return new WaitForSeconds(productivity);

                if (interruptCurrentAction)
                    break;

                if(selected)
                    details.UpdateSecondaryGauge(++currentAction.productivitySpent, currentAction.productivityCost);
            }
            while (currentAction.productivitySpent < currentAction.productivityCost);

            if (interruptCurrentAction)
            {
                interruptCurrentAction = false;
                details.HideSecondaryGauge();
                continue;
            }

            //  current action is now ready to be performed!
            actionQueue[0].whatHappens.Invoke();

            //  remove it from the action queue and update the details panel
            actionQueue.RemoveAt(0);

            if(selected)
                details.UpdateActionQueue();
        }
        while (actionQueue.Count != 0);
        busy = false;
    }

    public override bool QueueAction(Action toQueue)
    {
        if (actionQueue.Count >= actionQueueLimit)
            return false;

        if (!resources.RequestResources(toQueue.cost))
        {
            //  insufficient funds
            return false;
        }
        actionQueue.Add(toQueue);

        //  if not busy, trigger action
        if (!busy)
            StartCoroutine(PerformQueuedActions());

        if(selected)
            details.UpdateActionQueue();

        return true;
    }

    public void CancelQueuedAction(int index)
    {
        if(index == 0)
            interruptCurrentAction = true;

        resources.Refund(actionQueue[index].cost);
        actionQueue.RemoveAt(index);
    }

    public void TrainUnit(Unit toTrain)
    {
        Unit trainee = Instantiate(toTrain, transform.position, Quaternion.identity, UnitManager.Instance.transform);

        //  issue a default command to the new trainee to move to a rally point
        trainee.MoveToPosition(rallyPoint.transform.position);
    }
}
