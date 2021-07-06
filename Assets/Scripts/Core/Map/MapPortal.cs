using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Cinemachine;
using Manager.Sound;

//일정 범위안에 들어오면 다음맵으로 넘어가게 해주는 포탈 스크립트
public class MapPortal : MonoBehaviour
{
    #region On Inspector Variables
    //플레이어 오브젝트
    [Header("Player Object")]
    [SerializeField] private GameObject playerObject = null;

    //인게임 카메라
    [Header("Ingame Camera")]
    [SerializeField] private GameObject ingameCamera = null;

    //지금 포탈 오브젝트
    [Header("Now Portal Object")]
    [SerializeField] private GameObject nowPortalObject = null;
    
    //현재맵과 다음맵과 다음맵 플레이어 시작위치 정해주는 변수들
    [Header("Now Map Object")]
    [SerializeField] private GameObject nowMapObject = null;
    [Header("Next Map Object")]
    [SerializeField] private GameObject nextMapObject = null;
   

    [Header("Max Portal Glow")]
    [SerializeField] [Range(0,500.0f)] private float maxGlow = 10;

    //기본 포탈 밝기
    [Header("Deafult Portal Glow")]
    [SerializeField] [Range(0,500.0f)] private float deafultPortalGlow = 10;


    #endregion


    #region Out Inspector Variables
    private Transform playerTf = null;
    private Transform nowPortalTf = null;
    private Light2D portalLight = null;
    private GameObject sceneChangefade = null;
    #endregion

    void Start()
    {
        playerTf = playerObject.transform;
        nowPortalTf = nowPortalObject.transform;
        portalLight = GetComponent<Light2D>();
        portalLight.intensity = 3;
        sceneChangefade = GameObject.Find("SceneChangeFade");
    }

    void Update()
    {
        if (Vector3.Distance(playerTf.position, nowPortalTf.position) <= 15) // 일정 거리 안에 들어오면 포탈 빛 점점 밝아지는 곳
        {
            portalLight.intensity = deafultPortalGlow + maxGlow - (Vector3.Distance(playerTf.position, nowPortalTf.position) / 15) * maxGlow;
        }
        else // 기본 포탈 밝기 
        {
            portalLight.intensity = deafultPortalGlow;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            if (nowMapObject.name == "Tuto_2_Map")
            {
                SoundPlayer.instance.StopBGM();
                SoundPlayer.instance.PlayBGM("Bg_3");
            }
            else if (nowMapObject.name == "Tuto_3_Map")
            {
                SoundPlayer.instance.StopBGM();
                SoundPlayer.instance.PlayBGM("Bg_4");
            }
            else if (nowMapObject.name == "Tuto_4_Map")
            {
                SoundPlayer.instance.StopBGM();
                SoundPlayer.instance.PlayBGM("Bg_5");
            }
            else if (nowMapObject.name == "Tuto_5_Map")
            {
                SoundPlayer.instance.StopBGM();
                SoundPlayer.instance.PlayBGM("Bg_6");
            }

            sceneChangefade.GetComponent<Animator>().SetTrigger("SceneChange");
            nowMapObject.SetActive(false);
            nextMapObject.SetActive(true);
        }
    }
}
