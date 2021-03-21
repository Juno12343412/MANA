using MANA.Enums;
using UnityEngine;

[CreateAssetMenu(fileName = "New ItemData", menuName = "Item Data", order = 2)]
public class Item : ScriptableObject
{
    [SerializeField] int      iCode;

    [SerializeField] string   iName;

    [SerializeField] float    iCurHP;

    [SerializeField] float    iMaxHP;

    [SerializeField] float    iRadius;

    [SerializeField] bool     iExplosion;

    [SerializeField] float    iDrop;

    [SerializeField] ItemKind iKind;

    /// <summary>
    /// 아이템의 코드값
    /// </summary>
    public int Code => iCode;

    /// <summary>
    /// 아이템 이름
    /// </summary>
    public string Name { get => iName; set => iName = value; }

    /// <summary>
    /// 현재 체력
    /// </summary>
    public float CurHP { get => iCurHP; set => iCurHP = value; }

    /// <summary>
    /// 최대 체력
    /// </summary>
    public float MaxHP => iMaxHP;

    /// <summary>
    /// 드랍 확률
    /// </summary>
    public float Radius { get => iRadius; set => iRadius = value; }

    /// <summary>
    /// 폭발 유무
    /// </summary>
    public bool Explosion => iExplosion;

    /// <summary>
    /// 드랍 확률
    /// </summary>
    public float Drop { get => iDrop; set => iDrop = value; }

    public ItemKind Kind { get => iKind; set => iKind = value; }
}
