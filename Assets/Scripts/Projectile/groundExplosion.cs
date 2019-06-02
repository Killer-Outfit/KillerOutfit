using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundExplosion : MonoBehaviour
{
    private float fallSpeed;
    private float scaleAmount;
    private bool fall;
    private bool grow;
    private bool beginTimer;
    // Start is called before the first frame update
    void Start()
    {
        fallSpeed = 50f;
        fall = true;
        grow = false;
        beginTimer = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!beginTimer)
        {
            this.gameObject.GetComponent<Projectile>().currentTime = 0;
        }
        if (fall)
        {
            transform.Translate(new Vector3(0, -fallSpeed, 0), Space.World);
        }
        if (grow)
        {
            transform.localScale = new Vector3(transform.localScale.x + scaleAmount, transform.localScale.y + scaleAmount, transform.localScale.z + scaleAmount);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Plane"))
        {
            fall = false;
            grow = true;
            beginTimer = true;
            this.gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
