using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDBase.UI.Common;
using UDBase.Utils;
using TMPro;
using Zenject;
using UDBase.Controllers.LogSystem;

public class TextBox : MonoBehaviour, ILogContext
{
    [Inject]
    ILog _log;

    [Inject]
    UIManager _ui;

    KeySetting _talkKey;
    
    [SerializeField] private List<string> _talkList = new List<string>();

    void Start()
    {
        _talkKey = new KeySetting(KeyCode.Q, Next);
    }

    void Update()
    {
        _talkKey.Update();
    }

    public void SetTalk(List<string> _list, bool _talk)
    {
        _talkList = _list;
        _ui.Show("TalkUI");
        if (_talk) _ui.Show("TalkUI_Box");
        else _ui.Show("TextUI_InteractionBox");
    }

    void Next()
    {
        if (_talkList[_talkList.Count + 1] != null)
        {
            _ui.Hide("TextUI_TalkText");
            _ui.Show("TextUI_TalkText");
            _ui.Find("TextUI_TalkText").GetComponent<TextMeshProUGUI>().text = _talkList[_talkList.Count + 1];
        }
        else // 대화 종료
        {
            _ui.Hide("TalkUI");
        }
    }
}
