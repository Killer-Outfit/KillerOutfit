﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickable : MonoBehaviour
{
    public bool selected;
    private bool ready;
    private Renderer rend;
    private SpriteRenderer innerBox;
    private SpriteRenderer outerBox;
    private MeshRenderer purchaseText;
    private string type;
    private GameObject player;
    private bool rotate;
    private float originalY;
    private bool up;
    private GameObject menuModel;
    private GameObject[] clickables;
    private Camera menuCamera;
    private Camera mainCamera;
    [HideInInspector]
    public bool stickInputAccepted;
    public bool hoverB;
    //private Vector3[] clickablePortPos;


    public bool unlocked;
    public GameObject outfitDisplay;
    public outfits item;
    public int cost;
    //public GameObject box;

    [SerializeField]
    private AudioClip[] menuHover;

    public AudioClip menuClick;

    private AudioSource audioSource;

    private bool hoverSound;
    private float startYRot;
    public bool misc;

    // Start is called before the first frame update
    void Start()
    {
        ready = true;
        innerBox = transform.parent.GetChild(1).gameObject.GetComponent<SpriteRenderer>();
        outerBox = transform.parent.GetChild(4).gameObject.GetComponent<SpriteRenderer>();
        purchaseText = transform.parent.GetChild(3).gameObject.GetComponent<MeshRenderer>();
        startYRot = outfitDisplay.transform.eulerAngles.y;
        audioSource = GetComponent<AudioSource>();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        menuCamera = GameObject.Find("OutfitCamera").GetComponent<Camera>();
        clickables = GameObject.FindGameObjectsWithTag("Clickable");
        /*clickablePortPos = new Vector3[clickables.Length];
        for(int i = 0; i < clickables.Length; i++)
        {
            clickablePortPos[i] = menuCamera.WorldToViewportPoint(clickables[i].transform.position);
        }*/
        up = true;
        originalY = outfitDisplay.transform.position.y;
        rotate = false;
        rend = GetComponent<Renderer>();
        rend.enabled = false;
        type = item.outfitType;
        player = GameObject.Find("PlayerBody");
        menuModel = GameObject.Find("OutfitModel");
        //Debug.Log(type);
        stickInputAccepted = true;
    }

    // Update is called once per frame
    void Update()
    {
		if (menuCamera.enabled)
		{
			if (unlocked)
			{
                purchaseText.enabled = false;
				if (selected)
                {
                    innerBox.material.color = Color.blue;
                }
				else
				{
                    innerBox.material.color = Color.green;
                }
			} else
			{
                if (!hoverB)
                {
                    selected = false;
                }
                if (ready)
                {
                    if (selected)
                    {
                        purchaseText.GetComponent<TextMesh>().text = "Purchase?\n" + "-" + cost.ToString() + " Scraps";
                        innerBox.material.color = Color.yellow;
                    }
                    else
                    {
                        purchaseText.GetComponent<TextMesh>().text = cost.ToString() + " Scraps";
                        innerBox.material.color = Color.red;
                    }
                }
                purchaseText.enabled = true;
            }
			if (rotate && !misc)
			{
				outfitDisplay.transform.eulerAngles = new Vector3(0, outfitDisplay.transform.eulerAngles.y + 1f, 0);
			}
            else if(!rotate && !misc)
            {
				outfitDisplay.transform.eulerAngles = new Vector3(outfitDisplay.transform.eulerAngles.x, startYRot, 0);
            }
            if (rotate && misc)
            {
                outfitDisplay.transform.eulerAngles = new Vector3(-90f, outfitDisplay.transform.eulerAngles.y + 1f, 0);
            }
            else if(!rotate && misc)
            {
                outfitDisplay.transform.eulerAngles = new Vector3(-90f, startYRot, 0);
            }

            if (outfitDisplay.transform.eulerAngles.y < originalY + 3f && up)
			{
				outfitDisplay.transform.position = new Vector3(0.0f, outfitDisplay.transform.position.y + 0.5f, 0.0f);
			}else if(outfitDisplay.transform.position.y == originalY + 3f)
			{
				up = false;
			}

			if (outfitDisplay.transform.position.y < originalY + 2f && up)
			{
				outfitDisplay.transform.position = new Vector3(outfitDisplay.transform.position.x, outfitDisplay.transform.position.y + 0.05f, outfitDisplay.transform.position.z);
			}
			else if (outfitDisplay.transform.position.y >= originalY + 2f)
			{
				up = false;
			}

			if (outfitDisplay.transform.position.y > originalY - 2f && !up)
			{
				outfitDisplay.transform.position = new Vector3(outfitDisplay.transform.position.x, outfitDisplay.transform.position.y - 0.05f, outfitDisplay.transform.position.z);
			}
			else if (outfitDisplay.transform.position.y <= originalY - 2f)
			{
				up = true;
			}

			if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
			{
				stickInputAccepted = true;
			}
			
			if (hoverB)
			{
				hover();

                if(hoverSound)
                {
                    Hover();
                }
			}else
			{
                hoverSound = true;
				rend.enabled = false;
                outerBox.enabled = false;
				rotate = false;
			}
		}
    }

    void OnMouseOver()
    {
        //rotate = true;
        //rend.enabled = true;
        hoverB = true;
        for(int i = 0; i < clickables.Length; i++)
        {
            if (clickables[i].name != this.name)
            {
                clickables[i].GetComponent<clickable>().hoverB = false;
            }
        }
        /*if (Input.GetMouseButtonDown(0) && !selected)
        {
            press();
            if(unlocked)
                unlockSelect();
            else
                lockSelect();
            selected = true;
        }*/
    }
    public void hover()
    {
        rotate = true;
        rend.enabled = false;
        outerBox.enabled = true;
        if ((Input.GetButtonDown("AButton") || Input.GetMouseButtonDown(0))) //&& !selected)
        {
            if (unlocked)
            {
                Click();
                unlockSelect();
            }
            else
            {
                Click();
                lockSelect();
            }
        }
        if (stickInputAccepted)
        {
            if (Input.GetAxis("Vertical") > 0)
            {
                move("up");
            }
            else if (Input.GetAxis("Vertical") < 0)
            {
                move("down");
            }
            if (Input.GetAxis("Horizontal") > 0)
            {
                move("right");
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                move("left");
            }
        }
    }
    void OnMouseExit()
    {
        //rotate = false;
        //rend.enabled = false;
    }

    private void move(string direction)
    {
        for (int i = 0; i < clickables.Length; i++)
        {
            if (clickables[i].name == this.name)
            {
                if (direction == "up" && i % 3 != 0)
                {
                    swap(i, -1);
                }
                else if (direction == "right" && i != 3 && i != 4 && i != 5)
                {
                    swap(i, 3);
                }
                else if (direction == "left" && i != 0 && i != 1 && i != 2)
                {
                    swap(i, -3);
                }
                else if (direction == "down" && i != 2 && i != 5)
                {
                    swap(i, +1);
                }
            }
        }
    }

    private void swap(int index, int modifier)
    {
        rotate = false;
        rend.enabled = false;
        outerBox.enabled = false;
        hoverB = false;
        clickables[index + modifier].GetComponent<clickable>().hoverB = true;
        clickables[index + modifier].GetComponent<clickable>().stickInputAccepted = false;
    }

    private void unlockSelect()
    {
        player.GetComponent<playerNew>().changeOutfit(item);
        menuModel.GetComponent<menuCharacter>().changeOutfit(item);
        for(int i = 0; i < clickables.Length; i++)
        {
            if (clickables[i].name != this.name && clickables[i].GetComponent<clickable>().item.outfitType == type)
            {
                clickables[i].GetComponent<clickable>().selected = false;
            }
        }
        selected = true;
    }

    private void lockSelect()
    {
        if (!selected)
        {
            // display are you sure text
            selected = true;
        }else
        {
            purchaseItem();
        }
    }


    public void purchaseItem()
    {
        //Debug.Log("in purchase item");
        if(player.GetComponent<playerNew>().spendScraps(cost))
        {
            //Debug.Log("SpentScraps");
            unlocked = true;
            selected = false;
        }else
        {
            //Debug.Log("NotEnoughScraps");
            selected = false;
            StartCoroutine("notEnough");
        }
    }
    IEnumerator notEnough()
    {
        ready = false;
        purchaseText.GetComponent<TextMesh>().text = "Not Enough Scraps";
        yield return new WaitForSeconds(.4f);
        ready = true;
    }

    public void Hover()
    {
        hoverSound = false;
        AudioClip clip = GetMenuHover();
        audioSource.PlayOneShot(clip);
    }

    public void Click()
    {
        audioSource.PlayOneShot(menuClick);
    }

    private AudioClip GetMenuHover()
    {
        return menuHover[UnityEngine.Random.Range(0, menuHover.Length)];
    }
}
