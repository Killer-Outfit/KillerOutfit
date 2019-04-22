using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ambientDelay : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] sounds;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        AudioClip clip = GetRandomClip();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            AudioClip clip = GetRandomClip();
            audioSource.clip = clip;
            audioSource.PlayDelayed(Random.Range(5, 10));
        }
    }

    private AudioClip GetRandomClip()
    {
        return sounds[UnityEngine.Random.Range(0, sounds.Length)];
    }
}
