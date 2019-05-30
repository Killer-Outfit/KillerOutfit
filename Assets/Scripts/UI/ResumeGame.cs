using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeGame : MonoBehaviour
{
    GameObject pauseMenu;
    private GameObject healthBar;
    private GameObject energyBar;
    private GameObject scrapCount;
    private GameObject score;
    private Camera maincam;
    private Camera shopcam;
    private Canvas can;

    // Start is called before the first frame update
    void Awake()
    {
        healthBar = GameObject.Find("Player Health");
        energyBar = GameObject.Find("Player Energy");
        score = GameObject.Find("Score");
        scrapCount = GameObject.Find("ScrapCount");
        pauseMenu = GameObject.Find("PauseMenuElements");
        can = GameObject.Find("Main Canvas").GetComponent<Canvas>();
        maincam = GameObject.Find("Main Camera").GetComponent<Camera>();
        shopcam = GameObject.Find("OutfitCamera").GetComponent<Camera>();
    }
    public void ContinueGame()
    {
        Debug.Log("resume button pressed");
        energyBar.SetActive(true);
        healthBar.SetActive(true);
        score.SetActive(true);
        scrapCount.SetActive(true);
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void ReturnToShopMenu()
    {
        energyBar.SetActive(true);
        healthBar.SetActive(true);
        score.SetActive(true);
        scrapCount.SetActive(true);
        maincam.enabled = false;
        shopcam.enabled = true;
        can.enabled = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }
}
