using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Whistle : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] whistleSound;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Whistle()
    {
        AudioClip clip = GetRandomClip();
        audioSource.PlayOneShot(clip, 1);
    }

    private AudioClip GetRandomClip()
    {
        return whistleSound[UnityEngine.Random.Range(0, whistleSound.Length)];
    }
}
