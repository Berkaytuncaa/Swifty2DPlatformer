using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableManager : MonoBehaviour
{
    // Serialized so can assign blocks in the Inspector,
    // avoiding expensive FindObjectsOfType at runtime
    [SerializeField] private List<DestroyableBlock> _allBlocks = new List<DestroyableBlock>();

    void Start()
    {
        // Fallback: auto-find if not manually assigned in Inspector
        if (_allBlocks.Count == 0)
        {
            DestroyableBlock[] found = FindObjectsOfType<DestroyableBlock>(true);
            _allBlocks.AddRange(found);
        }
    }

    public void OnPlayerDied()
    {
        foreach (DestroyableBlock block in _allBlocks)
        {
            block.ResetBlock(); // Uses the clean reset method, not raw SetActive
        }
    }
}
