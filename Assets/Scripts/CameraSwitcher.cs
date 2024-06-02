using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera vcam1;
    [SerializeField] private CinemachineVirtualCamera vcam2;
    [SerializeField] private GameObject blockObject;

    void SwitchCamera()
    {
        if (vcam1.Priority > vcam2.Priority)
        {
            vcam1.Priority = 0;
            vcam2.Priority = 10;
            blockObject.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            vcam1.Priority = 10;
            vcam2.Priority = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SwitchCamera();
        }
    }
}
