using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pooling;
using MANA.Enums;

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

        if (_kind == ParticleKind.Hit || _kind == ParticleKind.Dash || _kind == ParticleKind.Obs2)
        {
            var v = _particles[(int)_kind]?.transform.GetChild(0);
            v.localEulerAngles = _particles[(int)_kind].transform.localEulerAngles;
        }

        Invoke("Release", 1f);
    }

    public override void Release()
    {
        if (_kind != ParticleKind.NONE)
            _particles[(int)_kind]?.SetActive(false);

        base.Release();
    }
}
