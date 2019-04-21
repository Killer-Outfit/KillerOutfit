using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// General script for enemies.
public class EnemyGeneric : MonoBehaviour
{
    // Stats
    public float maxHP;
    public float damage;

    public GameObject Scrap;

    [HideInInspector]
    public float health;

    [HideInInspector]
    public GameObject overmind;

    // Called when the player hits the enemy.
    public void TakeDamage(float atk, bool isKnockdown)
    {
        Damage(atk);
        if (isKnockdown == true)
        {
            GetComponent<EnemyMovement>().Knockdown();
        }
        else
        {
            GetComponent<EnemyMovement>().Stagger();
        }
    }

    // Reduce HP
    public void Damage(float atk)
    {
        health -= atk;
        if(health <= 0)
        {
            Die();
        }
    }

    // Default death behavior. Overridden.
    public virtual void Die()
    {
        Destroy(this.gameObject);
    }

    // Enemy starts their attack. Called by EnemyMovement when in position to attack. Overridden in enemy classes.
    public void DoAttack()
    {
        GetComponent<EnemyMovement>().StopForAttack();
        StartCoroutine("Attack");
    }

    // Default attack to be overridden.
    protected virtual IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<EnemyMovement>().ResumeMovement();
    }
}
