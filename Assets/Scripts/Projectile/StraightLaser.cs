using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightLaser : MonoBehaviour
{
    private float maxLength;
    private float incriment;
    private float currentSize;
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("hey bitch");
        speed = 1;
        transform.Rotate(0, 180, 0);
        transform.position = new Vector3(transform.position.x, transform.position.y + 2.5f, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("hey bitches");
        transform.Translate(Vector3.forward * speed);
        /*if (currentSize < maxLength)
        {
            transform.localScale = new Vector3(1, currentSize, 1);
            currentSize += incriment;
        }*/
    }
}
