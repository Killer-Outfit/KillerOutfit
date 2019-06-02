using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O2MExplosion : MonoBehaviour
{
    public float maxScale = 2;
    public float growSpeed = 1;
    public float growAccel = 0.95f;
    public float explodeSpeed = 1;
    public float explodeAccel = 1.05f;
    public float lifetime = 1;

    private bool hit;
    private float currentExplode;
    private Renderer render;
    private float delayTimer;

    // Start is called before the first frame update
    void Start()
    {
        hit = false;
        currentExplode = 0;
        render = GetComponent<Renderer>();
        delayTimer = 0;

        transform.Rotate(new Vector3(0, Random.Range(-360, 360), 0));
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(this.gameObject);
        }

        if (transform.localScale.x < maxScale)
        {
            growSpeed = growSpeed * growAccel;
            transform.localScale = transform.localScale * (1.005f + (growSpeed * Time.deltaTime));
        }

        if (currentExplode < 1)
        {
            explodeSpeed = explodeSpeed * explodeAccel;
            currentExplode += explodeSpeed * Time.deltaTime;
        }
        render.material.SetFloat("_DissolveAmount", currentExplode);
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "Enemy" && hit == false)
        {
            hit = true;
            c.gameObject.GetComponent<EnemyGeneric>().TakeDamage(10f, false);
        }
    }
}
