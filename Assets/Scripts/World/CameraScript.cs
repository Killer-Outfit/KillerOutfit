using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private GameObject player;
    private Vector3 curPlayerPortPos;
    private Camera mainCam;

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
        mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Get the transform 
    private void Awake()
    {
        player = GameObject.Find("PlayerBody");
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
        shakeAmount = 0.5f;
        StartCoroutine("Shake");
    }

    private IEnumerator Shake()
    {
        Vector3 originalPos = camTransform.localPosition;
        Quaternion originalRot = camTransform.rotation;
        while(shakeDuration > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
            camTransform.Rotate(new Vector3(0, 0, Random.Range(-shakeAmount, shakeAmount)));
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
        camTransform.rotation = originalRot;
    }

    public void revive()
    {
        Vector3 revivePos = new Vector3(player.transform.position.x + 29.705215f, 2.09f, -14.63f);
        transform.position = revivePos;
    }
}
