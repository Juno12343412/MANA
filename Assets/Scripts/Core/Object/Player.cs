using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDBase.Controllers.ObjectSystem;
using UDBase.Controllers.LogSystem;
using MANA.Enums;
using UDBase.Utils;
using Zenject;

public class Player : PlayerMachine
{
    ULogger _log;

    [SerializeField] private float _comboGauge = 0;

    [SerializeField] private Collider2D[] _attackColiders;
    public Vector2 _attackDir = Vector2.zero;

    [SerializeField] private Sprite _upImg;

    private SpriteRenderer _renderer;
    private Animator _animtor;
    private Rigidbody2D _rigid2D;

    
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
    }

    protected sealed override void IdleEvent()
    {
        base.IdleEvent();
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

                if (_rigid2D.velocity.x <= _player._stats.MoveSpeed && _rigid2D.velocity.x >= -_player._stats.MoveSpeed)
                {
                    _rigid2D.velocity += new Vector2(x, 0) * _player._stats.MoveSpeed * 0.05f;
                }
            }
            else
            {
                _animtor.SetBool("isWalk", false);

                _rigid2D.velocity = new Vector2(0, _rigid2D.velocity.y);
                _player._stats.State = PlayerState.Idle;
                _log.Message("초기화");
            }
        }
    }

    protected sealed override void RunEvent()
    {
        base.RunEvent();

        // 움직이는 로직
    }

    protected sealed override void JumpEvent()
    {
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
    }

    protected sealed override void AttackEvent()
    {
        base.AttackEvent();

        _player._stats.IsAttack = true;
        _attackColiders[0].gameObject.SetActive(true);

        _animtor.SetBool("isAttack", true);
        _animtor.SetFloat("Attack", _comboGauge);

        StartCoroutine(GetKeyTime());
    }

    protected sealed override void SpecialAttackEvent()
    {
        base.SpecialAttackEvent();

        if (_player._stats.IsJump)
        {
            _player._stats.State = PlayerState.Idle;
            return;
        }
    }

    protected sealed override void DeadEvent()
    {
        base.DeadEvent();
    }

    IEnumerator Combo(float time = 0f)
    {
        float progress = 0f;
        yield return null;

        while (progress <= time)
        {
            _log.Message("Combo " + _player._stats.State);

            // 방향 공격
            if (_player._stats.State == PlayerState.Attack)
            {
                // ...
                _attackDir = new Vector2(Input.GetAxisRaw("Horizontal"), 0);

                if (Input.GetKeyDown(KeyCode.Z))
                {
                    _log.Message("Combo2 : " + _comboGauge);

                    if (_comboGauge != 2)
                        _comboGauge++;
                    else
                        _comboGauge = 0;

                    break;
                }

                // 하단 공격
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    break;
                }
            }
            else if (_player._stats.State == PlayerState.Jump)
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    _attackDir = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
                }
            }
            // 하단 이동
            //if (Input.GetKeyDown(KeyCode.DownArrow))
            //{
            //    _rigid2D.velocity = Vector2.down * MyStats.JumpPower * 2;
            //}
            progress += Time.deltaTime;
            yield return null;
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
        _player._stats.IsAttack = false;
        _attackColiders[0].gameObject.SetActive(false);

        _animtor.SetBool("isAttack", false);
        _animtor.SetFloat("Attack", 0);
    }

    IEnumerator GetKeyTime(float time = 1f, PlayerState state = PlayerState.NONE)
    {
        float progress = 0f;

        while (progress <= time)
        {
            if (Input.anyKeyDown)
            {
                StartCoroutine(Combo(1f));
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
        _rigid2D.velocity = new Vector2(_rigid2D.velocity.x, 1 * _player._stats.JumpPower);
        StartCoroutine(GetKeyTime(1f, _player._stats.State));

        while (progress <= time)
        {
            _renderer.sprite = _upImg;
            Debug.Log("이미지 : " + _renderer.sprite);

            if (Input.GetKey(KeyCode.C) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space))
            {
                _rigid2D.velocity += Vector2.up * _player._stats.JumpPower * Time.deltaTime;
                _renderer.sprite = _upImg;
                _log.Message("점프력 : " + _rigid2D.velocity);
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
        _animtor.SetInteger("Jump", 0);
        _player._stats.IsJump = false;
    }
}