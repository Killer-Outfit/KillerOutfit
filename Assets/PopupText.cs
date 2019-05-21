using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupText : MonoBehaviour
{
    private float speedY;
    private float speedX;
    private float time;
    [HideInInspector]
    public string currentText;

    void Start()
    {
        currentText = "needNum";
        speedY = .03f;
        speedX = -.005f;
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(currentText);
        GetComponent<TextMesh>().text = currentText;
        transform.Translate(speedX, speedY, 0f);
        time += Time.deltaTime;
        if (time > 2f)
        {
            currentText = "";
        }
    }
}
