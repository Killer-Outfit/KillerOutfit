using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickable : MonoBehaviour
{
    private bool unlocked;
    private bool selected;
    private Renderer rend;
    private string type;
    private GameObject player;
    private bool rotate;
    private float originalY;
    private bool up;

    public GameObject outfitDisplay;
    public outfits item;
    public int cost;
    public GameObject box;
    // Start is called before the first frame update
    void Start()
    {
        up = true;
        originalY = outfitDisplay.transform.position.y;
        rotate = false;
        rend = GetComponent<Renderer>();
        rend.enabled = false;
        unlocked = false;
        selected = false;
        type = item.outfitType;
        player = GameObject.Find("PlayerBody");
    }

    // Update is called once per frame
    void Update()
    {
        if (unlocked)
        {
            if (selected)
                box.GetComponent<Renderer>().material.color = Color.blue;
            else
                box.GetComponent<Renderer>().material.color = Color.green;
        }else
        {
            if (selected)
                box.GetComponent<Renderer>().material.color = Color.yellow;
            else
                box.GetComponent<Renderer>().material.color = Color.red;
        }

        if (rotate)
        {
            outfitDisplay.transform.eulerAngles = new Vector3(0, outfitDisplay.transform.eulerAngles.y + .5f, 0);
        }
        else if(outfitDisplay.transform.rotation.y % 360 != 0)
        {
            outfitDisplay.transform.eulerAngles = new Vector3(0, outfitDisplay.transform.eulerAngles.y + .5f, 0);
        }

        if(outfitDisplay.transform.position.y < originalY + 3f && up)
        {
            outfitDisplay.transform.position = new Vector3(0.0f, outfitDisplay.transform.position.y + 0.5f, 0.0f);
        }else if(outfitDisplay.transform.position.y == originalY + 3f)
        {
            up = false;
        }

        if (outfitDisplay.transform.position.y < originalY - 3f && !up)
        {
            outfitDisplay.transform.position = new Vector3(0.0f, outfitDisplay.transform.position.y - 0.5f, 0.0f);
        }
        else if (outfitDisplay.transform.position.y == originalY - 3f)
        {
            up = true;
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

    void OnMouseExit()
    {
        rotate = false;
        rend.enabled = false;
    }

    private void unlockSelect()
    {
        player.GetComponent<playerNew>().changeOutfit(item);
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
