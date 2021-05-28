using System;
using System.Collections.Generic;
using UnityEngine;
using MANA.Enums;
using UDBase.Utils;
using System.Collections;
using UDBase.Controllers.LogSystem;
using Zenject;

namespace UDBase.Controllers.ObjectSystem {

    public class PlayerMachine : MonoBehaviour, IObject, ILogContext
    {
        [Inject]
        ILog _log;

        public string Name { get; private set; }
        public ObjectKind Kind { get; private set; }

        ///// <summary>
        ///// 플레이어의 기본정보
        ///// </summary>
        //[Serializable]
        //public class Stats {

        //    /// <summary>
        //    /// 현재 체력
        //    /// </summary>
        //    public float CurHP { get; set; }

        //    /// <summary>
        //    /// 최대 체력
        //    /// </summary>
        //    public float MaxHP { get; set; }

        //    /// <summary>
        //    /// 공격력
        //    /// </summary>
        //    public int Damage { get; set; }

        //    /// <summary>
        //    /// 공격 쿨타임
        //    /// </summary>
        //    public float AttackDuration { get; set; }

        //    /// <summary>
        //    /// 특수공격 쿨타임
        //    /// </summary>
        //    public float SpecialAttackDuration { get; set; }

        //    /// <summary>
        //    /// 공격 속도
        //    /// </summary>
        //    public float AttackSpeed { get; set; }

        //    /// <summary>
        //    /// 이동 속도
        //    /// </summary>
        //    public float MoveSpeed { get; set; }

        //    /// <summary>
        //    /// 점프하는 힘
        //    /// </summary>
        //    public float JumpPower { get; set; }

        //    /// <summary>
        //    /// 현재 공격중인가 ?
        //    /// </summary>
        //    public bool IsAttack { get; set; }

        //    /// <summary>
        //    /// 현재 특수공격중인가 ?
        //    /// </summary>
        //    public bool IsSpecialAttack { get; set; }

        //    /// <summary>
        //    /// 현재 점프중인가 ?
        //    /// </summary>
        //    public bool IsJump { get; set; }

        //    /// <summary>
        //    /// 현재 대화중인가 ?
        //    /// </summary>
        //    public bool IsTalk { get; set; }

        //    /// <summary>
        //    /// 현재 움직이는 중인가 ?
        //    /// </summary>
        //    public bool IsMove { get; set; }

        //    /// <summary>
        //    /// 마나를 가지고 있는가 ?
        //    /// </summary>
        //    public bool IsMana { get; set; }

        //    public PlayerState State { get; set; }
        //}
        //public Stats MyStats;
        
        [Inject]
        public PlayerManager _player;
        protected Dictionary<string, KeySetting> playerKeys;

        public bool IsDead() {
            return _player._stats.CurHP <= 0;
        }

        public void Kill() {
            _player._stats.CurHP = 0;
        }

        protected virtual void Start() {

            PlayerSetting(_log);
        }

        protected virtual void Update() {

            if (!Input.anyKey)
                IdleEvent();

            foreach (var key in playerKeys)
                key.Value.Update();

            //if (IsDead()) {

            //    MyStats.State = PlayerState.Dead;
            //    DeadEvent();
            //}
        }

        /// <summary>
        /// 플레이어 셋팅하는 함수
        /// </summary>
        protected virtual void PlayerSetting(ILog log) {

            Debug.Log("IN");

            _player._stats = new PlayerManager.Stats();
            playerKeys = new Dictionary<string, KeySetting>();

            Name = gameObject.name;     // 이름은 나중에 바꿔도 됨
            Kind = ObjectKind.Player;
            _player._stats.State = PlayerState.Idle;

            playerKeys.Add("MoveL", new KeySetting(KeyCode.LeftArrow, WalkEvent));
            playerKeys.Add("MoveR", new KeySetting(KeyCode.RightArrow, WalkEvent));
            
            playerKeys.Add("Attack", new KeySetting(KeyCode.Z, AttackEvent, KeyKind.Down));
            playerKeys.Add("SpecialAttack", new KeySetting(KeyCode.X, SpecialAttackEvent, KeyKind.Down));

            playerKeys.Add("Jump1", new KeySetting(KeyCode.C, JumpEvent, KeyKind.Down));
            playerKeys.Add("Jump2", new KeySetting(KeyCode.Space, JumpEvent, KeyKind.Down));
            playerKeys.Add("Jump3", new KeySetting(KeyCode.UpArrow, JumpEvent, KeyKind.Down));

            playerKeys.Add("Option", new KeySetting(KeyCode.Escape, OptionEvent, KeyKind.Down));
        }

        /// <summary>
        /// 가만히 있을 때 실행하는 함수
        /// </summary>
        protected virtual void IdleEvent() {

            _player._stats.IsAttack = false;
            _player._stats.IsSpecialAttack = false;
            _player._stats.IsMove = false;

            _player._stats.State = PlayerState.Idle;
        }

        /// <summary>
        /// 걷고 있을 때 실행하는 함수
        /// </summary>
        protected virtual void WalkEvent() {

            _player._stats.State = PlayerState.Walk;
        }

        /// <summary>
        /// 달리고 있을 때 실행하는 함수
        /// </summary>
        protected virtual void RunEvent() {

            _player._stats.State = PlayerState.Run;
        }

        /// <summary>
        /// 점프하고 있을 때 실행하는 함수
        /// </summary>
        protected virtual void JumpEvent() {

            _player._stats.State = PlayerState.Jump;
        }

        /// <summary>
        /// 대화하고 있을 때 실행하는 함수
        /// </summary>
        protected virtual void TalkEvent() {

            _player._stats.State = PlayerState.Talk;
        }

        /// <summary>
        /// 때릴 때 실행하는 함수
        /// </summary>
        protected virtual void AttackEvent() {

            _player._stats.State = PlayerState.Attack;
        }

        /// <summary>
        /// 때릴 때 실행하는 함수
        /// </summary>
        protected virtual void SpecialAttackEvent() {

            _player._stats.State = PlayerState.SpecialAttack;
        }

        /// <summary>
        /// 죽었을 때 실행하는 함수
        /// </summary>
        protected virtual void DeadEvent() {

            _player._stats.State = PlayerState.Dead;
        }

        /// <summary>
        /// 옵션 키를 눌렀을 때의 함수
        /// </summary>
        protected virtual void OptionEvent() {

            _player._stats.State = PlayerState.Option;
        }

        protected virtual void AnimFrameStart()
        {

        }

        protected virtual void AnimFrameUpdate()
        {

        }

        protected virtual void AnimFrameEnd()
        {
        }
    }
}