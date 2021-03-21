using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDBase.Controllers.LogSystem;
using UDBase.Controllers.ObjectSystem;
using Zenject;
using MANA.Enums;
using UDBase.Utils;

namespace UDBase.Controllers.ObjectSystem
{

    public class ItemMachine : MonoBehaviour, IObject, ILogContext
    {
        [Inject]
        ILog _log;

        public string Name { get; private set; }
        public ObjectKind Kind { get; private set; }

        /// <summary>
        /// 플레이어의 기본정보
        /// </summary>
        [Serializable]
        public class Stats
        {
            public Item Table { get; set; }

            public ItemState State { get; set; }
        }
        public Stats MyStats { get; set; }
        protected Dictionary<string, KeySetting> itemKeys;

        public bool IsDead() {

            return MyStats.Table.CurHP <= 0;
        }

        public void Kill() {

            MyStats.Table.CurHP = 0;
        }

        protected virtual void Start() {

            ItemSetting(_log);
        }

        protected virtual void Update() {

            if (MyStats.State == ItemState.Idle)
                IdleEvent();

            if (IsDead()) {

                MyStats.State = ItemState.Dead;
                DeadEvent();
            }
        }

        /// <summary>
        /// 플레이어 셋팅하는 함수
        /// </summary>
        protected virtual void ItemSetting(ILog log) {

            itemKeys.Add("Eat1", new KeySetting(KeyCode.C, Eat));
            itemKeys.Add("Eat2", new KeySetting(KeyCode.UpArrow, Eat));
            itemKeys.Add("Eat3", new KeySetting(KeyCode.Space, Eat));
        }

        void Eat() {

            if (Kind == ObjectKind.Item) {

                GameObject obj = GameObject.FindGameObjectWithTag("Player");
                if (obj != null) {

                    if (Vector2.Distance(gameObject.transform.position, obj.transform.position) <= MyStats.Table.Radius) {

                        if (MyStats.Table.Explosion)
                            Explosion();
                        
                        Callback();
                    }
                }
            }
        }

        void Explosion() {

        }

        /// <summary>
        /// 죽었을 때 실행하는 함수
        /// </summary>
        protected virtual void IdleEvent() {

            MyStats.State = ItemState.Idle;
        }

        /// <summary>
        /// 죽었을 때 실행하는 함수
        /// </summary>
        protected virtual void DeadEvent() {

            MyStats.State = ItemState.Dead;
            Kill();
        }

        /// <summary>
        /// 먹었을 때 실행 하는 함수
        /// </summary>
        protected virtual void Callback() {

            MyStats.State = ItemState.Dead;
        }
    }
}