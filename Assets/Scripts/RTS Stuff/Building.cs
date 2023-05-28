using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : Selectable
{
    [SerializeField]
    private int buildQuota;
    private int buildProgress;

    [SerializeField]
    public int cost;

    private bool isPreview;
    private List<GameObject> obstacles;

    [SerializeField]
    private GameObject  underConstruction,
                        built,
                        decorated,
                        damaged;

    [SerializeField]
    public List<Jobs> skillsToTrainHere;

    public void Initialize()
    {
        buildProgress = 0;
        underConstruction.SetActive(true);
        built.SetActive(false);
        damaged.SetActive(false);
        BuildingManager.Instance.AddConstructedBuilding(this);
    }

    public override void Select(bool select)
    {
        base.Select(select);

        if(selected)
            details.UpdateSecondaryGauge(buildProgress, buildQuota);
    }

    public void SetAsPreview()
    {
        isPreview = true;
        obstacles = new List<GameObject>();
    }
    
    public bool Construct()
    {
        ++buildProgress;

        if (selected)
            details.UpdateSecondaryGauge(buildProgress, buildQuota);

        if (buildProgress >= buildQuota)
        {
            ConstructionComplete();
            return false;
        }
        else
            return true;
    }

    private void ConstructionComplete()
    {
        //  change model
        underConstruction.SetActive(false);
        built.SetActive(true);
        damaged.SetActive(false);

        //  poof
        //  etc
    }

    public bool Repair()
    {
        if(++health >= maxHealth)
        {
            damaged.SetActive(false);
            health = maxHealth;
            return false;
        }
        return true;
    }

    public override bool TakeDamage(int damage, Crab whoAttackedMe)
    {
        if(health < maxHealth)
            damaged.SetActive(true);

        return base.TakeDamage(damage, whoAttackedMe);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!isPreview)
            return;

        if (collision.GetComponent<Building>() || collision.GetComponent<Resource>())
            obstacles.Add(collision.gameObject);

        input.ValidatePreview(obstacles.Count == 0);
    }

    private void OnTriggerExit(Collider collision)
    {
        if (!isPreview)
            return;

        //        if (obstacles.Contains(collision.gameObject))
        obstacles.Remove(collision.gameObject);

        input.ValidatePreview(obstacles.Count == 0);
    }

    /*    public override Action[] GetActions()
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
        }*/
}
