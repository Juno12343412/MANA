using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

enum BlendingType : byte
{
    In,
    Out
}


public class CamBlending : MonoBehaviour
{
    [Header("Cinemachines")]
    [SerializeField] private CinemachineVirtualCamera cam1;
    [SerializeField] private CinemachineVirtualCamera cam2;
    [SerializeField] private BlendingType type;
    [SerializeField] private GameObject reward;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            if(type == BlendingType.In)
            {
                cam1.Priority = 10;
                cam2.Priority = 11;
                if (reward != null) reward.SetActive(true);
            }
            else
            {
                cam1.Priority = 11;
                cam2.Priority = 10;
            }
        }
    }
}
