using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableBlock : MonoBehaviour
{
    public static DestroyableBlock Instance;

    // Static list that holds ALL block instances
    public static List<DestroyableBlock> AllBlocks = new List<DestroyableBlock>();

    [SerializeField] private GameObject _destroyedPartA;
    [SerializeField] private GameObject _destroyedPartB;
    [SerializeField] private float _partLifetime = 2f;
    void OnEnable()
    {
        AllBlocks.Add(this);
    }

    void OnDisable()
    {
        // Remove from list when disabled so list stays clean
        AllBlocks.Remove(this);
        SpawnDestroyedParts();
    }
    private void SpawnDestroyedParts()
    {
        if (_destroyedPartA != null)
        {
            GameObject partA = Instantiate(_destroyedPartA, transform.position, transform.rotation);
            Rigidbody2D rbA = partA.GetComponent<Rigidbody2D>();
            if (rbA != null)
                rbA.AddForce(new Vector2(Random.Range(-3f, 3f), Random.Range(2f, 5f)), ForceMode2D.Impulse);
            Destroy(partA, _partLifetime);
        }

        if (_destroyedPartB != null)
        {
            GameObject partB = Instantiate(_destroyedPartB, transform.position, transform.rotation);
            Rigidbody2D rbB = partB.GetComponent<Rigidbody2D>();
            if (rbB != null)
                rbB.AddForce(new Vector2(Random.Range(-3f, 3f), Random.Range(2f, 5f)), ForceMode2D.Impulse);
            Destroy(partB, _partLifetime);
        }
    }
    void OnDestroy()
    {
        AllBlocks.Remove(this);
    }

    private IEnumerator DestroyTheBlock()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(DestroyTheBlock());
        }
    }
}
