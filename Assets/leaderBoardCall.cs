using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leaderBoardCall : MonoBehaviour
{
    private GameObject player;
    public GameObject restartBut;
    public GameObject mainBut;
    public GameObject contBut;
    public GameObject entry;
    
    private void Awake()
    {
        player = GameObject.Find("PlayerBody");
    }
    void OnEnable()
    {
        int score = player.GetComponent<playerNew>().score;
        if (score > PlayerPrefs.GetInt("3rdScore", 0))
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

    public void finishEntry()
    {
        restartBut.SetActive(true);
        mainBut.SetActive(true);
        contBut.SetActive(true);
        entry.SetActive(false);
    }
}
