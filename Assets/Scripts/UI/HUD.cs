using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    private AudioManager audioManager;
    private bool _isPaused = false;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_isPaused)
        {
            Pause();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && _isPaused)
        {
            Resume();
        }
        else if(Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }

        if (_isPaused)
        {
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
        }
    }

    public void Pause()
    {
        _isPaused = true;
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
        _isPaused = false;
        audioManager.PlaySFX(audioManager.click);
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
}
