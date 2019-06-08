using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightball : MonoBehaviour
{

    private float hitTimer;

    // Start is called before the first frame update
    void Start()
    {
        hitTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(hitTimer >= 0)
        {
            hitTimer -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "Enemy" && hitTimer <= 0)
        {
            c.gameObject.GetComponent<EnemyGeneric>().TakeDamage(5f, false);
            hitTimer = 0.2f;
        }
    }
}
