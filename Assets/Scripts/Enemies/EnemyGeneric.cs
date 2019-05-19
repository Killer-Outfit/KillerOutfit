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

    // Blatantly and shamelessly stolen from the camera script
    public float shakeDuration = 0f;
    public float shakeAmount = 0.2f;
    public float decreaseFactor = 6f;

    protected bool dead = false;

    // Called when the player hits the enemy.
    public void TakeDamage(float atk, bool isKnockdown)
    {
        Damage(atk);
        if (isKnockdown == true)
        {
            GetComponent<EnemyMovement>().Knockdown(0.4f);
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
        doShake(0.3f);
        if (health <= 0 && dead == false)
        {
            dead = true;
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

    private void doShake(float duration)
    {
        shakeDuration = duration;
        shakeAmount = 1f;
        StartCoroutine("Shake");
    }

    private IEnumerator Shake()
    {
        Vector3 originalPos = transform.localPosition;
        Quaternion originalRot = transform.rotation;
        while (shakeDuration > 0)
        {
            transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
            shakeAmount -= Time.deltaTime * decreaseFactor;
            if (shakeAmount < 0)
            {
                shakeAmount = 0;
            }
            shakeDuration -= Time.deltaTime;
            yield return null;
        }
        shakeDuration = 0f;
        transform.localPosition = originalPos;
        transform.rotation = originalRot;
    }
}
