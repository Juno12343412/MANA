using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Cinemachine;

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
    [Header("Next Map Start Tf")]
    [SerializeField] private Transform nextMapStartTf = null;

    //포탈 밝기 동작 범위
    [Header("Max Portal Glow")]
    [SerializeField] [Range(0,30.0f)] private float inPortalDistance = 10;

    //기본 포탈 밝기
    [Header("Deafult Portal Glow")]
    [SerializeField] [Range(0,30.0f)] private float deafultPortalGlow = 10;


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
        if (Vector3.Distance(playerTf.position, nowPortalTf.position) <= inPortalDistance) // 일정 거리 안에 들어오면 포탈 빛 점점 밝아지는 곳
        {
            portalLight.intensity = (inPortalDistance + deafultPortalGlow) - Vector3.Distance(playerTf.position, nowPortalTf.position);
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
            sceneChangefade.GetComponent<Animator>().SetTrigger("SceneChange");
            nowMapObject.SetActive(false);
            nextMapObject.SetActive(true);
            playerTf.position = nextMapStartTf.position;
            ingameCamera.GetComponent<CinemachineBrain>().enabled = false;
            ingameCamera.transform.SetPositionAndRotation(nextMapStartTf.position, nextMapStartTf.rotation);
            ingameCamera.GetComponent<CinemachineBrain>().enabled = true;

        }
    }
}
