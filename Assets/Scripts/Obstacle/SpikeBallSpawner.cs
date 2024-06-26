using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBallSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject spikeBallPrefab;
    [SerializeField] private float spawnDelay = 2.0f;
    private Vector2 _spawnPoint;

    private void Start()
    {
        _spawnPoint = transform.position;
        StartCoroutine(SpawnSpikeBalls());
    }

    private IEnumerator SpawnSpikeBalls()
    {
        while (true)
        {
            GameObject spikeBall = Instantiate(spikeBallPrefab, _spawnPoint, Quaternion.identity);

            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
