using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O3Spotlight : MonoBehaviour
{
    public float lifetime;

    [HideInInspector]
    public GameObject target;
    [HideInInspector]
    public float damage;

    private float timer;
    private float hitInterval;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
        hitInterval = 0.5f;
        transform.Rotate(new Vector3(90, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<Light>().spotAngle < 30)
        {
            GetComponent<Light>().spotAngle += Time.deltaTime * 100;
        }

        if(target != null)
        {
            transform.position = new Vector3(target.transform.position.x, target.transform.position.y + 10, target.transform.position.z);
            hitInterval += Time.deltaTime;
            if (hitInterval >= 0.5f)
            {
                target.GetComponent<EnemyGeneric>().TakeDamage(damage, false);
                hitInterval = 0f;
            }
        }

        timer += Time.deltaTime;
        if (timer > lifetime)
        {
            Destroy(this.gameObject);
        }
    }
}
