using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipyScaler : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.GetComponent<Transform>().localScale = new Vector3(5, 5, 0);
        }
    }
}
