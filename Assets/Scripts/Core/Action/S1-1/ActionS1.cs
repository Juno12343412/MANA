using System.Collections;
using System.Collections.Generic;
using UDBase.Controllers.ObjectSystem;
using UDBase.Controllers.LogSystem;
using UDBase.Controllers.ParticleSystem;
using UnityEngine;
using Zenject;

public class ActionS1 : ActionEvent
{
    [Header("Text")]
    [SerializeField] private List<string> _textList1;

    void Start()
    {
        AddAction("S1-1-1", () =>
            {
                MovingActor("S1-1-2", _actorObjs[0], _movingPos[0], 5f);
            }, 1f);
        AddAction("S1-1-2", () =>
        {
            TextBox.instance.SetTalk(_textList1, true, _actorObjs[0].transform.position);
            if (TextBox.instance.isTalkEnd)
                PlayAction("S1-1-3");
        }, 1f);
        AddAction("S1-1-3", () =>
        {
            MovingActor("", _actorObjs[0], _movingPos[0], 5f);
        }, 1f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            PlayAction("S1-1-1");
    }
}
