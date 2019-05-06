using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickable : MonoBehaviour
{
    public bool selected;
    private Renderer rend;
    private string type;
    private GameObject player;
    private bool rotate;
    private float originalY;
    private bool up;
    private GameObject menuModel;
    private GameObject[] clickables;
    private Camera menuCamera;
    private Camera mainCamera;
    public bool hoverB;


    public bool unlocked;
    public GameObject outfitDisplay;
    public outfits item;
    public int cost;
    public GameObject box;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        menuCamera = GameObject.Find("OutfitCamera").GetComponent<Camera>();
        clickables = GameObject.FindGameObjectsWithTag("Clickable");
        up = true;
        originalY = outfitDisplay.transform.position.y;
        rotate = false;
        rend = GetComponent<Renderer>();
        rend.enabled = false;
        type = item.outfitType;
        player = GameObject.Find("PlayerBody");
        menuModel = GameObject.Find("OutfitModel");
        Debug.Log(type);
    }

    // Update is called once per frame
    void Update()
    {
        if (unlocked)
        {
            if (selected)
            {
                box.GetComponent<Renderer>().material.color = Color.blue;
            }
            else
            {
                box.GetComponent<Renderer>().material.color = Color.green;
            }
        } else
        {
            if (selected)
            {
                box.GetComponent<Renderer>().material.color = Color.yellow;
            }
            else
            {
                box.GetComponent<Renderer>().material.color = Color.red;
            }
        }

        if (rotate)
        {
            outfitDisplay.transform.eulerAngles = new Vector3(0, outfitDisplay.transform.eulerAngles.y + 1f, 0);
        }
        else
        {
            outfitDisplay.transform.eulerAngles = new Vector3(outfitDisplay.transform.eulerAngles.x, 0, 0);
        }

        if(outfitDisplay.transform.eulerAngles.y < originalY + 3f && up)
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

        if (Input.GetButtonDown("BButton"))
        {
            menuCamera.enabled = false;
            mainCamera.enabled = true;
        }

        if (hoverB)
        {
            hover();
        }
    }

    void OnMouseOver()
    {
        rotate = true;
        rend.enabled = true;
        if (Input.GetMouseButtonDown(0) && !selected)
        {
            press();
            if(unlocked)
                unlockSelect();
            else
                lockSelect();
            selected = true;
        }
    }
    public void hover()
    {
        rotate = true;
        rend.enabled = true;
        if (Input.GetButtonDown("AButton") && !selected)
        {
            press();
            if (unlocked)
                unlockSelect();
            else
                lockSelect();
            selected = true;
        }
    }
    void OnMouseExit()
    {
        rotate = false;
        rend.enabled = false;
    }

    private void unlockSelect()
    {
        player.GetComponent<playerNew>().changeOutfit(item);
        menuModel.GetComponent<menuCharacter>().changeOutfit(item);
        for(int i = 0; i < clickables.Length; i++)
        {
            Debug.Log(clickables[i]);
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
}
