using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : Singleton<BuildingManager>
{
    private ResourceManager resources;
    private InputManager input;

    [SerializeField]
    private List<Building> constructed;

    [SerializeField]
    private List<Building> buildings;

    [SerializeField]
    private GameObject buildPanel;

    [SerializeField]
    private ActionButton buttonPrefab;

    [SerializeField]
    private List<ActionButton> buttons;

    private void Start()
    {
        resources = ResourceManager.Instance;
        input = InputManager.Instance;
    }

    public Building FindNearestBuildingToMe(Villager villager)
    {
        if (constructed.Count == 0)
            return null;

        //  WRITE THIS SOMEHOW???

        return constructed[0];
    }

    public Building FindNearestBuildingToMe(Villager villager, Jobs canTrain)
    {
        if (constructed.Count == 0)
            return null;

        //  WRITE THIS SOMEHOW???

        foreach(Building built in constructed)
            if (built.skillsToTrainHere.Contains(canTrain))
                return built;

        return null;
    }

    public void OpenBuildMenu(bool open)
    {
        if(!open)
        {
            buildPanel.SetActive(false);
            return;
        }
        //  update which buttons are presented, based on which buildings are unlocked
        //  make buttons uninteractable if the player doesn't have enough resources
        //  make buttons display the correct information
        buildPanel.SetActive(true);
    }

    public void AddConstructedBuilding(Building built)
    {
        constructed.Add(built);
    }

    public void Build(Building toBuild)
    {
        if (resources.RequestResources(toBuild.cost))
        {
            input.SelectBuildingSite(toBuild);
            buildPanel.SetActive(false);
        }
        else
            print("Insufficient funds to build this building");
    }

    public int HowManyBuildings()
    {
        return constructed.Count;
    }

    public void DestroyBuilding(Building destroyed)
    {
        constructed.Remove(destroyed);
        Destroy(destroyed.gameObject);

        //  if there are no more units, game over
        if(constructed.Count == 0 && UnitManager.Instance.HowManyUnits() == 0)
            LevelManager.Instance.GameOver();
    }
}
