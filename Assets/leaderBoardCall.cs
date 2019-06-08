using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leaderBoardCall : MonoBehaviour
{
    private GameObject player;
    private GameObject restartBut;
    private GameObject mainBut;
    private GameObject contBut;
    private GameObject entry;
    
    private void Start()
    {
        player = GameObject.Find("PlayerBody");
        restartBut = GameObject.Find("Restart (Button)");
        mainBut = GameObject.Find("Main Menu (Button)");
        contBut = GameObject.Find("Continue (Button)");
        entry = GameObject.Find("New Leaderboard Entry");
    }
    void OnEnable()
    {
        int score = player.GetComponent<playerNew>().score;
        if(score > PlayerPrefs.GetInt("3rdScore", 0))
        {
            restartBut.SetActive(false);
            mainBut.SetActive(false);
            contBut.SetActive(false);
            entry.SetActive(true);
        }
        else
        {
            restartBut.SetActive(true);
            mainBut.SetActive(true);
            contBut.SetActive(true);
            entry.SetActive(false);
        }
    }
}
