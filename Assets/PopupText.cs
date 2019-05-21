using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupText : MonoBehaviour
{
    [HideInInspector]
    public float scallar;
    private float speed;

    [HideInInspector]
    public Vector3 targetPos;
    [HideInInspector]
    public string currentText;
    [HideInInspector]
    public float[] colorValues;
    // RED 212, 23, 19
    // PURPLE 148, 0, 224
    // YELLOW 255, 237, 0

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(scallar, scallar, scallar);
        colorValues = new float[3];
        scallar = 0f;
        Debug.Log(transform.position);
        speed = 1.1f;
        this.GetComponent<TextMesh>().text = currentText;
        colorValues[0] = 212f;
        colorValues[1] = 23f;
        colorValues[2] = 19f;
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(colorValues[0]);
        this.GetComponent<TextMesh>().color = new Color(colorValues[0], colorValues[1], colorValues[2]);
        transform.localScale += new Vector3(scallar, scallar, scallar);
    }
}
