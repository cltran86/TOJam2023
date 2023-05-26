using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionArea : MonoBehaviour
{
    public List<Selectable> selection;
    private Selectable inQuestion;

    private void Awake()
    {
        selection = new List<Selectable>(1);
    }
    public List<Selectable> GetSelection()
    {
        //  If selection contains any units, return a list of only units
        return selection;
    }

    private void OnTriggerEnter(Collider other)
    {
        inQuestion = other.GetComponent<Selectable>();

        if (inQuestion)
            selection.Add(inQuestion);
    }
    private void OnTriggerExit(Collider other)
    {
        inQuestion = other.GetComponent<Selectable>();

        if (inQuestion)
            selection.Remove(inQuestion);
    }
}
