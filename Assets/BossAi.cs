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
    [SerializeField] private float hp = 5.0f;
    [SerializeField] private float _Speed = 5.0f;
    [SerializeField] private float _delay = 1.0f;
    [SerializeField] private float _groggyDelay = 2.0f;
    [Header("Boss Signal")]
    [SerializeField] private GameObject wallHitSignal;

    Animator _animtor;
    SpriteRenderer _renderer;
    Rigidbody2D _rigid;
    private float speed;
    private Material _OriginMaterail;
    Vector3 _hurtDir = Vector3.zero;
    GameObject targetObj;
    BossState state = BossState.Idle;
    BossState prevState = BossState.Idle;
    bool pattern1Dis = false;
    int groggyCount = 0;
    bool pattern3End = false;
    

    Vector3 start;
    Vector3 target;
    float heightArc;
    float bestY = -10;
    bool search = false;

    void Start()
    {
        _OriginMaterail = GetComponent<SpriteRenderer>().material;
        targetObj = GameObject.Find("Player");

        _animtor = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
        _rigid = GetComponent<Rigidbody2D>();

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
        state = BossState.Idle;
        TurnBody();
        _animtor.SetBool("isIdle", true);
        StartCoroutine(CR_ChangePattern(_delay));
    }
    void Groggy()
    {
        state = BossState.Groggy;
        groggyCount = 0;

        _animtor.SetBool("isIdle", false);
        _animtor.SetBool("isGroggy", true);
        StartCoroutine(CR_Groggy());
    }

    void Pattern1()
    {
        state = BossState.Pattern1;

        TurnBody();
        _animtor.SetBool("isIdle", false);
        _animtor.SetInteger("Pattern", 1);

        prevState = BossState.Pattern1;
        groggyCount++;
    }

    void Pattern2()
    {
        state = BossState.Pattern2;

        TurnBody();
        _animtor.SetBool("isIdle", false);
        _animtor.SetInteger("Pattern", 2);

        prevState = BossState.Pattern2;
        groggyCount++;
    }

    void Pattern3()
    {
        state = BossState.Pattern3;

        TurnBody();
        _animtor.SetBool("isIdle", false);
        _animtor.SetInteger("Pattern", 3);

        pattern3End = true;
        start = new Vector3(transform.position.x , -6.35658f, 1);
        target = new Vector3(targetObj.transform.position.x, start.y, 1);
        heightArc = Vector3.Distance(start, target) * 0.65f;
        bestY = -10.0f;
        search = false;

        prevState = BossState.Pattern3;
        groggyCount++;
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

    private void OnTriggerStay2D(Collider2D other)
    {
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
        wallHitSignal.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
        _animtor.SetBool("isIdle", false);
        _animtor.SetInteger("Pattern", 0);
        _animtor.SetBool("isGroggy", false);
        state = BossState.HitWall;
        _animtor.SetBool("isWallHit", true);
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
        state = BossState.Dead;
        _animtor.SetBool("isDead", true);
    }

    IEnumerator CR_ChangePattern(float _time)
    {
        yield return new WaitForSeconds(_time);
        if (state == BossState.Dead)
            yield return null;
        if (state == BossState.HitWall)
        {
            Idle();
        }
        else if (groggyCount == 6)
        {
            Groggy();
        }
        else if (pattern1Dis)
        {
            if (prevState != BossState.Pattern1)
            {
                Pattern1();
            }
            else
            {
                Pattern2();
            }
        }
        else
        {
            if (prevState != BossState.Pattern2)
            {
                Pattern2();
            }
            else
            {
                Pattern3();
            }
        }
    }

    IEnumerator CR_Groggy()
    {
        yield return new WaitForSeconds(_groggyDelay);
        FrameEnd();
    }
    public void FrameEnd()
    {
        _animtor.SetBool("isIdle", false);
        _animtor.SetInteger("Pattern", 0);
        _animtor.SetBool("isGroggy", false);
        _animtor.SetBool("isWallHit", false);
        _animtor.SetBool("isFall", false);
        _animtor.SetBool("Landing", false);
        state = BossState.Idle;
        Idle();
    }

    public void Pattern2Move()
    {
        StartCoroutine(CR_Pattern2Move());
    }

    IEnumerator CR_Pattern2Move()
    {
        float acc = 1.01f;
        while (state == BossState.Pattern2)
        {
            transform.position += new Vector3(-_hurtDir.x, 0, 0) * speed * acc * Time.deltaTime;
            acc += 0.03f;
            yield return null;
        }
    }

    void FixedUpdate()
    {
        if (pattern3End)
        {
            float x0 = start.x;
            float x1 = target.x;
            float distance = x1 - x0;
            float nextX = Mathf.MoveTowards(transform.position.x, x1, 15 * Time.deltaTime);
            float baseY = Mathf.Lerp(start.y, target.y, (nextX - x0) / distance);
            float arc = heightArc * (nextX - x0) * (nextX - x1) / (-0.25f * distance * distance);
            Vector3 nextPosition = new Vector3(nextX, baseY + arc, 1);
            transform.position = nextPosition;

            if (transform.position.y > bestY)
            {
                bestY = transform.position.y;
            }
            else if (search == false)
            {
                _animtor.SetInteger("Pattern", 0);
                _animtor.SetBool("isFall", true);
                search = true;
            }
            if(transform.position.y == start.y  && search == true)
            {
                _animtor.SetBool("Landing", true);
                transform.position = new Vector3(target.x, start.y, 1);
                wallHitSignal.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
                pattern3End = false;
            }
        }
    }
}
