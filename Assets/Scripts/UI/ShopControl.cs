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
    private GameObject[] papCams;
    private int curFrame;
    private int nextFrameEvent;
    private int amountOfFlashes;
    private List<Color> flashColors;
    //private Canvas shopCanvas;
    // Start is called before the first frame update
    void Awake()
    {
        curFrame = 0;
        nextFrameEvent = 0;
        amountOfFlashes = 0;
        papCams = GameObject.FindGameObjectsWithTag("flash");
        player = GameObject.Find("PlayerBody");
        openable = false;
        can = GameObject.Find("Main Canvas").GetComponent<Canvas>();
        maincam = GameObject.Find("Main Camera").GetComponent<Camera>();
        shopcam = GameObject.Find("OutfitCamera").GetComponent<Camera>();
        shopResume = GameObject.Find("ResumeGame_ShopMenu (Button)");
        resume = GameObject.Find("ResumeGame (Button)");
        shopResume.SetActive(false);
        shopOpen = false;
        flashColors = new List<Color>();
        flashColors.Add(new Color(189f, 191f, 0f, 255f));
        flashColors.Add(new Color(0f, 111f, 191f, 255f));
        flashColors.Add(new Color(48f, 0f, 191f, 255f));
        flashColors.Add(new Color(255f, 255f, 255f, 255f));
        //shopCanvas = GameObject.Find("ShopCanvas").GetComponent<Canvas>();
    }


    // Update is called once per frame
    void Update()
    {
       
        Debug.Log(curFrame.ToString() + " " + nextFrameEvent.ToString());
        if (shopOpen && curFrame == nextFrameEvent)
        {
            Debug.Log("frames match");
            curFrame = 0;
            nextFrameEvent = Random.Range(15, 60);
            amountOfFlashes = Random.Range(1, 4);
            StartCoroutine("flashCameras");

        }
        else if (shopOpen)
        {
            curFrame += 1;
        }
        if (Input.GetButtonDown("AButton") && openable)
		{
			maincam.enabled = !maincam.enabled;
            shopcam.enabled = !shopcam.enabled;
            can.enabled = !can.enabled;
            shopOpen = !shopOpen;
            player.GetComponent<playerMove>().active = !player.GetComponent<playerMove>().active;
            player.GetComponent<playerNew>().active = !player.GetComponent<playerNew>().active;
            //shopCanvas.enabled = !shopCanvas.enabled;
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

    IEnumerator flashCameras()
    {
        for(int i = 0; i < amountOfFlashes; i++)
        {
            int flashIndex = Random.Range(0, papCams.Length);
            float snapAngle = Random.Range(87f, 121f);
            int colorIndex = Random.Range(0, flashColors.Count);
            float waitTime = Random.Range(.05f, .3f);
            Debug.Log("snapCam");
            papCams[flashIndex].GetComponent<paparaziCamera>().snap(flashColors[colorIndex], snapAngle);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
