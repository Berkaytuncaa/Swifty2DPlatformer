using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterPlant : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject plantbulletPrefab;
    [SerializeField] private float spawnDelay = 2.0f;
    [SerializeField] private float yOffset = 0.5f;
    [SerializeField] private float xOffset = 0.3f;
    [SerializeField] private ParticleSystem poisonParticle;
    private Vector2 _spawnPoint;

    private void Start()
    {
        _spawnPoint = new Vector2(transform.position.x + xOffset, transform.position.y + yOffset);
        StartCoroutine(SpawnPlantBullets());
    }

    private IEnumerator SpawnPlantBullets()
    {
        while (true)
        {
            GameObject plantBullet = Instantiate(plantbulletPrefab, _spawnPoint, Quaternion.identity);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("PlantBullet"))
        {
            poisonParticle.Play();
            Destroy(collision.gameObject);
        }
    }
}
