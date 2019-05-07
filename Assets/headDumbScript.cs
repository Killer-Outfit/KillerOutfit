using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class headDumbScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x != -22.2f)
        {
            transform.position = new Vector3(-22.2f, transform.position.y, transform.position.z);
        }
    }
}
