using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pooling;
using MANA.Enums;
using UDBase.Controllers.LogSystem;

public class Particle : PoolingObject
{
    public ParticleKind _kind = ParticleKind.NONE;
    public GameObject[] _particles = null;

    public override string objectName => "Particle";
    public override void Init()
    {
        base.Init();

        if (_kind != ParticleKind.NONE)
            _particles[(int)_kind]?.SetActive(true);

        Invoke("Release", 1f);
    }

    public override void Release()
    {
        if (_kind != ParticleKind.NONE)
            _particles[(int)_kind]?.SetActive(false);

        base.Release();
    }
}
