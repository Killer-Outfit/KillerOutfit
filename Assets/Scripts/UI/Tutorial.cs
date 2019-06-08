using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public float[] tutorialPositions;
    [TextArea]
    public string[] tutorialMessages;

    private Camera mainCam;
    private GameObject player;
    private Text activeMessage;
    private GameObject messageBox;
    //private GameObject dialogueBackground;
    private int currentTutorialNum;
    private GameObject healthBar;
    private GameObject energyBar;
    private GameObject scrapCount;
    private GameObject score;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("PlayerBody");
        mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        currentTutorialNum = 0;
        messageBox = GameObject.Find("Tutorial");
        activeMessage = messageBox.transform.GetChild(0).GetComponent<Text>();
        //activeMessage.enabled = false;
        messageBox.SetActive(false);
        //dialogueBackground = GameObject.Find("Dialogue");
        //dialogueBackground.SetActive(false);
        healthBar = GameObject.Find("Player Health");
        energyBar = GameObject.Find("Player Energy");
        score = GameObject.Find("Score");
        scrapCount = GameObject.Find("ScrapCount");
    }

    // Update is called once per frame
    void Update()
    {
        if (tutorialPositions.Length > 0)
        {
            if (mainCam.transform.position.x >= tutorialPositions[currentTutorialNum])
            {
                DisplayTutorial(currentTutorialNum);
                //Debug.Log("current tutorial: " + currentTutorialNum);
            }
        }
    }

    private void DisplayTutorial(int num)
    {
        player.GetComponent<playerNew>().input = false;
        healthBar.SetActive(false);
        energyBar.SetActive(false);
        score.SetActive(false);
        scrapCount.SetActive(false);
        //dialogueBackground.SetActive(true);
        //activeMessage.enabled = true;
        messageBox.SetActive(true);
        activeMessage.text = tutorialMessages[num];
        Time.timeScale = 0;

        if (Input.GetButtonDown("AButton") || Input.GetKeyDown(KeyCode.Space) || Input.anyKey && Input.GetAxis("Horizontal") == 0)
        {
            player.GetComponent<playerNew>().input = true;
            //Debug.Log("tutorial acknowledged");
            Time.timeScale = 1;
            healthBar.SetActive(true);
            energyBar.SetActive(true);
            score.SetActive(true);
            scrapCount.SetActive(true);
            //dialogueBackground.SetActive(false);
            //activeMessage.enabled = false;
            messageBox.SetActive(false);
            currentTutorialNum += 1;
        }
        else if (Input.GetButtonDown("BButton") || Input.GetKeyDown(KeyCode.E))
        {
            player.GetComponent<playerNew>().input = true;
            //Debug.Log("tutorial acknowledged");
            Time.timeScale = 1;
            healthBar.SetActive(true);
            energyBar.SetActive(true);
            score.SetActive(true);
            scrapCount.SetActive(true);
            //dialogueBackground.SetActive(false);
            //activeMessage.enabled = false;
            this.GetComponent<Tutorial>().enabled = false;
            Destroy(messageBox);
        }
    }
}
