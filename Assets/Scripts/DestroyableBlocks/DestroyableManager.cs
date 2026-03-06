using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableManager : MonoBehaviour
{
    private List<DestroyableBlock> allBlocksIncludingInactive = new List<DestroyableBlock>();

    void Start()
    {
        // FindObjectsOfType with true flag includes inactive objects
        DestroyableBlock[] blocks = FindObjectsOfType<DestroyableBlock>(true);
        allBlocksIncludingInactive.AddRange(blocks);
    }

    public void OnPlayerDied()
    {
        // Reactivate ALL blocks, including currently inactive ones
        foreach (DestroyableBlock block in allBlocksIncludingInactive)
        {
            block.gameObject.SetActive(true);
        }
    }
}
