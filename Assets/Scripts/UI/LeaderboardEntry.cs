using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardEntry : MonoBehaviour
{
    InputField inputField;

    void Awake()
    {
        inputField = GetComponent<InputField>();
        inputField.onValueChanged.AddListener(delegate { Manage(); });
    }
    void Manage()
    {
        string text = inputField.text;
        if (text != inputField.text.ToUpper())
        {
            inputField.text = inputField.text.ToUpper();
        }
    }
}
