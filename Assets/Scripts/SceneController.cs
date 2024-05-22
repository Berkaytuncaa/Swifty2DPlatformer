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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            NextLevel();

            Rigidbody2D playerRigidbody = collision.GetComponent<Rigidbody2D>();
            if (playerRigidbody != null)
            {
                playerRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX;
            }
        }
    }
}
