using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum Skills
{
    gather,
    build,
    heal,
    art,
    fight,
    archery,
    shield
}

public enum Jobs
{
    Villager,
    Builder,
    Artist,
    Healer,
    Fighter,
    Archer,
    Shield
}

public class Villager : Unit
{
    [SerializeField]
    private Jobs mainJob, subJob;

    [SerializeField]
    private GameObject[] jobEquipment;

    [SerializeField]
    protected float gatherSpeed = 1,
                    learnSpeed = 1;

    [SerializeField]
    protected int   gathered,
                    capacity = 10;

    [Header("[0] Gather\n[1] Build\t[2] Heal\t\t[3] Art\n[4] Fight\t[5] Archery\t[6] Shield")]
    [SerializeField]
    private bool[] skills;

    private IEnumerator currentOrders;

    private Crab target;

    private bool    canAttack = true,
                    targetIsInRange;

    private float timeSinceLastAttack;

    public IEnumerator GatherResources(Resource toGather)
    {
        if(currentOrders != null)
            StopCoroutine(currentOrders);
        currentOrders = GatherResources(toGather);

        while (true)
        {
            yield return StartCoroutine(NavigateTo(toGather));

            animator.SetBool("Gathering", true);

            while (toGather.HasMore() && gathered < capacity)
            {
                yield return new WaitForSeconds(1 / gatherSpeed);
                gathered += toGather.Gather();
            }
            animator.SetBool("Gathering", false);

            Building toDeliverTo = BuildingManager.Instance.FindNearestBuildingToMe(this);

            yield return StartCoroutine(NavigateTo(toDeliverTo));

            ResourceManager.Instance.AddResource(0, gathered);
            gathered = 0;
        }
    }

    public List<Jobs> TrainingOptions()
    {
        List<Jobs> options = new List<Jobs>();

        if(mainJob == Jobs.Villager)
        {
            options.Add(Jobs.Builder);
            options.Add(Jobs.Fighter);
        }
        else if(mainJob == Jobs.Builder)
        {
            options.Add(Jobs.Artist);
            options.Add(Jobs.Healer);
        }
        else if(mainJob == Jobs.Fighter)
        {
            options.Add(Jobs.Archer);
            options.Add(Jobs.Shield);
        }

        //  Add more for subjob stuff later

        return options;
    }

    public IEnumerator TrainNewJob(Jobs toTrain)
    {
        if (currentOrders != null)
            StopCoroutine(currentOrders);
        currentOrders = TrainNewJob(toTrain);

        Building facility = BuildingManager.Instance.FindNearestBuildingToMe(this, toTrain);

        if(facility == null)
        {
            print("There are no buildings which can train this job.");
            yield break;
        }
        yield return StartCoroutine(NavigateTo(facility));

        int learnedAmount = 0,
            quota = KnowledgeManager.Instance.learnQuotas[ (int) toTrain ];

        do
        {
            if(selected)
                details.UpdateSecondaryGauge(learnedAmount, quota);

            yield return new WaitForSeconds(1f / learnSpeed);
        }
        while (++learnedAmount != quota);

        ChangeMainJob(toTrain);

        if(selected)
            details.ShowDetails(this);

        yield return StartCoroutine(NavigateTo(facility.transform.position + Vector3.back * 10));
    }

    public void ChangeMainJob(Jobs toChangeTo)
    {
        for (int i = 0; i != jobEquipment.Length; ++i)
            jobEquipment[i].SetActive(i == (int) toChangeTo);

        skills[(int)toChangeTo] = true;
        gameObject.name = toChangeTo.ToString();
        mainJob = toChangeTo;
    }

    //  BUILDER FUNCTIONS

    public bool CanBuild()
    {
        return skills[1];
    }

    public void OpenBuildMenu()
    {
        BuildingManager.Instance.OpenBuildMenu(true);
    }

    public IEnumerator Build(Building toBuild)
    {
        if (!skills[(int)Skills.build])
            yield break;

        if (currentOrders != null)
            StopCoroutine(currentOrders);
        currentOrders = Build(toBuild);

        yield return StartCoroutine(NavigateTo(toBuild));

        animator.SetBool("Building", true);
        do
        {
            yield return new WaitForSeconds(1f / buildSpeed);
        }
        while (toBuild.Construct());

        animator.SetBool("Building", false);

        //  Move outside of the building???
        StartCoroutine(NavigateTo(toBuild.transform.position + Vector3.back * 10));
    }

    public void Heal(Unit patient)
    {
        if (!skills[(int)Skills.heal])
            return;
    }

    public void DoArt()
    {
        if (!skills[(int)Skills.art])
            return;
    }

    public void Fight(Crab enemy)
    {
        /*if (skills[(int)Skills.archery])
        {

        }
        else if (skills[(int)Skills.shield])
        {

        }
        else */if (skills[(int)Skills.fight])
        {
            print("Commanding villager to attack");
            StartCoroutine(Fighting(enemy));
        }
    }

    private IEnumerator Fighting(Crab enemy)
    {
        if (currentOrders != null)
            StopCoroutine(currentOrders);
        currentOrders = Fighting(enemy);

        target = enemy;
        do
        {
            if(!targetIsInRange)
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed);

            else if(canAttack && targetIsInRange)
            {
                target.TakeDamage(attack, this);
                canAttack = false;
            }
            timeSinceLastAttack += Time.deltaTime;
            if (timeSinceLastAttack > 1f / attackSpeed)
                canAttack = true;

            yield return null;
        }
        while (target != null) ;
    }

    public override bool TakeDamage(int damage, Crab whoAttackedMe)
    {
        if(target == null)
            target = whoAttackedMe;

        return base.TakeDamage(damage, whoAttackedMe);
    }

    protected override void Die()
    {
        UnitManager.Instance.DestroyUnit(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        Crab enemy = other.GetComponent<Crab>();

        if(target == null && enemy != null)
        {
            target = enemy;
            targetIsInRange = true;
        }
        else if (target != null && enemy == target)
            targetIsInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        Crab enemy = other.GetComponent<Crab>();

        if (enemy == target)
            targetIsInRange = false;
    }
}
