using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lazerSpawner : MonoBehaviour
{
    public Collider laser;
    public Collider targetTracker;
    public ParticleSystem target;
    private GameObject mainCam; 
    private Vector3 pos;
    private float rotate;
    private List<ParticleSystem> targets;
    private Vector2 xRange;
    private Vector2 zRange;
    private Vector3 targetSpawnPos;
    private Vector3 targetSpawnRot;
    private bool startLasers;
    // Start is called before the first frame update
    void Start()
    {
        startLasers = false;
        targets = new List<ParticleSystem>();
        //transform.Rotate(180, 0, 0);
        mainCam = GameObject.Find("Main Camera");
        
        xRange = new Vector2(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x - 7.4f, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x + 7.4f);
        Debug.Log(xRange);
        zRange = new Vector2(-4, 3);
        StartCoroutine("spawn");
    }

    // Update is called once per frame
    void Update()
    {
        if (startLasers)
        {
            //transform.LookAt(targets[1].transform.position, transform.down);
            //Quaternion.Inverse(
            Collider las = Instantiate(laser, transform.position, transform.rotation);
            las.transform.LookAt(targets[1].transform.position);
        }
    }

    IEnumerator spawn()
    {
        transform.Rotate(-90, 0f, 179.17f);
        for (int i = 0; i < 20; i++)
        {
            targetSpawnPos = new Vector3(Random.Range(xRange[0], xRange[1]), 0, Random.Range(zRange[0], zRange[1]));
            Debug.Log(targetSpawnPos);
            ParticleSystem ring = Instantiate(target, targetSpawnPos, transform.rotation);
            targets.Add(ring);
            yield return new WaitForSeconds(.1f);
        }
        startLasers = true;

    }
}
