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

            public AIState State { get; set; }

            /// <summary>
            /// 몬스터가 드랍할 아이템
            /// </summary>
            public Item[] DropItems { get; set; }
        }
        public Stats MyStats { get; set; }
        KeySetting aiKey;

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

            aiKey?.Update();

            if (IsDead())
            {
                MyStats.State = AIState.Dead;
            }
        }

        /// <summary>
        /// AI 셋팅하는 함수
        /// </summary>
        protected virtual void AISetting(ILog log)
        {

            Name = gameObject.name;     // 이름은 나중에 바꿔도 됨

            MyStats = new Stats();
            MyStats.IsTrack = false;

            aiKey = new KeySetting(KeyCode.Q, Talk);
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
                GameObject obj = GameObject.FindGameObjectWithTag("Player");
                if (obj != null)
                {

                    if (Vector2.Distance(gameObject.transform.position, obj.transform.position) <= MyStats.Radius)
                    {

                        obj.GetComponent<PlayerMachine>().MyStats.State = PlayerState.Talk;
                        Callback();
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

        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (Kind == ObjectKind.Item)
                {
                    Callback();
                }
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (Kind == ObjectKind.Item)
                {
                    Callback();
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
        protected virtual void Callback()
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
    }
}