using System.Collections.Generic;
using UnityEngine;
using MANA.Enums;
using Manager.Sound;

public class Action5 : ActionEvent
{
    [Header("Text")]
    [SerializeField] private List<string> _textList1;
    [SerializeField] private List<string> _textList2;

    [SerializeField] private GameObject _camera = null;
    [SerializeField] private GameObject _boss = null;

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
            if (TextBox.instance.isTalkEnd)
            {
                TextBox.instance.isTalkEnd = false;
                return false;
            }
            TextBox.instance.SetTalk(_textList1, false, _actorObjs[0].transform.position);
            return true;
        });

        Delay(2f);

        AddAction(() =>
        {
            SoundPlayer.instance.PlaySound("Npc1_1");
            return false;
        });

        AddAction(() =>
        {
            if (TextBox.instance.isTalkEnd)
            {
                TextBox.instance.isTalkEnd = false;
                return false;
            }
            TextBox.instance.SetTalk(_textList2, false, _actorObjs[0].transform.position);
            return true;
        });

        Delay(2f);

        AddAction(() =>
        {
            _playerManager._stats.IsAction = false;

            _camera.SetActive(false);
            _playerManager._stats.IsEvent = false;

            // 여기에 보스 시작 넣으삼
            _boss.SetActive(true);
            //gameObject.SetActive(false);

            EndAction();
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
