using System;
using System.Collections;
using UnityEngine;
using MANA.Enums;
using UDBase.Utils;
using Zenject;
using UDBase.Controllers.LogSystem;

namespace UDBase.Controllers.ObjectSystem
{
    public class AIMachine : MonoBehaviour, IObject, ILogContext
    {
        [Inject]
        ILog _log;

        public string Name { get; protected set; }
        public ObjectKind Kind { get; protected set; }

        /// <summary>
        /// AI의 기본정보
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
            /// 현재 방어력
            /// </summary>
            public int Armor { get; set; }

            /// <summary>
            /// 공격 속도
            /// </summary>
            public float AttackSpeed { get; set; }

            /// <summary>
            /// 이동 속도
            /// </summary>
            public float MoveSpeed { get; set; }

            /// <summary>
            /// 대화 거리
            /// </summary>
            public float Radius { get; set; }

            /// <summary>
            /// 현재 추적중인가 ?
            /// </summary>
            public bool IsTrack { get; set; }

            /// <summary>
            /// 현재 정찰중인가 ?
            /// </summary>
            public bool IsPatrol { get; set; }

            /// <summary>
            /// 현재 공격중인가 ?
            /// </summary>
            public bool IsAttack { get; set; }

            /// <summary>
            /// 현재 대화중인가 ?
            /// </summary>
            public bool IsTalk { get; set; }

            public AIState State { get; set; }

            /// <summary>
            /// 몬스터가 드랍할 아이템
            /// </summary>
            public Item[] DropItems { get; set; }
        }
        public Stats MyStats { get; set; }
        KeySetting aiKey;

        protected GameObject targetObj = null;

        public bool IsDead()
        {
            return MyStats.CurHP <= 0;
        }

        public void Kill()
        {
            MyStats.CurHP = 0;
        }

        protected virtual void Start()
        {

            AISetting(_log);
            StartFSM();
        }

        protected virtual void Update()
        {
            if (IsDead())
                MyStats.State = AIState.Dead;

            CheckTalkRadius();
            aiKey?.Update();
        }

        /// <summary>
        /// AI 셋팅하는 함수
        /// </summary>
        protected virtual void AISetting(ILog log)
        {
            Name = gameObject.name;     // 이름은 나중에 바꿔도 됨

            MyStats = new Stats();
            MyStats.IsTrack = false;

            aiKey = new KeySetting(KeyCode.Q, Talk, KeyKind.Down);
            targetObj = GameObject.FindGameObjectWithTag("Player");
        }

        void StartFSM()
        {

            MyStats.State = AIState.Idle;
            StartCoroutine(MyStats.State.ToString());
        }

        void ChanageState()
        {

            if (MyStats.State != AIState.Dead)
                StartCoroutine(MyStats.State.ToString());
            else
                DeadEvent();
        }

        IEnumerator Idle()
        {

            while (MyStats.State == AIState.Idle)
            {
                IdleEvent();
                yield return null;
            }
            ChanageState();
        }

        IEnumerator Patrol()
        {

            while (MyStats.State == AIState.Patrol)
            {
                PatrolEvent();
                yield return null;
            }
            ChanageState();
        }

        IEnumerator Track()
        {

            while (MyStats.State == AIState.Track)
            {
                TrackEvent();
                yield return null;
            }
            ChanageState();
        }

        IEnumerator Attack()
        {

            while (MyStats.State == AIState.Attack)
            {
                AttackEvent();
                yield return null;
            }
            ChanageState();
        }

        void Talk()
        {

            if (Kind == ObjectKind.NPC)
            {
                if (targetObj != null)
                {
                    if (Vector2.Distance(gameObject.transform.position, targetObj.transform.position) <= MyStats.Radius)
                    {
                        targetObj.GetComponent<PlayerMachine>()._player._stats.State = PlayerState.Talk;
                        Callback(targetObj);
                    }
                }
            }
        }

        void ItemDrop()
        {

            if (MyStats.DropItems != null)
            {
                for (int i = 0; i <= MyStats.DropItems.Length; i++)
                {
                    if (MyStats.DropItems[i] && UnityEngine.Random.Range(0f, 100f) <= MyStats.DropItems[i].Drop)
                    {

                        // 소환
                    }
                }
            }
        }

        void CheckTalkRadius()
        {
            if (Kind == ObjectKind.NPC)
            {
                if (targetObj != null)
                {
                    if (Vector2.Distance(gameObject.transform.position, targetObj.transform.position) <= MyStats.Radius && !MyStats.IsTalk)
                    {
                        MyStats.IsTalk = true;
                        targetObj.GetComponent<PlayerMachine>()._player._stats.State = PlayerState.Talk;
                        CallbackEnter(targetObj);
                    }
                    else if (Vector2.Distance(gameObject.transform.position, targetObj.transform.position) > MyStats.Radius && MyStats.IsTalk)
                    {
                        MyStats.IsTalk = false;
                        CallbackExit(targetObj);
                    }
                }
            }
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                switch (Kind)
                {
                    case ObjectKind.Item:
                        Callback(other.gameObject);
                        break;
                    default:
                        break;
                }
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                switch (Kind)
                {
                    case ObjectKind.Item:
                        Callback(other.gameObject);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 가만히 있을 때 실행하는 함수
        /// </summary>
        protected virtual void IdleEvent()
        {

        }

        /// <summary>
        /// 정찰 하고 있을 때 실행하는 함수
        /// </summary>
        protected virtual void PatrolEvent()
        {

        }

        /// <summary>
        /// 추적하고 있을 때 실행하는 함수
        /// </summary>
        protected virtual void TrackEvent()
        {

        }

        /// <summary>
        /// 해당 AI와 대화를 할 때 실행하는 함수
        /// </summary>
        protected virtual void CallbackEnter(GameObject _pObj)
        {
     
        }

        /// <summary>
        /// 해당 AI와 대화 범위를 나갈 때
        /// </summary>
        protected virtual void CallbackExit(GameObject _pObj)
        {

        }

        /// <summary>
        /// 해당 AI와 대화를 할 때 실행하는 함수
        /// </summary>
        protected virtual void Callback(GameObject _pObj)
        {

        }

        /// <summary>
        /// 때리고 있을 때 실행하는 함수
        /// </summary>
        protected virtual void AttackEvent()
        {

        }

        /// <summary>
        /// 죽을 때 실행하는 함수
        /// </summary>
        protected virtual void DeadEvent()
        {
            ItemDrop();
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