using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EsraParticle : MonoBehaviour
{
    [SerializeField] private ParticleSystem loveParticle;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            loveParticle.Play();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            loveParticle.Play();
        }
    }
}
