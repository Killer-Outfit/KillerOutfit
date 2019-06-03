using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrapFloorHitter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider col)
    {
        if ((col.gameObject.name == "Plane" || col.gameObject.name == "Plane (1)"))
        {
            transform.parent.gameObject.GetComponent<Scraps>().stopFalling();
        }
    }

    public void activate()
    {
        this.GetComponent<BoxCollider>().enabled = true;
    }
}
