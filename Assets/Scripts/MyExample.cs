using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MyExample : MonoBehaviour
{
    [SerializeField]
    GameObject item;
    [SerializeField]
    Button myButton;
    private ButtonClickDelegate onButtonClicked1;
    private ButtonClickDelegate onButtonClicked2;

    public delegate void ButtonClickDelegate(GameObject item);

    private void Start()
    {
        onButtonClicked1 = DoSomethingFunction;

        myButton.onClick.AddListener(() => onButtonClicked1(item));

        onButtonClicked1 = DoSomethingElseFunction;

//        myButton.onClick.AddListener(() => onButtonClicked2(item));
    }
    private void DoSomethingFunction(GameObject item)
    {
        print("Do Something Function");
    }

    private void DoSomethingElseFunction(GameObject item)
    {
        print("Do Something Else Function");
    }
}
