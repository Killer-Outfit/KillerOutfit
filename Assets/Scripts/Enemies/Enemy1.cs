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
        for(float i=0; i<1.2f; i += Time.deltaTime)
        {
            if(GetComponent<EnemyMovement>().state != "doingattack")
            {
                break;
            }

            if(i >= 0f && i < 0.5f)
            {
                yield return null;
            }
            else if(i >= 0.5f && i < 0.6f)
            {
                atkBox.GetComponent<MeshRenderer>().enabled = true;
                if (!hitPlayer)
                {
                    AtkDetect();
                }
                yield return null;
            }
            else
            {
                atkBox.GetComponent<MeshRenderer>().enabled = false;
                yield return null;
            }
        }
        atkBox.GetComponent<MeshRenderer>().enabled = false;
        GetComponent<EnemyMovement>().ResumeMovement();
    }

    // Check if the attack hitbox hit the player
    private void AtkDetect()
    {
        Collider[] cols = Physics.OverlapSphere(atkBox.bounds.center, atkBox.radius, LayerMask.GetMask("Default"));
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
