using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetRing : MonoBehaviour
{
    private float emissionVal = 500f;
    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<ParticleSystem>().enableEmission = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision collision)
    {
        var emision = GetComponent<ParticleSystem>().emission;
        emision.rateOverTime = emissionVal;

    }
}
