using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Selectable : MonoBehaviour
{
    protected Animator animator;

    protected ResourceManager resources;
    protected Details details;
    protected InputManager input;

    public string selectedName;
    public Sprite selectedImage;
    [TextArea]
    public string selectedDescription;

    [SerializeField]
    private GameObject selectionReticle;
    protected bool selected;

    [SerializeField]
    protected Action[] actions;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }
    protected virtual void Start()
    {
        resources = ResourceManager.Instance;
        details = Details.Instance;
        input = InputManager.Instance;
    }

    public virtual void Select(bool select)
    {
        selected = select;
        selectionReticle.SetActive(select);
    }
/*
    public virtual bool QueueAction(Action toQueue)
    {
        return false;
    }
*/
    //public abstract Action[] GetActions();

    //    protected abstract void GetPrimaryGaugeValue();
    //  protected abstract void GetSecondaryGaugeValue();
}
