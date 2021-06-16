using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//맵의 원근감을 위해 배경 스프라이트의 위치를 조정해주는 스크립트
public class BackgroundMove : MonoBehaviour
{
    #region On Inspector Variables
    //Camera
    [Header("- Camera")]
    [SerializeField] private Camera ingameCamera = null;

    //Far-Sprite
    [Header("- FarSprite")]
    [SerializeField] private GameObject[] far_Resource = null;
    [Range(0.0f , 5.0f)]
    [SerializeField] private float far_MoveSpeed = 0;

    //Mid-Sprite
    [Header("- MidSprite")]
    [SerializeField] private GameObject[] mid_Resource = null;
    [Range(0.0f, 5.0f)]
    [SerializeField] private float mid_MoveSpeed = 0;

    //Near-Sprite
    [Header("- NearSprite")]
    [SerializeField] private GameObject[] near_Resource = null;
    [Range(0.0f, 5.0f)]
    [SerializeField] private float near_MoveSpeed = 0;
    #endregion

    #region Out Inspector Variables
    private Transform cameraT;
    private Transform[] farTf;
    private Transform[] midTf;
    private Transform[] nearTf;
    private Vector3[] farFirst;
    private Vector3[] midFirst;
    private Vector3[] nearFirst;
    #endregion

    void Start()
    {
        cameraT = ingameCamera.transform;
        if (far_Resource.Length > 0)
        {
            farTf = new Transform[far_Resource.Length];
            farFirst = new Vector3[far_Resource.Length];
            for (int i = 0; i < far_Resource.Length; i++)
            {
                farTf[i] = far_Resource[i].transform;
                farFirst[i] = farTf[i].position;
            }
        }
        if (mid_Resource.Length > 0)
        {
            midTf = new Transform[mid_Resource.Length];
            midFirst = new Vector3[mid_Resource.Length];
            for (int i = 0; i < mid_Resource.Length; i++)
            {
                midTf[i] = mid_Resource[i].transform;
                midFirst[i] = midTf[i].position;
            }
        }
        if (near_Resource.Length > 0)
        {
            nearTf = new Transform[near_Resource.Length];
            nearFirst = new Vector3[near_Resource.Length];
            for (int i = 0; i < near_Resource.Length; i++)
            {
                nearTf[i] = near_Resource[i].transform;
                nearFirst[i] = nearTf[i].position;
            }
        }
    }
    void Update()
    {
        if (far_Resource.Length > 0)
        {
            for (int i = 0; i < far_Resource.Length; i++)
            {
                float x = cameraT.position.x + (farFirst[i].x - cameraT.position.x) * far_MoveSpeed;
                //farTf[i].position = Vector3.Lerp(farTf[i].position, new Vector3(x, farTf[i].position.y, 0), Time.deltaTime);
                farTf[i].position = new Vector3(x, farTf[i].position.y, 0);
            }
        }
        if (mid_Resource.Length > 0)
        {
            for (int i = 0; i < mid_Resource.Length; i++)
            {
                float x = cameraT.position.x + (midFirst[i].x - cameraT.position.x) * mid_MoveSpeed;

                //midTf[i].position = Vector3.Lerp(midTf[i].position, new Vector3(x, midTf[i].position.y, 0), Time.deltaTime);
                midTf[i].position = new Vector3(x, midTf[i].position.y, 0);
            }
        }
        if (near_Resource.Length > 0)
        {
            for (int i = 0; i < near_Resource.Length; i++)
            {
                float x = cameraT.position.x + (nearFirst[i].x - cameraT.position.x) * near_MoveSpeed;
                //nearTf[i].position = Vector3.Lerp(nearTf[i].position, new Vector3(x, nearTf[i].position.y, 0), Time.deltaTime);
                nearTf[i].position = new Vector3(x, nearTf[i].position.y, 0);
            }
        }
    }
}
