using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamBlending : MonoBehaviour
{
    [Header("Cinemachines")]
    [SerializeField] private CinemachineVirtualCamera cam1;
    [SerializeField] private CinemachineVirtualCamera cam2;

    bool isChange = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            isChange = !isChange;
            if(isChange)
            {
                cam1.Priority = 10;
                cam2.Priority = 11;
            }
            else
            {
                cam1.Priority = 11;
                cam2.Priority = 10;
            }
        }
    }
}
