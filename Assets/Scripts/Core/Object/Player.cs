using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDBase.Controllers.ObjectSystem;
using UDBase.Controllers.LogSystem;
using Cinemachine;
using MANA.Enums;

public class Player : PlayerMachine
{
    ULogger _log;

    public Vector2 _attackDir = Vector2.zero;

    [SerializeField] private float _comboGauge = 0;

    [SerializeField] private Collider2D[] _attackColiders;

    [SerializeField] private Sprite _startImg;
    [SerializeField] private Sprite _upImg;

    private SpriteRenderer _renderer;
    private Animator _animtor;
    private Rigidbody2D _rigid2D;

    bool isStart = false;

    protected sealed override void PlayerSetting(ILog log)
    {
        _log = log.CreateLogger(this);
        base.PlayerSetting(log);

        _player._stats.CurHP = _player._stats.MaxHP = 100;
        _player._stats.Damage = _player._stats.Damage = 10;
        _player._stats.AttackDuration = _player._stats.AttackDuration = 2;
        _player._stats.SpecialAttackDuration = _player._stats.SpecialAttackDuration = 100;
        _player._stats.AttackSpeed = _player._stats.AttackSpeed = 2;
        _player._stats.JumpPower = 5;
        _player._stats.MoveSpeed = 5;

        _renderer = GetComponent<SpriteRenderer>();
        _rigid2D = GetComponent<Rigidbody2D>();
        _animtor = GetComponent<Animator>();
        _animtor.enabled = false;

        _renderer.sprite = _startImg;

        Invoke("StartEvent", 5f);
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
        if (!_player._stats.IsAttack)
        {
            base.WalkEvent();

            // 움직이는 로직
            float x = Input.GetAxisRaw("Horizontal");
            _attackColiders[0].offset = new Vector2(x, 0);
            if (x != 0)
            {
                _animtor.SetBool("isWalk", true);

                _renderer.flipX = x == 1 ? false : true;

                _attackColiders[0].GetComponent<BoxCollider2D>().offset = new Vector2(x, 0);

                _rigid2D.velocity += new Vector2(x, 0) * _player._stats.MoveSpeed * 0.05f;

                _attackDir.x = x;

                if (_rigid2D.velocity.x > _player._stats.MoveSpeed)
                    _rigid2D.velocity = new Vector2(_player._stats.MoveSpeed, _rigid2D.velocity.y);
                else if (_rigid2D.velocity.x < -_player._stats.MoveSpeed)
                    _rigid2D.velocity = new Vector2(-_player._stats.MoveSpeed, _rigid2D.velocity.y);
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
        if (!_player._stats.IsJump)
        {
            base.JumpEvent();

            _player._stats.IsJump = true;
            StartCoroutine(GetJumpForce(1));
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
        _attackColiders[0].gameObject.SetActive(true);

        _animtor.SetBool("isAttack", true);
        //_animtor.SetFloat("Attack", _comboGauge);

        StartCoroutine(GetKeyTime());
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
        bool  attack = false;
        float progress = 0f;
        yield return null;

        while (progress <= time)
        {
            // 방향 공격
            if (_player._stats.State == PlayerState.Attack)
            {
                // ...
                _attackDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

                if (Input.GetKeyDown(KeyCode.Z) && _player._stats.IsAttack)
                {
                    _animtor.SetFloat("AttackY", _attackDir.y);
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

    protected override void AnimFrameStart()
    {

    }

    protected override void AnimFrameUpdate()
    {

    }

    protected override void AnimFrameEnd()
    {
        if (_animtor.GetBool("isStart"))
            _animtor.SetBool("isStart", false);
        else
        {
            _player._stats.IsAttack = false;
            _attackColiders[0].gameObject.SetActive(false);

            _animtor.SetBool("isAttack", false);
            _animtor.SetFloat("Attack", 0);
            _animtor.SetInteger("AttackY", 0);
            _animtor.SetBool("isHurt", false);
        }
    }

    IEnumerator GetKeyTime(float time = 1f, PlayerState state = PlayerState.NONE)
    {
        float progress = 0f;

        while (progress <= time)
        {
            if (Input.anyKeyDown)
            {
                StartCoroutine(Combo(2f));
                break;
            }
            progress += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator GetJumpForce(float time = 1f)
    {
        // 점프 로직
        float progress = 0f;

        _animtor.SetInteger("Jump", 1);
        _renderer.sprite = _upImg;
        _rigid2D.velocity = new Vector2(_rigid2D.velocity.x, _player._stats.JumpPower);

        StartCoroutine(GetKeyTime(1f, _player._stats.State));

        while (progress <= time)
        {
            _renderer.sprite = _upImg;

            if (Input.GetKey(KeyCode.C) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space))
            {
                _log.Message("rid : " + _rigid2D.velocity.y);
                _rigid2D.velocity += Vector2.up * _player._stats.JumpPower * Time.deltaTime;
                _renderer.sprite = _upImg;

                progress += Time.deltaTime;
                yield return null;
            }
            else if (Input.GetKeyUp(KeyCode.C) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.Space))
            {
                yield return new WaitForSeconds(0.25f);
                break;
            }
        }
        _animtor.SetInteger("Jump", 2);
        StartCoroutine(GetKeyTime(1, _player._stats.State));
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (!isStart && _rigid2D.velocity.y != 0)
            return;

        _animtor.SetInteger("Jump", 0);
        _player._stats.IsJump = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("EnemyAttack") && !_animtor.GetBool("isHurt"))
        {
            GetComponent<CinemachineImpulseSource>().GenerateImpulse();
            _animtor.SetBool("isHurt", true);
        }
    }

    void StartEvent()
    {
        isStart = true;

        _animtor.enabled = true;
        _animtor.SetBool("isStart", true);
    }
}