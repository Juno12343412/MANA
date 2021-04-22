using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UDBase.Controllers.ObjectSystem;
using UDBase.Controllers.LogSystem;
using UDBase.UI.Common;
using MANA.Enums;
using Zenject;
using Cinemachine;

public class Obstacle : AIMachine
{
    [Inject]
    UIManager _ui;

    ULogger _log;
    Animator _animtor;

    [SerializeField] private GameObject _particle;
    [SerializeField] private GameObject _interaction;

    protected sealed override void AISetting(ILog log)
    {
        _log = log.CreateLogger(this);
        _log.Message("AI 셋팅");

        _animtor = GetComponent<Animator>();

        base.AISetting(log);

        Kind = ObjectKind.NPC;
        MyStats.CurHP = MyStats.MaxHP = 100;
        MyStats.Radius = 5f;
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

    protected sealed override void CallbackEnter(GameObject pObj)
    {
        _log.Message("AI CallbackEnter");

        switch (Kind)
        {
            case ObjectKind.NPC:
                //_log.Message("Pos : " + Camera.main.WorldToScreenPoint(transform.position + Vector3.up));

                //_ui.Show("InteractionUI");
                //_ui.Find("InteractionUI").transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up);
                //_ui.Find("InteractionUI_Talk").GetComponent<TextMeshProUGUI>().text = "대화";
                break;
            default:
                break;
        }
    }

    protected sealed override void CallbackExit(GameObject pObj)
    {
        _log.Message("AI CallbackExit");

        switch (Kind)
        {
            case ObjectKind.NPC:
                //_ui.Hide("InteractionUI");
                break;
            default:
                break;
        }
    }

    protected sealed override void Callback(GameObject pObj)
    {
        _log.Message("AI Callback");

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
        //_particle.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Attack"))
        {
            _log.Message("Hit : " + MyStats.CurHP);

            GetComponent<CinemachineImpulseSource>().GenerateImpulse();
            _animtor.SetBool("isHurt", true);
            //_particle.SetActive(true);

            MyStats.CurHP -= 10;
        }
    }
}