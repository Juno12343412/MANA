using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDBase.Controllers.ObjectSystem;
using UDBase.Controllers.LogSystem;
using MANA.Enums;
using UDBase.Utils;

public class Player : PlayerMachine
{
    ULogger _log;

    [SerializeField] private GameObject[] _attackColiders;
    [SerializeField] private Vector2 _attackDir = Vector2.zero;

    private SpriteRenderer _renderer;
    private Animator _animtor;
    private Rigidbody2D _rigid2D;

    protected sealed override void PlayerSetting(ILog log)
    {
        _log = log.CreateLogger(this);
        base.PlayerSetting(log);

        MyStats.CurHP = MyStats.MaxHP = 100;
        MyStats.JumpPower = 5;
        MyStats.MoveSpeed = 5;

        _renderer = GetComponent<SpriteRenderer>();
        _rigid2D = GetComponent<Rigidbody2D>();
        _animtor = GetComponent<Animator>();
    }

    protected sealed override void IdleEvent()
    {

        _log.Message("Idle");

        base.IdleEvent();
        _animtor.SetBool("isWalk", false);
    }

    protected sealed override void WalkEvent()
    {

        base.WalkEvent();

        // 움직이는 로직
        float x = Input.GetAxisRaw("Horizontal");
        if (x != 0)
        {
            _animtor.SetBool("isWalk", true);

            _renderer.flipX = x == 1 ? false : true;

            if (_rigid2D.velocity.x <= MyStats.MoveSpeed && _rigid2D.velocity.x >= -MyStats.MoveSpeed)
            {
                _rigid2D.velocity += new Vector2(x, 0) * MyStats.MoveSpeed * 0.05f;
                _log.Message("x : " + _rigid2D.velocity);
            }
        }
        else
        {
            _animtor.SetBool("isWalk", false);

            //_rigid2D.velocity = new Vector2(0, _rigid2D.velocity.y);
            MyStats.State = PlayerState.Idle;
            _log.Message("초기화");
        }
    }

    protected sealed override void RunEvent()
    {

        base.RunEvent();

        // 움직이는 로직
    }

    protected sealed override void JumpEvent()
    {

        base.JumpEvent();

        // 점프하는 로직
        if (!MyStats.IsJump)
        {

            //MyStats.IsJump = true;
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

    }

    protected sealed override void SpecialAttackEvent()
    {

        base.SpecialAttackEvent();

        if (MyStats.IsJump)
        {

            MyStats.State = PlayerState.Idle;
            return;
        }
    }

    protected sealed override void DeadEvent()
    {
        base.DeadEvent();
    }

    void Combo()
    {

        _log.Message("Combo");
        // 방향 공격
        if (MyStats.State == PlayerState.Attack)
        {

            // ...
            _attackDir = new Vector2(Input.GetAxisRaw("Horizontal"), 0);

            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                return;
            }

            // 하단 공격
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                return;
            }
        }

        // 하단 이동
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _rigid2D.velocity = Vector2.down * MyStats.JumpPower * 2;
        }
    }

    void AnimFrameStart()
    {

    }

    void AnimFrameUpdate()
    {

        if (MyStats.State == PlayerState.Attack)
        {

            // 방향에 따른 방향 공격
        }
    }

    void AnimFrameEnd()
    {

    }

    IEnumerator GetKeyTime(float time = 1f, PlayerState state = PlayerState.NONE)
    {

        float progress = 0f;

        while (progress <= time)
        {

            if (Input.anyKeyDown)
            {

                //if (state == PlayerState.Attack || state == PlayerState.SpecialAttack || state == PlayerState.Jump || state == PlayerState.Walk) {

                //    Combo();
                //    progress = time;
                //}
                Combo();
                break;
            }
            progress += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator GetJumpForce(float time = 1f)
    {

        float progress = 0f;

        // 점프 로직
        _rigid2D.velocity = new Vector2(_rigid2D.velocity.x, 1 * MyStats.JumpPower);
        StartCoroutine(GetKeyTime(1f, MyStats.State));

        while (progress <= time)
        {

            if (Input.GetKey(KeyCode.C) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space))
            {

                _rigid2D.velocity += Vector2.up * MyStats.JumpPower * Time.deltaTime;
                _log.Message("점프력 : " + _rigid2D.velocity);
                progress += Time.deltaTime;
                yield return null;
            }
            else if (Input.GetKeyUp(KeyCode.C) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.Space))
            {
                break;
            }
        }
        StartCoroutine(GetKeyTime(1, MyStats.State));
    }
}