using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UDBase.Controllers.ObjectSystem;
using UDBase.Controllers.LogSystem;
using UDBase.Controllers.ParticleSystem;
using UDBase.Utils;
using Cinemachine;
using MANA.Enums;
using Zenject;

enum BossState
{
    Idle,
    Groggy,
    Pattern1,
    Pattern2,
    Pattern3,
    HitWall,
    Dead,
    End
}


public class BossAi : MonoBehaviour
{

    [SerializeField] private BoxCollider2D _PatternCollider1;
    [SerializeField] private BoxCollider2D _PatternCollider2;
    [SerializeField] private BoxCollider2D _PatternCollider3;
    [SerializeField] private Material _HitMaterial;
    [Header("Boss Stats")]
    [SerializeField] private float _hp = 50.0f;
    [SerializeField] private float _Speed = 5.0f;
    [SerializeField] private float _delay = 0.0f;

    Animator _animtor;
    SpriteRenderer _renderer;
    Rigidbody2D _rigid;
    private float maxHp;
    private float hp;
    private float speed;
    private Material _OriginMaterail;
    Vector3 _hurtDir = Vector3.zero;
    GameObject targetObj;
    BossState state = BossState.Idle;
    BossState prevState = BossState.Idle;
    bool pattern1Dis = false;
    int groggyCount = 0;

    void Start()
    {
        _OriginMaterail = GetComponent<SpriteRenderer>().material;
        targetObj = GameObject.Find("Player");

        _animtor = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
        _rigid = GetComponent<Rigidbody2D>();

        hp = maxHp = _hp;
        speed = _Speed;
        state = BossState.Idle;
        Idle();
    }

    void Update()
    {
        if (hp <= 0)
        {
            hp = 0;
            state = BossState.Dead;
            DeadEvent();
        }

    }


    void Idle()
    {
        _animtor.SetBool("isIdle", true);
        _animtor.SetInteger("Pattern", 0);
        _animtor.SetBool("isGroggy", false);
        StartCoroutine(CR_ChangePattern(_delay));
        Debug.Log("기본자세");
    }
    void Groggy()
    {
        _animtor.SetBool("isIdle", false);
        _animtor.SetInteger("Pattern", 0);
        _animtor.SetBool("isGroggy", true);
        Debug.Log("그로기");
        StartCoroutine(CR_Test());
    }

    void Pattern1()
    {
        _animtor.SetBool("isIdle", false);
        _animtor.SetInteger("Pattern", 1);
        _animtor.SetBool("isGroggy", false);

        prevState = BossState.Pattern1;
        groggyCount++;
        Debug.Log("패턴1");
        StartCoroutine(CR_Test());
    }

    void Pattern2()
    {
        _animtor.SetBool("isIdle", false);
        _animtor.SetInteger("Pattern", 2);
        _animtor.SetBool("isGroggy", false);

        prevState = BossState.Pattern2;
        groggyCount++;
        Debug.Log("패턴2");
        StartCoroutine(CR_Test());
    }

    void Pattern3()
    {
        _animtor.SetBool("isIdle", false);
        _animtor.SetInteger("Pattern", 3);
        _animtor.SetBool("isGroggy", false);

        prevState = BossState.Pattern3;
        groggyCount++;
        Debug.Log("패턴3");
        StartCoroutine(CR_Test());
    }
    IEnumerator EnemyErase(float _time)
    {
        yield return new WaitForSeconds(_time);
        Destroy(gameObject, _time);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Attack") && state != BossState.HitWall)
        {
            Hurt(other.gameObject);
        }
        if (state == BossState.Pattern2 && other.gameObject.CompareTag("Wall"))
        {
            HitWall();
        }
        if (other.gameObject.CompareTag("Player"))
        {
            pattern1Dis = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            pattern1Dis = false;
        }
    }

    void Hurt(GameObject obj)
    {
        GetComponent<CinemachineImpulseSource>().GenerateImpulse();

        obj.SetActive(false);

        hp -= 1;
        StartCoroutine(CR_HitFx());
        if (state != BossState.Dead)
        {
            StartCoroutine(TimeUtils.TimeStop(0.05f));
        }
    }

    IEnumerator CR_HitFx()
    {
        float a = 1;
        while (a > 0)
        {
            GetComponent<SpriteRenderer>().material = _HitMaterial;
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, a);
            a -= 0.1f;
            yield return new WaitForSeconds(0.001f);
        }
        GetComponent<SpriteRenderer>().material = _OriginMaterail;
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }

    void HitWall()
    {
        state = BossState.HitWall;
        StartCoroutine(CR_ChangePattern(0.3f));
    }


    public void TurnBody()
    {
        if (targetObj.transform.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);

        }
        else if (targetObj.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);

        }
        _hurtDir = transform.localScale;
    }

    void DeadEvent()
    {
        Vector2 _dir = new Vector2(-_hurtDir.x, 5f);

        _rigid.velocity = Vector2.zero;
        _rigid.AddForce(_dir * speed / 2f, ForceMode2D.Impulse);
        GetComponent<BoxCollider2D>().enabled = false;

        StartCoroutine(EnemyErase(3f));
    }

    IEnumerator CR_ChangePattern(float _time)
    {
        if (hp <= 0)
        {
            hp = 0;
            state = BossState.Dead;
            yield return null;
        }
        yield return new WaitForSeconds(_time);
        if (state == BossState.HitWall)
        {
            state = BossState.Idle;
            Idle();
        }
        else if (groggyCount == 6)
        {
            groggyCount = 0;
            state = BossState.Groggy;
            Groggy();
        }
        else if (pattern1Dis)
        {
            if (prevState != BossState.Pattern1)
            {
                state = BossState.Pattern1;
                Pattern1();
            }
            else
            {
                state = BossState.Pattern2;
                Pattern2();
            }
        }
        else
        {
            if (prevState != BossState.Pattern2)
            {
                state = BossState.Pattern2;
                Pattern2();
            }
            else
            {
                state = BossState.Pattern3;
                Pattern3();
            }
        }
    }

    IEnumerator CR_Test()
    {
        yield return new WaitForSeconds(_delay);
        FrameEnd();
    }
    public void FrameEnd()
    {
        //_PatternCollider1.enabled = false;
        //_PatternCollider2.enabled = false;
        //_PatternCollider3.enabled = false;
        state = BossState.Idle;
        Idle();
        //애니메이터 변수 초기화;
    }
}
