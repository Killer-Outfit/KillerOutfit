using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour
{
    Vector3 movementVector;
    public float movementSpeed;
    private float turningSpeed;
    float vVelocity;
    CharacterController controller;
    private Camera mainCam;
    private Vector3 curPlayerPortPos;
    private bool attacking;
    private bool stagger;
    private bool rightFacing;
    private Vector3 right;
    private Vector3 left;
    Animator anim;
    private GameObject pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        turningSpeed = 0;
        anim = GetComponent<Animator>();
        right = new Vector3(0, -60, 0);
        left = new Vector3(0, 60, 0);
        rightFacing = true;
        attacking = false;
        stagger = false;
        controller = GetComponent<CharacterController>();
        vVelocity = -10;
        pauseMenu = GameObject.Find("PauseMenuElements");
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        normalMovement();
        curPlayerPortPos = mainCam.WorldToViewportPoint(transform.position);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("the escape key was pressed");
            if (!pauseMenu.activeInHierarchy)
            {
                Debug.Log("pause the game");
                PauseGame();
            }
            else if (pauseMenu.activeInHierarchy)
            {
                Debug.Log("play the game");
                ContinueGame();
            }
        }
    }

    private void normalMovement()
    {
        // Get stick inputs
        float vertical = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
        float horizontal = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
        anim.SetFloat("RunSpeed", Mathf.Max(Mathf.Abs(vertical), Mathf.Abs(horizontal)) * 3);

        if(Input.GetAxis("Horizontal") < 0)
        {
            rightFacing = false;
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            rightFacing = true;
        }

        if ((Input.GetAxis("Vertical") < 0 && transform.position.z < -4f) || (Input.GetAxis("Vertical") > 0 && transform.position.z > 3f))
        {
            vertical = 0;

        }
        if (curPlayerPortPos.x < 0f)
        {
            horizontal += 1f;
        }
        else if(curPlayerPortPos.x > 1f)
        {
            horizontal -= 1f;
        }

        if (!attacking && !stagger) { 

            Vector2 inputs = new Vector2(horizontal, vertical);
            inputs = Vector2.ClampMagnitude(inputs, 1);
            movementVector.x = inputs.x;
            movementVector.z = inputs.y;
            vVelocity = 0;
            if (controller.isGrounded == false || transform.position.y > 0)
            {
                //Debug.Log("I am off the ground");
                vVelocity = Physics.gravity.y / 10;
            }
            movementVector.y = vVelocity;
            controller.Move(movementVector * Time.deltaTime * movementSpeed);

            // play run animation when the player is moving
            if (vertical != 0 || horizontal != 0)
            {
                //anim.Play("HumanoidRun");
                anim.SetBool("isIdle", false);
            }
            else
            {
                //anim.SetTrigger("stopRun");
                anim.SetBool("isIdle", true);
            }
        }
        else
        {
            if (Input.GetAxis("Horizontal") > 0)
            {
                rightFacing = true;
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                rightFacing = false;
            }
        }
        if (rightFacing)
        {
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }else
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    private void ContinueGame()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void setAttacking(bool isAttack)
    {
        attacking = isAttack;
    }

    public void setStagger(bool isStagger)
    {
        stagger = isStagger;
    }
}
