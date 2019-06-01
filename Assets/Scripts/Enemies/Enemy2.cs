using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Subscript for enemy 2. Attack data for ranged "push".
public class Enemy2 : EnemyGeneric
{
    public GameObject proj; // Projectile prefab
    public Transform handTransform; // Transform of the rig's hand, used for projectile positioning.

    private void Start()
    {
        health = maxHP;
        overmind = GameObject.Find("Overmind");
        overmind.GetComponent<Overmind>().AddRanged(this.gameObject);
    }

    // Attack timing
    protected override IEnumerator Attack()
    {
        GetComponent<EnemyMovement>().anim.SetFloat("atkspd", 1.2f);
        bool atk = true;
        for (float i = 0; i < 1f; i += Time.deltaTime)
        {
            if (GetComponent<EnemyMovement>().state != "doingattack")
            {
                atk = false;
                break;
            }
            else
            {
                yield return null;
            }
        }

        if(atk)
        {
            GameObject hand = Instantiate(proj, handTransform.position, Quaternion.identity);
            hand.GetComponent<HandProjMove>().direction = GetComponent<EnemyMovement>().direction;
            hand.GetComponent<HandProjMove>().damage = damage;
        }

        for (float i = 0; i < 0.6f; i += Time.deltaTime)
        {
            if (GetComponent<EnemyMovement>().state != "doingattack")
            {
                atk = false;
                break;
            }
            else
            {
                yield return null;
            };
        }

        if (GetComponent<EnemyMovement>().state == "doingattack")
        {
            GetComponent<EnemyMovement>().ResumeMovement();
        }
    }

    public override void Die()
    {
        overmind.GetComponent<Overmind>().RemoveRanged(this.gameObject);
        GetComponent<EnemyMovement>().Die(0.5f);
        int droppedScraps = Random.Range(1, 11);
        for (int i = 0; i < droppedScraps; i++)
        {
            Instantiate(Scrap, transform.position, Quaternion.identity);
        }
    }
}
