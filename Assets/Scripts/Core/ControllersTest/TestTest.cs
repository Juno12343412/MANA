using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UDBase.Controllers.LogSystem;
using UDBase.Controllers.EventSystem;
using UDBase.Controllers.SceneSystem;
using UDBase.UI.Common;
using System;

public class TestTest : MonoBehaviour
{
    [Inject]
    ILog _log;

    [Inject]
    IEvent _event;

    [Inject]
    IScene _scene;

    [Inject]
    UIManager _ui;

    Test player;

    Action<Test> ptsd;

    void Awake()
    {
        DontDestroyOnLoad(this);

        player = new Test(_log);

        ptsd += (ang) => ang.TestFunc1();

        _event.Subscribe(player, ptsd);                           // EX 1
        _event.Subscribe<Test>(player, (ang) => ang.TestFunc2()); // EX 2
      
        _event.Fire(player);
        
        _event.Unsubscribe(ptsd);
        
        _event.Fire(player);


        //_scene.LoadScene("Game");
    }
    
    [ContextMenu("Show")]
    public void ShowS()
    {
        _ui.Show("Sex1");
    }
}
