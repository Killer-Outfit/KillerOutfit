using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseSound : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] loseSound;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play()
    {
        AudioClip clip = GetLoseSound();
        audioSource.PlayOneShot(clip);
    }

    private AudioClip GetLoseSound()
    {
        return loseSound[UnityEngine.Random.Range(0, loseSound.Length)];
    }
}
