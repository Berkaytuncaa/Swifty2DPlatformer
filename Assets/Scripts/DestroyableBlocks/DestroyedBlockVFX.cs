using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedBlockVFX : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rb;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        _animator.SetTrigger("Break");

        // Collide only with Platform layer
        int platformLayer = LayerMask.NameToLayer("Platforms");
        Physics2D.IgnoreLayerCollision(gameObject.layer, gameObject.layer);
        for (int i = 0; i < 32; i++)
        {
            if (i != platformLayer)
            {
                Physics2D.IgnoreLayerCollision(gameObject.layer, i);
            }
        }
    }
}
