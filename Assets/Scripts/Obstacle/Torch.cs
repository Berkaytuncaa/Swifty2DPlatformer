using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    private bool _isBurning = false;
    private Animator _anim;
    [SerializeField] private float burnDelay;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        _anim.SetBool("isBurning", _isBurning);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !_isBurning)
        {
            StartCoroutine(StartBurningAfterDelay());
        }
        else if (collision.CompareTag("Player") && _isBurning)
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            player.Die();
            _isBurning = false;
        }
    }

    IEnumerator StartBurningAfterDelay()
    {
        // SFX will player here
        yield return new WaitForSeconds(burnDelay);
        _isBurning = true;
    }
}
