using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDBase.Controllers.ObjectSystem;
using UDBase.Controllers.ParticleSystem;
using UDBase.UI.Common;
using Zenject;

public class ActionEvent : MonoBehaviour
{
    [Inject]
    protected readonly ParticleManager _particleManager;

    [Inject]
    protected readonly UIManager _uiManager;

    [Inject]
    protected PlayerManager _playerManager;


    [Header("Action")]
    [Tooltip("해당 액션에 필요한 연기자들")]
    [SerializeField] protected GameObject[] _actorObjs = null;

    [Tooltip("액션에서 움직일 위치들")]
    [SerializeField] protected Vector3[] _movingPos = null;

    protected Queue<Tuple<Func<bool>, float>> _actions = new Queue<Tuple<Func<bool>, float>>();

    protected bool  _isAwake = false;
    protected bool  _isStart = false;
    protected float _actionTime = 0f;

    protected void AddAction(Func<bool> _func, float _duration = 0f)
    {
        _actions.Enqueue(new Tuple<Func<bool>, float>(_func, _duration));
    }

    protected bool Empty()
    {
        if (_actions.Count != 0)
            return false;
        return true;
    }

    protected void ClearAction()
    {
        _actions.Clear();
    }

    protected void Delay(float _duration)
    {
        _actions.Enqueue(new Tuple<Func<bool>, float>(() => { return true; }, _duration));
    }

    protected void UpdateAction()
    {
        if (!_isAwake)
        {
            _isAwake = true;
            _uiManager.Find("BaseUI").GetComponent<Animator>().SetInteger("isTalk", 0);
        }

        if (!Empty())
        {
            _actionTime += Time.deltaTime;
            var scd = _actions.Peek();

            if (scd.Item1() == false || (scd.Item2 != 0f && _actionTime > scd.Item2))
            {
                _actionTime = 0f;
                if (!Empty())
                    _actions.Dequeue();
            }
        }
    }

    protected void EndAction()
    {
        _playerManager._stats.IsAction = false;

        _isStart = false;
        ClearAction();
        gameObject.SetActive(false);

        _playerManager._stats.IsEvent = false;
        _uiManager.Find("BaseUI").GetComponent<Animator>().SetInteger("isTalk", 1);
    }

    protected bool IsActorAlive(GameObject _obj)
    {
        foreach (var obj in _actorObjs)
        {
            if (obj == _obj)
            {
                if (obj.activeSelf)
                    return true;
                else
                    return false;
            }
        }
        return false;
    }

    protected void MovingActor(GameObject _obj, Vector3 _pos, float _speed)
    {
        StartCoroutine(Moving(_obj, _pos, _speed));
    }

    IEnumerator Moving(GameObject _obj, Vector3 _pos, float _speed)
    {
        float distance = Vector3.Distance(_obj.transform.localPosition, _pos);
        float progress = 0f;

        _obj.GetComponent<Animator>().SetBool("isMove", true);

        if (GameObject.FindGameObjectWithTag("Player").transform.localPosition.x < transform.position.x)
        {
            _obj.GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (GameObject.FindGameObjectWithTag("Player").transform.localPosition.x > transform.position.x)
        {
            _obj.GetComponent<SpriteRenderer>().flipX = true;
        }
        yield return null;

        if (_obj != null)
        {
            while (progress <= distance)
            {
                Debug.Log("Moving ...");

                _obj.transform.localPosition = Vector3.Lerp(_obj.transform.localPosition, _pos, _speed * Time.deltaTime);
                progress += Time.deltaTime;
                yield return null;
            }
        }
        _obj.GetComponent<Animator>().SetBool("isMove", false);
    }

    void Update()
    {
        if (_isStart)
            UpdateAction();
    }
}
