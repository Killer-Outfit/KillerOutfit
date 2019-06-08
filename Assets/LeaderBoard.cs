using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour
{
    private Text firstText;
    [HideInInspector]
    public string firstName;
    [HideInInspector]
    public int firstScore;
    private Text secondText;
    [HideInInspector]
    public string secondName;
    [HideInInspector]
    public int secondScore;
    private Text thirdText;
    [HideInInspector]
    public string thirdName;
    [HideInInspector]
    public int thirdScore;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("hello");
        firstText = transform.GetChild(0).gameObject.GetComponent<Text>();
        secondText = transform.GetChild(1).gameObject.GetComponent<Text>();
        thirdText = transform.GetChild(2).gameObject.GetComponent<Text>();

        firstName = PlayerPrefs.GetString("1stName", "");
        secondName = PlayerPrefs.GetString("2ndName", "");
        thirdName = PlayerPrefs.GetString("3rdName", "");

        firstScore = PlayerPrefs.GetInt("1stScore", 0);
        secondScore = PlayerPrefs.GetInt("2ndScore", 0);
        thirdScore = PlayerPrefs.GetInt("3rdScore", 0);

        if (firstScore > 0)
        {
            firstText.text = "1st: " + firstName + " " + firstScore.ToString();
        }
        else
        {
            firstText.text = "1st: ";
        }

        if (secondScore > 0)
        {
            secondText.text = "2nd: " + secondName + " " + secondScore.ToString();
        }
        else
        {
            secondText.text = "2nd: ";
        }

        if (thirdScore > 0)
        {
            thirdText.text = "3rd: " + thirdName + " " + thirdScore.ToString();
        }
        else
        {
            thirdText.text = "3rd: ";
        }
    }

    public void updateLeaderBoard(string tag, int score, string playerName)
    {
        int tempScore = PlayerPrefs.GetInt(tag + "Score", 0);
        string tempName = PlayerPrefs.GetString(tag + "Name", "");
        PlayerPrefs.SetInt(tag + "Score", score);
        PlayerPrefs.SetString(tag + "Name", playerName);
        if(tag == "1st")
        {
            firstText.text = "1st: " + PlayerPrefs.GetString("1stName", "") + " " + PlayerPrefs.GetInt("1stScore", 0).ToString();
            updateLeaderBoard("2nd", tempScore, tempName);
        }else if (tag == "2nd")
        {
            secondText.text = "2nd: " + PlayerPrefs.GetString("2ndName", "") + " " + PlayerPrefs.GetInt("2ndScore", 0).ToString();
            updateLeaderBoard("3rd", tempScore, tempName);
        }
        else if (tag == "3rd")
        {
            thirdText.text = "3rd: " + PlayerPrefs.GetString("3rdName", "") + " " + PlayerPrefs.GetInt("3rdScore", 0).ToString();
        }
    }
}
