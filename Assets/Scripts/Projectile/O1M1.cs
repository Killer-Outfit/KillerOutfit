using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O1M : MonoBehaviour
{
    private bool hit;
    private bool deathflag;
    private float timer;
    [HideInInspector]
    public float damage;

    public GameObject hitParticle;

    public ParticleSystem burster;

    // Start is called before the first frame update
    void Start()
    {
        hit = false;
        deathflag = false;
        timer = 5;
        damage = 20;
        transform.Rotate(0, 180, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward/9);
        if (timer >= 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            if (deathflag == false)
            {
                deathflag = true;
                hit = true;
                Burst();
            }
        }
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "Enemy" && hit == false)
        {
            hit = true;
            GameObject p = Instantiate(hitParticle, transform.position, transform.rotation, null);
            p.transform.Rotate(0, 90, 0);
            c.gameObject.GetComponent<EnemyGeneric>().TakeDamage(10f, true);
            Burst();
        }
    }

    private void Burst()
    {
        // This splits the particle off so it doesn't get deleted with the parent
        burster.gameObject.transform.SetParent(null);

        ParticleSystem.EmissionModule em = burster.emission;

        // this stops the particle from creating more bits
        em.enabled = true;

        Destroy(burster.gameObject, 0.5f);
        Destroy(this.gameObject);
    }
}
