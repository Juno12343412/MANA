using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDBase.Controllers.ParticleSystem;
using Zenject;
using MANA.Enums;
using Manager.Sound;

public class PlayerAttack : MonoBehaviour
{
    [Inject]
    readonly ParticleManager _particleManager;

    BoxCollider2D _collider = null;

    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            SoundPlayer.instance.PlaySound("Obj_Dmg_on_1");

            _particleManager?.ShowParticle(ParticleKind.Hit2, other.transform.position);
            _particleManager?.ShowParticle(ParticleKind.Hit, other.transform.position, GetRot());
        }
        if (other.CompareTag("Wall") || other.CompareTag("Grass"))
        {
            SoundPlayer.instance.PlaySound("Obj_Dmg_off");
        }
    }

    Vector3 GetRot()
    {
        Vector3 v = Vector3.zero;
        if (_collider.offset.x == 1)
            v.y = -90;
        else if (_collider.offset.x == -1)
            v.y = 90;

        if (_collider.offset.y == 1)
            v.x = 0;
        else if (_collider.offset.y == -1)
            v.x = 90;

        return v;
    }
}
