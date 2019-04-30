﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skyLazer : MonoBehaviour
{
    private float maxLength;
    private float incriment;
    private float currentSize;
    private float speed;
    private float rotate;

    // Start is called before the first frame update
    void Start()
    {
        /*maxLength = 140.0f;
        incriment = 7f;
        currentSize = 0.0f;
       // transform.rotation = new Vector3(90, 0, 0);
        transform.Rotate(90, 0, 0);
        */
        speed = 1;
        rotate = Random.Range(-120f, -60f);
        transform.Rotate(rotate, 180 + rotate, 0);
        transform.position = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed);
        /*if (currentSize < maxLength)
        {
            transform.localScale = new Vector3(1, currentSize, 1);
            currentSize += incriment;
        }*/
    }
}
