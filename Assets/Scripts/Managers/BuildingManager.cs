using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : Singleton<BuildingManager>
{
    private ResourceManager resources;
    private InputManager input;

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
    public void OpenBuildMenu()
    {
        //  update which buttons are presented, based on which buildings are unlocked
        //  make buttons uninteractable if the player doesn't have enough resources
        //  make buttons display the correct information
        buildPanel.SetActive(true);
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
}
