using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Zenject;
using UDBase.Controllers.ObjectSystem;
using UDBase.Controllers.LogSystem;
using MANA.Enums;



public class Destructible : AIMachine
{
    
    ULogger _log;
    Animator _animtor;
    [SerializeField] private DestructKind kind;
    [SerializeField] private GameObject _particle;
    [SerializeField] private float maxHp;


    protected sealed override void AISetting(ILog log)
    {
        _log = log.CreateLogger(this);
        _log.Message("AI 셋팅");

        _animtor = GetComponent<Animator>();

        base.AISetting(log);

        Kind = ObjectKind.Obstacle;
        MyStats.CurHP = MyStats.MaxHP = maxHp;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Attack"))
        {
            _log.Message(kind.ToString() + " Hit : " + MyStats.CurHP);

            if(kind != DestructKind.Grass)
                GetComponent<CinemachineImpulseSource>().GenerateImpulse();
            
            MyStats.CurHP -= 1;
            if(MyStats.CurHP <= 0)
            {
                Dead();
            }
        }
    }

    void Dead()
    {
        //_particle.SetActive(true);
        Destroy(gameObject);
    }
}
