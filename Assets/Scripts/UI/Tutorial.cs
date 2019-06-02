using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public float[] tutorialPositions;
    public string[] tutorialMessages;

    private Camera mainCam;
    private Text activeMessage;
    private int currentTutorialNum;
    private GameObject healthBar;
    private GameObject energyBar;
    private GameObject scrapCount;
    private GameObject score;

    // Start is called before the first frame update
    void Awake()
    {
        mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        currentTutorialNum = 0;
        activeMessage = GameObject.Find("TutorialText").GetComponent<Text>();
        activeMessage.enabled = false;
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
        healthBar.SetActive(false);
        energyBar.SetActive(false);
        score.SetActive(false);
        scrapCount.SetActive(false);
        activeMessage.enabled = true;
        activeMessage.text = tutorialMessages[num];
        Time.timeScale = 0;

        if (Input.GetButtonDown("AButton") || Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log("tutorial acknowledged");
            Time.timeScale = 1;
            healthBar.SetActive(true);
            energyBar.SetActive(true);
            score.SetActive(true);
            scrapCount.SetActive(true);
            activeMessage.enabled = false;
            currentTutorialNum += 1;
        }
    }
}
