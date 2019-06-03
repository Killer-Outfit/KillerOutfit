using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopControl : MonoBehaviour
{
	
	private Camera maincam;
	private Camera shopcam;
    private Canvas can;
    private bool shopOpen;
    private GameObject shopResume;
    private GameObject resume;
    private GameObject player;
    public bool openable;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("PlayerBody");
        openable = false;
        can = GameObject.Find("Main Canvas").GetComponent<Canvas>();
        maincam = GameObject.Find("Main Camera").GetComponent<Camera>();
        shopcam = GameObject.Find("OutfitCamera").GetComponent<Camera>();
        shopResume = GameObject.Find("ResumeGame_ShopMenu (Button)");
        resume = GameObject.Find("ResumeGame (Button)");
        shopResume.SetActive(false);
        shopOpen = false;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("AButton") && openable)
		{
			maincam.enabled = !maincam.enabled;
            shopcam.enabled = !shopcam.enabled;
            can.enabled = !can.enabled;
            shopOpen = !shopOpen;
            player.GetComponent<playerMove>().active = !player.GetComponent<playerMove>().active;
            player.GetComponent<playerNew>().active = !player.GetComponent<playerNew>().active;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && shopOpen == true)
        {
            //Debug.Log("pause menu called in shop screen");
            if (maincam.enabled == false)
            {
                resume.SetActive(false);
                shopResume.SetActive(true);
                //Debug.Log("pause menu is open");
                maincam.enabled = true;
                shopcam.enabled = false;
                can.enabled = true;
            }
            else
            {
                resume.SetActive(true);
                shopResume.SetActive(false);
                //Debug.Log("pause menu is closed");
                maincam.enabled = false;
                shopcam.enabled = true;
                can.enabled = false;
            }
        }
    }
}
