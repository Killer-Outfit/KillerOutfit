using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickable : MonoBehaviour
{
    public bool selected;
    private Renderer rend;
    private SpriteRenderer innerBox;
    private SpriteRenderer outerBox;
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
        innerBox = transform.parent.GetChild(1).gameObject.GetComponent<SpriteRenderer>();
        outerBox = transform.parent.GetChild(3).gameObject.GetComponent<SpriteRenderer>();
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
				if (selected)
                {
                    
					//box.GetComponent<Renderer>().material.color = Color.blue;
                    innerBox.color = Color.blue;
                }
				else
				{
					//box.GetComponent<Renderer>().material.color = Color.green;
                    innerBox.color = Color.green;
                }
			} else
			{
				if (selected)
				{
					//box.GetComponent<Renderer>().material.color = Color.yellow;
                    innerBox.color = Color.yellow;
                }
				else
                {
					//box.GetComponent<Renderer>().material.color = Color.red;
                    innerBox.color = Color.red;
                }
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
        if ((Input.GetButtonDown("AButton") || Input.GetMouseButtonDown(0)) && !selected)
        {
            press();
            if (unlocked)
            {
                Click();
                unlockSelect();
            }
            else
                lockSelect();
            selected = true;
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
        

        /*
        for(int i = 0; i < clickablePortPos.Length; i++)
        {
            if(clickablePortPos[i].x > menuCamera.WorldToViewportPoint(transform.position).x)
            {

            }
        }*/
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
    }

    private void lockSelect()
    {
        if (!selected)
        {
            // display are you sure text
        }else
        {
            purchaseItem();
        }
    }


    public void purchaseItem()
    {
        if(player.GetComponent<playerNew>().spendScraps(cost))
        {
            unlocked = true;
        }else
        {
            StartCoroutine("shake");
        }
    }

    private void press()
    {

    }
    IEnumerable shake()
    {
        yield return null;
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
