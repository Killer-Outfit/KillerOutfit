using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O3M2 : MonoBehaviour
{
    public GameObject lightball;
    private GameObject ball;
    private Transform player;
    
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("PlayerBody").transform;
        ball = Instantiate(lightball, transform.position, Quaternion.identity, this.transform);
        ball.transform.Translate(2.5f, 0, 0);
        timer = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, Time.deltaTime * 200, 0);
        transform.position = new Vector3(player.position.x, player.position.y + 2, player.position.z);
        
        if (timer >= 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}