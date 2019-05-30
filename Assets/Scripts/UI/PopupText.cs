using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Used for the PopupScoreText prefab
public class PopupText : MonoBehaviour
{/*
    private float speedY;
    private float speedX;
    private float time;
    private string currentText;

    void Start()
    {
        currentText = "needNum";
        speedY = .1f;
        speedX = -.005f;
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(currentText);
        transform.Translate(speedX, speedY, 0f);
        time += Time.deltaTime;
        if (time > 2f)
        {
            currentText = "";
            Destroy(this.gameObject);
        }
    }

    public void assignText(string dispText)
    {
        currentText = dispText;
        GetComponent<TextMesh>().text = currentText;
    }*/
    private float speed;
    private float time;

    void Start()
    {
        speed = 1f;
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Get the location of the UI element you want the 3d onject to move towards
        Vector3 screenPoint = GameObject.Find("Score").transform.position + new Vector3(0, 0, 5);  //the "+ new Vector3(0,0,5)" ensures that the object is so close to the camera you dont see it

        //find out where this is in world space
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPoint);

        //move towards the world space position
        transform.position = Vector3.MoveTowards(transform.position, worldPos, speed);
        time += Time.deltaTime;
        if (time > 2f || transform.position == worldPos)
        {
            //GameObject.Find("PlayerBody").GetComponent<playerNew>().scraps += 1;
            //GameObject.Find("PlayerBody").GetComponent<playerNew>().scraps += 1;
            Destroy(this.gameObject);
        }
    }
    public void assignText(string dispText)
    {
        GetComponent<TextMesh>().text = dispText;
    }
}
