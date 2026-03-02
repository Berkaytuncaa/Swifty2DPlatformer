using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableBlock : MonoBehaviour
{
    public static DestroyableBlock Instance;

    void Awake()
    {
        Instance = this;
    }
    /* TODO: with this script we want to achieve: 
        - On collision block will be set active(false) and will create 2 prefabs,
        those prefabs will be affected by gravity
        - on Player Dead those blocks will be active again THERE IS AN ERROR HERE:
        WHEN PLAYER DIES ONLY ONE OF THE OBJECTS BECOMING ACTIVE OTHERS STAYS INACTIVE
        - prefabs will Destroy(gameobject) after a while 1,5f maybe
        - perhaps player will bounce off of the block
    */
    private IEnumerator DestroyTheBlock()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }

    public void OnPlayerDied()
    {
        gameObject.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(DestroyTheBlock());
        }
    }
}
