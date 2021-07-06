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
    public List<string> _yesTalkList = new List<string>();
    public List<string> _noTalkList = new List<string>();
    public static TextBox instance = new TextBox();

    public bool isTalkEnd = false;

    public int _curText = -1;
    public int _textState = 0;

    bool _isInteraction = false;
    string _isState = "Left";
    public string _isAccept = "No";

    public GameObject _cameraObj = null;

    void Start()
    {
        instance = this;

        _talkKeys = new Dictionary<string, KeySetting>();

        _talkKeys.Add("Next", new KeySetting(KeyCode.J, Next, KeyKind.Down));
        _talkKeys.Add("LA", new KeySetting(KeyCode.LeftArrow, Left_Select, KeyKind.Down));
        _talkKeys.Add("RA", new KeySetting(KeyCode.RightArrow, Right_Select, KeyKind.Down));
        _talkKeys.Add("L", new KeySetting(KeyCode.A, Left_Select, KeyKind.Down));
        _talkKeys.Add("R", new KeySetting(KeyCode.D, Right_Select, KeyKind.Down));
        _talkKeys.Add("Accept1", new KeySetting(KeyCode.Return, Accept, KeyKind.Down));
        _talkKeys.Add("Accept2", new KeySetting(KeyCode.Space, Accept, KeyKind.Down));
    }

    void Update()
    {
        foreach (var key in _talkKeys)
            key.Value.Update();
    }

    public void SetTalk(List<string> _list, bool _talk, Vector3 _pos, string _camera = "")
    {
        if (_player._stats.IsTalk)
            return;

        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().OnAction();

        isTalkEnd = false;

        _markObj.SetActive(false);

        _ui.Find("BaseUI").GetComponent<Animator>().SetInteger("isTalk", 0);

        _talkList = _list;

        _ui.Show("TalkUI");

        _player._stats.IsTalk = true;
        _player._stats.IsEvent = true;
        _curText = 0;
        _textState = 0;
        _isInteraction = false;

        Vector3 v = Camera.main.WorldToScreenPoint(_pos);
        v.x = (v.x - Screen.width / 2) /** (1280 / Screen.width)*/;
        v.y = 0.8f; v.z = 0;
        
        _ui.Find("TextUI_SmallBox").transform.localPosition = v;

        if (!_talk)
            Next();
    }

    void Next()
    {
        if (!_player._stats.IsTalk)
            return;

        _textState++;

        if (_talkList.Count > _curText && !_talkList[_curText].Contains("[Interaction]") && !_talkList[_curText].Contains("[Small]"))
        {
            // 기본
            if (_textState == 1)
            {
                Invoke("ShowMark", 0.2f * _talkList[_curText].Length);

                _isInteraction = false;

                _ui.Hide("TextUI_SkipText2");
                _ui.Hide("TextUI_SmallBox");
                _ui.Hide("TextUI_InteractionBox");
                _ui.Show("TextUI_Box");

                _ui.Hide("TextUI_TalkText");
                _ui.Show("TextUI_TalkText");
                _ui.Find("TextUI_TalkText").GetComponent<TextMeshProUGUI>().text = _talkList[_curText];

            }
            // 스킵
            else if (_textState == 2)
            {
                _ui.Hide("TextUI_TalkText");

                _ui.Find("TextUI_SkipText2").GetComponent<TextMeshProUGUI>().text = _talkList[_curText];
                _ui.Show("TextUI_SkipText2");

                _curText++;
                _textState = 0;
            }
        }
        else if (_talkList.Count > _curText && _talkList[_curText].Contains("[Interaction]"))
        {

            // 기본
            if (_textState == 1)
            {
                _isInteraction = true;

                _ui.Hide("TextUI_SkipText3");
                _ui.Hide("TextUI_SmallBox");
                _ui.Hide("TextUI_Box");
                _ui.Show("TextUI_InteractionBox");

                _ui.Hide("TextUI_Talk");
                _ui.Show("TextUI_Talk");
                _ui.Find("TextUI_Talk").GetComponent<TextMeshProUGUI>().text = _talkList[_curText].Replace("[Interaction]", "");

            }
            // 스킵
            else if (_textState == 2)
            {
                _ui.Hide("TextUI_TalkText");

                _ui.Find("TextUI_SkipText3").GetComponent<TextMeshProUGUI>().text = _talkList[_curText].Replace("[Interaction]", "");
                _ui.Show("TextUI_SkipText3");

                _curText++;
                _textState = 0;
            }
        }
        else if (_talkList.Count > _curText && _talkList[_curText].Contains("[Small]"))
        {
            // 기본
            if (_textState == 1)
            {
                _isInteraction = false;

                _ui.Hide("TextUI_SkipText");

                _ui.Show("TextUI_SmallBox");
                _ui.Hide("TextUI_InteractionBox");
                _ui.Hide("TextUI_Box");

                _ui.Hide("TextUI_SizeText");
                _ui.Hide("TextUI_ShowText");

                _ui.Show("TextUI_SizeText");
                _ui.Show("TextUI_ShowText");

                _ui.Find("TextUI_SizeText").GetComponent<TextMeshProUGUI>().text = _talkList[_curText].Replace("[Small]", "");
                _ui.Find("TextUI_ShowText").GetComponent<TextMeshProUGUI>().text = _talkList[_curText].Replace("[Small]", "");

            }
            // 스킵
            else if (_textState == 2)
            {
                _ui.Hide("TextUI_ShowText");

                _ui.Find("TextUI_SkipText").GetComponent<TextMeshProUGUI>().text = _talkList[_curText].Replace("[Small]", "");
                _ui.Show("TextUI_SkipText");

                _curText++;
                _textState = 0;
            }
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

        _isState = "Left";
    }

    void Right_Select()
    {
        if (!_isInteraction)
            return;

        _ui.Find("TextUI_OK").GetComponent<Image>().color = new Color(255f, 255f, 255f, 0f);
        _ui.Find("TextUI_OK_Base").SetActive(false);

        _ui.Find("TextUI_NO").GetComponent<Image>().color = new Color(255f, 255f, 255f, 255f);
        _ui.Find("TextUI_NO_Base").SetActive(true);

        _isState = "Right";
    }

    void Accept()
    {
        if (!_isInteraction)
            return;

        if (_isState == "Left")
        {
            _isAccept = "Yes";
            _curText = 0;
            _talkList = _yesTalkList;
            Next();
        }
        else
        {
            _isAccept = "No";
            _curText = 0;
            _talkList = _noTalkList;
            Next();
        }
    }

    void ShowMark()
    {
        if (!_markObj.activeSelf)
            _markObj.SetActive(true);
    }

    public void TalkEnd()
    {
        if (!_player._stats.IsAction)
        {
            _cameraObj.SetActive(false);
            _ui.Find("BaseUI").GetComponent<Animator>().SetInteger("isTalk", 1);
        }

        _curText = 0;
        _textState = 0;

        _ui.Hide("TextUI_Box");
        _ui.Hide("TextUI_SmallBox");
        _ui.Hide("TextUI_InteractionBox");

        _player._stats.IsTalk = false;
        _player._stats.IsEvent = false;
        isTalkEnd = true;
    }
}
