using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class StartPoint : MonoBehaviour
{
    public GameObject player;
    public Camera ingameCamera;
    void Start()
    {
        player.transform.position = transform.position;
        ingameCamera.GetComponent<CinemachineBrain>().enabled = false;
        ingameCamera.transform.SetPositionAndRotation(transform.position, transform.rotation);
        ingameCamera.GetComponent<CinemachineBrain>().enabled = true;
    }

}
