using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDBase.Controllers.ObjectSystem;
using UDBase.Controllers.LogSystem;
using UDBase.Controllers.ParticleSystem;
using Cinemachine;
using MANA.Enums;
using Manager.Sound;
using Zenject;

public class SpecialItem : AIMachine
{
    ULogger _log;

    [Inject]
    readonly ParticleManager _particleManager;

    [Inject]
    PlayerManager _playerManager;

    [SerializeField] private List<string> _textList;

    protected sealed override void AISetting(ILog log)
    {
        _log = log.CreateLogger(this);

        base.AISetting(log);

        Kind = ObjectKind.Item;
        MyStats.CurHP = MyStats.MaxHP = 99;
        MyStats.Radius = 5f;
        MyStats.MoveSpeed = 5f;
        MyStats.IsPatrol = false;
    }

    protected sealed override void IdleEvent()
    {
        base.IdleEvent();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {

            GetItem();
        }
    }

    void GetItem()
    {
        SoundPlayer.instance.PlaySound("Item");
        SoundPlayer.instance.PlaySound("Map_stone_1");

        GetComponent<CinemachineImpulseSource>().GenerateImpulse();

        _playerManager._stats.IsDash = true;
        _particleManager.ShowParticle(ParticleKind.Explosion, transform.position, null, 1);

        gameObject.SetActive(false);

        Invoke("StartingTalk", 1f);
    }
    
    void StartingTalk()
    {
        TextBox.instance.SetTalk(_textList, false, transform.position);
    }
}
