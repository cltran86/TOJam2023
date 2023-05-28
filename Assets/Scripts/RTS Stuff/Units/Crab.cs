using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : MonoBehaviour
{
    [SerializeField]
    private int health,
                maxHealth,
                attack,
                defense,
                convertAmount,
                convertQuota;

    [SerializeField]
    private float   moveSpeed,
                    attackSpeed,
                    attackRange;

    [SerializeField]
    private SphereCollider detectionRange;

    private Selectable target;

    private bool    targetIsInRange,
                    canAttack = true;

    private void Start()
    {
        StartCoroutine(Attacking());
    }

    public void SetTarget(Selectable target)
    {
        this.target = target;
    }

    private void MoveTowardsTarget()
    {
        transform.LookAt(target.transform);
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
    }

    private IEnumerator Attacking()
    {
        while (true)
        {
            detectionRange.radius += 1f;

            if (target != null && !targetIsInRange)
                MoveTowardsTarget();

            if (target == null)
                yield return null;

            else if (canAttack)
            {
                if (targetIsInRange)
                {
                    if(target.TakeDamage(attack, this))
                    {
                        target = null;
                        targetIsInRange = false;
                    }
                    yield return new WaitForSeconds(1f / attackSpeed);
                }
                else
                    yield return null;
            }
            else
                yield return null;
        }
    }

    public bool TakeDamage(int damage, Villager whoAttackedMe)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
            return true;
        }

        if (target == null)
            target = whoAttackedMe;

        return false;
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Selectable>() == target)
            targetIsInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Selectable>() == target)
            targetIsInRange = false;
    }
}
