using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UDBase.UI.Common;
using Zenject;
using UDBase.Controllers.ObjectSystem;

public class PlayerUI : UIElement
{
    [Header("SKill")]
    [SerializeField] private Image _skillGauge;
    [SerializeField] private Image _skillImg;

    [Header("HP")]
    [SerializeField] private Image[] _hpGauge;

    [Header("Mana")]
    [SerializeField] private Text _manaText;

    [Inject]
    public PlayerManager _player;

    public float _fakeSkillGauge = 0f;
    public bool _isSkill = false;

    public int _hp = 0;

    void LateUpdate()
    {
        if (_player != null)
        {
            if (_fakeSkillGauge != _player._stats.SpecialAttackDuration && !_isSkill)
                StartCoroutine("SetSkill");

            if (_hp != _player._stats.CurHP)
                SetHP();
        }
    }

    IEnumerator SetSkill()
    {
        _isSkill = true;
        yield return null;

        if (_fakeSkillGauge < _player._stats.SpecialAttackDuration)
        {
            while (_fakeSkillGauge < _player._stats.SpecialAttackDuration)
            {
                _fakeSkillGauge += Time.deltaTime * 100f;

                _skillGauge.fillAmount = _fakeSkillGauge / 100;
                yield return null;
            }

            _fakeSkillGauge = _player._stats.SpecialAttackDuration;
            _isSkill = false;
        }
        else
        {
            while (_fakeSkillGauge > _player._stats.SpecialAttackDuration)
            {
                _fakeSkillGauge -= Time.deltaTime * 100f;

                _skillGauge.fillAmount = _fakeSkillGauge / 100;
                yield return null;
            }

            _fakeSkillGauge = _player._stats.SpecialAttackDuration;
            _isSkill = false;
        }
    }

    void SetHP()
    {
        _hp = (int)_player._stats.CurHP;

        for (int i = 0; i < _hpGauge.Length; i++)
        {
            _hpGauge[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < _hp; i++)
        {
            _hpGauge[i].gameObject.SetActive(true);
        }
    }
}
