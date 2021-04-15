using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDBase.Controllers.ObjectSystem;
using UDBase.Controllers.LogSystem;
using Zenject;

public class AI : AIMachine
{
    ULogger _log;

    protected sealed override void AISetting(ILog log)
    {
        _log = log.CreateLogger(this);
        _log.Message("AI 셋팅");

        base.AISetting(log);
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

    protected sealed override void Callback()
    {
        _log.Message("AI Callback");

        base.Callback();
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
    }
}
