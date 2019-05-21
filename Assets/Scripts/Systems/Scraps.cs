using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scraps : MonoBehaviour
{
    private float currentTime;
    private float lifeTime;
    private float bufferTimer;
    private float rotate;

    private bool doFalling;
    private float gravity;
    private float YSpeed;
    private float XSpeed;
    private float ZSpeed;

    private Color color;
    private Renderer rn;
    private MaterialPropertyBlock mt;
    public GameObject popupText;

    // Start is called before the first frame update
    void Start()
    {
        rotate = 0;
        currentTime = 0.0f;
        lifeTime = 15f;
        bufferTimer = 0.5f;

        XSpeed = Random.Range(-2f, 2f) * Time.deltaTime;
        ZSpeed = Random.Range(-2f, 2f) * Time.deltaTime;
        YSpeed = 20 * Time.deltaTime;
        gravity = 0.8f * Time.deltaTime;
        doFalling = true;

        color = Random.ColorHSV(0,1,1,1,1,1,1,1);
        rn = GetComponent<Renderer>();
        mt = new MaterialPropertyBlock();
        rn.GetPropertyBlock(mt);
        mt.SetColor("_Color", color);
        mt.SetFloat("_Depth", 0.03f);
        rn.SetPropertyBlock(mt);
        GetComponent<TrailRenderer>().startColor = color;
        GetComponent<TrailRenderer>().endColor = color;
    }

    // Update is called once per frame
    void Update()
    {
        if(doFalling == true)
        {
            YSpeed -= gravity;
            transform.Translate(new Vector3(XSpeed, YSpeed, ZSpeed));
        }

        transform.rotation = Quaternion.Euler(0, rotate, 0);
        rotate += 1f;
        if (currentTime < lifeTime)
        {
            currentTime += Time.deltaTime;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(currentTime > bufferTimer)
        {
            if (col.gameObject.tag == "Player")
            {
                Instantiate(popupText, transform.position, Quaternion.identity);
                //col.gameObject.GetComponent<playerNew>().scraps += 1;
                Destroy(this.gameObject);
            }
            if (col.gameObject.name == "Plane" || col.gameObject.name == "Plane (1)")
            {
                doFalling = false;
            }
        }
    }
}
