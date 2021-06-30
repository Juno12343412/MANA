﻿using System.Collections;
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

public class AI : AIMachine
{
    ULogger _log;
    Animator _animtor;
    SpriteRenderer _renderer;
    Rigidbody2D _rigid;

    [SerializeField] private GameObject _attackCollider;
    [SerializeField] private GameObject _particle;

    Vector3 _startPosition = Vector3.zero;
    Vector3 _moveDir = Vector3.zero;
    Vector3 _hurtDir = Vector3.zero;

    [Inject]
    readonly ParticleManager _particleManager;

    protected sealed override void AISetting(ILog log)
    {
        _log = log.CreateLogger(this);

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

        base.IdleEvent();

        MyStats.State = AIState.Patrol;
    }

    protected sealed override void PatrolEvent()
    {

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

        base.TrackEvent();

        if (Vector2.Distance(gameObject.transform.position, targetObj.transform.position) <= MyStats.Radius / 2)
        {
            _rigid.velocity = Vector2.zero;
            _animtor.SetBool("isMove", false);

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
            _hurtDir = moveVec;

            MyStats.State = AIState.Attack;
        }
        else if (Vector2.Distance(gameObject.transform.position, targetObj.transform.position) >= MyStats.Radius * 2f)
        {
            StartCoroutine(BackPosition());
        }
        else
        {
            if (!_animtor.GetBool("isMove"))
                _animtor.SetBool("isMove", true);

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
            _hurtDir = moveVec;

            _attackCollider.GetComponent<BoxCollider2D>().offset = new Vector2(moveVec.x, 0);

            transform.position += moveVec * MyStats.MoveSpeed * Time.deltaTime;
        }
    }

    protected sealed override void AttackEvent()
    {
        base.AttackEvent();

        if (!MyStats.IsAttack)
        {
            if (Vector2.Distance(gameObject.transform.position, targetObj.transform.position) > MyStats.Radius / 2)
            {
                _rigid.velocity = Vector2.zero;
                MyStats.State = AIState.Patrol;
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
                _hurtDir = moveVec;

                MyStats.IsAttack = true;
                StartCoroutine("AttackLogic");
            }
        }
    }

    protected sealed override void DeadEvent()
    {

        base.DeadEvent();

        Vector2 _dir = new Vector2(-_hurtDir.x, 5f);

        _rigid.velocity = Vector2.zero;
        _rigid.AddForce(_dir * MyStats.MoveSpeed / 2f, ForceMode2D.Impulse);
        GetComponent<BoxCollider2D>().enabled = false;

        StartCoroutine(EnemyErase(3f));
    }

    protected sealed override void Callback(GameObject pObj)
    {

        base.Callback(pObj);
    }

    protected override void AnimFrameStart()
    {
        _particleManager?.ShowParticle(ParticleKind.Hit, transform.position);
    }

    protected override void AnimFrameUpdate()
    {
    }

    protected override void AnimFrameEnd()
    {
        _animtor.SetBool("isHurt", false);

        if (_animtor.GetBool("isAttack"))
            _animtor.SetBool("isAttack", false);
    }

    IEnumerator PatrolLogic()
    {
        _moveDir = new Vector3(Random.Range(-1f, 1f), 0f, 0f);
        float progress = 0f;
        yield return new WaitForSeconds(1f);

        while (MyStats.State == AIState.Patrol && progress <= 1.5f)
        {
            if (!_animtor.GetBool("isMove"))
                _animtor.SetBool("isMove", true);

            progress += Time.deltaTime;

            if (_moveDir.x <= 0)
                _renderer.flipX = false;
            else
                _renderer.flipX = true;

            transform.position += _moveDir * MyStats.MoveSpeed * Time.deltaTime;

            yield return null;
        }

        MyStats.IsPatrol = false;
        _animtor.SetBool("isMove", false);

        yield return new WaitForSeconds(1f);
    }

    IEnumerator AttackLogic()
    {
        if (MyStats.IsAttack)
        {
            yield return new WaitForSeconds(1f);
            _animtor.SetBool("isAttack", true);
            _attackCollider.SetActive(true);
            yield return new WaitForSeconds(0.05f);
            _attackCollider.SetActive(false);
            yield return new WaitForSeconds(1.5f);
            MyStats.IsAttack = false;
        }
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

    IEnumerator EnemyErase(float _time)
    {
        yield return new WaitForSeconds(_time);
        Destroy(gameObject, _time);
        //float progress = 1f;

        //while (_renderer.color.a > 0.1f)
        //{
        //    Debug.Log(progress + " : " + _renderer.color.a);
        //    _renderer.color = new Color(1f, 1f, 1f, 255f/155f);
        //    progress -= Time.deltaTime;
        //    yield return null;
        //}
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Attack") && !_animtor.GetBool("isHurt"))
        {
            GetComponent<CinemachineImpulseSource>().GenerateImpulse();

            _rigid.AddForce(new Vector2(-_hurtDir.x, 0f) * 5f, ForceMode2D.Impulse);
            _animtor.SetBool("isHurt", true);
            other.gameObject.SetActive(false);
            _attackCollider.SetActive(false);
            MyStats.IsAttack = false;

            MyStats.CurHP -= 10;

            if (!IsDead())
            {
                StartCoroutine(TimeUtils.TimeStop(0.05f));
            }
        }
    }
}