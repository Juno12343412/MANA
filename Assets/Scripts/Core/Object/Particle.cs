using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pooling;

public class Particle : PoolingObject
{
    public override string objectName => "Particle";
    public override void Init()
    {
        base.Init();

        Invoke("Release", 1f);
    }

    public override void Release()
    {
        base.Release();
    }
}
