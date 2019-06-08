using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VolumetricLines;

public class skyLazer : MonoBehaviour
{
    private float speed;
    private BoxCollider skyLaserCollider;
    private VolumetricLineBehavior vmLines;
    public GameObject explosionSphere;

    // Start is called before the first frame update
    void Start()
    {
        skyLaserCollider = this.gameObject.GetComponent<BoxCollider>();
        vmLines = this.gameObject.GetComponent<VolumetricLineBehavior>();
        speed = 50f;
        //Instantiate(explosionSphere, transform.position, transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        skyLaserCollider.size = new Vector3(skyLaserCollider.size.x, skyLaserCollider.size.y, skyLaserCollider.size.z + speed * 2);
        vmLines.EndPos = new Vector3(vmLines.EndPos.x, vmLines.EndPos.y, vmLines.EndPos.z + speed);
        Vector3 pos = new Vector3(transform.position.x, 0f, transform.position.z);
        //Instantiate(explosionSphere, pos, transform.rotation);
    }
}
