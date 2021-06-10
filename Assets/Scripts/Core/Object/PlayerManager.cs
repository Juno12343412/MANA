using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDBase.Controllers.LogSystem;
using MANA.Enums;
using Zenject;

namespace UDBase.Controllers.ObjectSystem
{
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
            public float CurHP;

            /// <summary>
            /// 최대 체력
            /// </summary>
            public float MaxHP;

            /// <summary>
            /// 공격력
            /// </summary>
            public int Damage;

            /// <summary>
            /// 공격 쿨타임
            /// </summary>
            public float AttackDuration;

            /// <summary>
            /// 특수공격 쿨타임
            /// </summary>
            public float SpecialAttackDuration;

            /// <summary>
            /// 공격 속도
            /// </summary>
            public float AttackSpeed;

            /// <summary>
            /// 이동 속도
            /// </summary>
            public float MoveSpeed;

            /// <summary>
            /// 점프하는 힘
            /// </summary>
            public float JumpPower;

            /// <summary>
            /// 현재 공격중인가 ?
            /// </summary>
            public bool IsAttack;

            /// <summary>
            /// 현재 특수공격중인가 ?
            /// </summary>
            public bool IsSpecialAttack;

            /// <summary>
            /// 현재 점프중인가 ?
            /// </summary>
            public bool IsJump;

            /// <summary>
            /// 현재 대화중인가 ?
            /// </summary>
            public bool IsTalk;

            /// <summary>
            /// 현재 움직이는 중인가 ?
            /// </summary>
            public bool IsMove;

            /// <summary>
            /// 마나를 가지고 있는가 ?
            /// </summary>
            public bool IsMana;

            public PlayerState State;
        }
        public Stats _stats;

        ILog _log;

        [Inject]
        public void Init(Stats stats, ILog log)
        {
            _stats = stats;
            _log = log;
        }
    }
}