using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionQueueButton : MonoBehaviour
{
    [SerializeField]
    private Image icon;

    public void PopulateButton(Sprite icon)
    {
        this.icon.sprite = icon;
    }
}
