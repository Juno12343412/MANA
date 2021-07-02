using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDBase.Controllers.ObjectSystem;
using UDBase.Controllers.LogSystem;
using UDBase.Controllers.ParticleSystem;
using Cinemachine;
using MANA.Enums;
using Zenject;

public class ActionEvent : MonoBehaviour
{
    ULogger _log;

    [Inject]
    readonly ParticleManager _particleManager;

    [Inject]
    PlayerManager _playerManager;

    [Header("Action")]
    [Tooltip("해당 액션에 필요한 연기자들")]
    [SerializeField] protected GameObject[] _actorObjs = null;

    [Tooltip("액션에서 움직일 위치들")]
    [SerializeField] protected Vector3[] _movingPos = null;

    protected Dictionary<string, Tuple<Action, float>> _actions = new Dictionary<string, Tuple<Action, float>>();

    protected Action _curAction = null;
    protected string _curTake = "";

    /// <summary>
    /// 액션을 추가하는 함수
    /// </summary>
    protected void AddAction(string _take, Action _func, float _duration = 1f)
    {
        Debug.Log("Add : " + _take);

        _actions?.Add(_take, new Tuple<Action, float>(_func, _duration));
    }

    protected void PlayAction(string _take)
    {
        Debug.Log("Play : " + _take + " : " + _actions[_take].Item2);

        _curAction = _actions[_take].Item1;
        _curTake = _take;

        Invoke("WaitAction", _actions[_take].Item2);
    }

    void WaitAction()
    {
        _curAction();
    }

    protected void EndAction()
    {
        Debug.Log("End");

        gameObject.SetActive(false);
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

    protected void MovingActor(string _take, GameObject _obj, Vector3 _pos, float _speed)
    {
        StartCoroutine(Moving(_take, _obj, _pos, _speed));
    }

    IEnumerator Moving(string _take, GameObject _obj, Vector3 _pos, float _speed)
    {
        float distance = Vector3.Distance(_obj.transform.position, _pos);
        float progress = 0f;
        yield return null;

        if (_obj != null)
        {
            while (progress <= distance)
            {
                Debug.Log("Moving ...");

                _obj.transform.position = Vector3.Lerp(_obj.transform.position, _pos, _speed * Time.deltaTime);
                progress += Time.deltaTime;
                yield return null;
            }
        }

        if (_take != "")
            PlayAction(_take);
        else
            EndAction();
    }
}
