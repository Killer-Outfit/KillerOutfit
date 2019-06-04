using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O1M3 : MonoBehaviour
{
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = 5;
        foreach( GameObject e in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            e.GetComponent<EnemyGeneric>().TakeDamage(10f, true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
