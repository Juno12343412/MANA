using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grasses : MonoBehaviour
{
    [SerializeField] private float hp = 1;

    public void DestroyGrass()
    {
        Destroy(gameObject);
    }
}
