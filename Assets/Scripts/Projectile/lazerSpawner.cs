using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lazerSpawner : MonoBehaviour
{
    public Collider skyLaser;
    public Collider targetTracker;
    public ParticleSystem target;
    public AudioClip laserSpawn;
    //public AudioClip explosion;
    public AudioClip laserDestroy;

    private GameObject mainCam; 
    private Vector3 pos;
    private float rotate;
    private List<ParticleSystem> targets;
    private Vector2 xRange;
    private Vector2 zRange;
    private Vector3 targetSpawnPos;
    private Vector3 targetSpawnRot;
    private bool startLasers;
    private Quaternion targetRotation;
    private GameObject player;
    private AudioSource source;
    
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        player = GameObject.Find("PlayerBody");
        startLasers = false;
        targets = new List<ParticleSystem>();
        //transform.Rotate(180, 0, 0);
        mainCam = GameObject.Find("Main Camera");
        
        xRange = new Vector2(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x - 17.4f, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x + 17.4f);
        //Debug.Log(xRange);
        zRange = new Vector2(-4, 3);
        StartCoroutine("spawn");
    }

    // Update is called once per frame
    void Update()
    {
        /*if (startLasers)
        {
            var rTarget = Random.Range(0, 10);
            Vector3 spawnPos = new Vector3(targets[rTarget].transform.position.x, targets[rTarget].transform.position.y + 20f, targets[rTarget].transform.position.z);
            Collider las = Instantiate(skyLaser, spawnPos, transform.rotation);
        }*/
    }

    IEnumerator spawn()
    {
        transform.Rotate(90, 0f, 179.17f);
        
        for (int i = 0; i < 10; i++)
        {
            targetSpawnPos = new Vector3(Random.Range(xRange[0], xRange[1]), 0, Random.Range(zRange[0], zRange[1]));
            //Debug.Log(targetSpawnPos);
            source.PlayOneShot(laserSpawn);
            ParticleSystem ring = Instantiate(target, targetSpawnPos, transform.rotation);
            targets.Add(ring);
            yield return new WaitForSeconds(.1f);
        }
        StartCoroutine("spawnLaser");
    }

    IEnumerator spawnLaser()
    {
        for (int i = 0; i < targets.Count; i++)
        {
            Vector3 spawnPos = new Vector3(targets[i].transform.position.x, targets[i].transform.position.y + 20f, targets[i].transform.position.z);
            Collider las = Instantiate(skyLaser, spawnPos, transform.rotation);
            Vector3 tSpawnPos = new Vector3(targets[i].transform.position.x, targets[i].transform.position.y, targets[i].transform.position.z);
            targets[i].GetComponent<targetRing>().spawnExplosionsB = true;
            yield return new WaitForSeconds(.1f);
        }
    }

    void OnDestroy()
    {
        for(int i = 0; i < targets.Count; i++)
        {
            AudioSource.PlayClipAtPoint(laserDestroy, targets[i].gameObject.transform.position);
            Destroy(targets[i].gameObject);
        }
    }
}
