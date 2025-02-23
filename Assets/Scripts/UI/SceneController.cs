using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// this script is being used by FinishPoint object
/// although the name is scene controller it will also handle other implementations
/// </summary>
public class SceneController : MonoBehaviour
{
    [SerializeField] private Animator transationAnim;

    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void NextLevel()
    {
        if (IsLastScene())
        {
            LoadHomeScreen();
        }
        else
        {
            StartCoroutine(LoadLevel());
        }
    }

    public void PlaySelectedChapter(int levelID)
    {
        String chapterName = "DEMOChapter" + levelID;
        StartCoroutine(OpenChapter());
        SceneManager.LoadScene(chapterName);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void SetDeathScreen()
    {
        StartCoroutine(DeathScreen());
    }

    IEnumerator LoadLevel()
    {
        transationAnim.SetTrigger("End");
        audioManager.PlaySFX(audioManager.transition);
        yield return new WaitForSeconds(1.3f);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        transationAnim.SetTrigger("Start");
    }

    IEnumerator DeathScreen()
    {
        transationAnim.SetTrigger("End");
        yield return new WaitForSeconds(1.3f);
        transationAnim.SetTrigger("Start");
    }

    IEnumerator OpenChapter()
    {
        transationAnim.SetTrigger("End");
        audioManager.PlaySFX(audioManager.transition);
        yield return new WaitForSeconds(1.3f);
        transationAnim.SetTrigger("Start");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UnlockNewLevel();
            NextLevel();

            Rigidbody2D playerRigidbody = collision.GetComponent<Rigidbody2D>();
            if (playerRigidbody != null)
            {
                playerRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX;
            }
        }
    }

    void UnlockNewLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetString("ChapterTime_" + (currentIndex - 1), Timer.instance.timerText.text);

        if (SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
        {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
        }
        PlayerPrefs.Save();
    }

    private bool IsLastScene()
    {
        return SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1;
    }

    private void LoadHomeScreen()
    {
        StartCoroutine(LoadHomeScreenCoroutine());
    }

    IEnumerator LoadHomeScreenCoroutine()
    {
        transationAnim.SetTrigger("End");
        audioManager.PlaySFX(audioManager.transition);
        yield return new WaitForSeconds(1.3f);
        SceneManager.LoadScene("HomeScreen");
        transationAnim.SetTrigger("Start");
    }
}
