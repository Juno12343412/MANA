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

    [Inject]
    PlayerManager _player;

    ULogger _log;
    Animator _animtor;

    [SerializeField] private GameObject _interaction;

    [SerializeField] private List<string> _textList;

    protected sealed override void AISetting(ILog log)
    {
        _log = log.CreateLogger(this);

        _animtor = GetComponent<Animator>();

        base.AISetting(log);

        Kind = ObjectKind.NPC;
        MyStats.CurHP = MyStats.MaxHP = 100;
        MyStats.Radius = 5f;
    }

    protected sealed override void IdleEvent()
    {

        base.IdleEvent();
    }

    protected sealed override void PatrolEvent()
    {

        base.PatrolEvent();
    }

    protected sealed override void TrackEvent()
    {

        base.TrackEvent();
    }

    protected sealed override void CallbackEnter(GameObject pObj)
    {

        switch (Kind)
        {
            case ObjectKind.NPC:
                //_log.Message("Pos : " + Camera.main.WorldToScreenPoint(transform.position + Vector3.up));

                //_ui.Show("InteractionUI");
                //_ui.Find("InteractionUI").transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up);
                //_ui.Find("InteractionUI_Talk").GetComponent<TextMeshProUGUI>().text = "대화";
                _interaction.SetActive(true);
                break;
            default:
                break;
        }
    }

    protected sealed override void CallbackExit(GameObject pObj)
    {

        switch (Kind)
        {
            case ObjectKind.NPC:
                //_ui.Hide("InteractionUI");
                TextBox.instance.TalkEnd();
                _interaction.SetActive(false);
                break;
            default:
                break;
        }
    }

    protected sealed override void Callback(GameObject pObj)
    {
        switch (Kind)
        {
            case ObjectKind.NPC:
                //_ui.Hide("InteractionUI");
                TextBox.instance.SetTalk(_textList, true, transform.position);
                break;
            default:
                break;
        }
    }

    protected sealed override void AttackEvent()
    {

        base.AttackEvent();
    }

    protected sealed override void DeadEvent()
    {

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
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Attack"))
        {

            GetComponent<CinemachineImpulseSource>().GenerateImpulse();
            _animtor.SetBool("isHurt", true);

            MyStats.CurHP -= 10;
        }
    }
}