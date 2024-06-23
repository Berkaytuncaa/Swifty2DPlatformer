using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("---------- Audio Source ----------")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource movementSource;

    [Header("---------- Audio Clip ----------")]
    public AudioClip background;
    public AudioClip click;
    public AudioClip movement;
    public AudioClip jump;
    public AudioClip death;
    public AudioClip transition;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlayMovementSFX()
    {
        if (!movementSource.isPlaying)
        {
            movementSource.clip = movement;
            movementSource.Play();
        }
    }

    public void StopMovementSFX()
    {
        if (movementSource.isPlaying)
        {
            movementSource.Stop();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
