using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Torch : MonoBehaviour
{
    public bool isBurning = false;
    private Animator _anim;
    [SerializeField] private GameObject torchLight;
    [SerializeField] private float burnDelay;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        torchLight.SetActive(false);
    }

    private void Update()
    {
        _anim.SetBool("isBurning", isBurning);

        if (!isBurning)
        {
            torchLight.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isBurning)
        {
            StartCoroutine(StartBurningAfterDelay());
        }
        else if (collision.CompareTag("Player") && isBurning)
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            player.Die();
            isBurning = false;
            torchLight.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isBurning)
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            player.Die();
            isBurning = false;
            torchLight.SetActive(false);
        }
    }

    IEnumerator StartBurningAfterDelay()
    {
        // SFX will player here
        yield return new WaitForSeconds(burnDelay);
        isBurning = true;
        torchLight.SetActive(true);
    }
}
