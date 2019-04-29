using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lazerSpawner : MonoBehaviour
{
    public Collider laser;
    public ParticleSystem target;
    private Vector3 pos;
    private float rotate;
    private List<ParticleSystem> targets;
    // Start is called before the first frame update
    void Start()
    {
        targets = new List<ParticleSystem>();
        transform.Rotate(180, 0, 0);

    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target.transform);
        Instantiate(laser, transform.position, transform.LookAt(target.transform));
    }
}
