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

    private int numReg;

    public float punchSpeed = 1f;
    public float punchDecel = 0.5f;
    private float curPunchSpeed;

    public int numBounces = 3;

    private CharacterController controller;
    public GameObject armature;
    private ParticleSystem ps;
    private ParticleSystem.EmissionModule em;
    private AudioSource source;

    [SerializeField]
    private AudioClip[] hitTaunt;
    [SerializeField]
    private AudioClip[] hurtSound;

    [SerializeField]
    private AudioClip[] dieSound;

    FMOD.Studio.EventInstance music;
    FMOD.Studio.Bus bus;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        bus = FMODUnity.RuntimeManager.GetBus("Bus:/");
        bus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
        music = FMODUnity.RuntimeManager.CreateInstance("event:/Music/Midboss");
        music.start();
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
                GetComponent<EnemyMovement>().anim.SetTrigger("Recover");
                vulnerable = false;
                vulnTimer = 5f;
                em.rateOverTime = 0;
                StartCoroutine("Recovery");
            }
        }
        //if (Mathf.Sign(groundTimer) == 1)
        //{
        //    groundTimer -= Time.deltaTime;
        //    Debug.Log(groundTimer);
        //}
        //else
        //{
        //    Debug.Log("groundTimer finished");
        //    grounded = CheckGrounded();
        //    Debug.Log(grounded);
        //}
    }

    public override void TakeDamage(float atk, bool isKnockdown)
    {
        if(vulnerable)
        {
            AudioClip clip = GetRandomHurtSound();
            source.PlayOneShot(clip);
            Damage(atk);
            //GetComponent<EnemyMovement>().Stagger();
        }
    }

    public override void DoAttack()
    {
        GetComponent<EnemyMovement>().StopForAttack();
        int thisattack = Random.Range(0, 2);
        numReg++;
        if (thisattack == 0)
        {
            GetComponent<EnemyMovement>().anim.SetTrigger("Attack");
            StartCoroutine("Attack");
        }
        else if (thisattack == 1 || numReg == 3)
        {
            numReg = 0;
            GetComponent<EnemyMovement>().anim.SetTrigger("StartBounce");
            StartCoroutine("Bounce");
        }
    }

    // Punch attack timing
    protected override IEnumerator Attack()
    {
        hitPlayer = false;
        bool speedBurst = false;
        for(float i=0; i<2.7f; i += Time.deltaTime)
        {
            if(GetComponent<EnemyMovement>().state != "doingattack")
            {
                break;
            }

            if(i >= 0f && i < 0.7f)
            {
                yield return null;
            }
            else if (i >= 0.7f && i < 1.7f)
            {
                //atkBox.GetComponent<MeshRenderer>().enabled = true;
                if(!speedBurst)
                {
                    curPunchSpeed = punchSpeed;
                    speedBurst = true;
                }

                if (!hitPlayer && i <= 1.4f)
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
            else if (i >= 1.7f && i < 2.7f)
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
        yield return new WaitForSeconds(0.8f);
        StartCoroutine("armatureDown");

        // Do the set number of bounces
        for (int i = 0; i < numBounces; i++)
        {
            // Target a position near the player
            Vector3 nextTarget = playerTransform.position;
            Debug.Log("Next target " + nextTarget);
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
            //Debug.Log("grounded check before yield: " + grounded);
            yield return null;
            //Debug.Log("grounded check after yield: " + grounded);

            // Boss is in the air presumably.
            while (!grounded)
            {
                if (Mathf.Sign(groundTimer) == 1)
                {
                    groundTimer -= Time.deltaTime;
                    Debug.Log(groundTimer);
                }
                else
                {
                    Debug.Log("groundTimer finished");
                    grounded = CheckGrounded();
                    Debug.Log(grounded);
                }
                //Debug.Log("grounded check while not grounded: " + grounded);
                if (jumpForce > -30f)
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
            Debug.Log("shockwave creation called");
            Instantiate(shockwave, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
        }
        StartCoroutine("armatureUp");
        GetComponent<EnemyMovement>().anim.SetTrigger("Land");
        GetComponent<EnemyMovement>().state = "stunned";

        //if (GetComponent<EnemyMovement>().state == "doingattack")
        //{
        //    GetComponent<EnemyMovement>().ResumeMovement();
        //}
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
                AudioClip clip = GetRandomHitTaunt();
                source.PlayOneShot(clip);
                GameObject p = Instantiate(hitParticle, atkBox.bounds.center, transform.rotation, null);
                p.transform.Rotate(0, 90, 0);
                var main = p.GetComponent<ParticleSystem>().main;
                main.startColor = new Color(255, 0, 0, 255);
            }
        }
    }

    public override void Die()
    {
        AudioClip clip = GetRandomDieSound();
        source.PlayOneShot(clip);
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
        Collider[] cols = Physics.OverlapSphere(groundDetector.bounds.center, groundDetector.radius, LayerMask.GetMask("Plane"));
        foreach (Collider c in cols)
        {
            if (c.gameObject.name.Contains("Plane") || c.gameObject.name.Contains("Road"))
            {
                return true;
            }
        }
        return false;
    }

    private IEnumerator armatureUp()
    {
        for(float i = 0; i < 0.2f; i += Time.deltaTime)
        {
            armature.transform.localPosition = new Vector3(0, armature.transform.localPosition.y + 1.2f, 0);
            yield return null;
        }
        armature.transform.localPosition = new Vector3(0, 0, 0);
    }

    private IEnumerator armatureDown()
    {
        for (float i = 0; i < 0.2f; i += Time.deltaTime)
        {
            armature.transform.localPosition = new Vector3(0, armature.transform.localPosition.y - 1.5f, 0);
            yield return null;
        }

    }

    private IEnumerator Recovery()
    {
        yield return new WaitForSeconds(1f);
        GetComponent<EnemyMovement>().ResumeMovement();
        yield return null;
    }

    private AudioClip GetRandomHitTaunt()
    {
        return hitTaunt[UnityEngine.Random.Range(0, hitTaunt.Length)];
    }

    private AudioClip GetRandomHurtSound()
    {
        return hurtSound[UnityEngine.Random.Range(0, hurtSound.Length)];
    }

    private AudioClip GetRandomDieSound()
    {
        return dieSound[UnityEngine.Random.Range(0, dieSound.Length)];
    }
}
