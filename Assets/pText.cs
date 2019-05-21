using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pText : MonoBehaviour
{
    private float speed;
    private float time;
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        speed = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0f, speed, 0f);
        time += Time.deltaTime;
        if(time > 10f)
        {
            Destroy(this);
        }
    }
}
