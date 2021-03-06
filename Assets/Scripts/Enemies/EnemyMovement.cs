﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Defined in prefab
    public string movementType;
    public float speed;

    private Transform playerTransform;

    [HideInInspector]
    public Animator anim;

    private CharacterController controller;
    private EnemyGeneric enemClass;

    private float vertical;
    private float horizontal;
    private float gravity;
    private float wanderTimer;
    private Vector3 movementVector;
    private Camera mainCam;

    private Vector3 attackMoveTarget;

    private float stagTimer;
    private float knockSpeed;
    private float dieTime;

    private Color32 originalColor;

    public SkinnedMeshRenderer render;

    //Public variables accessed by other scripts. Do not need to be set manually.
    [HideInInspector]
    public float pDist;
    [HideInInspector]
    public bool wantsToAttack;
    [HideInInspector]
    public int direction;
    [HideInInspector]
    public string state;


    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        playerTransform = GameObject.Find("PlayerBody").transform;
        anim = this.GetComponent<Animator>();
        controller = this.GetComponent<CharacterController>();
        enemClass = this.GetComponent<EnemyGeneric>();
        direction = -1;
        state = "idle";
        wantsToAttack = false;
        movementVector = new Vector3(0, 0, 0);
        IdleMove();
        CheckPlayer();

        originalColor = render.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        // Change horizontal and vertical movement values based on state.
        if (state == "idle")
        {
            wanderTimer -= Time.deltaTime;
            if (wanderTimer <= 0)
            {
                CheckPlayer();
                IdleMove();
            }
            if(wantsToAttack == true)
            {
                state = "attacking";
            }
        }
        else if (state == "attacking")
        {
            AttackMove();
        }
        else
        {
            horizontal = 0;
            vertical = 0;
        }

        // Check for lower wall collision
        if (transform.position.z < -4.1f)
        {
            controller.Move(new Vector3(0, 0, 0.05f));
        }

        if (controller.isGrounded)
        {
            gravity = 0;
        }
        else
        {
            gravity = -5;
        }

        // Update distance to player, used by the controller
        pDist = Mathf.Abs(Vector3.Distance(new Vector3(playerTransform.position.x, 0, playerTransform.position.z), new Vector3(this.transform.position.x, 0, this.transform.position.z)));

        movementVector = new Vector3(direction * horizontal, gravity, vertical);
        controller.Move(movementVector.normalized * speed * Time.deltaTime);
    }

    // Updates the current idle movement on a random timer, based on behavior type (aggressive, defensive, stationary).
    void IdleMove()
    {
        wanderTimer = Random.Range(1f, 1.5f);
        float randStop = Random.Range(0, 1f);

        if ((movementType == "aggressive" || pDist > 5) && randStop > 0.2)
        {
            anim.SetBool("Walking", true);
            vertical = Random.Range(-1f, 1f);
            horizontal = Random.Range(1f, -0.25f);
        }
        else if (movementType == "defensive" && randStop > 0.3)
        {
            anim.SetBool("Walking", true);
            vertical = Random.Range(-1f, 1f);
            horizontal = Random.Range(0.25f, -0.75f);
        }
        else if (movementType == "miniboss" && randStop > 0.4f)
        {
            anim.SetBool("Walking", true);
            vertical = Random.Range(-1f, 1f);
            horizontal = Random.Range(1f, -0.25f);
        }
        else
        {
            anim.SetBool("Walking", false);
            horizontal = 0;
            vertical = 0;
        }
    }

    // Moves in front of the player to attack.
    void AttackMove()
    {
        anim.SetBool("Walking", true);
        // Set attackMoveTarget to a space just in front of the player, depending on which side the enemy is on
        Vector3 tmp = playerTransform.position;
        tmp.x += -direction * 2;
        attackMoveTarget = tmp;

        if (movementType == "aggressive")
        {
            float playerX = attackMoveTarget.x;
            float enemyX = this.transform.position.x;
            float hDiff = playerX - enemyX;
            if (Mathf.Abs(hDiff) < 0.1)
            {
                horizontal = 0;
            }
            else
            {
                horizontal = direction * Mathf.Sign(hDiff);
            }
        }
        else if (movementType == "defensive")
        {
            horizontal = Random.Range(0.25f, -0.75f);
        }
        else if(movementType == "miniboss")
        {
            horizontal = Random.Range(1f, -0.25f);
        }

        float playerZ = attackMoveTarget.z;
        float enemyZ = this.transform.position.z;
        float vDiff = playerZ - enemyZ;
        if (Mathf.Abs(vDiff) < 0.1)
        {
            vertical = 0;
        }
        else
        {
            vertical = Mathf.Sign(vDiff);
        }

        CheckPlayer();
        CheckForAttack();
    }

    // Called when the enemy is moving to attack the player. If lined up with the player, do attack sequence.
    void CheckForAttack()
    {
        if(movementType == "aggressive")
        {
            if (horizontal == 0 && vertical == 0)
            {
                enemClass.DoAttack();
            }
        }
        else if (movementType == "defensive")
        {
            if (vertical == 0)
            {
                enemClass.DoAttack();
            }
        }
        else if (movementType == "miniboss")
        {
            if (vertical == 0)
            {
                enemClass.DoAttack();
            }
        }
    }

    // Sets the enemy's direction for movement and facing. -1 is facing LEFT (right of the player), 1 is facing RIGHT (left of the player).
    void CheckPlayer()
    {
        float playerX = playerTransform.position.x;
        float enemyX = this.transform.position.x;
        float diff = Mathf.Sign(playerX - enemyX);
        if (diff != direction)
        {
            direction = -direction;
            this.transform.Rotate(new Vector3(0, 180, 0));
        }
    }

    public void StopForAttack()
    {
        state = "doingattack";
        wantsToAttack = false;
    }

    public void ResumeMovement()
    {
        anim.SetTrigger("BackToIdle");
        wanderTimer = 0;
        state = "idle";
    }

    public void Stagger()
    {
        StopCoroutine("KnockdownCR");
        if (state != "dying")
        {
            anim.SetTrigger("Stagger");
            stagTimer = 0.6f;
            if (state != "stagger")
            {
                state = "stagger";
                StartCoroutine("StaggerCR");
            }
        }
    }

    private IEnumerator StaggerCR()
    {
        for (float i=0; i < stagTimer; stagTimer-=Time.deltaTime)
        {
            StartCoroutine(FlashDamage());
            yield return null;
        }
        if (state != "dead")
            ResumeMovement();
    }

    public void Knockdown()
    {
        StopCoroutine("StaggerCR");
        if (state != "dying")
        {
            anim.SetTrigger("Knockdown");
            state = "knockdown";
            knockSpeed = 0.4f;
            StartCoroutine("KnockdownCR");
        }
    }

    private IEnumerator KnockdownCR()
    {
        StartCoroutine(FlashDamage());
        for (float i = 0.5f; i > 0; i -= Time.deltaTime)
        {
            controller.Move(new Vector3(-direction * knockSpeed, 0, 0));
            if(knockSpeed > 0)
            {
                knockSpeed -= 0.02f;
            }

            yield return null;
        }
        //int rand = Random.Range(5, 15);
        //if (rand < 7)
        //    rand = 5;
        //else if (rand > 12)
        //    rand = 15;
        //else
        //    rand = 10;
        //float ranF = (float) rand;
        //ranF /= 10;
        //Debug.Log(ranF);
        yield return new WaitForSeconds(1);
        StartCoroutine("GetUp");
    }

    private IEnumerator GetUp()
    {
        //Debug.Log("GetUp called");
        anim.SetTrigger("GetUp");
        yield return new WaitForSeconds(1.2f);
        ResumeMovement();
    }

    private IEnumerator KnockdownCRDeath()
    {
        //anim.SetBool("Walking", false);
        for (float i = 0.5f; i > 0; i -= Time.deltaTime)
        {
            controller.Move(new Vector3(-direction * knockSpeed, 0, 0));
            if (knockSpeed > 0)
            {
                knockSpeed -= 0.02f;
            }
            yield return null;
        }
        //Debug.Log("starting flash");
        StartCoroutine("FlashDeath");
    }

    public void Die(float time = 2f)
    {
        StopCoroutine("StaggerCR");
        StopCoroutine("KnockdownCR");
        if (state != "dying")
        {
            anim.SetTrigger("Death");
            anim.SetBool("Walking", false);
            state = "dying";
            dieTime = time;
            knockSpeed = 0.4f;
            StartCoroutine("KnockdownCRDeath");
        }
    }

    public IEnumerator FlashDamage()
    {
        render.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        //Debug.Log("flash at " + f);
        render.material.color = originalColor;
        yield return null;
    }

    public IEnumerator FlashDeath()
    {
        for (float f = 0.1f; f > 0; f -= Time.deltaTime)
        {
            render.material.color = Color.red;
            yield return new WaitForSeconds(f);
            //Debug.Log("flash at " + f);
            render.material.color = originalColor;
            yield return new WaitForSeconds(f);
        }
        Destroy(this.gameObject);
    }

    // Move away from walls.
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            movementVector.z = -movementVector.z;
        }
    }
}
