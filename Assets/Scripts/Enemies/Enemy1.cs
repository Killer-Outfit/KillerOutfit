using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Subscript for enemy 1. Attack data for melee swing.
public class Enemy1 : EnemyGeneric
{
    public SphereCollider atkBox;
    private bool hitPlayer;

    private void Start()
    {
        health = maxHP;
        overmind = GameObject.Find("Overmind");
        overmind.GetComponent<Overmind>().AddMelee(this.gameObject);
    }

    // Attack timing
    protected override IEnumerator Attack()
    {
        hitPlayer = false;
        yield return new WaitForSeconds(0.4f);
        for(float i=0; i < 0.3f; i+= Time.deltaTime)
        {
            if(!hitPlayer)
            {
                AtkDetect();
            }
        }
        yield return new WaitForSeconds(0.4f);
        GetComponent<EnemyMovement>().ResumeMovement();
    }

    // Check if the attack hitbox hit the player
    private void AtkDetect()
    {
        Collider[] cols = Physics.OverlapSphere(atkBox.bounds.center, 1.8f, LayerMask.GetMask("Default"));
        foreach(Collider c in cols)
        {
            if (c.gameObject.tag == "Player")
            {
                c.gameObject.GetComponent<playerNew>().decreaseHealth(damage);
                hitPlayer = true;
            }
        }
    }

    public override void Die()
    {
        overmind.GetComponent<Overmind>().RemoveMelee(this.gameObject);
        GetComponent<EnemyMovement>().Die(0.5f);
        int droppedScraps = Random.Range(1, 11);
        for(int i=0; i < droppedScraps; i++)
        {
            Instantiate(Scrap, transform.position, Quaternion.identity);
        }
    }

}
