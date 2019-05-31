using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandProjMove : MonoBehaviour
{
    private bool hit;
    private bool deathflag;
    private float timer;
    private Color fader;
    [HideInInspector]
    public int direction;
    [HideInInspector]
    public float damage;

    public GameObject hitParticle;

    public ParticleSystem emitter;

    // Start is called before the first frame update
    void Start()
    {
        //transform.Rotate(Vector3.right, 90);
        //transform.Rotate(Vector3.forward, 90);
        transform.Rotate(90, 90 * direction, 0, Space.Self);
        hit = false;
        deathflag = false;
        timer = 1;
        fader = GetComponent<MeshRenderer>().material.color;
        fader.a = 0;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3((float)direction / 10, 0, 0), Space.World);
        if(timer >= 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            if (deathflag == false)
            {
                deathflag = true;
                hit = true;
                DetachParticles();
                StartCoroutine("Fade");
            }
        }
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "Player" && hit == false)
        {
            hit = true;
            //c.gameObject.GetComponent<playerNew>().decreaseHealth(damage);
            GameObject p = Instantiate(hitParticle, transform.position, transform.rotation, null);
            p.transform.Rotate(0, 90, 0);
            var main = p.GetComponent<ParticleSystem>().main;
            main.startColor = new Color(255, 0, 0, 255);
            Debug.Log("Hit player");
            c.gameObject.GetComponent<playerNew>().decreaseHealth(10f);
        }
    }

    private IEnumerator Fade()
    {
        float fadeTimer = 0.5f;
        for(float i=0; i< fadeTimer; i += Time.deltaTime)
        {
            GetComponent<MeshRenderer>().material.color = Color.Lerp(GetComponent<MeshRenderer>().material.color, fader, fadeTimer * Time.deltaTime);
            yield return null;
        }
        Destroy(this.gameObject);
    }

    private void DetachParticles()
    {
        // This splits the particle off so it doesn't get deleted with the parent
        emitter.gameObject.transform.SetParent(null);

        ParticleSystem.EmissionModule em = emitter.emission;

        // this stops the particle from creating more bits
        em.enabled = false;

        Destroy(emitter.gameObject, 0.5f);
    }
}
