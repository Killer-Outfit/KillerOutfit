using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public Mesh destroyedMesh;
    public ParticleSystem deathParticles;
    public float fadeDelay;
    public float fadeTime;

    public GameObject scrap;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void doBreak()
    {
        GetComponent<MeshFilter>().mesh = destroyedMesh;
        int droppedScraps = Random.Range(1, 3);
        for (int i = 0; i < droppedScraps; i++)
        {
            Instantiate(scrap, transform.position, Quaternion.identity);
        }
        StartCoroutine("DeathFade");
    }

    private IEnumerator DeathFade()
    {
        float alpha = GetComponent<Renderer>().material.color.a;
        yield return new WaitForSeconds(fadeDelay);
        for (float i = fadeTime; i > 0f; i -= Time.deltaTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, 0, i));
            GetComponent<Renderer>().material.color = newColor;
            yield return null;
        }
        Destroy(this.gameObject);
    }
}
