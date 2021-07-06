using System.Collections.Generic;
using UnityEngine;
using MANA.Enums;
using Manager.Sound;

public class ActionS4 : ActionEvent
{
    [Header("Text")]
    [SerializeField] private List<string> _textList1;
    [SerializeField] private List<string> _textList2;
    [SerializeField] private List<string> _yesTextList;
    [SerializeField] private List<string> _yesTextList2;
    [SerializeField] private List<string> _noTextList;
    [SerializeField] private List<string> _noTextList2;

    [SerializeField] private GameObject _camera = null;

    void Start()
    {
        Delay(2f);

        AddAction(() =>
        {
            var v = (GameObject.FindGameObjectWithTag("Player").transform.position + _actorObjs[0].transform.position) * 0.5f;
            _camera.transform.position = new Vector3(v.x, _camera.transform.position.y, -10f);
            _camera.SetActive(true);
            return false;
        });

        Delay(2f);

        AddAction(() =>
        {
            SoundPlayer.instance.PlaySound("Npc3_1");
            return false;
        });

        AddAction(() =>
        {
            if (TextBox.instance.isTalkEnd)
            {
                TextBox.instance.isTalkEnd = false;
                return false;
            }
            TextBox.instance._yesTalkList = _yesTextList;
            TextBox.instance._noTalkList = _noTextList;
            TextBox.instance.SetTalk(_textList1, false, _actorObjs[0].transform.position);
            return true;
        });

        Delay(1f);

        AddAction(() =>
        {
            SoundPlayer.instance.PlaySound("Npc3_2");
            return false;
        });

        AddAction(() =>
        {
            if (TextBox.instance._isAccept == "Yes")
            {
                if (TextBox.instance.isTalkEnd)
                {
                    TextBox.instance.isTalkEnd = false;
                    return false;
                }
                TextBox.instance.SetTalk(_textList2, false, _actorObjs[0].transform.position);
                TextBox.instance._yesTalkList = _yesTextList2;
                TextBox.instance._noTalkList = _noTextList2;
                return true;
            }
            return false;
        });

        Delay(3f);

        AddAction(() =>
        {
            SoundPlayer.instance.PlaySound("Npc3_3");
            return false;
        });

        AddAction(() =>
        {
            _playerManager._stats.IsAction = false;

            _camera.SetActive(false);
            _playerManager._stats.IsEvent = false;
            return false;
        });
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !_isStart)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().OnAction();

            _playerManager._stats.IsAction = true;

            TextBox.instance.isTalkEnd = false;
            _playerManager._stats.IsEvent = true;
            _isStart = true;
        }
    }
}
