using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnterName : MonoBehaviour
{
    private string playerName;
    private string maxName;
    private GameObject textEntry;
    private GameObject player;
    private LeaderBoard leaderBoard;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("PlayerBody");
        textEntry = GameObject.Find("InputText");
        playerName = "";
        maxName = "";
        leaderBoard = GameObject.Find("LeaderBoard").GetComponent<LeaderBoard>();
    }

    // Update is called once per frame
    void Update()
    {
        if(textEntry.GetComponent<Text>().text.Length == 6)
        {
            maxName = textEntry.GetComponent<Text>().text;
        }
        if(textEntry.GetComponent<Text>().text.Length > 6)
        {
            textEntry.GetComponent<Text>().text = maxName;
        }
    }
    
    public void setName()
    {
        playerName = textEntry.GetComponent<Text>().text;
        //PlayerPrefs.SetString("CurrentPlayerName", playerName);
        int score = player.GetComponent<playerNew>().score;
        if (score > PlayerPrefs.GetInt("1stScore", 0))
        {
            leaderBoard.updateLeaderBoard("1st", score, playerName);
        }
        else if (score > PlayerPrefs.GetInt("2ndScore", 0))
        {
            leaderBoard.updateLeaderBoard("2nd", score, playerName);
        }
        else if (score > PlayerPrefs.GetInt("3rdScore", 0))
        {
            leaderBoard.updateLeaderBoard("3rd", score, playerName);
        }
        GameObject.Find("GameOverElements").GetComponent<leaderBoardCall>().finishEntry();
    }
}
