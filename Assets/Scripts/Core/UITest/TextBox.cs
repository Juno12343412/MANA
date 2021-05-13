using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UDBase.UI.Common;
using UDBase.Utils;
using TMPro;
using Zenject;
using UDBase.Controllers.LogSystem;
using UDBase.Controllers.ObjectSystem;

public class TextBox : MonoBehaviour, ILogContext
{
    [Inject]
    ILog _log;

    [Inject]
    UIManager _ui;

    [Inject]
    PlayerManager _player;

    protected Dictionary<string, KeySetting> _talkKeys;

    public List<string> _talkList = new List<string>();
    public static TextBox instance = new TextBox();

    int _curText = 0;
    bool _isInteraction = false;

    void Start()
    {
        instance = this;

        _talkKeys = new Dictionary<string, KeySetting>();

        _talkKeys.Add("Next", new KeySetting(KeyCode.Q, Next, KeyKind.Down));
        _talkKeys.Add("LA", new KeySetting(KeyCode.LeftArrow, Left_Select, KeyKind.Down));
        _talkKeys.Add("RA",new KeySetting(KeyCode.RightArrow, Right_Select, KeyKind.Down));
        _talkKeys.Add("L",new KeySetting(KeyCode.A, Left_Select, KeyKind.Down));
        _talkKeys.Add("R", new KeySetting(KeyCode.D, Right_Select, KeyKind.Down));
    }

    void Update()
    {
        foreach (var key in _talkKeys)
            key.Value.Update();
    }

    public void SetTalk(List<string> _list, bool _talk)
    {
        if (_player._stats.IsTalk)
            return;

        _ui.Find("BaseUI").GetComponent<Animator>().SetBool("isTalk", true);

        _talkList = _list;

        _ui.Show("TalkUI");

        _player._stats.IsTalk = true;
        _curText = 0;
        _isInteraction = false;

        Next();
    }

    void Next()
    {
        Debug.Log("In");

        if (!_player._stats.IsTalk)
            return;


        if (_talkList.Count > _curText && !_talkList[_curText].Contains("[Interaction]"))
        {
            Debug.Log("1");

            _isInteraction = false;

            _ui.Hide("TextUI_InteractionBox");
            _ui.Show("TextUI_Box");

            Debug.Log(_ui.Find("TextUI_TalkText").GetComponent<TextMeshProUGUI>() + " : " + _talkList[_curText]);

            _ui.Hide("TextUI_TalkText");
            _ui.Show("TextUI_TalkText");
            _ui.Find("TextUI_TalkText").GetComponent<TextMeshProUGUI>().text = _talkList[_curText];
            _curText++;
        }
        else if (_talkList.Count > _curText && _talkList[_curText].Contains("[Interaction]"))
        {
            Debug.Log("2");

            _isInteraction = true;

            _ui.Hide("TextUI_Box");
            _ui.Show("TextUI_InteractionBox");

            Debug.Log(_ui.Find("TextUI_TalkText").GetComponent<TextMeshProUGUI>() + " : " + _talkList[_curText]);

            _ui.Hide("TextUI_Talk");
            _ui.Show("TextUI_Talk");
            _ui.Find("TextUI_Talk").GetComponent<TextMeshProUGUI>().text = _talkList[_curText].Replace("[Interaction]", "");
            _curText++;
        }
        else // 대화 종료
        {
            Invoke("TalkEnd", 0.1f);
        }
    }

    void Left_Select()
    {
        if (!_isInteraction)
            return;

        _ui.Find("TextUI_NO").GetComponent<Image>().color = new Color(255f, 255f, 255f, 0f);
        _ui.Find("TextUI_NO_Base").SetActive(false);
        
        _ui.Find("TextUI_OK").GetComponent<Image>().color = new Color(255f, 255f, 255f, 255f);
        _ui.Find("TextUI_OK_Base").SetActive(true);
    }

    void Right_Select()
    {
        if (!_isInteraction)
            return;

        _ui.Find("TextUI_OK").GetComponent<Image>().color = new Color(255f, 255f, 255f, 0f);
        _ui.Find("TextUI_OK_Base").SetActive(false);

        _ui.Find("TextUI_NO").GetComponent<Image>().color = new Color(255f, 255f, 255f, 255f);
        _ui.Find("TextUI_NO_Base").SetActive(true);
    }

    public void TalkEnd()
    {
        Debug.Log(_ui.Find("BaseUI"));

        _ui.Find("BaseUI").GetComponent<Animator>().SetBool("isTalk", false);

        _ui.Hide("TalkUI");
        _ui.Hide("TextUI_InteractionBox");

        _player._stats.IsTalk = false;
    }
}
