using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Used in PopupUIText prefab
public class pText : MonoBehaviour
{
    private float speed;
    private float time;
    private float xSpeed;
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        speed = 2f;
        xSpeed = Random.Range(-1, 2);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(xSpeed, speed, 0f);
        time += Time.deltaTime;
        if(time > 10f)
        {
            Destroy(this);
        }
    }
}
