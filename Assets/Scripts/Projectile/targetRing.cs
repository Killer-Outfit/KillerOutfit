using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetRing : MonoBehaviour
{
    public bool laserSpawned;
    public GameObject explosionSphere;
    private float emissionVal = 500f;
    public bool spawnExplosionsB;
    private float t;
    // Start is called before the first frame update
    void Start()
    {
        t = 0f;
        spawnExplosionsB = false;
        laserSpawned = false;
        //GetComponent<ParticleSystem>().enableEmission = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnExplosionsB)
        {
            Debug.Log("it is true");
            t += Time.deltaTime;
        }
        if(t > .7f && spawnExplosionsB)
        {
            t = 0;
            Instantiate(explosionSphere, transform.position, transform.rotation);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.name);
        var emision = GetComponent<ParticleSystem>().emission;
        emision.rateOverTime = emissionVal;
    }
}
