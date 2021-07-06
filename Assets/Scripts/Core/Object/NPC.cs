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
using Manager.Sound;

public class NPC : AIMachine
{
    [Inject]
    UIManager _ui;

    [Inject]
    PlayerManager _player;

    ULogger _log;
    Animator _animtor;

    [SerializeField] private NPCKind _kind = NPCKind.Radia;
     
    [SerializeField] private GameObject _interaction;
    [SerializeField] private GameObject _cameraObj;

    [SerializeField] private List<string> _textList;
    [SerializeField] private List<string> _yesTextList;
    [SerializeField] private List<string> _noTextList;

    protected sealed override void AISetting(ILog log)
    {
        _log = log.CreateLogger(this);

        _animtor = GetComponent<Animator>();

        base.AISetting(log);

        Kind = ObjectKind.NPC;
        MyStats.CurHP = MyStats.MaxHP = 100;
        MyStats.Radius = 5f;
    }

    protected sealed override void CallbackEnter(GameObject pObj)
    {
        switch (Kind)
        {
            case ObjectKind.NPC:
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
                TextBox.instance.TalkEnd();
                _interaction.SetActive(false);
                break;
            default:
                break;
        }
    }

    protected sealed override void Callback(GameObject pObj)
    {
        if (_player._stats.IsTalk)
            return;

        switch (Kind)
        {
            case ObjectKind.NPC:
                _player._stats.IsAction = true;
                
                _interaction.SetActive(false);

                var v = (GameObject.FindGameObjectWithTag("Player").transform.position + transform.position) * 0.5f;
                _cameraObj.transform.position = new Vector3(v.x, transform.position.y, -10f);
                _cameraObj.SetActive(true);

                Invoke("WaitTalk", 2f);
                break;
            default:
                break;
        }
    }

    void WaitTalk()
    {
        switch (_kind)
        {
            case NPCKind.Radia:
                SoundPlayer.instance.PlaySound("Npc1_" + Random.Range(1, 4));
                break;
            case NPCKind.Guard:
                SoundPlayer.instance.PlaySound("Npc2_" + Random.Range(1, 4));
                break;
            case NPCKind.Elder:
                SoundPlayer.instance.PlaySound("Npc3_" + Random.Range(1, 4));
                break;
            case NPCKind.Baby:
                SoundPlayer.instance.PlaySound("Npc4_" + Random.Range(1, 4));
                break;
        }

        _player._stats.IsAction = false;

        TextBox.instance._yesTalkList = _yesTextList;
        TextBox.instance._noTalkList = _noTextList;
        TextBox.instance.SetTalk(_textList, false, transform.position, "Tuto_5_Cinemachine 2");
    }
}
