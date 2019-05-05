using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attacks : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] attackSounds;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Whoosh()
    {
        AudioClip clip = GetRandomClip();
        audioSource.PlayOneShot(clip);
    }

    private AudioClip GetRandomClip()
    {
        return attackSounds[UnityEngine.Random.Range(0, attackSounds.Length)];
    }
}

