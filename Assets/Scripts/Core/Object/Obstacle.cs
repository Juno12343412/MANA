using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDBase.Controllers.ObjectSystem;
using UDBase.Controllers.LogSystem;
using MANA.Enums;
using UDBase.Utils;
using Zenject;
using Cinemachine;

public class Obstacle : AIMachine
{
    ULogger _log;
    Animator _animtor;

    [SerializeField] private GameObject _particle;

    protected sealed override void AISetting(ILog log)
    {
        _log = log.CreateLogger(this);
        _log.Message("AI 셋팅");

        _animtor = GetComponent<Animator>();

        base.AISetting(log);

        Kind = ObjectKind.Item;
        MyStats.CurHP = MyStats.MaxHP = 100;
    }

    protected sealed override void IdleEvent()
    {
        _log.Message("AI Idle");

        base.IdleEvent();
    }

    protected sealed override void PatrolEvent()
    {
        _log.Message("AI Patrol");

        base.PatrolEvent();
    }

    protected sealed override void TrackEvent()
    {
        _log.Message("AI Track");

        base.TrackEvent();
    }

    protected sealed override void Callback()
    {
        _log.Message("AI Callback");

        base.Callback();
    }

    protected sealed override void AttackEvent()
    {
        _log.Message("AI Attack");

        base.AttackEvent();
    }

    protected sealed override void DeadEvent()
    {
        _log.Message("AI Dead");

        base.DeadEvent();

        Destroy(gameObject);
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
        _particle.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Attack"))
        {
            _log.Message("Hit : " + MyStats.CurHP);

            GetComponent<CinemachineImpulseSource>().GenerateImpulse();
            _animtor.SetBool("isHurt", true);
            _particle.SetActive(true);

            MyStats.CurHP -= 10;
        }
    }
}