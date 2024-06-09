using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class SetPlayerCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera vcam;

    void SetVcamPriority()
    {
        vcam.Priority = 20;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SetVcamPriority();
        }
    }
}
