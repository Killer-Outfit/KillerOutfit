using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifeTime;
    public int damage;
    public bool hasHit;

    private float currentTime;
    

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Whores");
        hasHit = false;
        currentTime = 0.0f;
    }
    void Update()
    {
        //Debug.Log("Slutty Slut");
        if(currentTime < lifeTime)
        {
            currentTime += Time.deltaTime;
        }else
        {
            Destroy(this.gameObject);
        }
        
    }
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //Debug.Log("You Whore Im going to fuck your mom till she gives you a brother");
            hasHit = true;
            collision.gameObject.GetComponent<EnemyGeneric>().TakeDamage(damage, false);
        }
    }
}