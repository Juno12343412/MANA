using System.Collections.Generic;
using UnityEngine;
using Manager.Sound;

public class ActionS1 : ActionEvent
{
    [Header("Text")]
    [SerializeField] private List<string> _textList1;
    [SerializeField] private GameObject _camera;

    void Start()
    {
        Delay(2f);

        // S1-1-1
        AddAction(() =>
        {
            MovingActor(_actorObjs[0], _movingPos[0], 0.5f);
            return false;
        }, 1f);

        Delay(4f);

        AddAction(() =>
        {
            _actorObjs[0].GetComponent<Animator>().SetBool("isMove", false);
            _camera.SetActive(true);
            return false;
        });

        Delay(2f);

        AddAction(() =>
        {
            SoundPlayer.instance.PlaySound("Npc1_1", 0.5f);
            return false;
        });

        // S1-1-2
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

        //// S1-1-3
        AddAction(() =>
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>().flipX = true;

            MovingActor(_actorObjs[0], _movingPos[1], 0.5f);
            return false;
        });

        Delay(4f);

        AddAction(() =>
        {
            _camera.SetActive(false);
            EndAction();
            return false;
        });
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().OnAction();

            _playerManager._stats.IsAction = true;
            _playerManager._stats.IsEvent = true;
            _isStart = true;
        }
    }
}
