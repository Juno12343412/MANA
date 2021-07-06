using System.Collections.Generic;
using UnityEngine;
using Manager.Sound;

public class ActionS2 : ActionEvent
{
    [Header("Text")]
    [SerializeField] private List<string> _textList1;
    [SerializeField] private GameObject _camera;

    void Start()
    {
        AddAction(() =>
        {
            _actorObjs[1].SetActive(true);
            return false;
        });
        
        // S1-2-1
        AddAction(() =>
        {
            if (!IsActorAlive(_actorObjs[1]))
            {
                _playerManager._stats.IsEvent = true;
                _isAwake = false;
                return false;
            }
            return true;
        });

        // S1-2-2
        AddAction(() =>
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>().flipX = true;

            _actorObjs[0].SetActive(true);
            MovingActor(_actorObjs[0], _movingPos[0], 1f);
            return false;
        });

        Delay(2.5f);

        AddAction(() =>
        {
            _actorObjs[0].GetComponent<Animator>().SetBool("isMove", false);

            var v = (GameObject.FindGameObjectWithTag("Player").transform.position + _actorObjs[0].transform.position) * 0.5f;
            _camera.transform.position = new Vector3(v.x, _camera.transform.position.y, -10f);
            _camera.SetActive(true);

            return false;
        });

        Delay(2f);

        AddAction(() =>
        {
            SoundPlayer.instance.PlaySound("Npc1_2", 0.5f);
            return false;
        });

        // S1-2-3
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

        //// S1-2-4
        AddAction(() =>
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>().flipX = false;
            SoundPlayer.instance.PlaySound("Npc1_3", 0.5f);

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

            _isStart = true;
            _isAwake = true;
        }
    }
}
