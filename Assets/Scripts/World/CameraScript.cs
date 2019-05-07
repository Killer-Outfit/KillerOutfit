﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject player;
    private Vector3 curPlayerPortPos;
    public Camera mainCam;

    public float smoothSpeed = 0.125f;

    // Cam shake
    private Transform camTransform;
    public float shakeDuration = 0f;
    public float shakeAmount = 0.2f;
    public float decreaseFactor = 6f;

    [HideInInspector]
    public bool locked;
    // Start is called before the first frame update
    void Start()
    {
        locked = false;
    }

    // Get the transform 
    private void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        curPlayerPortPos = mainCam.WorldToViewportPoint(player.transform.position);
        //Debug.Log(curPlayerPortPos);
        if((curPlayerPortPos.x >= .5f) && !locked)
        {
            Vector3 desiredPos = new Vector3(player.transform.position.x, mainCam.transform.position.y, mainCam.transform.position.z);
            Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
            transform.position = smoothedPos;
            //mainCam.transform.position = new Vector3(player.transform.position.x, mainCam.transform.position.y, mainCam.transform.position.z);
            //Debug.Log("on other half");
        }
    }

    public void doShake(float duration)
    {
        shakeDuration = duration;
        shakeAmount = 0.2f;
        StartCoroutine("Shake");
    }

    private IEnumerator Shake()
    {
        Vector3 originalPos = camTransform.localPosition;
        while(shakeDuration > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
            shakeAmount -= Time.deltaTime * decreaseFactor;
            if(shakeAmount < 0)
            {
                shakeAmount = 0;
            }
            shakeDuration -= Time.deltaTime;
            yield return null;
        }
        shakeDuration = 0f;
        camTransform.localPosition = originalPos;
    }
}
