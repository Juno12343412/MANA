using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDBase.Controllers.ObjectSystem;
using UDBase.Controllers.LogSystem;
using UDBase.Controllers.ParticleSystem;
using Cinemachine;
using MANA.Enums;
using Zenject;

public class Player : PlayerMachine
{
    ULogger _log;

    public Vector2 _attackDir = Vector2.zero;

    [SerializeField] private bool _isStart = false;

    [SerializeField] private float _comboGauge = 0;

    [SerializeField] private Collider2D[] _attackColiders;

    [SerializeField] private Sprite _startImg;
    [SerializeField] private Sprite _upImg;

    [SerializeField] private GameObject _playerEffect = null;

    private SpriteRenderer _renderer;
    private Animator _animtor;
    private Rigidbody2D _rigid2D;

    bool isStart = false;
    bool isDash = false;
    bool isGrand = false;

    [Inject]
    readonly ParticleManager _particleManager;

    protected sealed override void PlayerSetting(ILog log)
    {
        _log = log.CreateLogger(this);
        base.PlayerSetting(log);

        _player._stats.CurHP = _player._stats.MaxHP = 7;
        _player._stats.Damage = _player._stats.Damage = 10;
        _player._stats.AttackDuration = _player._stats.AttackDuration = 2;
        _player._stats.SpecialAttackDuration = _player._stats.SpecialAttackDuration = 1000;
        _player._stats.AttackSpeed = _player._stats.AttackSpeed = 2;
        _player._stats.JumpPower = 12;
        _player._stats.MoveSpeed = 7;

        _renderer = GetComponent<SpriteRenderer>();
        _rigid2D = GetComponent<Rigidbody2D>();
        _animtor = GetComponent<Animator>();
        _animtor.enabled = false;

        _renderer.sprite = _startImg;

        if (_isStart)
        {
            isStart = true;
            _animtor.enabled = true;
        }
    }

    protected sealed override void IdleEvent()
    {
        base.IdleEvent();

        if (!isStart)
            return;

        _animtor.SetBool("isWalk", false);
    }

    protected sealed override void WalkEvent()
    {
        if (!isStart)
            return;

        if (!_player._stats.IsAttack && !isDash)
        {
            base.WalkEvent();

            // 움직이는 로직
            float x = Input.GetAxisRaw("Horizontal");       

            _attackColiders[0].offset = new Vector2(x, 0);
            if (x != 0)
            {
                if (!_animtor.GetBool("isWalk"))
                {

                    _animtor.SetBool("isWalk", true);
                }
                
                if (!_player._stats.IsJump)
                    _particleManager.ShowParticle(ParticleKind.Move, transform.position);

                _renderer.flipX = x == 1 ? false : true;

                _attackColiders[0].GetComponent<BoxCollider2D>().offset = new Vector2(x, 0);

                _rigid2D.velocity += new Vector2(x, 0) * _player._stats.MoveSpeed * 0.5f;

                _attackDir.x = x;

                if (_rigid2D.velocity.x > _player._stats.MoveSpeed)
                    _rigid2D.velocity = new Vector2(_player._stats.MoveSpeed, _rigid2D.velocity.y);
                else if (_rigid2D.velocity.x < -_player._stats.MoveSpeed)
                    _rigid2D.velocity = new Vector2(-_player._stats.MoveSpeed, _rigid2D.velocity.y);

                if (Input.GetKeyDown(KeyCode.DownArrow) && _player._stats.IsDash && _player._stats.SpecialAttackDuration >= 20f) {
                    _particleManager.ShowParticle(ParticleKind.Move, transform.position, null, 20);
                    Dash(new Vector2(x, 0));
                }
            }
            else
            {
                _animtor.SetBool("isWalk", false);

                _rigid2D.velocity = new Vector2(0, _rigid2D.velocity.y);
                _player._stats.State = PlayerState.Idle;
            }
        }
    }

    protected sealed override void RunEvent()
    {
        base.RunEvent();

        if (!isStart)
            return;
        // 움직이는 로직
    }

    protected sealed override void JumpEvent()
    {
        if (!isStart)
            return;

        // 점프하는 로직
        if (!_player._stats.IsJump && _rigid2D.velocity.y <= 0.1f)
        {
            base.JumpEvent();

            _player._stats.IsJump = true;
            StartCoroutine(GetJumpForce(1f));
            //StartCoroutine(GradientCheck());
        }
    }

    protected sealed override void TalkEvent()
    {
        base.TalkEvent();

        if (!isStart)
            return;
    }

    protected sealed override void AttackEvent()
    {
        base.AttackEvent();

        if (!isStart && _player._stats.IsAttack)
            return;

        _player._stats.IsAttack = true;

        _attackDir.y = Input.GetAxisRaw("Vertical");
        
        _animtor.SetBool("isAttack", true);
        _animtor.SetFloat("AttackY", _attackDir.y);
    }

    protected sealed override void SpecialAttackEvent()
    {
        base.SpecialAttackEvent();

        if (!isStart)
            return;

        if (_player._stats.IsJump)
        {
            _player._stats.State = PlayerState.Idle;
            return;
        }
    }

    protected sealed override void DeadEvent()
    {
        base.DeadEvent();

        if (!isStart)
            return;
    }

    IEnumerator Combo(float time = 0f)
    {
        bool attack = false;
        float progress = 0f;
        yield return null;

        while (progress <= time)
        {
            // 방향 공격
            if (_player._stats.State == PlayerState.Attack)
            {
                if ((Input.GetKeyDown(KeyCode.Z) || Input.GetKey(KeyCode.Z)) && _player._stats.IsAttack)
                {
                    attack = true;
                    break;
                }
            }
            progress += Time.deltaTime;
            yield return null;
        }
        if (attack)
        {
            if (_comboGauge != 2)
                _comboGauge++;
            else
                _comboGauge = 0;
            _animtor.SetFloat("Attack", _comboGauge);
        }
    }

    public void AnimComboEvent()
    {
        StartCoroutine(Combo(1.2f));
    }

    protected override void AnimFrameStart()
    {

    }

    protected override void AnimFrameUpdate()
    {
        _attackColiders[0].gameObject.SetActive(true);
    }

    protected override void AnimFrameEnd()
    {
        if (_animtor.GetBool("isStart") && isGrand) {
            _animtor.SetBool("isStart", false);
            isGrand = false;
            isStart = true;
        }
        else
        {
            _player._stats.IsAttack = false;
            _attackColiders[0].gameObject.SetActive(false);

            if (_animtor.GetInteger("Jump") == 2)
            {
                _animtor.SetInteger("Jump", 0);
                _player._stats.IsJump = false;
            }

            _animtor.SetBool("isAttack", false);
            _animtor.SetFloat("Attack", 0);
            _animtor.SetInteger("AttackY", 0);
            _animtor.SetBool("isHurt", false);
        }
    }

    IEnumerator GradientCheck()
    {
        Debug.Log("그라디언트 체크");

        float progress = 0f;
        yield return null;
    
        while (progress <= 4.5f && _player._stats.IsJump)
        {
            progress += Time.deltaTime;
            yield return null;
        }
        
        if (_player._stats.IsJump && !isGrand)
        {
            Debug.Log("그라디언트 체크 끝");
            isGrand = true;
        }
    }

    IEnumerator GetJumpForce(float time = 1f)
    {
        // 점프 로직
        float progress = 0f;

        _animtor.SetInteger("Jump", 1);
        _renderer.sprite = _upImg;
        _rigid2D.AddForce(Vector3.up * _player._stats.JumpPower, ForceMode2D.Impulse);
        _particleManager.ShowParticle(ParticleKind.Move, transform.position, null, 20);
        yield return null;

        while (progress <= time)
        {
            if (Input.GetKey(KeyCode.C) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space))
            {
                _rigid2D.AddForce(Vector2.up * _player._stats.JumpPower * Time.deltaTime, ForceMode2D.Impulse);
                //_renderer.sprite = _upImg;

                progress += Time.deltaTime;
                yield return null;
            }
            else if (Input.GetKeyUp(KeyCode.C) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.Space))
            {
                yield return new WaitForSeconds(0.25f);
                break;
            }
        }
        if (!isGrand)
            _animtor.SetInteger("Jump", 2);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (!isStart)
            return;

        if (other.gameObject.CompareTag("Ground"))
        {
            if (!isGrand)
            {
                _animtor.SetInteger("Jump", 0);
                _player._stats.IsJump = false;
            }
            else
            {
                _animtor.SetInteger("Jump", 0);
                _player._stats.IsJump = false;
                _animtor.SetBool("isWalk", false);
                _animtor.SetBool("isStart", true);
                isStart = false;

                _particleManager?.ShowParticle(ParticleKind.Move, transform.position, null, 25);
            }
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (!isStart)
            return;

        if (other.gameObject.CompareTag("Ground"))
        {
            if (!isGrand)
            {
                StartCoroutine(GradientCheck());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("EnemyAttack") && !_animtor.GetBool("isHurt"))
        {
            other.gameObject.SetActive(false);

            GetComponent<CinemachineImpulseSource>().GenerateImpulse();
            _rigid2D.AddForce(new Vector2(-_attackDir.x, 0f) * 15f, ForceMode2D.Impulse);

            _animtor.SetBool("isHurt", true);
        }
    }

    public void StartEvent()
    {
        if (_isStart)
            return;

        isStart = true;

        _animtor.enabled = true;
        _animtor.SetBool("isStart", true);
    }

    void Dash(Vector2 _dir)
    {
        CancelInvoke();

        _particleManager?.ShowParticle(ParticleKind.Dash, transform.position, null, 5);

        ParticleSystem.MainModule r = _playerEffect.GetComponentInChildren<ParticleSystem>().main;
        r.startRotationY = _renderer.flipX == true ? 180f * Mathf.Deg2Rad : 0f;

        _playerEffect.SetActive(false);
        _playerEffect.SetActive(true);

        isDash = true;

        _player._stats.SpecialAttackDuration -= 20f;

        if (_player._stats.SpecialAttackDuration <= 0f)
            _player._stats.SpecialAttackDuration = 0f;

        GetComponent<CinemachineImpulseSource>().GenerateImpulse();
        _rigid2D.velocity = new Vector2(0f, _rigid2D.velocity.y);
        _rigid2D.AddForce(_dir * _player._stats.MoveSpeed * 4f, ForceMode2D.Impulse);

        Invoke("DashEnd", 0.15f);
        Invoke("DashEffectEnd", 1f);
    }

    void DashEffectEnd()
    {
        _playerEffect.SetActive(false);
    }

    void DashEnd()
    {
        isDash = false;
    }
}