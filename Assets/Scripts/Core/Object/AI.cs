using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDBase.Controllers.ObjectSystem;
using UDBase.Controllers.LogSystem;
using Cinemachine;
using MANA.Enums;

public class AI : AIMachine
{
    ULogger _log;
    Animator _animtor;
    SpriteRenderer _renderer;
    Rigidbody2D _rigid;

    [SerializeField] private GameObject _attackCollider;

    Vector3 _startPosition = Vector3.zero;
    Vector3 _moveDir = Vector3.zero;

    protected sealed override void AISetting(ILog log)
    {
        _log = log.CreateLogger(this);
        _log.Message("AI 셋팅");

        _animtor = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
        _rigid = GetComponent<Rigidbody2D>();
            
        _startPosition = transform.position;

        base.AISetting(log);

        Kind = ObjectKind.Enemy;
        MyStats.CurHP = MyStats.MaxHP = 100;
        MyStats.Radius = 5f;
        MyStats.MoveSpeed = 5f;
        MyStats.IsPatrol = false;
    }

    protected sealed override void IdleEvent()
    {
        _log.Message("AI Idle");

        base.IdleEvent();

        MyStats.State = AIState.Patrol;
    }

    protected sealed override void PatrolEvent()
    {
        _log.Message("AI Patrol");

        base.PatrolEvent();

        if (Vector2.Distance(gameObject.transform.position, targetObj.transform.position) <= MyStats.Radius)
        {
            _rigid.velocity = Vector2.zero;

            MyStats.State = AIState.Track;
        }
        else if (!MyStats.IsPatrol)
        {
            MyStats.IsPatrol = true;
            StartCoroutine(PatrolLogic());
        }
    }

    protected sealed override void TrackEvent()
    {
        _log.Message("AI Track");

        base.TrackEvent();

        if (Vector2.Distance(gameObject.transform.position, targetObj.transform.position) <= MyStats.Radius / 2)
        {
            _rigid.velocity = Vector2.zero;

            MyStats.State = AIState.Attack;
        }
        else if (Vector2.Distance(gameObject.transform.position, targetObj.transform.position) >= MyStats.Radius * 2f)
        {
            StartCoroutine(BackPosition());
        }
        else
        {
            Vector3 moveVec = Vector3.zero;

            if (targetObj.transform.position.x < transform.position.x)
            {
                moveVec = Vector3.left;
                _renderer.flipX = false;

            }
            else if (targetObj.transform.position.x > transform.position.x)
            {
                moveVec = Vector3.right;
                _renderer.flipX = true;
            }

            _log.Message("Track : " + moveVec);

            _attackCollider.GetComponent<BoxCollider2D>().offset = new Vector2(moveVec.x, 0);

            transform.position += moveVec * MyStats.MoveSpeed * Time.deltaTime;
        }
    }

    protected sealed override void AttackEvent()
    {
        _log.Message("AI Attack");

        base.AttackEvent();

        if (Vector2.Distance(gameObject.transform.position, targetObj.transform.position) > MyStats.Radius / 2)
        {
            _rigid.velocity = Vector2.zero;
            MyStats.IsAttack = false;

            MyStats.State = AIState.Patrol;
        }
        else if (!MyStats.IsAttack)
        {
            MyStats.IsAttack = true;
            StartCoroutine("AttackLogic");
        }
    }

    protected sealed override void DeadEvent()
    {
        _log.Message("AI Dead");

        base.DeadEvent();

        Destroy(gameObject);
    }

    protected sealed override void Callback(GameObject pObj)
    {
        _log.Message("AI Callback");

        base.Callback(pObj);
    }

    protected override void AnimFrameStart()
    {
    }

    protected override void AnimFrameUpdate()
    {
    }

    protected override void AnimFrameEnd()
    {
        _animtor.SetBool("isHurt", false);
    }

    IEnumerator PatrolLogic()
    {
        _moveDir = new Vector3(Random.Range(-1f, 1f), 0f, 0f);
        float progress = 0f;
        yield return new WaitForSeconds(1f);

        while (MyStats.State == AIState.Patrol && progress <= 1.5f)
        {
            progress += Time.deltaTime;

            if (_moveDir.x <= 0)
                _renderer.flipX = false;
            else
                _renderer.flipX = true;

            transform.position += _moveDir * MyStats.MoveSpeed * Time.deltaTime;

            yield return null;
        }

        MyStats.IsPatrol = false;
        yield return new WaitForSeconds(1f);
    }

    IEnumerator AttackLogic()
    {
        yield return new WaitForSeconds(1f);
        _attackCollider.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        _attackCollider.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        MyStats.IsAttack = false;
    }

    IEnumerator BackPosition()
    {
        _rigid.velocity = Vector2.zero;

        float progress = 0f;
        yield return new WaitForSeconds(1f);

        while (MyStats.State == AIState.Track && progress <= 2.5f)
        {
            progress += Time.deltaTime;

            Vector3 moveVec = Vector3.Lerp(transform.position, _startPosition, MyStats.MoveSpeed * Time.deltaTime);
            moveVec.y = _startPosition.y; moveVec.z = 0f;

            transform.position = moveVec;
            yield return null;
        }

        MyStats.State = AIState.Patrol;
        yield return new WaitForSeconds(1f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Attack"))
        {
            _log.Message("Hit : " + MyStats.CurHP);

            GetComponent<CinemachineImpulseSource>().GenerateImpulse();
            _animtor.SetBool("isHurt", true);
            _attackCollider.SetActive(false);

            MyStats.CurHP -= 10;
        }
    }
}