using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager.Sound;

public class Grasses : MonoBehaviour
{
    [SerializeField] private float hp = 1;

    public void DestroyGrass()
    {
        Destroy(gameObject);
        SoundPlayer.instance.PlaySound("Map_leaf_2", 0.25f);
    }
}
