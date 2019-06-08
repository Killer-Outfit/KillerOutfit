using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidBossSounds : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] jumpSounds;

    private AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void Jump()
    {
        AudioClip clip = GetRandomJump();
        source.PlayOneShot(clip);
    }

    private AudioClip GetRandomJump()
    {
        return jumpSounds[UnityEngine.Random.Range(0, jumpSounds.Length)];
    }
}
