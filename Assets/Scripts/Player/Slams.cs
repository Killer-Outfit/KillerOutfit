using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slams : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] randomSlams;

    private AudioSource audioSource;

    
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (!audioSource.isPlaying)
            audioSource.volume = 1f;
    }

    private void KnockDown()
    {
        audioSource.volume = 0.4f;
        AudioClip clip = GetRandomClip();
        audioSource.PlayOneShot(clip);
    }

    private void GroundSlam()
    {
        AudioClip clip = randomSlams[2];
        audioSource.PlayOneShot(clip);
    }

    private AudioClip GetRandomClip()
    {
        return randomSlams[UnityEngine.Random.Range(0, randomSlams.Length)];
    }
}
