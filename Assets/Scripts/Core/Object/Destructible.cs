using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Zenject;
using UDBase.Controllers.ObjectSystem;
using UDBase.Controllers.LogSystem;
using MANA.Enums;
using Pooling;

public class Destructible : AIMachine
{
    ULogger _log;
    Animator _animtor;
    [SerializeField] private DestructKind kind;
    [SerializeField] private GameObject particle;
    [SerializeField] private float maxHp;
    private float dir = 0.0f;

    ObjectPool<Particle> particlePool = new ObjectPool<Particle>();

    protected sealed override void AISetting(ILog log)
    {
        _log = log.CreateLogger(this);
        _log.Message("AI 셋팅");

        _animtor = GetComponent<Animator>();

        base.AISetting(log);

        Kind = ObjectKind.Obstacle;
        MyStats.CurHP = MyStats.MaxHP = maxHp;

        particlePool.Init(particle, 5);

        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Attack"))
        {
            _log.Message(kind.ToString() + " Hit : " + MyStats.CurHP);

            if(kind != DestructKind.Grass)
                GetComponent<CinemachineImpulseSource>().GenerateImpulse();
            Debug.Log("AA : " + other.gameObject.GetComponentInParent<Player>()._attackDir);
            switch (other.gameObject.GetComponentInParent<Player>()._attackDir)
            {
                case Vector2 v when v.Equals(Vector2.left):
                    
                    particlePool.Spawn(gameObject.transform.position, Quaternion.Euler(new Vector3(0,0,180)));
                    break;
                case Vector2 v when v.Equals(Vector2.right):
                    particlePool.Spawn(gameObject.transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
                    Debug.Log("right");

                    break;
                case Vector2 v when v.Equals(Vector2.up):
                    particlePool.Spawn(gameObject.transform.position, Quaternion.Euler(new Vector3(0, 0, 90)));
                    Debug.Log("up");

                    break;
                case Vector2 v when v.Equals(Vector2.down):
                    particlePool.Spawn(gameObject.transform.position, Quaternion.Euler(new Vector3(0, 0, 270)));
                    Debug.Log("down");

                    break;
            }

            MyStats.CurHP -= 1;
            if(MyStats.CurHP <= 0)
            {
                Dead(other.gameObject);
            }
        }
    }

    void Dead(GameObject obj)
    {
        Destroy(gameObject);
    }
}
