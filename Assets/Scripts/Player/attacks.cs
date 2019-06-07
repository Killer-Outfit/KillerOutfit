using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attacks : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] attackSounds;

    public AudioClip laser;
    public AudioClip heart;
    public AudioClip wave;

    private AudioSource audioSource;

    public float volume;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Whoosh()
    {
        AudioClip clip = GetRandomWhoosh();
        audioSource.PlayOneShot(clip, volume);
    }

    private void Wave()
    {
        audioSource.PlayOneShot(wave, volume);
    }

    private void Laser()
    { 
        audioSource.PlayOneShot(laser, volume);
    }

    private void Heart()
    {
        audioSource.PlayOneShot(heart, volume);
    }

    private AudioClip GetRandomWhoosh()
    {
        return attackSounds[UnityEngine.Random.Range(0, attackSounds.Length)];
    }
}

