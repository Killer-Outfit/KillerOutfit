using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandProjMove : MonoBehaviour
{
    private bool hit;
    private float timer;
    private Color fader;
    [HideInInspector]
    public int direction;
    [HideInInspector]
    public float damage;

    // Start is called before the first frame update
    void Start()
    {
        hit = false;
        timer = 1;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3((float)direction/10, 0, 0));
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player" && hit == false)
        {
            hit = true;
            Debug.Log("Hit player");
            collision.gameObject.GetComponent<playerNew>().decreaseHealth(50f);
        }
    }
}
