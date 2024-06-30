using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
    [Header("---------- Audio Source ----------")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource movementSource;

    [Header("---------- Chapter Clips ----------")]
    public AudioClip mainMenuMusic;
    public AudioClip surfaceMorning;
    public AudioClip surfaceEvening;
    public AudioClip cave;
    public AudioClip chipy;

    [Header("---------- Player Clips ----------")]
    public AudioClip movement;
    public AudioClip jump;
    public AudioClip death;

    [Header("---------- UI Clips ----------")]
    public AudioClip transition;
    public AudioClip click;

    [Header("---------- Obstacle Clips ----------")]
    public AudioClip spikeBall;

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
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        PlayBackgroundMusic(mainMenuMusic);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Adjust the music which we want to play in chapters.
        switch (scene.name)
        {
            case "MainMenu":
                SetBackgroundMusic(mainMenuMusic);
                break;
            case "Chapter1":
            case "Chapter2":
            case "Chapter3":
            case "Chapter4":
                SetBackgroundMusic(surfaceMorning);
                break;
            case "Chapter7":
                SetBackgroundMusic(surfaceEvening);
                break;
            case "Chapter8":
            case "Chapter9":
                SetBackgroundMusic(cave);
                break;
            case "Chapter100":
                SetBackgroundMusic(chipy);
                break;
            default:
                SetBackgroundMusic(mainMenuMusic);
                break;
        }
    }

    public void SetBackgroundMusic(AudioClip clip)
    {
        if (musicSource.clip != clip)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
    }

    public void PlayBackgroundMusic(AudioClip clip)
    {
        if (musicSource.clip != clip)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
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
