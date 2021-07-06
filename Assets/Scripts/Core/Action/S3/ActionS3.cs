using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using MANA.Enums;
using Manager.Sound;

public class ActionS3 : ActionEvent
{
    [Header("Text")]
    [SerializeField] private List<string> _textList1;
    [SerializeField] private List<string> _textList2;
    [SerializeField] private List<string> _textList3;
    [SerializeField] private List<string> _textList4;
    [SerializeField] private List<string> _textList5;

    [SerializeField] private GameObject _camera = null;
    [SerializeField] private CinemachineVirtualCamera _camera2 = null;

    void Start()
    {
        Delay(2f);

        AddAction(() =>
        {
            TextBox.instance.isTalkEnd = false;
            _camera.SetActive(true);
            return false;
        });

        Delay(2f);

        AddAction(() =>
        {
            SoundPlayer.instance.PlaySound("Npc2_1");
            return false;
        });

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

        Delay(0.1f);

        AddAction(() =>
        {
            _playerManager._stats.IsEvent = true;
            return false;
        });

        Delay(2f);

        AddAction(() =>
        {
            _playerManager._stats.IsEvent = true;

            MovingActor(_actorObjs[1], _movingPos[0], 0.5f);
            _actorObjs[1].GetComponent<SpriteRenderer>().flipX = false;
            return false;
        });

        Delay(5f);

        AddAction(() =>
        {
            SoundPlayer.instance.PlaySound("Npc1_3");
            return false;
        });

        AddAction(() =>
        {
             _actorObjs[1].GetComponent<Animator>().SetBool("isMove", false);
            return false;
        });

        AddAction(() =>
        {
            if (TextBox.instance.isTalkEnd)
            {
                TextBox.instance.isTalkEnd = false;
                return false;
            }
            TextBox.instance.SetTalk(_textList2, false, _actorObjs[1].transform.position);
            return true;
        });

        Delay(0.1f);

        AddAction(() =>
        {
            _playerManager._stats.IsEvent = true;
            return false;
        });

        Delay(1f);

        AddAction(() =>
        {
            _actorObjs[1].GetComponent<SpriteRenderer>().flipX = true;
            return false;
        });

        Delay(1f);

        AddAction(() =>
        {
            _actorObjs[1].GetComponent<SpriteRenderer>().flipX = false;
            return false;
        });

        Delay(1f);

        AddAction(() =>
        {
            _actorObjs[1].GetComponent<SpriteRenderer>().flipX = true;
            return false;
        });

        Delay(1f);

        AddAction(() =>
        {
            _actorObjs[1].GetComponent<SpriteRenderer>().flipX = false;
            return false;
        });

        Delay(1f);

        AddAction(() =>
        {
            _actorObjs[1].GetComponent<SpriteRenderer>().flipX = true;
            return false;
        });

        Delay(1f);

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
            TextBox.instance.SetTalk(_textList3, false, _actorObjs[1].transform.position);
            return true;
        });

        Delay(2f);

        AddAction(() =>
        {
            SoundPlayer.instance.PlaySound("E1_walk");
            return false;
        });

        AddAction(() =>
        {
            _actorObjs[0].GetComponent<Rigidbody2D>().AddForce(Vector2.up * 12.5f, ForceMode2D.Impulse);
            _particleManager.ShowParticle(ParticleKind.Move, _actorObjs[0].transform.position, null, 20);
            return false;
        });

        Delay(2f);

        AddAction(() =>
        {
            SoundPlayer.instance.PlaySound("Npc2_2");
            return false;
        });

        AddAction(() =>
        {
            if (TextBox.instance.isTalkEnd)
            {
                TextBox.instance.isTalkEnd = false;
                return false;
            }
            TextBox.instance.SetTalk(_textList4, false, _actorObjs[0].transform.position);
            return true;
        });

        Delay(1f);

        AddAction(() =>
        {
            _actorObjs[1].GetComponent<SpriteRenderer>().flipX = false;
            return false;
        });

        Delay(1f);

        AddAction(() =>
        {
            SoundPlayer.instance.PlaySound("Npc1_2");
            return false;
        });

        AddAction(() =>
        {
            if (TextBox.instance.isTalkEnd)
            {
                TextBox.instance.isTalkEnd = false;
                return false;
            }
            TextBox.instance.SetTalk(_textList5, false, _actorObjs[1].transform.position);
            return true;
        });

        Delay(1f);

        AddAction(() =>
        {
            _playerManager._stats.IsEvent = true;

            MovingActor(_actorObjs[1], _movingPos[1], 0.5f);
            _actorObjs[1].GetComponent<SpriteRenderer>().flipX = true;
            return false;
        });

        Delay(3f);

        AddAction(() =>
        {
            _playerManager._stats.IsAction = false;
            _playerManager._stats.IsEvent = false;
            //_camera2.m_Lens.OrthographicSize = 12f;
            _camera.SetActive(false);
            return false;
        });
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !_isStart)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().OnAction();

            _playerManager._stats.IsAction = true;

            _playerManager._stats.IsEvent = true;
            _isStart = true;
        }
    }
}
