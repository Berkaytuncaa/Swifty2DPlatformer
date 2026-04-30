using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedBlockVFX : MonoBehaviour
{
    private Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        _animator.SetTrigger("Break");
    }
}
