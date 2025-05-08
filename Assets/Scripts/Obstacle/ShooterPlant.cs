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
    private bool _canShoot = true;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _spawnPoint = new Vector2(transform.position.x + xOffset, transform.position.y + yOffset);
    }
    private void Update()
    {
        if (_canShoot)
        {
            StartCoroutine(ShootRoutine());
        }
    }

    private IEnumerator ShootRoutine()
    {
        _canShoot = false;

        _animator.SetTrigger("Shoot");

        yield return new WaitForSeconds(0.5f);

        _spawnPoint = new Vector2(transform.position.x + xOffset, transform.position.y + yOffset);

        Instantiate(plantbulletPrefab, _spawnPoint, Quaternion.identity);

        if (poisonParticle != null)
        {
            poisonParticle.Play();
        }

        yield return new WaitForSeconds(spawnDelay);

        _canShoot = true;
    }

}
