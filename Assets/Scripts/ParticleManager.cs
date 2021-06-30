using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pooling;
using Zenject;
using UDBase.Controllers.LogSystem;
using MANA.Enums;

namespace UDBase.Controllers.ParticleSystem
{
    public class ParticleManager : MonoBehaviour
    {
        [Serializable]
        public class Settings
        {
            [Tooltip("풀링할 오브젝트")]
            public GameObject _poolingObject;

            [Tooltip("부모 오브젝트")]
            public Transform _parentObject;
        }
        [SerializeField] private Settings _settings = new Settings();

        ObjectPool<Particle> _particlePool = new ObjectPool<Particle>();

        ILog _log;

        [Inject]
        public void Init(ILog log)
        {
            _settings._poolingObject = GameObject.Find("FX_Particles");
            _settings._parentObject  = GameObject.Find("Particles").transform;

            _particlePool.Init(_settings._poolingObject, 10, Vector3.zero, Quaternion.identity, _settings._parentObject);
            _log = log;
        }

        public void ShowParticle(ParticleKind _kind, Vector3? _pos = null, Quaternion? _rot = null, int _amount = 1)
        {
            if (_amount > 1) {
                for (int i = 0; i < _amount; i++) {

                    _particlePool.Find()._kind = _kind; 
                    _particlePool?.Spawn(_pos ?? Vector3.zero, _rot ?? Quaternion.identity, null);
                }
                return;
            } else {
                _particlePool.Find()._kind = _kind;
                _particlePool?.Spawn(_pos ?? Vector3.zero, _rot ?? Quaternion.identity, null);
            }
        }
    }
}