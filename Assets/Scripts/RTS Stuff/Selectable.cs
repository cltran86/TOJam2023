using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Selectable : MonoBehaviour
{
    protected Animator animator;

    protected ResourceManager resources;
    protected Details details;
    protected InputManager input;

    public int health, maxHealth;
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

    public virtual bool TakeDamage(int damage, Crab whoAttackedMe)
    {
        health -= damage;

        if (selected)
            details.UpdatePrimaryGauge();

        if (health <= 0)
        {
            Die();
            return true;
        }
        return false;
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
