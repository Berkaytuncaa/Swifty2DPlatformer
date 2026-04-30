using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableBlock : MonoBehaviour
{
    [SerializeField] private GameObject _destroyedPartA;
    [SerializeField] private GameObject _destroyedPartB;
    [SerializeField] private float _partLifetime = 2f;

    private bool _isBroken = false; // Guard flag

    // Called by manager on reset, cleanly restores state
    public void ResetBlock()
    {
        _isBroken = false;
        gameObject.SetActive(true);
    }

    private void BreakBlock()
    {
        if (_isBroken) return; // Prevent double-trigger
        _isBroken = true;
        
        StartCoroutine(DeactivateAfterDelay());
    }

    private void SpawnDestroyedParts()
    {
        SpawnPart(_destroyedPartA);
        SpawnPart(_destroyedPartB);
    }

    private void SpawnPart(GameObject prefab)
    {
        if (prefab == null) return;

        GameObject part = Instantiate(prefab, transform.position, transform.rotation);
        Rigidbody2D rb = part.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.AddForce(new Vector2(Random.Range(-3f, 3f), Random.Range(2f, 5f)), ForceMode2D.Impulse);

        Destroy(part, _partLifetime);
    }

    private IEnumerator DeactivateAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        SpawnDestroyedParts();
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            BreakBlock();
    }
}
