using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private Animator transationAnim;

    public void NextLevel()
    {
        StartCoroutine(LoadLevel());
    }

    public void SetDeathScreen()
    {
        StartCoroutine(DeathScreen());
    }

    IEnumerator LoadLevel()
    {
        transationAnim.SetTrigger("End");
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        transationAnim.SetTrigger("Start");
    }

    IEnumerator DeathScreen()
    {
        transationAnim.SetTrigger("End");
        yield return new WaitForSeconds(1);
        transationAnim.SetTrigger("Start");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            NextLevel();
        }
    }
}
