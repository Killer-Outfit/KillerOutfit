using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopControl : MonoBehaviour
{
	
	private Camera maincam;
	private Camera shopcam;
    private Canvas can;
    private GameObject player;
    public bool openable;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("PlayerBody");
        openable = false;
        can = GameObject.Find("Main Canvas").GetComponent<Canvas>();
        maincam = GameObject.Find("Main Camera").GetComponent<Camera>();
        shopcam = GameObject.Find("OutfitCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("AButton") && openable)
		{
			maincam.enabled = !maincam.enabled;
            shopcam.enabled = !shopcam.enabled;
            can.enabled = !can.enabled;
            player.GetComponent<playerMove>().active = !player.GetComponent<playerMove>().active;
            player.GetComponent<playerNew>().active = !player.GetComponent<playerNew>().active;
        }
    }
}
