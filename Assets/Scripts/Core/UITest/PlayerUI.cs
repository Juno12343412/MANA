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
    [SerializeField] private Image _hpGauge;

    [Header("Mana")]
    [SerializeField] private Text _manaText;

    [SerializeField] Player _player;
    void Start()
    {
        _player.MyStats.SpecialAttackDuration = 50;
        _player.MyStats.CurHP = 97;
    }

    void LateUpdate()
    {
        if (_player != null)
        {
            _skillGauge.fillAmount = _player.MyStats.SpecialAttackDuration / 100;
            _hpGauge.fillAmount = _player.MyStats.CurHP / _player.MyStats.MaxHP;
        }
    }
}
