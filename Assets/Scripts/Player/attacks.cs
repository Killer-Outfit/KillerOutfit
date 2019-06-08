using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attacks : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] attackSounds;
    [SerializeField]
    private AudioClip[] attackGrunts;
    [SerializeField]
    private AudioClip[] attackLolita;
    [SerializeField]
    private AudioClip[] laserTaunt;




    public AudioClip laser;
    public AudioClip heart;
    public AudioClip wave;
    public AudioClip heartTaunt;

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

    private void Grunt()
    {
        AudioClip clip = GetRandomGrunt();
        audioSource.PlayOneShot(clip, volume);
    }

    private void Lolita()
    {
        AudioClip clip = GetRandomLolita();
        audioSource.PlayOneShot(clip, volume);
    }

    private void LaserTaunt()
    {
        AudioClip clip = GetRandomLaserTaunt();
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

    private void HeartTaunt()
    {
        audioSource.PlayOneShot(heartTaunt, volume);
    }

    private AudioClip GetRandomWhoosh()
    {
        return attackSounds[UnityEngine.Random.Range(0, attackSounds.Length)];
    }

    private AudioClip GetRandomGrunt()
    {
        return attackGrunts[UnityEngine.Random.Range(0, attackGrunts.Length)];
    }

    private AudioClip GetRandomLolita()
    {
        return attackLolita[UnityEngine.Random.Range(0, attackLolita.Length)];
    }

    private AudioClip GetRandomLaserTaunt()
    {
        return laserTaunt[UnityEngine.Random.Range(0, laserTaunt.Length)];
    }
}

