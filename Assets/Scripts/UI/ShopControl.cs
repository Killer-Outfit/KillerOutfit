using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopControl : MonoBehaviour
{
	
	private Camera maincam;
	private Camera shopcam;
    private Canvas can;

    // Start is called before the first frame update
    void Start()
    {
        can = GameObject.Find("Main Canvas").GetComponent<Canvas>();
        maincam = GameObject.Find("Main Camera").GetComponent<Camera>();
        shopcam = GameObject.Find("OutfitCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("BButton"))
		{
			maincam.enabled = !maincam.enabled;
            shopcam.enabled = !shopcam.enabled;
            can.enabled = !can.enabled;
		}
    }
}
