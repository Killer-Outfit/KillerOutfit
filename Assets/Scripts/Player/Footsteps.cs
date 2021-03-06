﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] footstepSounds;

    private AudioSource audioSource;

    public AudioClip stomp;

    public float volume;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Step()
    {
        AudioClip clip = GetRandomClip();
        audioSource.PlayOneShot(clip, volume);
    }

    private void Stomp()
    {
        audioSource.PlayOneShot(stomp, volume);
    }

    private AudioClip GetRandomClip()
    {
        return footstepSounds[UnityEngine.Random.Range(0, footstepSounds.Length)];
    }
}
