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

    [SerializeField] private GameObject _markObj = null;

    public List<string> _talkList = new List<string>();
    public static TextBox instance = new TextBox();

    public bool isTalkEnd = false;

    int _curText = -1;
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

    public void SetTalk(List<string> _list, bool _talk, Vector3 _pos)
    {
        if (_player._stats.IsTalk)
            return;

        isTalkEnd = false;

        _markObj.SetActive(false);

        _ui.Find("BaseUI").GetComponent<Animator>().SetInteger("isTalk", 0);

        _talkList = _list;

        _ui.Show("TalkUI");

        _player._stats.IsTalk = true;
        _curText = 0;
        _isInteraction = false;

        var x = (_pos.x - Screen.width / 2) * (1280 / Screen.width);
        var y = (_pos.y - Screen.height / 2) * (720 / Screen.height);

        Debug.Log("pos : " + _pos + "\nx : " + x + " | y : " + y);

        _ui.Find("TextUI_SmallBox").transform.localPosition = new Vector2(x, y - y / 2.5f);

        Next();
    }

    void Next()
    {
        if (!_player._stats.IsTalk)
            return;

        if (_talkList.Count > _curText && !_talkList[_curText].Contains("[Interaction]") && !_talkList[_curText].Contains("[Small]"))
        {
            Invoke("ShowMark", 0.2f * _talkList[_curText].Length);

            _isInteraction = false;

            _ui.Hide("TextUI_SmallBox");
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
            _isInteraction = true;

            _ui.Hide("TextUI_SmallBox");
            _ui.Hide("TextUI_Box");
            _ui.Show("TextUI_InteractionBox");

            Debug.Log(_ui.Find("TextUI_TalkText").GetComponent<TextMeshProUGUI>() + " : " + _talkList[_curText]);

            _ui.Hide("TextUI_Talk");
            _ui.Show("TextUI_Talk");
            _ui.Find("TextUI_Talk").GetComponent<TextMeshProUGUI>().text = _talkList[_curText].Replace("[Interaction]", "");
            _curText++;
        }
        else if (_talkList.Count > _curText && _talkList[_curText].Contains("[Small]"))
        {
            _isInteraction = false;

            _ui.Show("TextUI_SmallBox");
            _ui.Hide("TextUI_InteractionBox");
            _ui.Hide("TextUI_Box");

            _ui.Hide("TextUI_SizeText");
            _ui.Hide("TextUI_ShowText");

            _ui.Show("TextUI_SizeText");
            _ui.Show("TextUI_ShowText");

            _ui.Find("TextUI_SizeText").GetComponent<TextMeshProUGUI>().text = _talkList[_curText].Replace("[Small]", "");
            _ui.Find("TextUI_ShowText").GetComponent<TextMeshProUGUI>().text = _talkList[_curText].Replace("[Small]", "");
            
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

    void ShowMark()
    {
        if (!_markObj.activeSelf)
            _markObj.SetActive(true);        
    }

    public void TalkEnd()
    {
        _ui.Find("BaseUI").GetComponent<Animator>().SetInteger("isTalk", 1);

        _ui.Hide("TalkUI");
        _ui.Hide("TextUI_SmallBox");
        _ui.Hide("TextUI_InteractionBox");

        _player._stats.IsTalk = false;
        isTalkEnd = true;
    }
}
