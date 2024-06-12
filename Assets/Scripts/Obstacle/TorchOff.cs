using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchOff : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Find all Torch objects in the scene
            Torch[] allTorches = FindObjectsOfType<Torch>();

            // Set isBurning to false for all Torch objects
            foreach (Torch torch in allTorches)
            {
                torch.isBurning = false;
            }
        }
    }
}
