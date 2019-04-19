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
        yield return new WaitForSeconds(0.5f);
        GameObject hand = Instantiate(proj, handTransform.position, Quaternion.identity);
        hand.GetComponent<HandProjMove>().direction = GetComponent<EnemyMovement>().direction;
        hand.GetComponent<HandProjMove>().damage = damage;
        yield return new WaitForSeconds(0.5f);
        GetComponent<EnemyMovement>().ResumeMovement();
    }

    public override void Die()
    {
        overmind.GetComponent<Overmind>().RemoveRanged(this.gameObject);
        GetComponent<EnemyMovement>().Die(0.5f);
    }
}
