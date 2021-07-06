using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Manager.Sound;

public class Title : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cam1;
    [SerializeField] private CinemachineVirtualCamera cam2;
    [SerializeField] private GameObject[] startStones;
    [SerializeField] Animator midLight;
    private bool isStart = false;

    void Start()
    {
        SoundPlayer.instance.PlayBGM("Bg_1");
    }

    void Update()
    {
        if(Input.anyKey && !isStart)
        {
            SoundPlayer.instance.StopBGM();
            SoundPlayer.instance.PlayBGM("Bg_2");
            SoundPlayer.instance.PlaySound("UI");

            isStart = true;
            cam1.Priority = 10;
            cam2.Priority = 11;
            for(int i = 0; i < startStones.Length; i++)
            {
                startStones[i].GetComponent<StoneObject>().StartIntro();
                midLight.SetBool("isStart", true);
            }
        }
    }
}
