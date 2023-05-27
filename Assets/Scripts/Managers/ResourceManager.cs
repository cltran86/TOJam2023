using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : Singleton<ResourceManager>
{
    [SerializeField]
    private int units,
                housing,
                coral;

    [SerializeField]
    private Text    housingUI,
                    coralUI;

    private void Start()
    {
        UpdateUI();
    }

    public void AddResource(int housing, int coral)
    {
        this.housing += housing;
        this.coral += coral;
        UpdateUI();
    }

    public bool RequestResources(int cost)
    {
        if (coral < cost)
            return false;

        coral -= cost;

        UpdateUI();
        return true;
    }

    private void UpdateUI()
    {
        housingUI.text = units + " / " + housing;
        coralUI.text = coral.ToString();
    }

    public void Refund(int cost)
    {
        coral += cost;
        UpdateUI();
    }
}
