using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Zenject;
using UDBase.Controllers.ObjectSystem;
using UDBase.Controllers.LogSystem;
using MANA.Enums;
using Manager.Sound;
using Pooling;

public class Destructible : AIMachine
{
    ULogger _log;
    Animator _animtor;
    [SerializeField] private DestructKind kind;
    [SerializeField] private float maxHp;
    [SerializeField] private GameObject reward;
    [SerializeField] private GameObject[] wallPiece;
    private float dir = 0.0f;
    private Vector3 originPos;
    Vector2 wallPieceDir;

    protected sealed override void AISetting(ILog log)
    {
        _log = log.CreateLogger(this);

        _animtor = GetComponent<Animator>();

        base.AISetting(log);

        Kind = ObjectKind.Obstacle;
        MyStats.CurHP = MyStats.MaxHP = maxHp;


        originPos = transform.position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Attack"))
        {

            if (kind != DestructKind.Grass)
                GetComponent<CinemachineImpulseSource>().GenerateImpulse();

            GetComponent<Animator>().SetBool("isHurt", true);
            StartCoroutine(ShakeObject());

            SoundPlayer.instance.PlaySound("Map_stone_2");

            MyStats.CurHP -= 1;
            if (MyStats.CurHP <= 0)
            {
                if (reward != null)
                    reward.SetActive(true);
                Dead(other.gameObject);
            }
        }
    }

    void Dead(GameObject obj)
    {
        SoundPlayer.instance.PlaySound("Map_stone_1");

        for (int i = 0; i < wallPiece.Length; i++)
        {
            wallPiece[i].SetActive(true);
            //switch (obj.gameObject.GetComponentInParent<Player>()._attackDir)
            //{
            //    case Vector2 v when v.Equals(Vector2.left):
            //        wallPieceDir = new Vector2(-1, 0);
            //        break;
            //    case Vector2 v when v.Equals(Vector2.right):
            //        wallPieceDir = new Vector2(1, 0);

            //        break;
            //    case Vector2 v when v.Equals(Vector2.up):
            //        wallPieceDir = new Vector2(0, 1);

            //        break;
            //    case Vector2 v when v.Equals(Vector2.down):
            //        wallPieceDir = new Vector2(0, -1);
            //        break;
            //}
            if(kind == DestructKind.DestructibleX)
                wallPieceDir = new Vector3(obj.transform.position.x, transform.position.y,0) - wallPiece[i].transform.position;
            else if(kind == DestructKind.DestructibleY)
                wallPieceDir = new Vector3(transform.position.x, obj.transform.position.y, 0) - wallPiece[i].transform.position;
            Vector3.Normalize(wallPieceDir);
            wallPieceDir = new Vector2(wallPieceDir.x * 2, wallPieceDir.y);
            //float length = Vector3.Distance(transform.position, wallPiece[i].transform.position);
            wallPiece[i].GetComponent<WallPiece>().BreakWall(wallPieceDir);
        }
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
    }

    IEnumerator ShakeObject()
    {
        float temp = 0.1f;
        float repeat = 3;
        float time = temp;
        while (time > 0)
        {
            //float randPosX = Random.Range(0, 2);
            //randPosX = randPosX == 0 ? -0.07f : 0.07f;
            //float randPosY = Random.Range(0, 2);
            //randPosY = randPosY == 0 ? -0.05f : 0.05f;
            float randPosX = Random.Range(-0.07f, 0.07f);
            float randPosY = Random.Range(-0.05f, 0.05f);
            Vector3 randPos = new Vector3(randPosX, randPosY, 0);

            transform.position += randPos;
            time -= temp / repeat;
            yield return new WaitForSeconds(temp / repeat);
        }
        transform.position = originPos;
    }

    public void DestroyObj()
    {
        Destroy(gameObject);
    }

    protected override void AnimFrameStart()
    {
    }
    protected override void AnimFrameEnd()
    {
        GetComponent<Animator>().SetBool("isHurt", false);
    }
}

