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

    private Animator _animator;
    private Vector2 _spawnPoint;
    private Coroutine _shootingCoroutine;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _spawnPoint = new Vector2(transform.position.x + xOffset, transform.position.y + yOffset);
    }

    private IEnumerator ShootRoutine()
    {
        while (true)
        {
            _animator.SetTrigger("Shoot");

            yield return new WaitForSeconds(0.5f);

            Instantiate(plantbulletPrefab, _spawnPoint, Quaternion.identity);

            if (poisonParticle != null)
            {
                poisonParticle.Play();
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && _shootingCoroutine == null)
        {
            _shootingCoroutine = StartCoroutine(ShootRoutine());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && _shootingCoroutine != null)
        {
            StopCoroutine(_shootingCoroutine);
            _shootingCoroutine = null;
        }
    }
}
