using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Cinemachine;

public class StoneObject : MonoBehaviour
{
    #region On Inspector Variables
    [SerializeField] float forcePower = 0f;
    [SerializeField] Transform playerPos;
    [SerializeField] GameObject[] startLights;
    [SerializeField] GameObject startEffects;

    #endregion

    #region Out Inspector Variables
    Rigidbody2D stoneRig;
    Animator animator;
    bool explosionEnd = false;
    SpriteRenderer sprite;
    #endregion

    void Start()
    {
        explosionEnd = false;
        gameObject.SetActive(true);
        stoneRig = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        for (int i = 0; i < startLights.Length; i++)
        {
            startLights[i].SetActive(false);
        }
        int randDir = Random.Range(0, 2); //

        animator.SetInteger("shakeDir", randDir);
    }



    public void Explosion()
    {
        if (startEffects != null)
        {
            startEffects.SetActive(true);
        }
        stoneRig.constraints = RigidbodyConstraints2D.None;
        Vector3 distance = transform.position - playerPos.position;
        distance = Vector3.Normalize(distance);
        distance.y = distance.y < 0 ? 0.3f : distance.y;


        stoneRig.AddForce(distance * forcePower, ForceMode2D.Impulse);
        animator.SetBool("isStart", true);
        for (int i = 0; i < startLights.Length; i++)
        {
            startLights[i].SetActive(false);
        }
        if (gameObject.name == "Tuto_1_Ground_St_12")
        {
            GetComponent<CinemachineImpulseSource>().GenerateImpulse(new Vector3(3, 3, 3));
        }
        StartCoroutine(CR_StoneErase(2));
        explosionEnd = true;
    }

    public void StartLight(int num)
    {
        startLights[num].SetActive(true);
        startLights[num].GetComponent<Light2D>().intensity = 0;
        StartCoroutine(CR_IntensityUp(startLights[num].GetComponent<Light2D>()));
    }

    IEnumerator CR_IntensityUp(Light2D light)
    {
        while (light.intensity <= 14)
        {
            light.intensity = Mathf.Lerp(light.intensity, 15, 0.4f * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator CR_StoneErase(float _time)
    {
        yield return new WaitForSeconds(_time);
        float progress = 1f;

        while (sprite.color.a > 0.1f)
        {
            sprite.color = new Color(1, 1, 1, progress);
            progress -= Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }

    IEnumerator CR_CameraShake()
    {
        while (!explosionEnd)
        {
            yield return new WaitForSeconds(0.1f);
            GetComponent<CinemachineImpulseSource>().GenerateImpulse();
            yield return null;
        }
    }

    public void StartCameraShake()
    {
        if (gameObject.name == "Tuto_1_Ground_St_12")
        {
            StartCoroutine(CR_CameraShake());
        }
    }
}
