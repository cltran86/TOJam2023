using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Resources
{
    wood,
    food,
    medicine
}
[System.Serializable]
public struct Fund
{
    public GameObject   panelUI;
    public Text         counter;
    public int          amount,
                        capacity;
}

public class ResourceManager : Singleton<ResourceManager>
{
    [SerializeField]
    private Fund[] funds;

    private bool sufficient;

    protected override void Awake()
    {
        base.Awake();
        InitializeUI();
    }

    private void InitializeUI()
    {
        foreach(Fund fund in funds)
        {
            fund.counter.text = fund.amount.ToString();

            if (fund.amount == 0)
                fund.panelUI.SetActive(false);
        }
    }
    public bool RequestResources(int[] costs)
    {
        //  Check for sufficient funds first
        for(int i = 0; i != costs.Length; ++i)
            if (funds[i].amount < costs[i])
                return false;

        //  Pay the costs
        for (int i = 0; i != costs.Length; ++i)
            funds[i].amount -= costs[i];

        UpdateUI();
        return true;
    }
    private void UpdateUI()
    {
        foreach (Fund fund in funds)
            fund.counter.text = fund.amount.ToString();
    }

    public void Refund(int[] costs)
    {
        for (int i = 0; i != costs.Length; ++i)
            funds[i].amount += costs[i];

        UpdateUI();
    }

    public bool AlterFunds(Resources resource, int amount)
    {
        sufficient = true;
        funds[(int)resource].amount += amount;

        if(funds[(int)resource].amount < 0)
        {
            funds[(int)resource].amount = 0;
            sufficient = false;
        }
        funds[(int)resource].counter.text = funds[(int)resource].amount.ToString();

        return sufficient;
    }

    private void OnValidate()
    {
        for(int f = 0; f != funds.Length; ++f)
        {
            funds[f].amount = Mathf.Clamp(funds[f].amount, 0, funds[f].capacity);
        }
    }
}
