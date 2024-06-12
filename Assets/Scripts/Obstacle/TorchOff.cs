using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchOff : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Torch[] allTorches = FindObjectsOfType<Torch>();

            foreach (Torch torch in allTorches)
            {
                torch.isBurning = false;
            }
        }
    }
}
