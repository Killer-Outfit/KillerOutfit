using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardEntry : MonoBehaviour
{
    private string[] letters;
    private bool stickInputAccepted;
    public bool hover;
    private void Start()
    {
        letters = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        stickInputAccepted = false;   
    }
    private void Update()
    {
        if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
        {
            stickInputAccepted = true;
        }
        if ((Input.GetButtonDown("AButton") || Input.GetMouseButtonDown(0)))
        {
        }
    }
}
