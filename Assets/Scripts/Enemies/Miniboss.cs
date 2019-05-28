using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Subscript for Miniboss.
public class Miniboss : EnemyGeneric
{
    public SphereCollider atkBox;
    public GameObject hitParticle;
    private bool hitPlayer;

    private bool vulnerable;
    private float vulnTimer;

    public float punchSpeed = 10f;
    public float punchDecel = 1f;
    private float curPunchSpeed;

    private CharacterController controller;

    private void Start()
    {
        health = maxHP;
        vulnerable = false;
        vulnTimer = 5f;
        curPunchSpeed = 0f;
        controller = this.GetComponent<CharacterController>();
        overmind = GameObject.Find("Overmind");
        overmind.GetComponent<Overmind>().AddMiniboss(this.gameObject);
    }

    private void Update()
    {
        if(vulnerable)
        {
            vulnTimer -= Time.deltaTime;
            if(vulnTimer <= 0)
            {
                vulnerable = false;
                vulnTimer = 5f;
            }
        }
    }

    public override void TakeDamage(float atk, bool isKnockdown)
    {
        if(vulnerable)
        {
            Damage(atk);
            GetComponent<EnemyMovement>().Stagger();
        }
    }

    public override void DoAttack()
    {
        GetComponent<EnemyMovement>().StopForAttack();
        int thisattack = Random.Range(0,1);
        if(thisattack == 0)
        {
            StartCoroutine("Attack");
        }
        else if(thisattack == 1)
        {
            StartCoroutine("Bounce");
        }
    }

    // Punch attack timing
    protected override IEnumerator Attack()
    {
        hitPlayer = false;
        bool speedBurst = false;
        for(float i=0; i<3f; i += Time.deltaTime)
        {
            if(GetComponent<EnemyMovement>().state != "doingattack")
            {
                break;
            }

            if(i >= 0f && i < 1f)
            {
                yield return null;
            }
            else if (i >= 1f && i < 2f)
            {
                //atkBox.GetComponent<MeshRenderer>().enabled = true;
                if(!speedBurst)
                {
                    curPunchSpeed = punchSpeed;
                    speedBurst = true;
                }
                if (!hitPlayer)
                {
                    AtkDetect();
                }
                curPunchSpeed -= punchDecel * Time.deltaTime;
                controller.Move(new Vector3(GetComponent<EnemyMovement>().direction * curPunchSpeed, 0, 0));
                yield return null;
            }
            else if (i >= 2f && i < 3f)
            {
                //atkBox.GetComponent<MeshRenderer>().enabled = false;
                yield return null;
            }
        }
        //atkBox.GetComponent<MeshRenderer>().enabled = false;
        if (GetComponent<EnemyMovement>().state == "doingattack")
        {
            GetComponent<EnemyMovement>().ResumeMovement();
        }
    }

    // Bounce attack
    private IEnumerator Bounce()
    {
        yield return null;
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
                GameObject p = Instantiate(hitParticle, atkBox.bounds.center, transform.rotation, null);
                p.transform.Rotate(0, 90, 0);
                var main = p.GetComponent<ParticleSystem>().main;
                main.startColor = new Color(255, 0, 0, 255);
            }
        }
    }

    public override void Die()
    {
        overmind.GetComponent<Overmind>().RemoveMiniboss();
        GetComponent<EnemyMovement>().Die(0.5f);
        int droppedScraps = Random.Range(10, 50);
        for(int i=0; i < droppedScraps; i++)
        {
            Instantiate(Scrap, transform.position, Quaternion.identity);
        }
    }

}
