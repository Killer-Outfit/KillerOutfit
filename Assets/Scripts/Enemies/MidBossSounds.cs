using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidBossSounds : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] jumpSounds;
    [SerializeField]
    private AudioClip[] chargeSounds;
    [SerializeField]
    private AudioClip[] slipSounds;
    [SerializeField]
    private AudioClip[] getupSounds;

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

    private void Charge()
    {
        AudioClip clip = GetRandomCharge();
        source.PlayOneShot(clip);
    }

    private void Slip()
    {
        AudioClip clip = GetRandomSlip();
        source.PlayOneShot(clip);
    }

    private void Getup()
    {
        AudioClip clip = GetRandomSlip();
        source.PlayOneShot(clip);
    }

    private AudioClip GetRandomJump()
    {
        return jumpSounds[UnityEngine.Random.Range(0, jumpSounds.Length)];
    }

    private AudioClip GetRandomCharge()
    {
        return chargeSounds[UnityEngine.Random.Range(0, chargeSounds.Length)];
    }

    private AudioClip GetRandomSlip()
    {
        return slipSounds[UnityEngine.Random.Range(0, slipSounds.Length)];
    }

    private AudioClip GetRandomGetup()
    {
        return getupSounds[UnityEngine.Random.Range(0, getupSounds.Length)];
    }
}
