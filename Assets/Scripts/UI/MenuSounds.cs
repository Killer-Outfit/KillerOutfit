using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSounds : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] menuHover;

    public AudioClip menuClick;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Hover()
    {
        AudioClip clip = GetMenuHover();
        audioSource.PlayOneShot(clip);
    }

    public void Click()
    {
        audioSource.PlayOneShot(menuClick);
    }

    private AudioClip GetMenuHover()
    {
        return menuHover[UnityEngine.Random.Range(0, menuHover.Length)];
    }
}
