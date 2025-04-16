using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfinderApple : MonoBehaviour
{
    private bool _isCollected = false;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isCollected = true;
            _animator.SetBool("isCollected", _isCollected);
            StartCoroutine(WaitAndDestroy());
        }
    }

    private IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
