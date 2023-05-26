using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum Skills
{
    build,
    heal,
    art,
    fight,
    archery,
    shield
}

public class Villager : Unit
{
    [Header("[0] Build\t[1] Heal\t\t[2] Art\n[3] Fight\t[4] Archery\t[5] Shield")]
    [SerializeField]
    private bool[] skills;

    public void LearnSkill(int toLearn)
    {
        //  Find the nearest building that teaches this skill
        //  navigate to the building
        //  train...

        skills[toLearn] = true;

        //  need to instantiate a new villager with the appropriate model and animations?
    }

    public void GatherResources(Resource toGather)
    {
        
    }

    public void OpenBuildMenu()
    {
        BuildingManager.Instance.OpenBuildMenu();
    }

    public void Build(Building toBuild)
    {
        if (!skills[(int)Skills.build])
            return;
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

    public void Fight(Unit enemy)
    {
        if (skills[(int)Skills.archery])
        {

        }
        else if (skills[(int)Skills.shield])
        {

        }
        else if (skills[(int)Skills.fight])
        {

        }
    }
}
