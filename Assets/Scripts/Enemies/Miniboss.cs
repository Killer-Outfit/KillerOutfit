using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Subscript for Miniboss.
public class Miniboss : EnemyGeneric
{
    public SphereCollider atkBox;
    public SphereCollider groundDetector;
    public GameObject hitParticle;
    public GameObject shockwave;
    private Transform playerTransform;
    private bool hitPlayer;

    public bool vulnerable;
    private float vulnTimer;
    private bool grounded;
    private float groundTimer;

    public float punchSpeed = 1f;
    public float punchDecel = 0.5f;
    private float curPunchSpeed;

    public int numBounces = 3;

    private CharacterController controller;
    private ParticleSystem ps;
    private ParticleSystem.EmissionModule em;

    private void Start()
    {
        health = maxHP;
        vulnerable = false;
        grounded = true;
        vulnTimer = 5f;
        groundTimer = 0.5f;
        curPunchSpeed = 0f;
        controller = this.GetComponent<CharacterController>();
        overmind = GameObject.Find("Overmind");
        overmind.GetComponent<Overmind>().AddMiniboss(this.gameObject);
        playerTransform = GameObject.Find("PlayerBody").transform;
        ps = GetComponent<ParticleSystem>();
        em = ps.emission;
        em.rateOverTime = 0;
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
                em.rateOverTime = 0;
            }
        }

        groundTimer -= Time.deltaTime;
        if(groundTimer <= 0)
        {
            grounded = CheckGrounded();
            Debug.Log(grounded);
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
        int thisattack = Random.Range(0, 2);
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

                controller.Move(new Vector3(GetComponent<EnemyMovement>().direction * curPunchSpeed, 0, 0));
                if(curPunchSpeed > 0)
                {
                    curPunchSpeed -= punchDecel * Time.deltaTime;
                }
                else
                {
                    curPunchSpeed = 0;
                }
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
        yield return null;
    }

    // Bounce attack
    private IEnumerator Bounce()
    {
        float jumpForce;
        // Windup, add particle later
        yield return new WaitForSeconds(1.5f);

        // Do the set number of bounces
        for (int i = 0; i < numBounces; i++)
        {
            // Target a position near the player
            Vector3 nextTarget = playerTransform.position;
            nextTarget.x += Random.Range(-2f, 2f);
            nextTarget.z += Random.Range(-2f, 2f);

            jumpForce = 30f;
            Vector3 moveDirection = nextTarget - transform.position;
            moveDirection = moveDirection.normalized * 5 * Time.deltaTime;
            moveDirection.y = jumpForce * Time.deltaTime;

            // Leap, should set grounded to false, and don't check grounding for 0.5 seconds.
            controller.Move(moveDirection);
            grounded = false;
            groundTimer = 0.5f;
            yield return null;

            // Boss is in the air presumably.
            while(!grounded)
            {
                if(jumpForce > -30f)
                {
                    jumpForce -= 30 * Time.deltaTime;
                }
                moveDirection = nextTarget - transform.position;
                moveDirection = moveDirection.normalized * 5 * Time.deltaTime;
                moveDirection.y = jumpForce * Time.deltaTime;
                controller.Move(moveDirection);
                yield return null;
            }
            // Create shockwave
            Instantiate(shockwave, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
        }

        if (GetComponent<EnemyMovement>().state == "doingattack")
        {
            GetComponent<EnemyMovement>().ResumeMovement();
        }
        vulnerable = true;
        em.rateOverTime = 10;
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

    private bool CheckGrounded()
    {
        Collider[] cols = Physics.OverlapSphere(groundDetector.bounds.center, groundDetector.radius, LayerMask.GetMask("Default"));
        foreach (Collider c in cols)
        {
            if (c.gameObject.name.Contains("Plane") || c.gameObject.name.Contains("Road"))
            {
                return true;
            }
        }
        return false;
    }
}
