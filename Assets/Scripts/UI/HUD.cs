using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void Pause()
    {
        audioManager.PlaySFX(audioManager.click);
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void Home()
    {
        audioManager.PlaySFX(audioManager.click);
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void Restart()
    {
        audioManager.PlaySFX(audioManager.click);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void Resume()
    {
        audioManager.PlaySFX(audioManager.click);
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
}
