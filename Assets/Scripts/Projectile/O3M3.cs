using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O3M3 : MonoBehaviour
{
    public GameObject spotlight;
    public float lifetime;

    private GameObject tmpSpot;
    private GameObject nearestEnemy;
    private float timer;
    private float hitInterval;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            SpawnLight(g);
        }

        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SpawnLight(GameObject target)
    {
        GameObject light = Instantiate(spotlight, transform.position, Quaternion.identity, null);
        light.GetComponent<O3Spotlight>().target = target;
        light.GetComponent<O3Spotlight>().damage = 4f;
    }
}