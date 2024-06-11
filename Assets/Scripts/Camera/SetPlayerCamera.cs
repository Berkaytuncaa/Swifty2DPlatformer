using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class SetPlayerCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera vcam;
    [SerializeField] private GameObject cameraSwitcher;
    [SerializeField] private GameObject blockObject;

    void SetVcamPriority()
    {
        vcam.Priority = 20;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SetVcamPriority();
            cameraSwitcher.SetActive(true);
            blockObject.SetActive(false);
        }
    }
}
