using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public struct Action
{
    public Sprite icon;
    public string label;
    public int[] cost;
    public int productivityCost,
                productivitySpent;
    public UnityEvent whatHappens;
}

public class ActionButton : MonoBehaviour
{
    [SerializeField]
    private Image icon;

    [SerializeField]
    private Text label;

    [SerializeField]
    private GameObject[] costs;

    [SerializeField]
    private Text[] amounts;

    private Selectable actor;
    private Action action;

    public void PopulateButton(Selectable actor, Action action)
    {
        this.actor = actor;
        this.action = action;
        icon.sprite = action.icon;
        label.text = action.label;

        for(int i = 0; i != action.cost.Length; ++i)
        {
            if (action.cost[i] == 0)
                costs[i].SetActive(false);
            else
            {
                amounts[i].text = action.cost[i].ToString();
                costs[i].SetActive(true);
            }
        }
    }

    public void OnClick()
    {
        //actor.QueueAction(action);
    }
}
