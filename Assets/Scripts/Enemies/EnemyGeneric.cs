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
    public GameObject chargeParticle;
    public GameObject HPBar;
    private GameObject curChargeParticle;

    [HideInInspector]
    public float health;

    [HideInInspector]
    public GameObject overmind;

    // Blatantly and shamelessly stolen from the camera script
    public float shakeDuration = 0f;
    public float shakeAmount = 0.2f;
    public float decreaseFactor = 6f;

    protected bool dead = false;
    public bool deadForPlayer = false;

    // Called when the player hits the enemy.
    public virtual void TakeDamage(float atk, bool isKnockdown)
    {
        GetComponent<EnemyMovement>().anim.SetFloat("atkspd", 1f);
        Damage(atk);
        if (curChargeParticle != null)
        {
            Destroy(curChargeParticle);
        }
        if(dead == false)
        {
            if (isKnockdown == true)
            {
                GetComponent<EnemyMovement>().Knockdown();
            }
            else
            {
                GetComponent<EnemyMovement>().Stagger();
            }
        }
    }

    // Reduce HP
    public void Damage(float atk)
    {
        if (dead == false)
        {
            health -= atk;
            HPBar.GetComponent<EnemyHPBar>().UpdateBars(health / maxHP);
            doShake(0.3f);
            if (health <= 0 && dead == false)
            {
                deadForPlayer = true;
                dead = true;
                Die();
            }
        }
    }

    // Default death behavior. Overridden.
    public virtual void Die()
    {
        Destroy(this.gameObject);
    }

    // Enemy starts their attack. Called by EnemyMovement when in position to attack.
    public virtual void DoAttack()
    {
        GetComponent<EnemyMovement>().StopForAttack();
        Vector3 particlepos = new Vector3(transform.position.x, transform.position.y + 2.5f, transform.position.z);
        curChargeParticle = Instantiate(chargeParticle, particlepos, Quaternion.identity);
        var main = curChargeParticle.GetComponent<ParticleSystem>().main;
        main.startColor = new Color(255, 0, 0, 100);
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
        shakeAmount = 0.7f;
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
