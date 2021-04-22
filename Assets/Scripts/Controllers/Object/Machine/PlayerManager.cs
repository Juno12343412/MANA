using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDBase.Controllers.LogSystem;
using MANA.Enums;
using Zenject;

public class PlayerManager : MonoBehaviour
{
    public string Name { get; private set; }
    public ObjectKind Kind { get; private set; }

    /// <summary>
    /// 플레이어의 기본정보
    /// </summary>
    [Serializable]
    public class Stats
    {

        /// <summary>
        /// 현재 체력
        /// </summary>
        public float CurHP { get; set; }

        /// <summary>
        /// 최대 체력
        /// </summary>
        public float MaxHP { get; set; }

        /// <summary>
        /// 공격력
        /// </summary>
        public int Damage { get; set; }

        /// <summary>
        /// 공격 쿨타임
        /// </summary>
        public float AttackDuration { get; set; }

        /// <summary>
        /// 특수공격 쿨타임
        /// </summary>
        public float SpecialAttackDuration { get; set; }

        /// <summary>
        /// 공격 속도
        /// </summary>
        public float AttackSpeed { get; set; }

        /// <summary>
        /// 이동 속도
        /// </summary>
        public float MoveSpeed { get; set; }

        /// <summary>
        /// 점프하는 힘
        /// </summary>
        public float JumpPower { get; set; }

        /// <summary>
        /// 현재 공격중인가 ?
        /// </summary>
        public bool IsAttack { get; set; }

        /// <summary>
        /// 현재 특수공격중인가 ?
        /// </summary>
        public bool IsSpecialAttack { get; set; }

        /// <summary>
        /// 현재 점프중인가 ?
        /// </summary>
        public bool IsJump { get; set; }

        /// <summary>
        /// 현재 대화중인가 ?
        /// </summary>
        public bool IsTalk { get; set; }

        /// <summary>
        /// 현재 움직이는 중인가 ?
        /// </summary>
        public bool IsMove { get; set; }

        /// <summary>
        /// 마나를 가지고 있는가 ?
        /// </summary>
        public bool IsMana { get; set; }

        public PlayerState State { get; set; }
    }
    public Stats _stats;

    ILog _log;

    [Inject]
    public void Init(Stats stats, ILog log)
    {
        Debug.Log("Init");

        _stats = stats;
        _log = log;
    }
}